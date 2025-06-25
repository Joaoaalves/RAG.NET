import { Component, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideInfo } from '@ng-icons/lucide';
import { Wallet } from 'src/app/models/wallet';
import { WalletService } from 'src/app/services/wallet.service';

@Component({
  templateUrl: './wallet.component.html',
  imports: [NgIcon],
  providers: [provideIcons({ lucideInfo })],
  standalone: true,
})
export class WalletComponent implements OnInit {
  wallet!: Wallet;

  constructor(private walletService: WalletService) {}

  ngOnInit(): void {
    this.loadWallet();
  }

  loadWallet() {
    this.walletService.getInfo().subscribe((wallet) => {
      this.wallet = wallet;
    });
  }
}
