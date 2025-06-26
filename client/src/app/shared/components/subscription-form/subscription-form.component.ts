import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SubscriptionService } from 'src/app/services/subscription.service';

@Component({
  selector: 'app-subscription-form',
  templateUrl: './subscription-form.component.html',
  imports: [CommonModule],
  providers: [],
  standalone: true,
})
export class SubscriptionFormComponent {
  loading = false;
  error: string | null = null;
  constructor(private subscriptionService: SubscriptionService) {}

  subscribe() {
    this.loading = true;
    this.error = null;

    this.subscriptionService.startSubscription().subscribe({
      next: (url) => {
        window.location.href = url;
      },
      error: () => {
        this.error = 'Erro ao iniciar o pagamento.';
        this.loading = false;
      },
    });
  }
}
