import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  standalone: true,
})
export class AboutComponent implements AfterViewInit {
  ngAfterViewInit() {
    gsap.registerPlugin(ScrollTrigger);

    gsap.set('#about-card', {
      y: '-50%',
      rotationX: 30,
      z: -100,
    });

    gsap.to('#about-card', {
      y: 0,
      rotationX: 0,
      z: 0,
      scrollTrigger: {
        trigger: '#about-card',
        start: 'top bottom',
        end: 'center center',
        scrub: true,
      },
    });
  }
}
