import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-price-calculator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './price-calculator.component.html',
})
export class PriceCalculatorComponent {
  @Input() embeddingCostPerMillion!: number;
  @Input() examplePages?: number;

  @Input() conversationInputCostPerMillion!: number;

  @Input() conversationOutputCostPerMillion!: number;

  formatCurrency(value: number): string {
    return value.toLocaleString(undefined, {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2,
    });
  }

  get exampleEmbeddingCost(): string | null {
    if (!this.examplePages) return null;
    const cost =
      (this.embeddingCostPerMillion / 1_000_000) * (this.examplePages * 250);
    return this.formatCurrency(cost);
  }
}
