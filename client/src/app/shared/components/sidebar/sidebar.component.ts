import { UserService } from 'src/app/services/user.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from 'src/app/models/user';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

// Icons
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideCalendarSync,
  lucideDatabase,
  lucideLayers,
  lucideLogOut,
  lucideMenu,
  lucideWallet,
  lucideX,
} from '@ng-icons/lucide';

// Services
import { AuthService } from 'src/app/services/auth.service';

// Components
import { NavItemComponent } from './nav-item.component';
import { UserCardComponent } from './user-card.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgIcon, NavItemComponent, UserCardComponent],
  providers: [
    provideIcons({
      lucideDatabase,
      lucideWallet,
      lucideLayers,
      lucideLogOut,
      lucideMenu,
      lucideCalendarSync,
      lucideX,
    }),
  ],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  @Output() toggleNavEvent = new EventEmitter<boolean>();

  navItems = [
    {
      icon: 'lucideLayers',
      label: 'Workflows',
      onClick: () => this.navigateWorkflows(),
      url: '/dashboard/workflows',
    },
    {
      icon: 'lucideDatabase',
      label: 'Providers',
      onClick: () => this.navigateProviders(),
      url: '/dashboard/providers',
    },
    {
      icon: 'lucideWallet',
      label: 'Wallet',
      onClick: () => this.navigateWallet(),
      url: '/dashboard/wallet',
    },
    {
      icon: 'lucideCalendarSync',
      label: 'Subscription',
      onClick: () => this.navigateSubscription(),
      url: '/dashboard/subscriptions',
    },
    {
      icon: 'lucideLogOut',
      label: 'Logout',
      onClick: () => this.logout(),
      url: '/login',
    },
  ];

  sidebarOpen = true;
  user$: Observable<User | null>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private userService: UserService
  ) {
    this.user$ = this.userService.user$;
  }

  ngOnInit(): void {
    this.userService.getInfo().subscribe();
  }

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
    this.toggleNavEvent.emit(this.sidebarOpen);
  }

  navigateWorkflows() {
    this.router.navigate(['/dashboard/workflows']);
  }

  navigateProviders() {
    this.router.navigate(['/dashboard/providers']);
  }

  navigateWallet() {
    this.router.navigate(['/dashboard/wallet']);
  }

  navigateSubscription() {
    this.router.navigate(['/dashboard/subscriptions']);
  }

  logout() {
    this.authService.logout();
    this.userService.clearCache();
    this.router.navigate(['/login']);
  }
}
