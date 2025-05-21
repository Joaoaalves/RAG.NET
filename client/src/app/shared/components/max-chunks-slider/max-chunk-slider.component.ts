import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { UsageTooltipComponent } from '../usage-tooltip/usage-tooltip.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-max-chunk-slider',
  templateUrl: './max-chunk-slider.component.html',
  styleUrls: ['./max-chunk-slider.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MaxChunkSliderComponent),
      multi: true,
    },
  ],
  imports: [UsageTooltipComponent, CommonModule],
})
export class MaxChunkSliderComponent implements ControlValueAccessor, OnInit {
  min = 100;
  max = 1200;
  step = 50;

  value = 100;

  onChange = (v: number) => {};
  onTouched = () => {};

  ngOnInit() {
    this.writeValue(this.value);
  }

  writeValue(val: number): void {
    if (val != null) {
      this.value = Math.min(this.max, Math.max(this.min, val));
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  decrement() {
    this.setValue(this.value - this.step);
  }

  increment() {
    this.setValue(this.value + this.step);
  }

  onInput(event: Event) {
    const newVal = parseInt((event.target as HTMLInputElement).value, 10);
    this.setValue(newVal);
  }

  private setValue(v: number) {
    this.value = Math.min(this.max, Math.max(this.min, v));
    this.onChange(this.value);
    this.onTouched();
  }

  get percent(): number {
    return ((this.value - this.min) / (this.max - this.min)) * 100;
  }
}
