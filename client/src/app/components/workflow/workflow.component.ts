import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Workflow } from 'src/app/models/workflow';
import { QueryEnhancer } from 'src/app/models/query-enhancer';
import { CommonModule } from '@angular/common';

// Components
import { QueryEnhancerConfigComponent } from 'src/app/shared/components/query-enhancer-config/query-enhancer-config.component';
import { CallbackUrlsFormComponent } from 'src/app/shared/components/callback-urls-form/callback-urls-form.component';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';
import { FilterConfigComponent } from 'src/app/shared/components/filter-config/filter-config.component';
import { Filter } from 'src/app/models/filter';
import { ProviderSettingsComponent } from 'src/app/shared/components/provider-settings/provider-settings.component';
import { WorkflowNameComponent } from './data/workflow-name.component';
import { WorkflowDescriptionComponent } from './data/workflow-description.component';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    WorkflowNameComponent,
    WorkflowDescriptionComponent,
    QueryEnhancerConfigComponent,
    ProviderSettingsComponent,
    FilterConfigComponent,
    CallbackUrlsFormComponent,
  ],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent implements OnInit {
  workflowId!: string;
  workflow!: Workflow;

  convProvider: { provider: number; model: string } = {
    provider: -1,
    model: '',
  };
  embProvider: { provider: number; model: string } = {
    provider: -1,
    model: '',
  };

  autoQueryEnhancer?: QueryEnhancer;
  hydeEnhancer?: QueryEnhancer;
  filter?: Filter;

  constructor(
    private readonly workflowService: WorkflowService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadWorkflowFromRoute();
  }

  get currentDate() {
    return new Date().toLocaleDateString();
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
      this.filter = workflow.filter;

      this.loadProvidersIds();
    });
  }

  private loadProvidersIds() {
    if (this.workflow) {
      this.embProvider.provider =
        this.workflow.embeddingProvider.provider === 'OPENAI' ? 0 : 1;

      this.embProvider.model = this.workflow.embeddingProvider.model;

      this.convProvider.provider =
        this.workflow.conversationProvider.provider === 'OPENAI' ? 0 : 1;
      this.convProvider.model = this.workflow.conversationProvider.model;
    }
  }
}
