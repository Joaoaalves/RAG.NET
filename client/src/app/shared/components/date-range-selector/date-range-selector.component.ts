import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-date-range-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './date-range-selector.component.html',
})
export class DateRangeSelectorComponent implements OnInit {
  @Input() start!: Date;
  @Input() end!: Date;
  @Output() rangeChange = new EventEmitter<{ start: Date; end: Date }>();

  presets = [
    { label: 'Last 7 days', days: 7 },
    { label: 'Last 30 days', days: 30 },
    { label: 'Last 90 days', days: 90 },
  ];

  validationError: string = '';

  ngOnInit(): void {
    this.validate();
  }

  get startString(): string {
    return this.start?.toISOString().split('T')[0];
  }

  get endString(): string {
    return this.end?.toISOString().split('T')[0];
  }

  onStartChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input?.value) return;

    const newStart = new Date(input.value);
    this.emitRange(newStart, this.end);
  }

  onEndChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input?.value) return;

    const newEnd = new Date(input.value);
    this.emitRange(this.start, newEnd);
  }

  emitRange(start: Date, end: Date) {
    const isValid = this.validateRange(start, end);
    if (isValid) {
      this.rangeChange.emit({ start, end });
    }
  }

  validateRange(start: Date, end: Date): boolean {
    if (start > end) {
      this.validationError = 'Start date must be before end date.';
      return false;
    }

    const diffTime = Math.abs(end.getTime() - start.getTime());
    const diffMonths = diffTime / (1000 * 60 * 60 * 24 * 30.44);

    if (diffMonths > 6) {
      this.validationError = 'Range must be less than 6 months.';
      return false;
    }

    this.validationError = '';
    return true;
  }

  selectPreset(days: number) {
    const end = new Date();
    const start = new Date();
    start.setDate(start.getDate() - days);
    this.emitRange(start, end);
  }

  clear() {
    const today = new Date();

    if (today.getDate() === 1) {
      const lastMonth = new Date(today.getFullYear(), today.getMonth(), 0);
      const firstDayOfLastMonth = new Date(
        lastMonth.getFullYear(),
        lastMonth.getMonth(),
        1
      );
      this.emitRange(firstDayOfLastMonth, today);
    } else {
      const firstDayOfThisMonth = new Date(
        today.getFullYear(),
        today.getMonth(),
        1
      );
      this.emitRange(firstDayOfThisMonth, today);
    }

    this.validationError = '';
  }

  validate() {
    if (this.start && this.end) {
      this.validateRange(this.start, this.end);
    }
  }
}
