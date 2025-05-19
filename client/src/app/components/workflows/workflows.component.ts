import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

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

  loadWorkflows(): void {
    this.workflowService.getWorkflows().subscribe((response) => {
      this.workflows = response;
    });
  }

  removeWorkflow(workflowId: string) {
    this.workflows = this.workflows.filter((w) => w.id !== workflowId);
  }
}
