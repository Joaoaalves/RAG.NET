import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-avatar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avatar.component.html',
  styles: [
    `
      :host {
        display: inline-block;
      }
    `,
  ],
})
export class AvatarComponent {
  @Input() src?: string;
  @Input() fallback = '';
  showFallback = false;

  onError() {
    this.showFallback = true;
  }
}
