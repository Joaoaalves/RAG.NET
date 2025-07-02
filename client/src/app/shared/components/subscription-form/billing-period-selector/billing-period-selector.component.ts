import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output,
  Signal,
  signal,
} from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { BillingPeriod } from 'src/app/models/subscription';

export interface BillingOption {
  period: BillingPeriod;
  label: string;
  price: number;
  discount: number;
  description: string;
}

@Component({
  selector: 'app-billing-period-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './billing-period-selector.component.html',
})
export class BillingPeriodSelectorComponent {
  @Input() isDisabled: boolean = false;
  @Input() selected: BillingPeriod | null = null;
  @Output() selectedChange = new EventEmitter<BillingPeriod>();
  @Input() billingOptions: BillingOption[] = [];

  onSelect(period: BillingPeriod) {
    if (!this.isDisabled) {
      this.selectedChange.emit(period);
    }
  }
}
