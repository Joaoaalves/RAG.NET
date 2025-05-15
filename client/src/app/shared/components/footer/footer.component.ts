import { Component, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { ionLogoGithub } from '@ng-icons/ionicons';
import { heroStar } from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  imports: [NgIcon],
  providers: [
    provideIcons({
      ionLogoGithub,
      heroStar,
    }),
  ],
  standalone: true,
})
export class FooterComponent implements OnInit {
  starsCount = '0';
  constructor() {}

  ngOnInit() {
    this.getStarsCount();
  }

  getStarsCount() {
    fetch('https://api.github.com/repos/Joaoaalves/RAG.NET')
      .then((response) => response.json())
      .then((data) => {
        this.starsCount = this.abbreviateNumber(data.stargazers_count);
      });
  }
  abbreviateNumber(value: number): string {
    if (value >= 1_000_000) {
      return (value / 1_000_000).toFixed(1).replace(/\.0$/, '') + 'M';
    }
    if (value >= 1_000) {
      return (value / 1_000).toFixed(1).replace(/\.0$/, '') + 'K';
    }
    return value.toString();
  }

  handleNavigate() {
    return window.open('https://github.com/Joaoaalves/RAG.NET', '_blank');
  }
}
