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
import {
  CreateVectorStorageRequest,
  SupportedVectorStorage,
} from 'src/app/models/vector-storage';

@Component({
  selector: 'app-qdrant-cloud-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './qdrant-cloud-form.component.html',
})
export class QdrantCloudFormComponent implements OnChanges {
  @Input() host?: string;
  @Input() apiKey?: string;

  @Output() submitForm = new EventEmitter<CreateVectorStorageRequest>();
  status = false;
  submitted = false;
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      host: ['', Validators.required],
      apiKey: ['', Validators.required],
    });

    this.status = this.apiKey != undefined;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['host'] || changes['apiKey']) {
      this.form.patchValue({
        host: this.host ?? '',
        apiKey: this.apiKey ?? '',
      });
    }
  }

  submit() {
    this.submitted = true;

    if (this.form.invalid) return;

    const { host, apiKey } = this.form.value;
    this.submitForm.emit({
      host,
      apiKey,
      provider: SupportedVectorStorage.QDRANT,
    });
  }
}
