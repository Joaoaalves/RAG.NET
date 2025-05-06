import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  QueryList,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { gsap } from 'gsap';
import { SplitText } from 'gsap/all';

@Component({
  selector: 'app-hero',
  templateUrl: './hero.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class HeroComponent implements AfterViewInit {
  constructor() {
    gsap.registerPlugin(SplitText);
  }

  ngAfterViewInit(): void {
    // Split text for staggered reveal
    const title = new SplitText('#hero-title', {
      type: 'words,chars',
      wordsClass: 'word',
      charsClass: 'char',
    });
    const paragraph = new SplitText('#hero-paragraph', {
      type: 'words,chars',
      wordsClass: 'word',
      charsClass: 'char',
    });

    title.chars.forEach((char: Element) => {
      char.classList.add(
        'text-transparent',
        'bg-gradient-to-br',
        'from-white',
        'via-neutral-50',
        'to-neutral-500',
        'bg-clip-text',
        'drop-shadow-[2px_0px_4px_#00000050]'
      );
    });
    paragraph.chars.forEach((char: Element) => {
      char.classList.add('text-neutral-200');
    });

    gsap.from(title.chars, {
      filter: 'blur(10px)',
      opacity: 0,
      stagger: 0.04,
      ease: 'power1.in',
    });

    gsap.from(paragraph.chars, {
      filter: 'blur(10px)',
      opacity: 0,
      stagger: 0.01,
      ease: 'power1.in',
      delay: 0.8,
    });

    gsap.from('#lamp-blur', {
      width: 0,
      opacity: 0,
      filter: 'blur(150px)',
      ease: 'circ.out',
      duration: 1,
      delay: 1,
    });
  }
}
