import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroTrash } from '@ng-icons/heroicons/outline';
import { WorkflowService } from 'src/app/services/workflow.service';
import { AlertComponent } from '../alert/alert.component';
import { Workflow } from 'src/app/models/workflow';

@Component({
  selector: 'app-workflow-card',
  imports: [CommonModule, NgIcon, AlertComponent],
  providers: [provideIcons({ heroTrash })],
  templateUrl: './workflow-card.component.html',
  host: {
    style: 'display: block',
  },
  standalone: true,
})
export class WorkflowCardComponent {
  constructor(private workflowService: WorkflowService) {}

  @Input() workflow!: Workflow;
  @Output() workflowDeleted = new EventEmitter<string>();

  deleteWorkflow() {
    return this.workflowService
      .deleteWorkflow(this.workflow.id)
      .subscribe((success) => {
        if (success) {
          this.workflowDeleted.emit(this.workflow.id);
        }
      });
  }
}
