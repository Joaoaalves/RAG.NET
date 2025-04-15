import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toast } from 'ngx-sonner';
import { Router } from '@angular/router';
// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  heroTrash,
  heroClipboard,
  heroCog8Tooth,
} from '@ng-icons/heroicons/outline';

// Components
import { AlertComponent } from '../alert/alert.component';

// Models
import { Workflow } from 'src/app/models/workflow';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';

@Component({
  selector: 'app-workflow-card',
  imports: [CommonModule, NgIcon, AlertComponent],
  providers: [provideIcons({ heroTrash, heroClipboard, heroCog8Tooth })],
  templateUrl: './workflow-card.component.html',
  host: {
    style: 'display: block',
  },
  standalone: true,
})
export class WorkflowCardComponent {
  constructor(
    private workflowService: WorkflowService,
    private route: Router
  ) {}

  @Input() workflow!: Workflow;
  @Output() workflowDeleted = new EventEmitter<string>();

  handleNavigateWorkflow() {
    return this.route.navigate([`/dashboard/workflows/${this.workflow.id}`]);
  }

  handleNavigateQuery() {
    return this.route.navigate([
      `/dashboard/workflows/${this.workflow.id}/query`,
    ]);
  }

  deleteWorkflow() {
    return this.workflowService
      .deleteWorkflow(this.workflow.id)
      .subscribe((success) => {
        if (success) {
          this.workflowDeleted.emit(this.workflow.id);
        }
      });
  }

  copyToClipboard() {
    const textToCopy = this.workflow.apiKey;
    navigator.clipboard
      .writeText(textToCopy)
      .then(() => {
        toast('Workflow API Key copied to clipboard!');
      })
      .catch((err) => {
        toast('Failed to copy text: ' + err);
      });
  }
}
