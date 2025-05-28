import { CommonModule } from '@angular/common';
import { Component, Input, SimpleChanges } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { toast } from 'ngx-sonner';

// Models
import { Filter, FilterStrategy } from 'src/app/models/filter';

// Components
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { InputComponent } from '../input/input.component';

// Services
import { FilterService } from 'src/app/services/filter.service';
import { NumberCounterInputComponent } from '../number-counter-input/number-counter-input.component';

@Component({
  selector: 'app-filter-config',
  standalone: true,
  templateUrl: './filter-config.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HlmSwitchComponent,
    NumberCounterInputComponent,
    InputComponent,
  ],
})
export class FilterConfigComponent {
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() filter?: Filter;
  @Input() workflowId!: string;
  @Input() strategy!: string | FilterStrategy;
  filterStrategies: { label: string; value: number }[] = [
    {
      label: 'Relevant Segment Extraction',
      value: 1,
    },
  ];
  configForm!: FormGroup;
  enabled$ = new BehaviorSubject<boolean>(false);

  constructor(
    private readonly fb: FormBuilder,
    private readonly filterService: FilterService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.filter) return;

    if (changes['filter'] && changes['filter'].currentValue) {
      this.enabled$.next(!!this.filter?.isEnabled);
      this.patchForm(changes['filter'].currentValue);
    }
  }

  private patchForm(filter: Filter): void {
    if (!this.configForm) return;
    this.configForm.patchValue({
      maxItems: filter.maxItems,
      isEnabled: filter.isEnabled,
      strategy: filter.strategy,
    });
  }

  initializeForm(): void {
    this.configForm = this.fb.group({
      maxItems: [
        this.filter?.maxItems ?? 0,
        [Validators.required, Validators.min(1), Validators.max(100)],
      ],
      isEnabled: [this.filter?.isEnabled ?? false],
      strategy: [this.mapStrategyFromStringToEnum(this.strategy)],
    });
  }

  toggleEnabled(): void {
    if (!this.filter) {
      this.enabled$.next(true);
      this.initializeForm();
      this.configForm.patchValue({
        isEnabled: true,
      });
      return;
    }

    this.filterService
      .toggleFilter(
        this.filter!,
        this.workflowId,
        this.strategy,
        !this.filter.isEnabled
      )
      .subscribe((filter) => {
        this.filter = filter;
        this.enabled$.next(filter.isEnabled);
        toast.success(
          `${this.title} ${this.filter.isEnabled ? 'enabled' : 'disabled'}`
        );
      });
  }

  updateFilter(updatedFilter: Filter): void {
    this.filterService
      .updateFilter(updatedFilter, this.workflowId, this.strategy)
      .subscribe((filter) => {
        this.filter = filter;
        toast.success(`${this.title} updated successfully`);
      });
  }

  createFilter(filterData: Filter): void {
    this.filterService
      .enableFilter(filterData, this.workflowId, this.strategy)
      .subscribe((filter) => {
        this.filter = filter;
        this.enabled$.next(true);
        toast.success(`${this.title} enabled successfully`);
      });
  }

  submitForm(): void {
    if (this.configForm.invalid) {
      this.displayValidationErrors();
      return;
    }

    const filterData = {
      ...this.filter,
      ...this.configForm.value,
    };

    if (!this.filter) {
      this.createFilter(filterData);
    } else {
      this.updateFilter(filterData);
    }
  }

  private displayValidationErrors(): void {
    Object.keys(this.configForm.controls).forEach((field) => {
      const control = this.configForm.get(field);
      if (control?.invalid) {
        control.markAsTouched({ onlySelf: true });
      }
    });

    if (this.configForm.get('maxItems')?.invalid) {
      const maxItemsControl = this.configForm.get('maxItems');
      if (maxItemsControl?.hasError('required')) {
        toast.error('Max items is required');

        return;
      }
      if (maxItemsControl?.hasError('min')) {
        toast.error('Max items must be a positive number');

        return;
      }
      if (maxItemsControl?.hasError('max')) {
        toast.error('Max items must be less than 100');
        return;
      }
    }

    if (this.configForm.invalid) {
      toast.error('Please fill in all required fields');
      return;
    }
  }

  mapStrategyFromStringToEnum(
    strategy: string | FilterStrategy
  ): FilterStrategy {
    switch (strategy) {
      case 'rse':
        return FilterStrategy.RSE;
      default:
        return FilterStrategy.RSE;
    }
  }
}
