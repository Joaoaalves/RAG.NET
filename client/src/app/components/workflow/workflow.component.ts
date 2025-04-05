import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Observable, tap, throwError } from 'rxjs';

// Models
import { QueryEnhancer } from '../../models/query-enhancer';
import { Workflow } from 'src/app/models/workflow';

// Components
import { QueryEnhancerConfigComponent } from 'src/app/shared/components/query-enhancer-config/query-enhancer-config.component';

// Services
import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';
import { WorkflowService } from '../../services/workflow.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, QueryEnhancerConfigComponent],
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

  onSaveAutoQuery(formData: any): Observable<QueryEnhancer> {
    if (!formData || !this.workflowId) {
      return throwError(
        () => new Error('Form data or Workflow ID is undefined')
      );
    }

    return this.queryEnhancerService
      .enableAutoQuery(formData, this.workflowId)
      .pipe(
        tap((queryEnhancer) => {
          if (queryEnhancer) {
            this.autoQueryEnhancer = queryEnhancer;
          }
        })
      );
  }

  onSaveHyde(formData: any): Observable<QueryEnhancer> {
    if (!formData || !this.workflowId) {
      return throwError(
        () => new Error('Form data or Workflow ID is undefined')
      );
    }

    return this.queryEnhancerService.enableHyde(formData, this.workflowId).pipe(
      tap((queryEnhancer) => {
        if (queryEnhancer) {
          this.hydeEnhancer = queryEnhancer;
        }
      })
    );
  }

  onUpdateAutoQuery(formData: any): Observable<QueryEnhancer> {
    if (!formData || !this.workflowId || !this.autoQueryEnhancer) {
      return throwError(
        () => new Error('Form data or Workflow ID is undefined')
      );
    }

    return this.queryEnhancerService
      .updateAutoQuery(formData, this.workflowId)
      .pipe(
        tap((updated) => {
          this.autoQueryEnhancer = updated;
        })
      );
  }

  onUpdateHyde(formData: any): Observable<QueryEnhancer> {
    if (!formData || !this.workflowId || !this.hydeEnhancer) {
      return throwError(
        () => new Error('Form data or Workflow ID is undefined')
      );
    }

    return this.queryEnhancerService.updateHyde(formData, this.workflowId).pipe(
      tap((updated) => {
        this.hydeEnhancer = updated;
      })
    );
  }

  onDeleteAutoQuery(): Observable<boolean> {
    if (!this.workflowId) {
      return throwError(() => new Error('Workflow ID is undefined'));
    }

    return this.queryEnhancerService.deleteAutoQuery(this.workflowId).pipe(
      tap((response) => {
        if (response) {
          this.autoQueryEnhancer = undefined;
        }
      })
    );
  }

  onDeleteHyde(): Observable<boolean> {
    if (!this.workflowId) {
      return throwError(() => new Error('Workflow ID is undefined'));
    }

    return this.queryEnhancerService.deleteHyde(this.workflowId).pipe(
      tap((response) => {
        if (response) {
          this.hydeEnhancer = undefined;
        }
      })
    );
  }
}
