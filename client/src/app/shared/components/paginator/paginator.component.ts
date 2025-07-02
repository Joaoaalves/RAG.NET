import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-paginator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './paginator.component.html',
})
export class PaginatorComponent {
  @Input() currentPage: number = 1;
  @Input() pageSize: number = 20;
  @Input() totalItems: number = 0;

  @Output() pageChange = new EventEmitter<number>();

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  goTo(page: number) {
    this.pageChange.emit(page);
  }
}
