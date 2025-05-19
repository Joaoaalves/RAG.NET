import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-nav-item',
  standalone: true,
  imports: [NgIcon, RouterModule, CommonModule],
  styles: [
    `
      :host {
        display: contents;
      }
    `,
  ],
  templateUrl: './nav-item.component.html',
})
export class NavItemComponent {
  @Input() icon!: string;
  @Input() label!: string;
  @Input() url!: string;
}
