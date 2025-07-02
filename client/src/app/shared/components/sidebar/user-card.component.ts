import { Component, Input } from '@angular/core';
import { AvatarComponent } from './avatar.component';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronRight } from '@ng-icons/lucide';
import { PlanType } from 'src/app/models/subscription';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-card',
  imports: [CommonModule, AvatarComponent, NgIcon],
  providers: [provideIcons({ lucideChevronRight })],
  templateUrl: './user-card.component.html',
  standalone: true,
})
export class UserCardComponent {
  name?: string = '';
  email?: string = '';

  constructor(private userService: UserService) {
    this.userService.user$.subscribe((user) => {
      this.name = user?.firstName;
      this.email = user?.email;
    });
  }
}
