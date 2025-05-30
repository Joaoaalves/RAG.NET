import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  BrnAlertDialogContentDirective,
  BrnAlertDialogTriggerDirective,
} from '@spartan-ng/brain/alert-dialog';
import {
  HlmAlertDialogActionButtonDirective,
  HlmAlertDialogCancelButtonDirective,
  HlmAlertDialogComponent,
  HlmAlertDialogContentComponent,
  HlmAlertDialogDescriptionDirective,
  HlmAlertDialogFooterComponent,
  HlmAlertDialogHeaderComponent,
  HlmAlertDialogTitleDirective,
} from '@spartan-ng/ui-alertdialog-helm';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  lucideInfo,
  lucideCircleX,
  lucideCircleCheck,
  lucideTriangleAlert,
  lucideX,
} from '@ng-icons/lucide';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [
    BrnAlertDialogContentDirective,
    BrnAlertDialogTriggerDirective,
    HlmAlertDialogActionButtonDirective,
    HlmAlertDialogCancelButtonDirective,
    HlmAlertDialogComponent,
    HlmAlertDialogContentComponent,
    HlmAlertDialogDescriptionDirective,
    HlmAlertDialogFooterComponent,
    HlmAlertDialogHeaderComponent,
    HlmAlertDialogTitleDirective,
    NgIcon,
    CommonModule,
  ],
  providers: [
    provideIcons({
      lucideInfo,
      lucideCircleX,
      lucideCircleCheck,
      lucideTriangleAlert,
      lucideX,
    }),
  ],
  templateUrl: './alert.component.html',
})
export class AlertComponent {
  @Input() id: string = '';
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() confirmText: string = 'Confirm';
  @Input() cancelText: string = 'Cancel';
  @Input() confirmLoading: boolean = false;
  @Input() variant: 'default' | 'destructive' | 'warning' | 'success' =
    'default';

  @Output() actionConfirmed = new EventEmitter<boolean>();

  confirmAction() {
    this.actionConfirmed.emit(true);
  }

  getVariantConfig() {
    switch (this.variant) {
      case 'destructive':
        return {
          icon: 'lucideCircleX',
          iconColor: 'alert-dest-icon',
          confirmBtn: 'alert-dest-btn',
        };
      case 'warning':
        return {
          icon: 'lucideTriangleAlert',
          iconColor: 'alert-warn-icon',
          confirmBtn: 'alert-default-btn',
        };
      case 'success':
        return {
          icon: 'lucideCircleCheck',
          iconColor: 'alert-success-icon',
          confirmBtn: 'alert-success-btn',
        };
      default:
        return {
          icon: 'lucideInfo',
          iconColor: 'alert-default-icon',
          confirmBtn: 'alert-default-btn',
        };
    }
  }
}
