import { Component } from '@angular/core';
import { NavLinksComponent } from '../nav-links/nav-links.component';
import { HamburgerMenuComponent } from '../hamburguer-menu/hamburger-menu.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  imports: [NavLinksComponent, HamburgerMenuComponent],
  templateUrl: './nav-bar.component.html',
  standalone: true,
})
export class NavBarComponent {
  constructor(private router: Router) {}

  navigateHome() {
    this.router.navigate(['/']).then(() => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }
}
