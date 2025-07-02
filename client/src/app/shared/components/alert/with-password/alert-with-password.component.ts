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
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-alert-with-password',
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
    FormsModule,
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
  templateUrl: './alert-with-password.component.html',
})
export class AlertWithPasswordComponent {
  @Input() id: string = '';
  @Input() title: string = '';
  @Input() description: string = '';
  @Input() confirmText: string = 'Confirm';
  @Input() cancelText: string = 'Cancel';
  @Input() confirmLoading: boolean = false;

  @Output() passwordConfirmed = new EventEmitter<string>();

  password: string = '';

  onConfirmClick(ctx: any) {
    this.passwordConfirmed.emit(this.password);
    ctx.close();
    this.password = '';
  }
}
