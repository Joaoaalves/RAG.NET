import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideSearch } from '@ng-icons/lucide';
import { ContentItem } from 'src/app/models/query';
import { ResultCardComponent } from './result-card.component';
import { FilteredResultComponent } from './filtered-result.component';

@Component({
  selector: 'app-query-result',
  templateUrl: './query-result.component.html',
  imports: [CommonModule, NgIcon, ResultCardComponent, FilteredResultComponent],
  providers: [
    provideIcons({
      lucideSearch,
    }),
  ],
  standalone: true,
  styles: `
    :host{
      display: contents;
    }
  `,
})
export class QueryResultComponent implements OnChanges {
  @Input() workflowId!: string;
  @Input() chunks: ContentItem[] = [];
  @Input() filteredContent: string[] = [];

  currentTab = 0;

  setTab(tab: number) {
    this.currentTab = tab;
  }

  ngOnChanges(changes: SimpleChanges) {
    const chunksChanged = !!changes['chunks'];
    const filteredChanged = !!changes['filteredContent'];

    const prevChunks = changes['chunks']?.previousValue as
      | ContentItem[]
      | undefined;
    const prevFiltered = changes['filteredContent']?.previousValue as
      | string[]
      | undefined;

    if (
      chunksChanged &&
      (this.filteredContent.length === 0 ||
        (prevFiltered && prevFiltered.length === this.filteredContent.length))
    ) {
      this.currentTab = 1;
    } else if (
      filteredChanged &&
      (this.chunks.length === 0 ||
        (prevChunks && prevChunks.length === this.chunks.length))
    ) {
      this.currentTab = 0;
    }
  }
}
