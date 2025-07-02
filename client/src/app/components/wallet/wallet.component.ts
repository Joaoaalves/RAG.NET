import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideInfo } from '@ng-icons/lucide';
import { forkJoin } from 'rxjs';
import {
  DailyTransactionsAggregate,
  Wallet,
  GetWalletRequest,
  Transaction,
} from 'src/app/models/wallet';
import { WalletService } from 'src/app/services/wallet.service';
import { BarChartComponent } from 'src/app/shared/components/bar-chart/bar-chart.component';
import { DateRangeSelectorComponent } from 'src/app/shared/components/date-range-selector/date-range-selector.component';
import { PaginatorComponent } from 'src/app/shared/components/paginator/paginator.component';
import { TransactionsTableComponent } from 'src/app/shared/components/transaction-table/transactions-table.component';

@Component({
  templateUrl: './wallet.component.html',
  imports: [
    NgIcon,
    BarChartComponent,
    TransactionsTableComponent,
    PaginatorComponent,
    DateRangeSelectorComponent,
    CommonModule,
  ],
  providers: [provideIcons({ lucideInfo })],
  standalone: true,
})
export class WalletComponent implements OnInit {
  wallet!: Wallet;
  transactionChartData: DailyTransactionsAggregate[] = [];

  currentPage = 1;
  pageSize = 20;
  totalCount = 0;

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

  onDateRangeChange({ start, end }: { start: Date; end: Date }) {
    this.start = start;
    this.end = end;
    this.currentPage = 1;
    this.loadWallet();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadWallet();
  }

  loadWallet() {
    const request: GetWalletRequest = {
      start: this.start,
      end: this.end,
    };

    forkJoin({
      wallet: this.walletService.getWallet(),
      dailyTransactions: this.walletService.getDailyTransactions(request),
      pagedTransactions: this.walletService.getTransactions({
        ...request,
        page: this.currentPage,
        pageSize: this.pageSize,
      }),
    }).subscribe(({ wallet, dailyTransactions, pagedTransactions }) => {
      this.wallet = wallet;

      this.wallet.transactions = pagedTransactions.items;
      this.totalCount = pagedTransactions.totalCount;

      const filledDays = this.fillMissingDays(
        dailyTransactions,
        this.start,
        this.end
      );

      this.transactionChartData = filledDays;

      this.barChartLabels = filledDays.map((t) => {
        const [year, month, day] = t.date.split('T')[0].split('-');
        return `${day}/${month}`;
      });

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
        d.date.split('T')[0],
        {
          ...d,
          date: d.date,
        },
      ])
    );

    const filled: DailyTransactionsAggregate[] = [];
    const cursor = new Date(start);

    while (cursor <= end) {
      const isoDate = cursor.toISOString().split('T')[0];
      filled.push(
        map.get(isoDate) ?? {
          date: `${isoDate}T00:00:00Z`,
          freeTokensConsumed: 0,
          paidTokensConsumed: 0,
        }
      );
      cursor.setUTCDate(cursor.getUTCDate() + 1);
    }

    return filled;
  }
}
