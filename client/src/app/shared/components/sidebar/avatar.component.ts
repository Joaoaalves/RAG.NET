import { UserService } from 'src/app/services/user.service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { PlanType } from 'src/app/models/subscription';

@Component({
  selector: 'app-avatar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './avatar.component.html',
  styles: [
    `
      :host {
        display: inline-block;
      }
    `,
  ],
})
export class AvatarComponent {
  fallback = '';
  src = '/img/placeholder.svg';
  plan?: PlanType;

  constructor(private userService: UserService) {
    this.userService.userInitials$.subscribe((initials) => {});

    this.userService.user$.subscribe((user) => {
      if (user) {
        this.plan = user.subscription.planType;
        this.fallback = user?.firstName[0] + user?.lastName[0];
      }
    });
  }

  showFallback = false;

  onError() {
    this.showFallback = true;
  }
}
