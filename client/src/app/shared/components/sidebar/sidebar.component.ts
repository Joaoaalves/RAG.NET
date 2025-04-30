import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-sidebar',
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  standalone: true,
})
export class SidebarComponent implements OnInit {
  private userInfoKey = 'userInfo';

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
  }

  user!: User;

  loadUserInfo() {
    const userInfo = localStorage.getItem(this.userInfoKey);

    if (userInfo) {
      this.user = JSON.parse(userInfo);
      return;
    }

    this.userService.getInfo().subscribe((user) => {
      localStorage.setItem(this.userInfoKey, JSON.stringify(user));
      this.user = user;
    });
  }

  logout() {
    this.authService.logout();
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
