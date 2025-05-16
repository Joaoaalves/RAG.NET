import {
  AfterViewInit,
  Component,
  Input,
  ElementRef,
  ViewChild,
} from '@angular/core';
import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { CommonModule } from '@angular/common';
import {
  heroChatBubbleOvalLeft,
  heroArrowUpCircle,
  heroCodeBracket,
} from '@ng-icons/heroicons/outline';

gsap.registerPlugin(ScrollTrigger);

@Component({
  selector: 'app-stats-card',
  standalone: true,
  imports: [NgIcon, CommonModule],
  providers: [
    provideIcons({
      heroChatBubbleOvalLeft,
      heroArrowUpCircle,
      heroCodeBracket,
    }),
  ],
  templateUrl: './stats-card.component.html',
})
export class StatsCardComponent implements AfterViewInit {
  @Input() stat!: { label: string; value: number; icon: string };
  @Input() index = 0;

  @ViewChild('card', { static: true }) cardRef!: ElementRef<HTMLElement>;
  @ViewChild('counter', { static: true }) counterRef!: ElementRef<HTMLElement>;

  ngAfterViewInit() {
    const obj = { value: 0 };

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: this.cardRef.nativeElement,
        start: 'top 80%',
        once: true,
      },
      onComplete: () => {
        gsap.set(this.cardRef.nativeElement, { clearProps: 'all' });
      },
    });

    tl.from(this.cardRef.nativeElement, {
      y: -50,
      opacity: 0,
      filter: 'blur(5px)',
      duration: 0.6,
    })
      .to(
        this.cardRef.nativeElement,
        { filter: 'blur(0px)', duration: 0.3 },
        '<'
      )
      .to(
        obj,
        {
          value: this.stat.value,
          duration: 1.5,
          ease: 'circ.out',
          roundProps: 'value',
          onUpdate: () => {
            this.counterRef.nativeElement.textContent = obj.value.toString();
          },
        },
        '<0.3'
      );
  }
}
