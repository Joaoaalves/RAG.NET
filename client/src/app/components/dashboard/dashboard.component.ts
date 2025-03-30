import { Component, OnInit } from '@angular/core';
import { WorkflowService } from '../../services/workflow.service';
import { Workflow } from '../../models/workflow';
import { WorkflowCardComponent } from 'src/app/shared/components/workflowCard/workflow-card.component';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from 'src/app/shared/sidebar/sidebar.component';

@Component({
  selector: 'app-dashboard',
  imports: [WorkflowCardComponent, SidebarComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  standalone: true,
})
export class DashboardComponent implements OnInit {
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
