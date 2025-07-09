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
  selector: 'app-byoc-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './byoc-form.component.html',
})
export class BYOCFormComponent implements OnChanges {
  @Input() apiKey: string | undefined;
  @Input() environment: string | undefined;
  status = false;
  @Output() submitForm = new EventEmitter<{
    environment: string;
    apiKey: string;
  }>();

  form: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      apiKey: ['', Validators.required],
      environment: ['', Validators.required],
    });

    this.status = this.apiKey != undefined;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['apiKey'] || changes['environment']) {
      this.form.patchValue({
        apiKey: this.apiKey ?? '',
        environment: this.environment ?? '',
      });
    }
  }

  submit() {
    this.submitted = true;
    if (this.form.invalid) return;

    this.submitForm.emit(this.form.value);
  }
}
