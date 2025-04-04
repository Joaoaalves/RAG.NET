import { QueryEnhancer } from './../../models/query-enhancer';
import { HlmSwitchComponent } from './../../../../libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { Workflow } from 'src/app/models/workflow';
import { WorkflowService } from './../../services/workflow.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';
import { QueryEnhancerConfigComponent } from 'src/app/shared/components/query-enhancer-config/query-enhancer-config.component';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    HlmSwitchComponent,
    ReactiveFormsModule,
    InputComponent,
    TextAreaComponent,
    QueryEnhancerConfigComponent,
  ],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;
  autoQueryEnhancer?: QueryEnhancer;
  hydeEnhancer?: QueryEnhancer;

  constructor(
    private readonly workflowService: WorkflowService,
    private readonly route: ActivatedRoute,
    private readonly queryEnhancerService: QueryEnhancerService
  ) {}

  ngOnInit(): void {
    this.loadWorkflowFromRoute();
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
      this.autoQueryEnhancer = workflow.queryEnhancers.find(
        (qe) => qe.type === 'AUTO_QUERY'
      );
      this.hydeEnhancer = workflow.queryEnhancers.find(
        (qe) => qe.type === 'HYPOTHETICAL_DOCUMENT_EMBEDDING'
      );
    });
  }

  onSaveAutoQuery(formData: any, workflowId: string): void {
    if (!formData) return;

    this.queryEnhancerService.enableAutoQuery(formData, workflowId).subscribe({
      next: () => {},
      error: (err: Error) => {
        console.error('Error enabling auto-query:', err);
      },
    });
  }

  onSaveHyde(formData: any, workflowId: string): void {
    if (!formData) return;

    this.queryEnhancerService.enableHyde(formData, workflowId).subscribe({
      next: () => {},
      error: (err: Error) => {
        console.error('Error enabling hyde:', err);
      },
    });
  }

  onDeleteAutoQuery(): void {
    if (!this.workflowId) return;

    this.queryEnhancerService
      .deleteAutoQuery(this.workflowId)
      .subscribe((response) => {
        if (response) {
          this.autoQueryEnhancer = undefined;
        }
      });
  }
  onDeleteHyde(): void {
    if (!this.workflowId) return;

    this.queryEnhancerService
      .deleteHyde(this.workflowId)
      .subscribe((response) => {
        if (response) {
          this.hydeEnhancer = undefined;
        }
      });
  }
}
