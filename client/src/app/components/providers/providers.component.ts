import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HlmToasterComponent } from 'libs/ui/ui-sonner-helm/src/lib/hlm-toaster.component';
import {
  GetProvidersResponse,
  SupportedProvider,
} from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';
import { ProviderComponent } from 'src/app/shared/components/provider/provider.component';

@Component({
  standalone: true,
  imports: [CommonModule, HlmToasterComponent, ProviderComponent],
  templateUrl: './providers.component.html',
})
export class ProvidersComponent implements OnInit {
  providers: GetProvidersResponse = [];
  constructor(private providersService: ProvidersService) {}

  ngOnInit(): void {
    this.getUserProviders();
  }

  getProviderApiKey(provider: string) {
    if (this.providers) {
      return (
        this.providers
          .filter((prov) => prov.provider.toLowerCase() === provider)
          .at(0)?.apiKey ?? ''
      );
    }

    return '';
  }

  getUserProviders() {
    this.providersService.getUserProviders().subscribe((response) => {
      this.providers = response;
    });
  }
}
