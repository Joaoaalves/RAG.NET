import { QueryEnhancer } from './../../models/query-enhancer';
import { HlmSwitchComponent } from './../../../../libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { Workflow } from 'src/app/models/workflow';
import { WorkflowService } from './../../services/workflow.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { BehaviorSubject } from 'rxjs';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    HlmSwitchComponent,
    ReactiveFormsModule,
    InputComponent,
    TextAreaComponent,
  ],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;

  autoQueryForm!: FormGroup;
  autoQueryEnabled$ = new BehaviorSubject<boolean>(false);
  private autoQueryEnhancer?: QueryEnhancer;

  constructor(
    private readonly workflowService: WorkflowService,
    private readonly route: ActivatedRoute,
    private readonly queryEnhancerService: QueryEnhancerService,
    private readonly fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadWorkflowFromRoute();
  }

  private initializeForm(): void {
    this.autoQueryForm = this.fb.group({
      maxQueries: [
        1,
        [Validators.required, Validators.min(1), Validators.max(10)],
      ],
      guidance: ['', Validators.required],
    });
  }

  private loadWorkflowFromRoute(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('workflowId');
      if (id) {
        this.workflowId = id;
        this.loadWorkflow(id);
      }
    });
  }

  private loadWorkflow(id: string): void {
    this.workflowService.getWorkflow(id).subscribe((workflow) => {
      this.workflow = workflow;
      this.updateAutoQueryState(workflow.queryEnhancers);
    });
  }

  private updateAutoQueryState(enhancers: QueryEnhancer[]): void {
    const autoQuery = enhancers.find((qe) => qe.type === 'AUTO_QUERY');

    const isEnabled = !!autoQuery;
    this.autoQueryEnabled$.next(isEnabled);

    if (isEnabled && autoQuery) {
      this.autoQueryEnhancer = autoQuery;
      this.patchAutoQueryForm(autoQuery);
    }
  }

  private patchAutoQueryForm(autoQuery: QueryEnhancer): void {
    this.autoQueryForm.patchValue({
      maxQueries: autoQuery.maxQueries ?? 1,
      guidance: autoQuery.guidance ?? '',
    });
  }

  onToggleAutoQuery(): void {
    this.autoQueryEnabled$.next(!this.autoQueryEnabled$.value);
  }

  onSaveAutoQuery(): void {
    if (this.autoQueryForm.invalid) return;

    this.queryEnhancerService
      .enableAutoQuery(this.autoQueryForm.value, this.workflowId)
      .subscribe({
        next: () => {},
        error: (err: Error) => {
          console.error('Error enabling auto-query:', err);
        },
      });
  }
}
