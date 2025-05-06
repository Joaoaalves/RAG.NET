import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-links',
  templateUrl: './nav-links.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class NavLinksComponent implements OnInit {
  activeSection: string = 'home';

  sections = [
    { id: 'home', label: 'Home' },
    { id: 'about', label: 'About' },
    { id: 'integrations', label: 'Integrations' },
    { id: 'roadmap', label: 'Roadmap' },
  ];

  @HostListener('window:scroll', [])
  onScroll() {
    for (let section of this.sections) {
      const el = document.getElementById(section.id);
      if (el) {
        const rect = el.getBoundingClientRect();
        if (rect.top <= 100 && rect.bottom >= 100) {
          this.activeSection = section.id;
          break;
        }
      }
    }
  }

  ngOnInit(): void {
    this.onScroll();
  }
}
