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
  @Input() exists!: boolean;

  @Output() delete = new EventEmitter<void>();
  @Output() toggleIsActive = new EventEmitter<void>();

  isDeletable: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isActive'] || changes['exists']) {
      this.isDeletable = this.exists && !this.isActive;
    }
  }

  onDelete() {
    this.delete.emit();
  }

  onToggleIsActive() {
    if (this.exists) {
      this.toggleIsActive.emit();
      this.isDeletable = !this.isActive;
      return;
    }

    this.isActive = !this.isActive;
  }
}
