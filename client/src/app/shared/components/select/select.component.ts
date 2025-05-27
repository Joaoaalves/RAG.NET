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

export type SelectValue = { label: string; value: string | number };

@Component({
  selector: 'app-select',
  host: { style: 'display: contents' },
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
  @Input() label = '';
  @Input() name = '';
  @Input() description = '';
  @Input() value?: string | number | Array<string | number> = '';
  @Input() placeholder = '';
  @Input() options: SelectValue[] | null = [];

  selectedOption?: SelectValue | SelectValue[];

  onChange = (value: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
    if (value == null) {
      this.selectedOption = undefined;
    } else if (Array.isArray(value)) {
      this.selectedOption = value.map(
        (v) => this.getOptionByValue(v) || { label: String(v), value: v }
      );
    } else {
      this.selectedOption = this.getOptionByValue(value) || {
        label: String(value),
        value,
      };
    }

    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  ngOnInit(): void {
    if (this.value != null) {
      this.writeValue(this.value);
      this.onChange(this.value);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['options'] || changes['value']) {
      this.writeValue(this.value);
      this.onChange(this.value);
    }
  }

  onModelChange(selected: SelectValue | SelectValue[] | undefined): void {
    if (selected === undefined) {
      this.selectedOption = undefined;
      this.value = undefined;
      this.onChange(undefined);
      return;
    }

    this.selectedOption = selected;
    const newValue = Array.isArray(selected)
      ? selected.map((s) => s.value)
      : selected.value;
    this.value = newValue;
    this.onChange(newValue);
  }

  private getOptionByValue(
    value: string | number | undefined
  ): SelectValue | undefined {
    return this.options?.find((o) => o.value === value);
  }
}
