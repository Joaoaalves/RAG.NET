import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { JobItem } from 'src/app/models/job';

// Services
import { EmbeddingService } from 'src/app/services/embedding.service';

// Components
import { SidebarComponent } from 'src/app/shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-dashboard-layout',
  templateUrl: './dashboard-layout.component.html',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  styles: `
  :host{
  display: contents;
  }
  `,
})
export class DashboardLayoutComponent {
  jobs$: Observable<JobItem[]>;

  constructor(private embeddingService: EmbeddingService) {
    this.jobs$ = this.embeddingService.jobs$;
  }
}
