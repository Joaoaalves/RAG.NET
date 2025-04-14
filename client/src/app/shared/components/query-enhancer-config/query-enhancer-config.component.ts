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
import { toast } from 'ngx-sonner';

// Components
import { InputComponent } from 'src/app/shared/components/input/input.component';
import { TextAreaComponent } from 'src/app/shared/components/text-area/text-area.component';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { HlmToasterComponent } from './../../../../../libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';

// Models
import { QueryEnhancer } from 'src/app/models/query-enhancer';
import { QueryEnhancerService } from 'src/app/services/query-enhancer.service';

@Component({
  selector: 'app-query-enhancer-config',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HlmSwitchComponent,
    HlmToasterComponent,
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
      guidance: ['', [Validators.required, Validators.minLength(20)]],
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
          toast.success(
            `${this.title} ${
              updatedQE.isEnabled ? 'enabled' : 'disabled'
            } successfully.`
          );
        },
        error: (err: Error) => {
          toast.error('Error toggling query enhancer', {
            description: 'An error occurred while toggling the query enhancer.',
          });
        },
      });
  }

  // If a QE already exists, update it; otherwise, create it.
  submitForm(): void {
    if (this.configForm.invalid) {
      if (!this.displayValidationErrors()) {
        return;
      }
    }

    const formData = this.configForm.value;

    if (this.queryEnhancer) {
      const updatedQE = { ...this.queryEnhancer, ...formData };
      this.qeService
        .updateQueryEnhancer(updatedQE, this.workflowId, this.type)
        .subscribe({
          next: (newQE: QueryEnhancer) => {
            this.queryEnhancer = newQE;
            this.enabled$.next(newQE.isEnabled);
            toast.success('Query Enhancer updated successfully', {
              description: 'The query enhancer has been updated.',
            });
          },
          error: (err: Error) => {
            toast.error('Error updating query enhancer', {
              description:
                'An error occurred while updating the query enhancer.',
            });
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
            toast.success('Query Enhancer created successfully', {
              description: 'The query enhancer has been created.',
            });
          },
          error: (err: Error) => {
            toast.error('Error creating query enhancer', {
              description:
                'An error occurred while creating the query enhancer.',
            });
          },
        });
    }
  }

  delete(): void {
    this.qeService.deleteQueryEnhancer(this.workflowId, this.type).subscribe({
      next: (success: boolean) => {
        if (success) {
          toast.success('Query Enhancer deleted successfully', {
            description: 'The query enhancer has been deleted.',
          });
          this.queryEnhancer = undefined;
          this.enabled$.next(false);
        }
      },
      error: (err: Error) => {
        toast.error('Error deleting query enhancer', {
          description: 'An error occurred while deleting the query enhancer.',
        });
      },
    });
  }

  get guidanceControl() {
    return this.configForm.get('guidance');
  }

  get maxQueriesControl() {
    return this.configForm.get('maxQueries');
  }

  private displayValidationErrors(): boolean {
    let isValid = true;

    if (this.guidanceControl && this.guidanceControl.invalid) {
      if (this.guidanceControl.hasError('required')) {
        toast.error('Guidance is required', {
          description: 'Please provide guidance for the query enhancer.',
        });
        isValid = false;
      } else if (this.guidanceControl.hasError('minlength')) {
        toast.error('Guidance is too short', {
          description: 'Guidance must be at least 20 characters long.',
        });
        isValid = false;
      }
    }

    if (this.maxQueriesControl && this.maxQueriesControl.invalid) {
      if (this.maxQueriesControl.hasError('required')) {
        toast.error('Max Queries is required', {
          description: 'Please provide a value for max queries.',
        });
        isValid = false;
      } else if (this.maxQueriesControl.hasError('min')) {
        toast.error('Max Queries is too low', {
          description: 'Max queries must be at least 1.',
        });
        isValid = false;
      } else if (this.maxQueriesControl.hasError('max')) {
        toast.error('Max Queries is too high', {
          description: 'Max queries cannot exceed 10.',
        });
        isValid = false;
      }
    }

    return isValid;
  }
}
