import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-hamburger-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hamburger-menu.component.html',
})
export class HamburgerMenuComponent {
  isOpen = false;
  activeSection: string = 'home';

  sections = [
    { id: 'home', label: 'Home' },
    { id: 'about', label: 'About' },
    { id: 'integrations', label: 'Integrations' },
    { id: 'roadmap', label: 'Roadmap' },
  ];

  toggleMenu(): void {
    this.isOpen = !this.isOpen;
  }

  @HostListener('document:click', ['$event.target'])
  onClickOutside(target: HTMLElement): void {
    const clickedInsideButton = target.closest('.menu-button');
    const clickedInsideMenu = target.closest('.menu-container');
    if (this.isOpen && !clickedInsideButton && !clickedInsideMenu) {
      this.isOpen = false;
    }
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
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
}
