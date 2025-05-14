import { CommonModule } from '@angular/common';
import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-timeline-item',
  templateUrl: './timeline-item.component.html',
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TimelineItemComponent {
  @Input() index!: number;
  @Input() title!: string;
  @Input() description!: string;
  @Input() colorHex!: string;
  @Input() status!: 'completed' | 'in-progress' | 'upcoming';
  @Input() active = false;

  get isEven() {
    return this.index % 2 === 0;
  }
  get isOdd() {
    return !this.isEven;
  }

  get dotBoxShadow() {
    return this.active
      ? '0 0 15px rgba(244,114,182,0.8), 0 0 30px rgba(56,189,248,0.6)'
      : '0 0 10px rgba(244,114,182,0.5)';
  }

  get cardBoxShadow() {
    return this.active
      ? '0 0 20px rgba(0,0,0,0.5), 0 0 10px rgba(244,114,182,0.3)'
      : '0 0 10px rgba(0,0,0,0.3)';
  }

  get borderRightStyle() {
    return this.isEven ? `4px solid ${this.colorHex}` : 'none';
  }

  get borderLeftStyle() {
    return this.isOdd ? `4px solid ${this.colorHex}` : 'none';
  }

  get statusText() {
    return this.status === 'completed'
      ? 'Done'
      : this.status === 'in-progress'
      ? 'In Progress'
      : 'Upcoming';
  }
}
