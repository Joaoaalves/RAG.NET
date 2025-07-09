import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-byoc-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './byoc-form.component.html',
  standalone: true,
})
export class BYOCFormComponent {
  environment = '';
  apiKey = '';

  @Output() submitForm = new EventEmitter<{
    environment: string;
    apiKey: string;
  }>();

  submit() {
    this.submitForm.emit({
      environment: this.environment,
      apiKey: this.apiKey,
    });
  }
}
