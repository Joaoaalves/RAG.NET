import { Component, Input } from '@angular/core';
import { AvatarComponent } from './avatar.component';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronRight } from '@ng-icons/lucide';

@Component({
  selector: 'app-user-card',
  imports: [CommonModule, AvatarComponent, NgIcon],
  providers: [provideIcons({ lucideChevronRight })],
  templateUrl: './user-card.component.html',
  standalone: true,
})
export class UserCardComponent {
  @Input() avatarUrl?: string;
  @Input() initials = '';
  @Input() name = '';
  @Input() email = '';
}
