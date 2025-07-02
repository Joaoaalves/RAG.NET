import { Component, Input, inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Transaction, TransactionSource } from 'src/app/models/wallet';
import { NgIcon } from '@ng-icons/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  imports: [NgIcon, CommonModule],
  standalone: true,
})
export class TransactionComponent {
  @Input() transaction!: Transaction;

  private readonly router = inject(Router);
  private readonly sanitizer = inject(DomSanitizer);

  isExpanded = false;

  navigateToWorkflow() {
    return this.router.navigate([
      '/dashboard/workflows/' + this.transaction.workflowId,
    ]);
  }

  toggleExpand() {
    this.isExpanded = !this.isExpanded;
  }

  get workflowId() {
    return this.transaction.workflowId.slice(-8);
  }

  get transactionDate() {
    return new Date(this.transaction.timeStamp).toLocaleString();
  }

  get cost() {
    return this.transaction.cost / 1000;
  }

  get source(): string {
    return this.transaction.source === TransactionSource.FREE ? 'Free' : 'Paid';
  }

  get parsedContextInfo(): SafeHtml {
    if (!this.transaction.contextInfo) return '';

    const html = this.transaction.contextInfo
      .split(';')
      .filter((entry) => entry.includes('='))
      .map((entry) => {
        const [key, value] = entry.split('=');
        return `<span class="block"><strong class="text-white">${key}</strong>: ${value}</span>`;
      })
      .join('');

    return this.sanitizer.bypassSecurityTrustHtml(html);
  }
}
