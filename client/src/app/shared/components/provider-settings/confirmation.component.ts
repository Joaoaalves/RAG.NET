import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideTriangleAlert } from '@ng-icons/lucide';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  standalone: true,
  providers: [provideIcons({ lucideTriangleAlert })],
  imports: [CommonModule, NgIcon],
})
export class ConfirmationComponent {
  @Input() title: string = 'Confirm Provider Change';
  @Input() message: string =
    'Switching providers may impact performance and cost. Proceed?';
  @Output() confirmEvent = new EventEmitter<void>();
  @Output() cancelEvent = new EventEmitter<void>();

  confirm() {
    this.confirmEvent.emit();
  }

  cancel() {
    this.cancelEvent.emit();
  }
}
