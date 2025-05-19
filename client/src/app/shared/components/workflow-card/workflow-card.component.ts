import {
  Component,
  EventEmitter,
  Input,
  Output,
  HostListener,
  ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { toast } from 'ngx-sonner';
import { Router } from '@angular/router';
// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import { heroTrash, heroClipboard } from '@ng-icons/heroicons/outline';
import { lucidePlus, lucideSearch } from '@ng-icons/lucide';

// Components
import { AlertComponent } from '../alert/alert.component';

// Models
import { Workflow } from 'src/app/models/workflow';

// Services
import { WorkflowService } from 'src/app/services/workflow.service';

@Component({
  selector: 'app-workflow-card',
  standalone: true,
  imports: [CommonModule, NgIcon, AlertComponent],
  providers: [
    provideIcons({ heroTrash, heroClipboard, lucidePlus, lucideSearch }),
  ],
  templateUrl: './workflow-card.component.html',
  host: {
    class:
      'relative group overflow-hidden cursor-pointer rounded-xl transition-all duration-200',
  },
})
export class WorkflowCardComponent {
  @Input() workflow!: Workflow;
  @Output() workflowDeleted = new EventEmitter<string>();

  constructor(
    private workflowService: WorkflowService,
    private router: Router,
    private el: ElementRef<HTMLElement>
  ) {}

  handleNavigateWorkflow() {
    this.router.navigate([`/dashboard/workflows/${this.workflow.id}`]);
  }

  deleteWorkflow() {
    this.workflowService
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
      .then(() => toast('Workflow API Key copied to clipboard!'))
      .catch((err) => toast('Failed to copy text: ' + err));
  }

  @HostListener('mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    const rect = this.el.nativeElement.getBoundingClientRect();
    const x = event.clientX - rect.left + 'px';
    const y = event.clientY - rect.top + 'px';
    this.el.nativeElement.style.setProperty('--mouse-x', x);
    this.el.nativeElement.style.setProperty('--mouse-y', y);
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.el.nativeElement.style.setProperty('--mouse-x', '-100%');
    this.el.nativeElement.style.setProperty('--mouse-y', '-100%');
  }
}
