import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pods-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './pods-form.component.html',
  standalone: true,
})
export class PodsFormComponent {
  podTypes = ['P1', 'P2', 'S1'];
  podSizes = ['X1', 'X2', 'X3', 'X4'];

  environment = '';
  podType = this.podTypes[0];
  podSize = this.podSizes[0];
  pods = 1;
  apiKey = '';

  @Output() submitForm = new EventEmitter<{
    environment: string;
    podType: string;
    podSize: string;
    pods: number;
    apiKey: string;
  }>();

  submit() {
    this.submitForm.emit({
      environment: this.environment,
      podType: this.podType,
      podSize: this.podSize,
      pods: this.pods,
      apiKey: this.apiKey,
    });
  }
}
