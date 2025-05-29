import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input } from '@angular/core';
import { gsap } from 'gsap';
import { HighlightTextPipe } from './highlight-text.pipe';

@Component({
  selector: 'app-about-item',
  imports: [CommonModule, HighlightTextPipe],
  templateUrl: './about-item.component.html',
  standalone: true,
})
export class AboutItemComponent implements AfterViewInit {
  @Input() title: string = 'About Us';
  @Input() subtitle: string = 'We are a team of passionate developers.';
  @Input() imageUrl: string = 'assets/images/about.jpg';
  @Input() index = 0;

  ngAfterViewInit() {
    var selector = `#about-card-${this.index}`;

    gsap.set(selector, {
      y: '-50%',
      rotationX: 30,
      z: -100,
    });

    gsap.to(selector, {
      y: 0,
      rotationX: 0,
      z: 0,
      scrollTrigger: {
        trigger: selector,
        start: 'top center',
        end: 'bottom center',
      },
    });
  }
}
