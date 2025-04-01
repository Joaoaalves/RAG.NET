import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

// Components
import { EnumMapperService } from './../../shared/services/enum-mapper.service';
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

// Services
import { WorkflowService } from 'src/app/services/workflow.service';

@Component({
  imports: [
    InputComponent,
    SelectComponent,
    TextAreaComponent,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './new-workflow.component.html',
  standalone: true,
})
export class NewWorkflowComponent implements OnInit {
  form!: FormGroup;
  error: string = '';
  chunkerStrategies: { label: string; value: number }[] = [];
  embeddingProviders: { label: string; value: number }[] = [];
  embeddingModelsResponse!: EmbeddingModelsResponse;
  modelOptions: EmbeddingModel[] = [];

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private router: Router,
    private mapper: EnumMapperService
  ) {}

  ngOnInit(): void {
    this.embeddingProviders = this.mapper.mapEnumToOptions(
      EmbeddingProviderEnum
    );
    this.chunkerStrategies = this.mapper.mapEnumToOptions(ChunkerStrategy);

    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      strategy: [ChunkerStrategy.PROPOSITION, Validators.required],
      settings: this.fb.group({
        threshold: [
          0.8,
          [Validators.required, Validators.min(0), Validators.max(1)],
        ],
        evaluationPrompt: [''],
        maxChunkSize: [
          600,
          [Validators.required, Validators.min(100), Validators.max(1000)],
        ],
      }),
      embeddingProvider: this.fb.group({
        provider: [EmbeddingProviderEnum.OPENAI, Validators.required],
        apiKey: ['', Validators.required],
        model: [null, Validators.required],
        vectorSize: [0, Validators.required],
      }),
    });

    this.workflowService.getEmbeddingModels().subscribe((response) => {
      this.embeddingModelsResponse = response;
      this.updateModelOptions(
        this.form.get('embeddingProvider.provider')?.value
      );
    });

    this.form
      .get('embeddingProvider.provider')
      ?.valueChanges.subscribe((providerValue: number) => {
        this.updateModelOptions(providerValue);
        this.form.get('embeddingProvider.model')?.reset();
        this.form.get('embeddingProvider.vectorSize')?.setValue(0);
      });

    this.form
      .get('embeddingProvider.model')
      ?.valueChanges.subscribe((modelValue: any) => {
        const selectedModel =
          typeof modelValue === 'object'
            ? modelValue
            : this.modelOptions.find((m) => m.value === modelValue);
        if (selectedModel) {
          this.form
            .get('embeddingProvider.vectorSize')
            ?.setValue(selectedModel.vectorSize);
        }
      });
  }

  updateModelOptions(provider: number): void {
    console.log(provider);
    if (provider === EmbeddingProviderEnum.OPENAI) {
      this.modelOptions = this.embeddingModelsResponse.openAI;
    } else if (provider === EmbeddingProviderEnum.VOYAGE) {
      this.modelOptions = this.embeddingModelsResponse.voyage;
    } else {
      this.modelOptions = [];
    }
  }

  createWorkflow(): void {
    console.log(this.embeddingModelsResponse);
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
      (id) => {
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        this.error = error;
      }
    );
  }
}
