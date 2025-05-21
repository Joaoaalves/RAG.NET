import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { filter, map, startWith, switchMap } from 'rxjs/operators';
import { ChunkerStrategy } from 'src/app/models/chunker';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ConversationModel } from 'src/app/models/chat';
import { RadarAxis } from 'src/app/services/radar-data.service';

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

  private chunkerConfig$() {
    return this.form$.pipe(
      filter((f): f is FormGroup => f !== null),
      switchMap((form) => {
        const stratCtrl = form.get('strategy')!;
        const thrCtrl = form.get('settings.threshold')!;
        const szCtrl = form.get('settings.maxChunkSize')!;

        return combineLatest([
          stratCtrl.valueChanges.pipe(startWith(stratCtrl.value)),
          thrCtrl.valueChanges.pipe(startWith(thrCtrl.value)),
          szCtrl.valueChanges.pipe(startWith(szCtrl.value)),
        ]);
      })
    );
  }

  radarAxes$(): Observable<RadarAxis[]> {
    return combineLatest([
      this.chunkerConfig$(),
      this.embModel$,
      this.convModel$,
    ]).pipe(
      map(([[strat, threshold, maxSz], emb, conv]) => {
        const chunkSpeed =
          strat === ChunkerStrategy.PARAGRAPH
            ? 8 + ((maxSz - 100) / 900) * 2
            : 6 - threshold * 2;

        const norm = (v: number, min: number, max: number) =>
          ((v - min) / (max - min)) * 100;

        const embSpeed = (norm(chunkSpeed, 0, 10) + norm(emb.speed, 0, 10)) / 2;
        const convSpeed = norm(conv.speed, 0, 10);

        const qualityPercent = this.calculateQuality(strat, maxSz, threshold);

        const costPercent = this.normalizeCost(
          emb.price,
          conv.inputPrice,
          conv.outputPrice
        );

        return [
          {
            name: 'Emb. Speed',
            min: 0,
            max: 100,
            current: Math.round(embSpeed),
          },
          {
            name: 'Cost',
            min: 0,
            max: 100,
            current: costPercent,
          },
          {
            name: 'Context',
            min: 0,
            max: 2500000,
            current: conv.contextWindow,
          },
          {
            name: 'Qry. Speed',
            min: 0,
            max: 100,
            current: Math.round(convSpeed),
          },
          {
            name: 'Quality',
            min: 0,
            max: 100,
            current: qualityPercent,
          },
        ];
      })
    );
  }

  embeddingCost$(): Observable<number> {
    return this.embModel$.pipe(map((m) => m.price));
  }

  private normalizeCost(emb: number, inp: number, out: number): number {
    const MAX_EMB = 0.3;
    const MAX_IN = 75;
    const MAX_OUT = 150;
    const MAX_TOTAL = MAX_EMB + MAX_IN + MAX_OUT;

    const total = emb + inp + out;
    const clamped = Math.min(Math.max(total, 0), MAX_TOTAL);
    return Math.round((clamped / MAX_TOTAL) * 100);
  }

  private calculateQuality(
    strat: ChunkerStrategy,
    maxSz: number,
    threshold: number
  ): number {
    let qualityRatio: number;

    if (strat === ChunkerStrategy.PARAGRAPH) {
      qualityRatio = this.paragraphQuality(maxSz);
    } else {
      qualityRatio = this.thresholdQuality(threshold);
    }

    return Math.round(qualityRatio * 100);
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
