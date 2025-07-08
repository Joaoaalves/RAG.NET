import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-serverless-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './serverless-form.component.html',
})
export class ServerlessFormComponent {
  clouds = ['gcp', 'aws', 'azure'];

  regionsMap: Record<string, string[]> = {
    aws: ['us-east-1', 'us-west-2', 'eu-west-1'],
    gcp: ['us-central-1', 'europe-west4'],
    azure: ['eastus2'],
  };

  selectedCloud: number = 0;
  selectedRegion = '';
  apiKey = '';

  get availableRegions(): string[] {
    const cloudKey = this.clouds[this.selectedCloud];
    return this.regionsMap[cloudKey] ?? [];
  }

  @Output() submitForm = new EventEmitter<{
    cloud: number;
    region: string;
    apiKey: string;
  }>();

  submit() {
    this.submitForm.emit({
      cloud: this.selectedCloud,
      region: this.selectedRegion,
      apiKey: this.apiKey,
    });
  }
}
