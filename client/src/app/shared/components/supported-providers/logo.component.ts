import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-logo',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './logo.component.html',
})
export class LogoComponent {
  @Input() implemented = false;
  @Input() src = '';
  @Input() title = '';
}
