import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { Component } from '@angular/core';
import { lucideCircleHelp } from '@ng-icons/lucide';

@Component({
  selector: 'app-usage-tooltip',
  imports: [CommonModule, NgIcon],
  templateUrl: './usage-tooltip.component.html',
  providers: [
    provideIcons({
      lucideCircleHelp,
    }),
  ],
  standalone: true,
  styles: `
  :host{
    display: contents;
  }
  `,
})
export class UsageTooltipComponent {}
