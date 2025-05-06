import { Component } from '@angular/core';
import { HeroComponent } from 'src/app/shared/components/hero/hero.component';
import { NavBarComponent } from 'src/app/shared/components/nav-bar/nav-bar.component';

@Component({
  templateUrl: './home.component.html',
  imports: [NavBarComponent, HeroComponent],
  standalone: true,
})
export class HomeComponent {}
