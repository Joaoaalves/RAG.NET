import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideArrowUp, lucideMessageCircle } from '@ng-icons/lucide';

@Component({
  selector: 'app-price-calculator',
  standalone: true,
  imports: [CommonModule, NgIcon],
  providers: [
    provideIcons({
      lucideArrowUp,
      lucideMessageCircle,
    }),
  ],
  templateUrl: './price-calculator.component.html',
})
export class PriceCalculatorComponent {
  @Input() embeddingCostPerMillion = 0;
  @Input() conversationInputCostPerMillion = 0;
  @Input() conversationOutputCostPerMillion = 0;

  formatCurrency(value: number): string {
    return value.toLocaleString(undefined, {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2,
    });
  }

  get oneDolarPages(): number | null {
    if (!this.embeddingCostPerMillion) return null;
    const tokensPerPage = 1000;

    const oneDolarTokens = 1_000_000 / this.embeddingCostPerMillion;
    const pages = Math.floor(oneDolarTokens / tokensPerPage);

    return pages;
  }
}
