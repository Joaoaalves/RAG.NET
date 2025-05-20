import { Component, forwardRef, HostBinding, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ControlValueAccessor,
  FormsModule,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { UsageTooltipComponent } from '../usage-tooltip/usage-tooltip.component';

@Component({
  selector: 'app-slider-input',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    UsageTooltipComponent,
  ],
  templateUrl: './slider-input.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SliderInputComponent),
      multi: true,
    },
  ],
  styleUrls: ['./slider-input.component.css'],
})
export class SliderInputComponent implements ControlValueAccessor, OnInit {
  value = 0.7;
  min = 0.1;
  max = 1.0;
  step = 0.01;

  onChange = (_: number) => {};
  onTouched = () => {};

  ngOnInit() {
    this.writeValue(this.value);
  }

  writeValue(val: number): void {
    this.value =
      val != null ? Math.min(this.max, Math.max(this.min, val)) : this.value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onInput(event: Event) {
    const newVal = parseFloat((event.target as HTMLInputElement).value);
    this.value = newVal;
    this.onChange(this.value);
  }

  get fillPercent(): number {
    return ((this.value - this.min) / (this.max - this.min)) * 100;
  }

  getBackground(): string {
    return `
      linear-gradient(
        to right,
        #ec4899 0%,
        #ec4899 ${this.fillPercent}%,
        #1f2937 ${this.fillPercent}%,
        #1f2937 100%
      )
    `;
  }
}
