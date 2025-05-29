import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  Input,
  ElementRef,
  ViewChild,
  Type,
} from '@angular/core';
import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { HighlightTextPipe } from './highlight-text.pipe';

gsap.registerPlugin(ScrollTrigger);

@Component({
  selector: 'app-about-item',
  imports: [CommonModule, HighlightTextPipe],
  templateUrl: './about-item.component.html',
  standalone: true,
})
export class AboutItemComponent implements AfterViewInit {
  @Input() title = 'About Us';
  @Input() subtitle = 'We are a team of passionate developers.';
  @Input() imageUrl = 'assets/images/about.jpg';
  @Input() index = 0;
  @Input() dynamicComponent!: Type<any>;

  @ViewChild('imageWrapper', { static: true }) imageEl!: ElementRef;
  @ViewChild('textWrapper', { static: true }) textEl!: ElementRef;

  ngAfterViewInit() {
    gsap.from(this.imageEl.nativeElement, {
      x: this.index % 2 === 0 ? -100 : 100,
      opacity: 0,
      duration: 1,
      ease: 'power3.out',
      scrollTrigger: {
        trigger: this.imageEl.nativeElement,
        start: 'top 80%',
      },
    });

    gsap.from(this.textEl.nativeElement, {
      x: this.index % 2 === 0 ? 100 : -100,
      opacity: 0,
      duration: 1,
      ease: 'power3.out',
      scrollTrigger: {
        trigger: this.textEl.nativeElement,
        start: 'top 80%',
      },
    });
  }
}
