import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import { ProviderComponent } from 'src/app/shared/components/provider/provider.component';

@Component({
  standalone: true,
  imports: [CommonModule, HlmToasterComponent, ProviderComponent],
  templateUrl: './providers.component.html',
})
export class ProvidersComponent {}
