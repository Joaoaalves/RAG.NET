import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { HlmSwitchComponent } from 'libs/ui/ui-switch-helm/src/lib/hlm-switch.component';
import { AlertComponent } from '../alert/alert.component';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideTrash } from '@ng-icons/lucide';

@Component({
  selector: 'app-vector-storage-card',
  templateUrl: './vector-storage-card.component.html',
  imports: [CommonModule, HlmSwitchComponent, AlertComponent, NgIcon],
  providers: [
    provideIcons({
      lucideTrash,
    }),
  ],
  standalone: true,
})
export class VectorStorageCard implements OnChanges {
  @Input() src!: string;
  @Input() providerTitle!: string;
  @Input() isActive!: boolean;
  @Input() isDeletable!: boolean;

  @Output() delete = new EventEmitter<void>();
  @Output() toggleEnable = new EventEmitter<void>();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isActive']) {
      this.isDeletable = !this.isActive;
    }
  }

  onDelete() {
    this.delete.emit();
  }

  onToggleEnable() {
    this.toggleEnable.emit();
  }
}
