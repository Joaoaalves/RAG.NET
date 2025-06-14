import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toast } from 'ngx-sonner';

// Components
import { WorkflowCardComponent } from 'src/app/shared/components/workflow-card/workflow-card.component';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';

// Models
import { Workflow } from 'src/app/models/workflow';

// Services
import { WorkflowService } from '../../services/workflow.service';
import { WorkflowNavBarComponent } from './workflow-nav-bar.component';

@Component({
  selector: 'app-dashboard',
  imports: [
    WorkflowCardComponent,
    WorkflowNavBarComponent,
    CommonModule,
    HlmToasterComponent,
  ],
  templateUrl: './workflows.component.html',
  standalone: true,
})
export class WorkflowsComponent implements OnInit {
  constructor(private workflowService: WorkflowService) {}

  ngOnInit(): void {
    this.loadWorkflows();
  }

  workflows: Workflow[] = [];
  filteredWorkflows: Workflow[] = [];

  loadWorkflows(): void {
    this.workflowService.getWorkflows().subscribe((response) => {
      this.workflows = response;
      this.filteredWorkflows = this.workflows;
    });
  }

  filterWorkflows(query: string) {
    this.filteredWorkflows = this.workflows;

    if (query.length > 0) {
      query = query.toLowerCase();
      this.filteredWorkflows = this.workflows.filter(
        (w) =>
          w.name.toLowerCase().includes(query) ||
          w.description.toLowerCase().includes(query)
      );
    }

    return this.filteredWorkflows;
  }

  removeWorkflow(workflowId: string) {
    const workflow = this.workflows.filter((w) => w.id === workflowId)[0];
    if (workflow) {
      this.workflows = this.workflows.filter((w) => w.id !== workflow.id);
      this.filteredWorkflows = this.filteredWorkflows.filter(
        (w) => w.id !== workflow.id
      );

      toast('Workflow deleted!', {
        description: `'${workflow.name}' was successfully deleted!`,
      });
      return;
    }

    toast('An error occurred!', {
      description: 'The workflow with provided ID was not found!',
    });
  }
}
