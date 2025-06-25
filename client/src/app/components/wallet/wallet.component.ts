import { Component } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideInfo } from '@ng-icons/lucide';

@Component({
  templateUrl: './wallet.component.html',
  imports: [NgIcon],
  providers: [provideIcons({ lucideInfo })],
  standalone: true,
})
export class WalletComponent {}
