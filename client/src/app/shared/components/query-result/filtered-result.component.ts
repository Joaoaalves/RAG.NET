import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-filtered-result',
  templateUrl: './filtered-result.component.html',
  standalone: true,
})
export class FilteredResultComponent {
  @Input() text!: string;
  @Input() index!: number;
}
