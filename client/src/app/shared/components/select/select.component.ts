import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  forwardRef,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { BrnSelectImports } from '@spartan-ng/brain/select';
import { HlmSelectImports } from '@spartan-ng/ui-select-helm';

@Component({
  selector: 'app-select',
  host: {
    style: 'display: block',
  },
  imports: [BrnSelectImports, HlmSelectImports, CommonModule],
  templateUrl: './select.component.html',
  standalone: true,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    },
  ],
})
export class SelectComponent
  implements OnInit, OnChanges, ControlValueAccessor
{
  @Input() label: string = '';
  @Input() name: string = '';
  @Input() description: string = '';
  @Input() value?: string | number = '';

  @Input() options: { value: string | number; label: string }[] = [];

  selectedOption: { label: string; value: string | number } | undefined;

  onChange = (value: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
    if (value === undefined || value === null) {
      this.selectedOption = undefined;
    } else {
      this.selectedOption = this.getOptionByValue(value) || {
        label: String(value),
        value,
      };
    }

    this.value = value;
    console.log(this.value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  ngOnInit(): void {
    if (this.value !== undefined && this.value !== null) {
      this.selectedOption = this.getOptionByValue(this.value) || {
        label: String(this.value),
        value: this.value,
      };

      this.onModelChange(this.selectedOption);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['options'] || changes['value']) {
      if (this.value !== undefined && this.value !== null) {
        this.selectedOption = this.getOptionByValue(this.value) || {
          label: String(this.value),
          value: this.value,
        };
        this.onChange(this.value);
      }
    }
  }

  onModelChange(selected: { label: string; value: string | number }): void {
    this.selectedOption = selected;
    this.value = selected.value;
    this.onChange(selected.value);
  }

  private getOptionByValue(
    value: string | number | undefined
  ): { label: string; value: string | number } | undefined {
    if (value === undefined) return undefined;
    return this.options.find((option) => option.value === value);
  }
}
