import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Workflow, WorkflowUpdateRequest } from 'src/app/models/workflow';
import { QueryEnhancer } from 'src/app/models/query-enhancer';
import { CommonModule } from '@angular/common';
import { Filter } from 'src/app/models/filter';
import { toast } from 'ngx-sonner';

// Components
import { QueryEnhancerConfigComponent } from 'src/app/shared/components/query-enhancer-config/query-enhancer-config.component';
import { CallbackUrlsFormComponent } from 'src/app/shared/components/callback-urls-form/callback-urls-form.component';
import { ProviderSettingsComponent } from 'src/app/shared/components/provider-settings/provider-settings.component';
import { WorkflowNameComponent } from './data/workflow-name.component';
import { WorkflowDescriptionComponent } from './data/workflow-description.component';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { FilterConfigComponent } from 'src/app/shared/components/filter-config/filter-config.component';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';
import { ProviderModel } from 'src/app/models/provider';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    WorkflowNameComponent,
    WorkflowDescriptionComponent,
    QueryEnhancerConfigComponent,
    ProviderSettingsComponent,
    FilterConfigComponent,
    HlmToasterComponent,
    CallbackUrlsFormComponent,
    HlmSwitchComponent,
  ],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent {
  workflowId!: string;
  workflow!: Workflow;

  convProvider: ProviderModel = {
    providerId: -1,
    providerName: '',
    model: '',
  };

  embProvider: ProviderModel = {
    providerId: -1,
    providerName: '',
    model: '',
  };

  autoQueryEnhancer?: QueryEnhancer;
  hydeEnhancer?: QueryEnhancer;
  filter?: Filter;

  constructor(
    private workflowService: WorkflowService,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('workflowId');
      if (id) {
        this.workflowId = id;
        this.loadWorkflow(id);
      }
    });
  }

  get currentDate() {
    return new Date().toLocaleDateString();
  }

  toggleEnabled(event: Event) {
    event.stopPropagation();
    this.workflowService
      .toggleWorkflow(!this.workflow.isActive, this.workflow.id)
      .subscribe((w) => {
        this.workflow.isActive = w.isActive;
        toast('Workflow updated!', {
          description: `Workflow ${
            this.workflow.isActive ? 'enabled' : 'disabled'
          } successfully!`,
        });
      });
  }

  updateWorkflow(data: WorkflowUpdateRequest) {
    this.workflowService
      .updateWorkflow(data, this.workflow.id)
      .subscribe((w) => {
        this.workflow = { ...this.workflow, ...w };
        this.loadProvidersIds();
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
    if (!this.workflow) return;

    this.embProvider = {
      providerName: this.workflow.embeddingProvider.providerName,
      providerId: this.workflow.embeddingProvider.providerId,
      model: this.workflow.embeddingProvider.model,
    };

    this.convProvider = {
      providerName: this.workflow.conversationProvider.providerName,
      providerId: this.workflow.conversationProvider.providerId,
      model: this.workflow.conversationProvider.model,
    };
  }
}
