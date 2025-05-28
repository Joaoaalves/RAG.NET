import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideMinus, lucidePlus } from '@ng-icons/lucide';

@Component({
  selector: 'app-number-counter-input',
  standalone: true,
  imports: [CommonModule, NgIcon],
  templateUrl: './number-counter-input.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumberCounterInputComponent),
      multi: true,
    },
    provideIcons({
      lucidePlus,
      lucideMinus,
    }),
  ],
})
export class NumberCounterInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() min = 0;
  @Input() max = Infinity;

  value = 0;

  onChange = (_: number) => {};
  onTouched = () => {};

  writeValue(val: number): void {
    this.value = this.clamp(val ?? 0);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  increment() {
    this.setValue(this.value + 1);
  }

  decrement() {
    this.setValue(this.value - 1);
  }

  onInput(event: Event) {
    const input = (event.target as HTMLInputElement).value;
    const parsed = Number(input);

    if (!isNaN(parsed)) {
      this.setValue(parsed);
    } else {
      this.value = this.min;
      this.onChange(this.value);
    }
  }

  private setValue(val: number) {
    const clamped = this.clamp(val);
    this.value = clamped;

    this.onChange(clamped);
  }

  private clamp(val: number): number {
    return Math.min(this.max, Math.max(this.min, val));
  }
}
