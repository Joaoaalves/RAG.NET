import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideInfo } from '@ng-icons/lucide';
import {
  DailyTransactionsAggregate,
  Wallet,
  GetWalletRequest,
} from 'src/app/models/wallet';
import { WalletService } from 'src/app/services/wallet.service';
import { BarChartComponent } from 'src/app/shared/components/bar-chart/bar-chart.component';
import { TransactionsTableComponent } from 'src/app/shared/components/transaction-table/transactions-table.component';

@Component({
  templateUrl: './wallet.component.html',
  imports: [
    NgIcon,
    BarChartComponent,
    TransactionsTableComponent,
    CommonModule,
  ],
  providers: [provideIcons({ lucideInfo })],
  standalone: true,
})
export class WalletComponent implements OnInit {
  wallet!: Wallet;
  dailyTransactions: DailyTransactionsAggregate[] = [];

  start: Date;
  end: Date = new Date();

  barChartLabels: string[] = [];
  barChartDatasets: any[] = [];

  constructor(private walletService: WalletService) {
    const now = new Date();
    this.start = new Date(now.getFullYear(), now.getMonth(), 1);
  }

  ngOnInit(): void {
    this.loadWallet();
  }

  loadWallet() {
    const request: GetWalletRequest = {
      start: this.start,
      end: this.end,
    };

    this.walletService.getInfo(request).subscribe((response) => {
      this.wallet = response.wallet;

      const filledDays = this.fillMissingDays(
        response.dailyTransactions,
        this.start,
        this.end
      );

      this.dailyTransactions = filledDays;

      this.barChartLabels = filledDays.map((t) =>
        t.date.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' })
      );

      this.barChartDatasets = [
        {
          label: 'Free Tokens',
          data: filledDays.map((t) => t.freeTokensConsumed),
        },
        {
          label: 'Paid Tokens',
          data: filledDays.map((t) => t.paidTokensConsumed),
        },
      ];
    });
  }

  private fillMissingDays(
    input: DailyTransactionsAggregate[],
    start: Date,
    end: Date
  ): DailyTransactionsAggregate[] {
    const map = new Map(
      input.map((d) => [
        new Date(d.date).toDateString(),
        {
          ...d,
          date: new Date(d.date),
        },
      ])
    );

    const filled: DailyTransactionsAggregate[] = [];
    const cursor = new Date(start);

    while (cursor <= end) {
      const key = cursor.toDateString();
      filled.push(
        map.get(key) ?? {
          date: new Date(cursor),
          freeTokensConsumed: 0,
          paidTokensConsumed: 0,
        }
      );
      cursor.setDate(cursor.getDate() + 1);
    }

    return filled;
  }
}
