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
import {
  ProviderData,
  ProviderOption,
  ProvidersResponse,
} from 'src/app/models/provider';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';

// Utils
import {
  getProviderKeyByValueFromResponse,
  mapValidProviders,
} from 'src/app/shared/utils/providers-utils';
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
import { map, Observable } from 'rxjs';
import { RadarAxis } from 'src/app/services/radar-data.service';

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
  error: string = '';
  chunkerStrategies: { label: string; value: number }[] = [];

  embeddingProviders: ProviderData[] = [];
  conversationProviders: ProviderData[] = [];

  embeddingOptions: ProviderOption[] = [];
  conversationOptions: ProviderOption[] = [];

  embeddingModelsResponse!: ProvidersResponse<EmbeddingModel>;
  conversationModelsResponse!: ProvidersResponse<ConversationModel>;
  embeddingModelOptions: EmbeddingModel[] = [];
  conversationModelOptions: ConversationModel[] = [];

  selectedEmbeddingModel: EmbeddingModel | null = null;
  selectedConversationModel: ConversationModel | null = null;

  radarAxes$!: Observable<RadarAxis[]>;
  embeddingCost$!: Observable<number>;
  conversationCosts$!: Observable<{ in: number; out: number }>;

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private router: Router,
    private metrics: WorkflowMetricsService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadModels();
    this.setupSubscriptions();

    this.metrics.init(
      this.form,
      this.form
        .get('embeddingProvider.model')!
        .valueChanges.pipe(
          map(
            (id) =>
              this.embeddingModelOptions.find((m) => m.value === id) ?? null
          )
        ),
      this.form
        .get('conversationProvider.model')!
        .valueChanges.pipe(
          map(
            (id) =>
              this.conversationModelOptions.find((m) => m.value === id) ?? null
          )
        )
    );

    this.radarAxes$ = this.metrics.radarAxes$();
    this.embeddingCost$ = this.metrics.embeddingCost$();
    this.conversationCosts$ = this.metrics.conversationCosts$();
  }

  private initForm(): void {
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
      }),
      conversationProvider: this.fb.group({
        provider: [-1, Validators.required],
        model: [null, Validators.required],
      }),
    });
  }

  get strategyValue(): number {
    return this.form.get('strategy')?.value;
  }

  public setChunkerStrategy(value: number) {
    this.form.patchValue({
      strategy: value,
    });
  }

  private loadModels(): void {
    this.workflowService.getEmbeddingModels().subscribe((response) => {
      this.embeddingModelsResponse = response;

      this.embeddingProviders = mapValidProviders(response);
      this.embeddingProviders.forEach((provider) => {
        this.embeddingOptions.push({
          label: provider.title,
          value: provider.id,
        });
      });

      const currentProvider = this.form.get(
        'embeddingProvider.provider'
      )?.value;
      this.updateEmbeddingModelOptions(currentProvider);
    });

    this.workflowService.getConversationModels().subscribe((response) => {
      this.conversationModelsResponse = response;

      this.conversationProviders = mapValidProviders(response);
      this.conversationProviders.forEach((provider) => {
        this.conversationOptions.push({
          label: provider.title,
          value: provider.id,
        });
      });

      const currentProvider = this.form.get(
        'conversationProvider.provider'
      )?.value;
      this.updateConversationModelOptions(currentProvider);
    });
  }

  private setupSubscriptions(): void {
    this.subscribeEmbeddingProviderChanges();
    this.subscribeConversationProviderChanges();
    this.subscribeEmbeddingModelChanges();
    this.subscribeConversationModelChanges();
  }

  private subscribeEmbeddingProviderChanges(): void {
    this.form
      .get('embeddingProvider.provider')
      ?.valueChanges.subscribe((providerValue: number) => {
        this.updateEmbeddingModelOptions(providerValue);
        this.form.get('embeddingProvider.model')?.reset();
        this.form.get('embeddingProvider.vectorSize')?.setValue(0);
      });
  }

  private subscribeConversationProviderChanges(): void {
    this.form
      .get('conversationProvider.provider')
      ?.valueChanges.subscribe((providerValue: number) => {
        this.updateConversationModelOptions(providerValue);
        this.form.get('conversationProvider.model')?.reset();
      });
  }

  private subscribeConversationModelChanges(): void {
    this.form
      .get('conversationProvider.model')
      ?.valueChanges.subscribe((modelValue: string) => {
        const selectedModel = this.conversationModelOptions.find(
          (m) => m.value === modelValue
        );

        if (selectedModel) {
          this.selectedConversationModel = selectedModel;
        }
      });
  }

  private subscribeEmbeddingModelChanges(): void {
    this.form
      .get('embeddingProvider.model')
      ?.valueChanges.subscribe((modelValue: string) => {
        const selectedModel = this.embeddingModelOptions.find(
          (m) => m.value === modelValue
        );

        if (selectedModel) {
          this.selectedEmbeddingModel = selectedModel;
          this.form
            .get('embeddingProvider.vectorSize')
            ?.setValue(selectedModel.vectorSize);
        } else {
          this.selectedEmbeddingModel = null;
        }
      });
  }

  private updateEmbeddingModelOptions(provider: number): void {
    const providerKey = getProviderKeyByValueFromResponse(
      provider,
      this.embeddingModelsResponse
    );
    if (providerKey && this.embeddingModelsResponse[providerKey]) {
      this.embeddingModelOptions = this.embeddingModelsResponse[providerKey];
    } else {
      this.embeddingModelOptions = [];
    }
  }

  private updateConversationModelOptions(provider: number): void {
    const providerKey = getProviderKeyByValueFromResponse(
      provider,
      this.conversationModelsResponse
    );
    if (providerKey && this.conversationModelsResponse[providerKey]) {
      this.conversationModelOptions =
        this.conversationModelsResponse[providerKey];
    } else {
      this.conversationModelOptions = [];
    }
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
