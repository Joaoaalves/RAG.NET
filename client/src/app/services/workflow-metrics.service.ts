import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { filter, map, startWith, switchMap } from 'rxjs/operators';
import { ChunkerStrategy } from 'src/app/models/chunker';

import { RadarAxis } from 'src/app/services/radar-data.service';
import { ConversationModel, EmbeddingModel } from '../models/models';

const EMPTY_EMB_MODEL: EmbeddingModel = {
  value: '',
  label: '',
  price: 0,
  speed: 0,
  maxContext: 0,
  vectorSize: 0,
};
const EMPTY_CONV_MODEL: ConversationModel = {
  value: '',
  label: '',
  inputPrice: 0,
  outputPrice: 0,
  speed: 0,
  contextWindow: 0,
  maxOutput: 0,
};

@Injectable({ providedIn: 'root' })
export class WorkflowMetricsService {
  private form$ = new BehaviorSubject<FormGroup | null>(null);
  private embModel$ = new BehaviorSubject<EmbeddingModel>(EMPTY_EMB_MODEL);
  private convModel$ = new BehaviorSubject<ConversationModel>(EMPTY_CONV_MODEL);

  /** Initialize with form and model-change observables */
  init(
    formGroup: FormGroup,
    embModelChanges$: Observable<EmbeddingModel | null>,
    convModelChanges$: Observable<ConversationModel | null>
  ): void {
    this.form$.next(formGroup);
    embModelChanges$
      .pipe(map((m) => m ?? EMPTY_EMB_MODEL))
      .subscribe(this.embModel$);
    convModelChanges$
      .pipe(map((m) => m ?? EMPTY_CONV_MODEL))
      .subscribe(this.convModel$);
  }

  /** Expose the current chunker configuration as [strategy, threshold, maxSize] */
  private chunkerConfig$(): Observable<[ChunkerStrategy, number, number]> {
    return this.form$.pipe(
      filter((f): f is FormGroup => f !== null),
      switchMap((form) => {
        const strat = form.get('strategy')!;
        const thr = form.get('settings.threshold')!;
        const sz = form.get('settings.maxChunkSize')!;
        return combineLatest([
          strat.valueChanges.pipe(startWith(strat.value)),
          thr.valueChanges.pipe(startWith(thr.value)),
          sz.valueChanges.pipe(startWith(sz.value)),
        ]);
      })
    );
  }

  /** Public observable of radar axes data */
  radarAxes$(): Observable<RadarAxis[]> {
    // thresholds for log normalization
    const PRICE_THRESHOLD = 30; // USD per million tokens
    const CONTEXT_THRESHOLD = 1_000_000; // tokens

    return combineLatest([
      this.chunkerConfig$(),
      this.embModel$,
      this.convModel$,
    ]).pipe(
      map(([[strat, threshold, maxSz], emb, conv]) => {
        const embSpeed = this.calculateEmbeddingSpeed(
          strat,
          threshold,
          maxSz,
          emb.speed
        );
        const convSpeed = this.normalizeLinear(conv.speed, 0, 10);
        const qualityPct = this.calculateQuality(strat, maxSz, threshold);

        const rawCost = this.calculateRawCost(
          strat,
          emb.price,
          conv.inputPrice,
          conv.outputPrice
        );
        const costPct = this.logNormalize(rawCost, PRICE_THRESHOLD);
        const contextPct = this.logNormalize(
          conv.contextWindow,
          CONTEXT_THRESHOLD
        );

        return [
          this.buildAxis('Emb. Speed', embSpeed),
          this.buildAxis('Cost', costPct),
          this.buildAxis('Context', contextPct),
          this.buildAxis('Qry. Speed', convSpeed),
          this.buildAxis('Quality', qualityPct),
        ];
      })
    );
  }

  /** Observable of just the embedding cost (raw) */
  embeddingCost$(): Observable<number> {
    return combineLatest([
      this.embModel$,
      this.convModel$,
      this.chunkerConfig$(),
    ]).pipe(
      map(([emb, conv, [strat, ,]]) =>
        this.calculateRawCost(
          strat,
          emb.price,
          conv.inputPrice,
          conv.outputPrice
        )
      )
    );
  }

  /** Build a RadarAxis object with 0–100 range */
  private buildAxis(name: string, currentPct: number): RadarAxis {
    return { name, min: 0, max: 100, current: Math.round(currentPct) };
  }

  /** Calculate raw cost including conversation if needed */
  private calculateRawCost(
    strat: ChunkerStrategy,
    embPrice: number,
    convIn: number,
    convOut: number
  ): number {
    let total = embPrice;
    if (
      strat === ChunkerStrategy.PROPOSITION ||
      strat === ChunkerStrategy.SEMANTIC
    ) {
      total += convIn + convOut;
    }
    return total;
  }

  /** Logarithmic normalization: maps [1…threshold] → [0…100] */
  private logNormalize(value: number, threshold: number): number {
    const v = Math.max(value, 1);
    return Math.min(100, (Math.log(v) / Math.log(threshold)) * 100);
  }

  /** Linear normalization: maps [min…max] → [0…100] */
  private normalizeLinear(v: number, min: number, max: number): number {
    return ((v - min) / (max - min)) * 100;
  }

  /** Calculate embedding speed as average of chunk-speed and model speed */
  private calculateEmbeddingSpeed(
    strat: ChunkerStrategy,
    threshold: number,
    maxSz: number,
    embSpeedModel: number
  ): number {
    const chunkSpeed =
      strat === ChunkerStrategy.PARAGRAPH
        ? 8 + ((maxSz - 100) / 900) * 2
        : 4 - threshold * 2;
    const normChunk = this.normalizeLinear(chunkSpeed, 0, 10);
    const normEmb = this.normalizeLinear(embSpeedModel, 0, 10);
    return (normChunk + normEmb) / 2;
  }

  private calculateFinalCost(
    normalizedCost: number,
    strat: ChunkerStrategy,
    threshold: number,
    maxSz: number
  ): number {
    let increase = 1;
    if (strat === ChunkerStrategy.PARAGRAPH) {
      if (maxSz > 1000) increase = 0.95;
      else if (maxSz < 500) increase = 1.05;
      return Math.min(100, Math.max(0, normalizedCost * increase));
    }
    increase = threshold > 0.95 ? 1.6 : 1.4;
    return Math.min(100, normalizedCost * increase);
  }

  private calculateQuality(
    strat: ChunkerStrategy,
    maxSz: number,
    threshold: number
  ): number {
    const ratio =
      strat === ChunkerStrategy.PARAGRAPH
        ? this.paragraphQuality(maxSz)
        : this.thresholdQuality(threshold);
    return ratio * 100;
  }

  private paragraphQuality(maxSz: number): number {
    const MIN = 100,
      LOW = 300,
      HIGH = 600,
      MAX = 1200;
    if (maxSz <= LOW) {
      return Math.max(0.2, ((maxSz - MIN) / (LOW - MIN)) * 0.6);
    } else if (maxSz <= HIGH) {
      return 0.6;
    } else {
      return Math.max(0.3, ((MAX - maxSz) / (MAX - HIGH)) * 0.6);
    }
  }

  private thresholdQuality(threshold: number): number {
    const base = 1 + threshold * 2;
    return ((base - 1) / 2) * 0.4 + 0.4;
  }

  conversationCosts$(): Observable<{ in: number; out: number }> {
    return this.convModel$.pipe(
      map((m) => ({ in: m.inputPrice, out: m.outputPrice }))
    );
  }
}
