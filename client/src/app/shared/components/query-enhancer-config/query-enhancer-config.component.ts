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
import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';

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
  @Input() workflowId!: string;
  @Input() type!: string;
  @Input() recommended: boolean = false;
  @Input() title!: string;
  @Input() description!: string;
  @Input() guidanceEnabled: boolean = false;
  @Input() maxQueriesEnabled: boolean = false;

  configForm!: FormGroup;
  enabled$ = new BehaviorSubject<boolean>(false);

  constructor(
    private readonly fb: FormBuilder,
    private readonly qeService: QueryEnhancerService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    if (this.queryEnhancer) {
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

  // Toggle the enabled state and update via the service.
  toggleEnabled(): void {
    if (!this.queryEnhancer) {
      // If no QE exists yet, simply toggle the local BehaviorSubject.
      this.enabled$.next(!this.enabled$.getValue());
      return;
    }
    const newStatus = !this.queryEnhancer.isEnabled;
    this.qeService
      .toggleQueryEnhancer(
        this.queryEnhancer,
        this.workflowId,
        this.type,
        newStatus
      )
      .subscribe({
        next: (updatedQE: QueryEnhancer) => {
          this.queryEnhancer = updatedQE;
          this.enabled$.next(updatedQE.isEnabled);
        },
        error: (err: Error) => {
          console.error('Error toggling enable:', err);
        },
      });
  }

  // If a QE already exists, update it; otherwise, create it.
  submitForm(): void {
    if (this.configForm.invalid) return;
    const formData = this.configForm.value;
    if (this.queryEnhancer) {
      const updatedQE = { ...this.queryEnhancer, ...formData };
      this.qeService
        .updateQueryEnhancer(updatedQE, this.workflowId, this.type)
        .subscribe({
          next: (newQE: QueryEnhancer) => {
            this.queryEnhancer = newQE;
            this.enabled$.next(newQE.isEnabled);
          },
          error: (err: Error) => {
            console.error('Error updating query enhancer:', err);
          },
        });
    } else {
      const newQE: QueryEnhancer = {
        ...formData,
        type: this.type,
        isEnabled: true,
      };
      this.qeService
        .enableQueryEnhancer(newQE, this.workflowId, this.type)
        .subscribe({
          next: (createdQE: QueryEnhancer) => {
            this.queryEnhancer = createdQE;
            this.enabled$.next(createdQE.isEnabled);
          },
          error: (err: Error) => {
            console.error('Error enabling query enhancer:', err);
          },
        });
    }
  }

  delete(): void {
    this.qeService.deleteQueryEnhancer(this.workflowId, this.type).subscribe({
      next: (success: boolean) => {
        if (success) {
          this.queryEnhancer = undefined;
          this.enabled$.next(false);
        }
      },
      error: (err: Error) => {
        console.error('Error deleting query enhancer:', err);
      },
    });
  }
}
