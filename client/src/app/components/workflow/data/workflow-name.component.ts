import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideCheck, lucidePencil, lucideX } from '@ng-icons/lucide';

@Component({
  selector: 'app-workflow-name',
  standalone: true,
  host: {
    display: 'contents',
  },
  imports: [CommonModule, FormsModule, NgIcon],
  providers: [
    provideIcons({
      lucideX,
      lucideCheck,
      lucidePencil,
    }),
  ],
  templateUrl: './workflow-name.component.html',
})
export class WorkflowNameComponent {
  @ViewChild('workflowName') nameInputRef?: ElementRef<HTMLInputElement>;
  @Input() name: string = '';
  @Output() saveEvent = new EventEmitter<{ name: string }>();
  @Output() cancelEvent = new EventEmitter<void>();

  isEditing: boolean = false;
  editedName: string = '';

  startEditing() {
    this.editedName = this.name;
    this.isEditing = true;

    setTimeout(() => {
      this.nameInputRef?.nativeElement.focus();
    });
  }

  save() {
    this.saveEvent.emit({ name: this.editedName });
    this.isEditing = false;
  }

  cancel() {
    this.cancelEvent.emit();
    this.isEditing = false;
  }
}
