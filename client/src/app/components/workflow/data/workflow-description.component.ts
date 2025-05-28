import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-workflow-description',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './workflow-description.component.html',
})
export class WorkflowDescriptionComponent {
  @Input() description: string = '';
  @Output() saveEvent = new EventEmitter<{ description: string }>();
  @Output() cancelEvent = new EventEmitter<void>();

  isEditing: boolean = false;
  editedDescription: string = '';

  startEditing() {
    this.editedDescription = this.description;
    this.isEditing = true;
  }

  save() {
    this.saveEvent.emit({
      description: this.editedDescription,
    });
    this.isEditing = false;
  }

  cancel() {
    this.cancelEvent.emit();
    this.isEditing = false;
  }
}
