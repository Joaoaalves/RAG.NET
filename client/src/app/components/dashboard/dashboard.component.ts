import { Component, OnInit } from '@angular/core';
import { WorkflowService } from '../../services/workflow.service';
import { Workflow } from '../../models/workflow';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
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
}
