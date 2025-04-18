import { CommonModule } from '@angular/common';
import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ],
  host: {
    style: 'display: block',
  },
  standalone: true,
})
export class InputComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() type: string = 'text';
  @Input() description: string = '';
  @Input() name: string = '';
  @Input() step?: number = 1;
  @Input() min?: number = 0;
  @Input() max?: number = 1;

  value: any;

  onChange = (value: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
    this.value = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {}

  onInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    let newValue: any = target.value;
    if (this.type === 'number') {
      newValue = target.value === '' ? null : Number(target.value);
    }
    this.value = newValue;
    this.onChange(newValue);
  }
}
