import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Components
import { SelectComponent } from 'src/app/shared/components/select/select.component';

// Models
import { ChunkerStrategy } from 'src/app/models/chunker';
import { CreateWorkflowRequest } from 'src/app/models/workflow';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ConversationModel } from 'src/app/models/chat';
import { ProviderOption } from 'src/app/models/provider';

import { mapChunkerStrategies } from 'src/app/shared/utils/chunker-utils';
import { ModelSpeedPipe } from './model-speed.pipe';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { ionStar, ionStarHalf, ionStarOutline } from '@ng-icons/ionicons';
import { UsageTooltipComponent } from 'src/app/shared/components/usage-tooltip/usage-tooltip.component';
import { SliderInputComponent } from 'src/app/shared/components/slider-input/slider-input.component';
import { MaxChunkSliderComponent } from 'src/app/shared/components/max-chunks-slider/max-chunk-slider.component';
import { RadarChartComponent } from 'src/app/shared/components/radar-chart/radar-chart.component';
import { PriceCalculatorComponent } from 'src/app/shared/components/price-calculator/price-calculator.component';
import { WorkflowMetricsService } from 'src/app/services/workflow-metrics.service';
import {
  map,
  Observable,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { RadarAxis } from 'src/app/services/radar-data.service';
import { ProviderSelectService } from 'src/app/services/provider-select.service';
import { WorkflowService } from 'src/app/services/workflow.service';

@Component({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SelectComponent,
    ModelSpeedPipe,
    UsageTooltipComponent,
    SliderInputComponent,
    MaxChunkSliderComponent,
    RadarChartComponent,
    PriceCalculatorComponent,
    NgIcon,
  ],
  providers: [provideIcons({ ionStar, ionStarHalf, ionStarOutline })],
  templateUrl: './new-workflow.component.html',
  standalone: true,
})
export class NewWorkflowComponent implements OnInit {
  form!: FormGroup;
  error = '';
  chunkerStrategies: { label: string; value: number | string }[] = [];

  embeddingOptions$!: Observable<{ value: string | number; label: string }[]>;
  conversationOptions$!: Observable<
    { value: string | number; label: string }[]
  >;
  embeddingModels$!: Observable<EmbeddingModel[]>;
  conversationModels$!: Observable<ConversationModel[]>;

  embeddingModel$!: Observable<EmbeddingModel | null>;
  conversationModel$!: Observable<ConversationModel | null>;

  radarAxes$!: Observable<RadarAxis[]>;
  embeddingCost$!: Observable<number>;
  conversationCosts$!: Observable<{ in: number; out: number }>;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private metrics: WorkflowMetricsService,
    private workflowService: WorkflowService,
    private ps: ProviderSelectService
  ) {}

  ngOnInit(): void {
    this.chunkerStrategies = mapChunkerStrategies();
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      strategy: [ChunkerStrategy.PROPOSITION, Validators.required],
      settings: this.fb.group({
        threshold: [
          0.9,
          [Validators.required, Validators.min(0), Validators.max(1)],
        ],
        evaluationPrompt: [''],
        maxChunkSize: [
          600,
          [Validators.required, Validators.min(100), Validators.max(1000)],
        ],
      }),
      embeddingProvider: this.fb.group({
        provider: [-1, Validators.required],
        model: [null, Validators.required],
        vectorSize: [0],
      }),
      conversationProvider: this.fb.group({
        provider: [-1, Validators.required],
        model: [null, Validators.required],
      }),
    });

    this.embeddingOptions$ = this.ps.getEmbeddingProviders();
    this.conversationOptions$ = this.ps.getConversationProviders();

    const embProvCtrl = this.form.get('embeddingProvider.provider')!;
    this.embeddingModels$ = embProvCtrl.valueChanges.pipe(
      startWith(embProvCtrl.value),
      switchMap((id) => this.ps.getEmbeddingModels(id)),
      tap(() => this.form.get('embeddingProvider.model')!.reset())
    );

    const embModelCtrl = this.form.get('embeddingProvider.model')!;
    this.embeddingModel$ = embModelCtrl.valueChanges.pipe(
      startWith(embModelCtrl.value),
      withLatestFrom(this.embeddingModels$),
      map(([val, list]) => list.find((m) => m.value === val) ?? null),
      tap((m) =>
        this.form
          .get('embeddingProvider.vectorSize')!
          .setValue(m?.vectorSize ?? 0)
      )
    );

    const convProvCtrl = this.form.get('conversationProvider.provider')!;
    this.conversationModels$ = convProvCtrl.valueChanges.pipe(
      startWith(convProvCtrl.value),
      switchMap((id) => this.ps.getConversationModels(id)),
      tap(() => this.form.get('conversationProvider.model')!.reset())
    );

    const convModelCtrl = this.form.get('conversationProvider.model')!;
    this.conversationModel$ = convModelCtrl.valueChanges.pipe(
      startWith(convModelCtrl.value),
      withLatestFrom(this.conversationModels$),
      map(([val, list]) => list.find((m) => m.value === val) ?? null)
    );

    this.metrics.init(this.form, this.embeddingModel$, this.conversationModel$);

    this.radarAxes$ = this.metrics.radarAxes$();
    this.embeddingCost$ = this.metrics.embeddingCost$();
    this.conversationCosts$ = this.metrics.conversationCosts$();
  }

  get strategyValue(): number {
    return this.form.get('strategy')?.value;
  }

  setChunkerStrategy(value: number): void {
    this.form.patchValue({ strategy: value });
  }

  createWorkflow(): void {
    if (this.form.invalid) {
      this.error = 'Please fill in all required fields correctly.';
      return;
    }
    const formValue = this.form.value;
    if (typeof formValue.embeddingProvider.model === 'object') {
      formValue.embeddingProvider.model =
        formValue.embeddingProvider.model.value;
    }
    const workflowDetails: CreateWorkflowRequest = formValue;
    this.workflowService.createWorkflow(workflowDetails).subscribe(
      (id) => this.router.navigate(['/dashboard/workflows']),
      (response) => {
        this.error = response.error.message;
      }
    );
  }
}
