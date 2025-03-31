import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { EnumMapperService } from './../../shared/services/enum-mapper.service';
import { ChunkerStrategy } from 'src/app/models/chunker';
import { CreateWorkflowRequest } from 'src/app/models/create-workflow';
import { EmbeddingProviderEnum } from 'src/app/models/embedding-provider';
import { WorkflowService } from 'src/app/services/workflow.service';
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { SelectComponent } from 'src/app/shared/components/select/select.component';
import { SidebarComponent } from 'src/app/shared/components/sidebar/sidebar.component';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [
    SidebarComponent,
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
        vectorSize: [
          0,
          [Validators.required, Validators.min(1), Validators.max(128000)],
        ],
      }),
    });
  }

  createWorkflow() {
    if (this.form.invalid) {
      this.error = 'Please fill in all required fields correctly.';
      return;
    }

    const workflowDetails: CreateWorkflowRequest = this.form.value;

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
