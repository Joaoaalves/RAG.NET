import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideBookOpenCheck,
  lucideChevronDown,
  lucideFileText,
  lucideLayers,
} from '@ng-icons/lucide';
import { ContentItem } from 'src/app/models/query';

@Component({
  selector: 'app-result-card',
  templateUrl: './result-card.component.html',
  imports: [CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideFileText,
      lucideLayers,
      lucideBookOpenCheck,
      lucideChevronDown,
    }),
  ],
  styles: `
    :host{
      display: contents;
    }
  `,
  standalone: true,
})
export class ResultCardComponent {
  @Input() contentItem!: ContentItem;
  @Input() index!: number;
}
