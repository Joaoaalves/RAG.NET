import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';
import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { CommonModule } from '@angular/common';

import { InputComponent } from 'src/app/shared/components/input/input.component';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';

import { QueryEnhancer } from 'src/app/models/query-enhancer';

@Component({
  selector: 'app-query-enhancer-config',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HlmSwitchComponent,
    InputComponent,
    TextAreaComponent,
  ],
  templateUrl: './query-enhancer-config.component.html',
})
export class QueryEnhancerConfigComponent implements OnInit, OnChanges {
  @Input() queryEnhancer?: QueryEnhancer;
  @Input() recommended: boolean = false;
  @Input() title!: string;
  @Input() description!: string;
  @Input() guidanceEnabled: boolean = false;
  @Input() maxQueriesEnabled: boolean = false;

  @Input() onDelete!: () => any;
  @Input() onSubmit!: (formData: any) => any;
  @Input() onUpdate!: (formData: any) => any;
  configForm!: FormGroup;

  enabled$ = new BehaviorSubject<boolean>(false);

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();

    if (this.queryEnhancer) {
      console.log('Query Enhancer:', this.queryEnhancer);
      this.enabled$.next(this.queryEnhancer.isEnabled);
      this.patchForm(this.queryEnhancer);
    }
    this.updateControlsState();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.configForm) {
      return;
    }

    if (changes['queryEnhancer'] && changes['queryEnhancer'].currentValue) {
      this.enabled$.next(!!this.queryEnhancer?.isEnabled);
      this.patchForm(changes['queryEnhancer'].currentValue);
    }
    if (changes['guidanceEnabled'] || changes['maxQueriesEnabled']) {
      this.updateControlsState();
    }
  }

  private initializeForm(): void {
    this.configForm = this.fb.group({
      maxQueries: [
        1,
        [Validators.required, Validators.min(1), Validators.max(10)],
      ],
      guidance: ['', Validators.required],
    });
  }

  private patchForm(queryEnhancer: QueryEnhancer): void {
    if (!this.configForm) return;

    this.configForm.patchValue({
      maxQueries: queryEnhancer.maxQueries ?? 1,
      guidance: queryEnhancer.guidance ?? '',
    });
  }

  private updateControlsState(): void {
    if (!this.configForm) return;

    const maxQueriesControl = this.configForm.get('maxQueries');
    const guidanceControl = this.configForm.get('guidance');

    if (maxQueriesControl) {
      if (!this.maxQueriesEnabled) {
        maxQueriesControl.disable();
      } else {
        maxQueriesControl.enable();
      }
    }
    if (guidanceControl) {
      if (!this.guidanceEnabled) {
        guidanceControl.disable();
      } else {
        guidanceControl.enable();
      }
    }
  }

  toggleEnabled(): void {
    if (!this.queryEnhancer) {
      var isEnabled = this.enabled$.getValue();
      this.enabled$.next(!isEnabled);
      return;
    }

    this.queryEnhancer.isEnabled = !this.queryEnhancer.isEnabled;
    this.onUpdate(this.queryEnhancer).subscribe({
      next: (updatedEnhancer: QueryEnhancer) => {
        this.queryEnhancer = updatedEnhancer;
        this.enabled$.next(updatedEnhancer.isEnabled);
      },
      error: (err: Error) => {
        console.error('Error toggling enable:', err);
      },
    });
  }

  submitForm(): void {
    if (this.configForm.invalid) return;
    this.onSubmit(this.configForm.value).subscribe({
      next: (newEnhancer: QueryEnhancer) => {
        this.queryEnhancer = newEnhancer;
        this.enabled$.next(newEnhancer.isEnabled);
      },
      error: (err: Error) => {
        console.error('Error submitting form:', err);
      },
    });
  }

  update(): void {
    if (this.configForm.invalid) return;
    this.onUpdate(this.configForm.value).subscribe({
      next: (updatedEnhancer: QueryEnhancer) => {
        this.queryEnhancer = updatedEnhancer;
      },
      error: (err: Error) => {},
    });
  }

  delete(): void {
    this.onDelete().subscribe({
      next: () => {
        this.enabled$.next(false);
      },
    });
  }
}
