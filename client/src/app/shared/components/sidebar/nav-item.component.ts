import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-nav-item',
  standalone: true,
  imports: [NgIcon, RouterModule, CommonModule],
  templateUrl: './nav-item.component.html',
  styles: [
    `
      :host {
        display: contents;
      }
    `,
  ],
})
export class NavItemComponent {
  @Input() icon!: string;
  @Input() label!: string;
  @Input() url!: string;

  @Input() onClick: (() => void) | undefined;

  handleClick(event: MouseEvent) {
    if (!this.url) {
      event.preventDefault();
    }

    if (this.onClick) {
      this.onClick();
    }
  }
}
