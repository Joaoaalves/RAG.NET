import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { gsap } from 'gsap';

@Component({
  selector: 'app-glow-background',
  standalone: true,
  templateUrl: './glow-background.component.html',
})
export class GlowBackgroundComponent implements AfterViewInit {
  ngAfterViewInit() {
    gsap.from('#glow', {
      opacity: 0,
      filter: 'blur(0px)',
      duration: 1.4,
      delay: 0.6,
      ease: 'sine.inOut',
      scrollTrigger: {
        trigger: '#glow',
        start: 'top bottom',
        end: 'center center',
      },
    });
  }
}
