import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-input',
  host: {
    style: 'display: block',
  },
  templateUrl: './input.component.html',
})
export class InputComponent {
  @Input() label: string = '';
  @Input() type: string = 'text';
  @Input() name: string = '';
  @Input() model: any;

  @Output() modelChange = new EventEmitter<any>();

  onInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.modelChange.emit(target.value);
  }
}
