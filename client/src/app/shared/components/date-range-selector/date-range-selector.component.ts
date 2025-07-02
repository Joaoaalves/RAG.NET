import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-date-range-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './date-range-selector.component.html',
})
export class DateRangeSelectorComponent {
  @Input() start!: Date;
  @Input() end!: Date;

  @Output() rangeChange = new EventEmitter<{ start: Date; end: Date }>();

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
    this.rangeChange.emit({ start: newStart, end: this.end });
  }

  onEndChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input?.value) return;

    const newEnd = new Date(input.value);
    this.rangeChange.emit({ start: this.start, end: newEnd });
  }
}
