import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { Transaction } from 'src/app/models/wallet';
import { TransactionComponent } from './transaction.component';
import {
  lucideChevronDown,
  lucideChevronUp,
  lucideExternalLink,
} from '@ng-icons/lucide';

@Component({
  selector: 'app-transactions-table',
  imports: [CommonModule, TransactionComponent],
  providers: [
    provideIcons({ lucideExternalLink, lucideChevronUp, lucideChevronDown }),
  ],
  templateUrl: './transactions-table.component.html',
  standalone: true,
})
export class TransactionsTableComponent {
  @Input() transactions: Transaction[] = [];
}
