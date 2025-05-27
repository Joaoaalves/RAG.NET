import { Component, Input, Output, EventEmitter } from '@angular/core';
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
  @Input() name: string = '';
  @Output() saveEvent = new EventEmitter<string>();
  @Output() cancelEvent = new EventEmitter<void>();

  isEditing: boolean = false;
  editedName: string = '';

  startEditing() {
    this.editedName = this.name;
    this.isEditing = true;
  }

  save() {
    this.saveEvent.emit(this.editedName);
    this.isEditing = false;
  }

  cancel() {
    this.cancelEvent.emit();
    this.isEditing = false;
  }
}
