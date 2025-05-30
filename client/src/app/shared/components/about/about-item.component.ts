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
import { NgIcon } from '@ng-icons/core';

import { Feat } from 'src/app/models/about';

gsap.registerPlugin(ScrollTrigger);

@Component({
  selector: 'app-about-item',
  imports: [CommonModule, HighlightTextPipe, NgIcon],
  templateUrl: './about-item.component.html',
  standalone: true,
})
export class AboutItemComponent {
  @Input() title = 'About Us';
  @Input() subtitle = 'We are a team of passionate developers.';
  @Input() imageUrl = 'assets/images/about.jpg';
  @Input() index = 0;
  @Input() feats: Feat[] = [];
  @Input() dynamicComponent!: Type<any>;
}
