import {
  Component,
  EventEmitter,
  Input,
  Output,
  HostListener,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { toast } from 'ngx-sonner';
import { Router } from '@angular/router';
// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroTrash, heroClipboard } from '@ng-icons/heroicons/outline';

// Components
import { AlertComponent } from '../alert/alert.component';

// Models
import { Workflow } from 'src/app/models/workflow';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';

@Component({
  selector: 'app-workflow-card',
  imports: [CommonModule, NgIcon, AlertComponent],
  providers: [provideIcons({ heroTrash, heroClipboard })],
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

  @HostListener('mousemove', ['$event'])
  handleMouseMove(event: MouseEvent) {
    const element = event.currentTarget as HTMLElement;
    const rect = element.getBoundingClientRect();
    const x = (event.clientX - rect.left) / rect.width;
    const rotateX = (event.clientY - rect.top) / rect.height - 0.5;
    const rotateY = x - 0.5;
    element.style.transform = `perspective(1000px) rotateX(${
      rotateX * -5
    }deg) rotateY(${rotateY * 5}deg)`;
    element.style.setProperty('--mouse-x', `${(x - 0.5) * 200}%`);
  }

  @HostListener('mouseleave', ['$event'])
  handleMouseLeave(event: MouseEvent) {
    const element = event.currentTarget as HTMLElement;
    element.style.setProperty('--mouse-x', '0%');
    element.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg)';
  }
}
