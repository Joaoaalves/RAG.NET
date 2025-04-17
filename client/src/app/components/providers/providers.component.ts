import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';

@Component({
  standalone: true,
  imports: [CommonModule, HlmToasterComponent],
  templateUrl: './providers.component.html',
})
export class ProvidersComponent {}
