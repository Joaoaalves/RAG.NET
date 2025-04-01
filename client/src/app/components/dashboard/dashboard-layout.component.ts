import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

// Components
import { SidebarComponent } from 'src/app/shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-dashboard-layout',
  templateUrl: './dashboard-layout.component.html',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
})
export class DashboardLayoutComponent {}
