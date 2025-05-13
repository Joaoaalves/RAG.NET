import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-timeline-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './timeline-item.component.html',
})
export class TimelineItemComponent {
  @Input() title!: string;
  @Input() description!: string;
  @Input() colorHex!: string;
}
