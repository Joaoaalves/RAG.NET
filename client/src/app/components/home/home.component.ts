import { Component } from '@angular/core';
import { AboutComponent } from 'src/app/shared/components/about/about.component';
import { FooterComponent } from 'src/app/shared/components/footer/footer.component';
import { HeroComponent } from 'src/app/shared/components/hero/hero.component';
import { IntegrationsComponent } from 'src/app/shared/components/integrations/integrations.component';
import { NavBarComponent } from 'src/app/shared/components/nav-bar/nav-bar.component';
import { RoadmapComponent } from 'src/app/shared/components/roadmap/roadmap.component';

import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { gsap } from 'gsap';
import { SplitText } from 'gsap/all';

@Component({
  templateUrl: './home.component.html',
  imports: [
    NavBarComponent,
    HeroComponent,
    AboutComponent,
    IntegrationsComponent,
    RoadmapComponent,
    FooterComponent,
  ],
  standalone: true,
})
export class HomeComponent {
  constructor() {
    gsap.registerPlugin(ScrollTrigger);

    gsap.registerPlugin(SplitText);
  }
}
