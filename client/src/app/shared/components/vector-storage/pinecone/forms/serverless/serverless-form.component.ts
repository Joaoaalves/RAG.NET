import {
  CreateVectorStorageRequest,
  SupportedVectorStorage,
} from './../../../../../../models/vector-storage';
import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-serverless-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './serverless-form.component.html',
})
export class ServerlessFormComponent implements OnChanges {
  @Input() cloud?: number;
  @Input() region?: string;
  @Input() apiKey?: string;
  status = false;

  @Output() submitForm = new EventEmitter<CreateVectorStorageRequest>();

  form: FormGroup;

  clouds = ['gcp', 'aws', 'azure'];

  regionsMap: Record<string, string[]> = {
    aws: ['us-east-1', 'us-west-2', 'eu-west-1'],
    gcp: ['us-central-1', 'europe-west4'],
    azure: ['eastus2'],
  };

  submitted = false;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      cloud: [0, Validators.required],
      region: ['', Validators.required],
      apiKey: ['', Validators.required],
    });

    this.status = this.apiKey != undefined;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['cloud'] || changes['region'] || changes['apiKey']) {
      this.form.patchValue({
        cloud: this.cloud ?? 0,
        region: this.region ?? '',
        apiKey: this.apiKey ?? '',
      });
    }
  }

  get selectedCloud(): number {
    return this.form.get('cloud')?.value;
  }

  get availableRegions(): string[] {
    const cloudKey = this.clouds[this.selectedCloud];
    return this.regionsMap[cloudKey] ?? [];
  }

  selectCloud(index: number) {
    this.form.patchValue({ cloud: index, region: '' });
  }

  selectRegion(region: string) {
    this.form.patchValue({ region });
  }

  submit() {
    this.submitted = true;
    if (this.form.invalid) return;

    const { cloud, region, apiKey } = this.form.value;
    this.submitForm.emit({
      cloud,
      region,
      apiKey,
      provider: SupportedVectorStorage.PINECONE,
    });
  }
}
