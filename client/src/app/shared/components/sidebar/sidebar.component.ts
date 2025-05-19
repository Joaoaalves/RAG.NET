import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideDatabase,
  lucideLayers,
  lucideLogOut,
  lucideMenu,
  lucideX,
} from '@ng-icons/lucide';
import { NavItemComponent } from './nav-item.component';
import { UserCardComponent } from './user-card.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgIcon, NavItemComponent, UserCardComponent],
  providers: [
    provideIcons({
      lucideDatabase,
      lucideLayers,
      lucideLogOut,
      lucideMenu,
      lucideX,
    }),
  ],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  navItems = [
    { icon: 'lucideLayers', label: 'Workflows', url: '/dashboard/workflows' },
    { icon: 'lucideDatabase', label: 'Providers', url: '/dashboard/providers' },
    { icon: 'lucideLogOut', label: 'Logout', url: '/logout' },
  ];

  sidebarOpen = true;

  private userInfoKey = 'userInfo';
  user!: User;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
  }

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  loadUserInfo() {
    const userInfo = localStorage.getItem(this.userInfoKey);
    if (userInfo) {
      this.user = JSON.parse(userInfo);
    } else {
      this.userService.getInfo().subscribe((user) => {
        localStorage.setItem(this.userInfoKey, JSON.stringify(user));
        this.user = user;
      });
    }
  }

  logout() {
    this.authService.logout();
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
