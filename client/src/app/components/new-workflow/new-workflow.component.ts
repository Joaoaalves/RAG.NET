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
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { SelectComponent } from 'src/app/shared/components/select/select.component';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';

// Models
import { ChunkerStrategy } from 'src/app/models/chunker';
import { CreateWorkflowRequest } from 'src/app/models/workflow';
import { EmbeddingProviderEnum } from 'src/app/models/embedding';
import {
  EmbeddingModel,
  EmbeddingModelsResponse,
} from 'src/app/models/embedding';
import {
  ConversationModel,
  ConversationModelsResponse,
  ConversationProviderEnum,
} from 'src/app/models/chat';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';
import { EnumMapperService } from 'src/app/shared/services/enum-mapper.service';

@Component({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputComponent,
    SelectComponent,
    TextAreaComponent,
  ],
  templateUrl: './new-workflow.component.html',
  standalone: true,
})
export class NewWorkflowComponent implements OnInit {
  form!: FormGroup;
  error: string = '';
  chunkerStrategies: { label: string; value: number }[] = [];
  embeddingProviders: { label: string; value: number }[] = [];
  conversationProviders: { label: string; value: number }[] = [];
  embeddingModelsResponse!: EmbeddingModelsResponse;
  conversationModelsResponse!: ConversationModelsResponse;
  embeddingModelOptions: EmbeddingModel[] = [];
  conversationModelOptions: ConversationModel[] = [];

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private router: Router,
    private mapper: EnumMapperService
  ) {}

  ngOnInit(): void {
    this.initializeOptions();
    this.initForm();
    this.loadModels();
    this.setupSubscriptions();
  }

  private initializeOptions(): void {
    this.embeddingProviders = this.mapper.mapEnumToOptions(
      EmbeddingProviderEnum
    );
    this.conversationProviders = this.mapper.mapEnumToOptions(
      ConversationProviderEnum
    );
    this.chunkerStrategies = this.mapper.mapEnumToOptions(ChunkerStrategy);
  }

  private initForm(): void {
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
        apiKey: ['', Validators.required],
        model: [null, Validators.required],
        vectorSize: [0, Validators.required],
      }),
      conversationProvider: this.fb.group({
        provider: [-1, Validators.required],
        apiKey: ['', Validators.required],
        model: [null, Validators.required],
      }),
    });
  }

  get strategyValue(): number {
    return this.form.get('strategy')?.value;
  }

  private loadModels(): void {
    this.workflowService.getEmbeddingModels().subscribe((response) => {
      this.embeddingModelsResponse = response;
      const currentProvider = this.form.get(
        'embeddingProvider.provider'
      )?.value;
      this.updateEmbeddingModelOptions(currentProvider);
    });

    this.workflowService.getConversationModels().subscribe((response) => {
      this.conversationModelsResponse = response;
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

  private subscribeEmbeddingModelChanges(): void {
    this.form
      .get('embeddingProvider.model')
      ?.valueChanges.subscribe((modelValue: any) => {
        const selectedModel =
          typeof modelValue === 'object'
            ? modelValue
            : this.embeddingModelOptions.find((m) => m.value === modelValue);
        if (selectedModel) {
          this.form
            .get('embeddingProvider.vectorSize')
            ?.setValue(selectedModel.vectorSize);
        }
      });
  }

  private updateEmbeddingModelOptions(provider: number): void {
    if (provider === EmbeddingProviderEnum.OPENAI) {
      this.embeddingModelOptions = this.embeddingModelsResponse.openAI;
    } else if (provider === EmbeddingProviderEnum.VOYAGE) {
      this.embeddingModelOptions = this.embeddingModelsResponse.voyage;
    } else {
      this.embeddingModelOptions = [];
    }
  }

  private updateConversationModelOptions(provider: number): void {
    if (provider === ConversationProviderEnum.OPENAI) {
      this.conversationModelOptions = this.conversationModelsResponse.openAI;
    } else if (provider === ConversationProviderEnum.ANTHROPIC) {
      this.conversationModelOptions = this.conversationModelsResponse.anthropic;
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
      (id) => this.router.navigate(['/dashboard']),
      (error) => (this.error = error)
    );
  }
}
