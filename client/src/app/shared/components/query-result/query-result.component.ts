import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ContentItem } from 'src/app/models/query';

@Component({
  selector: 'app-query-result',
  templateUrl: './query-result.component.html',
  imports: [CommonModule],
  standalone: true,
  styles: `
    :host{
      display: contents;
    }
  `,
})
export class QueryResultComponent {
  @Input() chunks: ContentItem[] = [];
  @Input() filteredContent: string[] = [];
}
