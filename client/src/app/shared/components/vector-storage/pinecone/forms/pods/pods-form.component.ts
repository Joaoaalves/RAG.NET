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
  selector: 'app-pods-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './pods-form.component.html',
})
export class PodsFormComponent implements OnChanges {
  @Input() apiKey: string | undefined;
  @Input() environment: string | undefined;
  @Input() podType: string | undefined;
  @Input() podSize: string | undefined;
  @Input() pods: number | undefined;
  status = false;
  @Output() submitForm = new EventEmitter<{
    environment: string;
    podType: string;
    podSize: string;
    pods: number;
    apiKey: string;
  }>();

  podTypes = ['P1', 'P2', 'S1'];
  podSizes = ['X1', 'X2', 'X3', 'X4'];

  form: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      apiKey: ['', Validators.required],
      environment: ['', Validators.required],
      podType: ['P1', Validators.required],
      podSize: ['X1', Validators.required],
      pods: [
        1,
        [Validators.required, Validators.min(1), Validators.max(10000)],
      ],
    });

    this.status = this.apiKey != undefined;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['apiKey'] ||
      changes['environment'] ||
      changes['podType'] ||
      changes['podSize'] ||
      changes['pods']
    ) {
      this.form.patchValue({
        apiKey: this.apiKey !== undefined ? this.apiKey : '',
        environment: this.environment !== undefined ? this.environment : '',
        podType: this.podType !== undefined ? this.podType : 'P1',
        podSize: this.podSize !== undefined ? this.podSize : 'X1',
        pods: this.pods !== undefined && this.pods > 0 ? this.pods : 1,
      });
    }
  }

  get podTypeValue(): string {
    return this.form.get('podType')?.value;
  }

  get podSizeValue(): string {
    return this.form.get('podSize')?.value;
  }

  selectPodType(type: string) {
    this.form.patchValue({ podType: type });
  }

  selectPodSize(size: string) {
    this.form.patchValue({ podSize: size });
  }

  submit() {
    this.submitted = true;
    if (this.form.invalid) return;

    this.submitForm.emit(this.form.value);
  }
}
