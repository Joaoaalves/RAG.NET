import { CommonModule } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { gsap } from 'gsap';
import { SplitText } from 'gsap/all';

@Component({
  selector: 'app-hero',
  templateUrl: './hero.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class HeroComponent implements AfterViewInit {
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
        'bg-gradient-to-b',
        'from-[#ffffff]',
        'via-[29%]',
        'via-[#ffffff]',
        'to-[#999999]',
        'bg-clip-text',
        'pb-3',
        'font-bold',
        'drop-shadow-[2px_0px_4px_#00000060]',
        '-tracking-4'
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

    gsap.fromTo(
      '#register-btn',
      {
        opacity: 0,
        filter: 'blur(20px)',
      },
      {
        opacity: 1,
        filter: 'blur(0px)',
        delay: 1.3,
        duration: 0.6,
        ease: 'power1.inOut',
      }
    );

    gsap.fromTo(
      '#docs-btn',
      {
        opacity: 0,
        filter: 'blur(20px)',
      },
      {
        opacity: 1,
        filter: 'blur(0px)',
        delay: 1.5,
        duration: 0.6,
        ease: 'power1.inOut',
      }
    );
  }
}
