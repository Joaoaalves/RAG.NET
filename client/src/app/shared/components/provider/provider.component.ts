import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toast } from 'ngx-sonner';
import {
  Provider,
  ProviderData,
  SupportedProvider,
} from 'src/app/models/provider';
import { ProvidersService } from 'src/app/services/providers.service';
import { provideIcons } from '@ng-icons/core';
import { NgIcon } from '@ng-icons/core';
import { AlertComponent } from '../alert/alert.component';
import {
  lucideCheck,
  lucideCircleArrowOutUpRight,
  lucideCircleX,
  lucideKey,
  lucidePencil,
  lucideTrash,
} from '@ng-icons/lucide';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AlertComponent,
    NgIcon,
  ],
  providers: [
    provideIcons({
      lucideCheck,
      lucideTrash,
      lucidePencil,
      lucideKey,
      lucideCircleArrowOutUpRight,
      lucideCircleX,
    }),
  ],
  standalone: true,
})
export class ProviderComponent implements OnInit, OnChanges {
  @Input() Provider!: Provider;

  providerId = '';
  form!: FormGroup;
  providerData: ProviderData | undefined;

  hasApiKey: boolean = false;
  isUpdating: boolean = false;
  lastValidValue: string = '';

  constructor(
    private fb: FormBuilder,
    private providersService: ProvidersService
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['Provider'] && changes['Provider'].currentValue) {
      this.initializeProvider();
    }
  }

  ngOnInit() {
    this.initializeProvider();
  }

  initializeProvider() {
    if (!this.Provider) return;

    this.form = this.fb.group({
      apiKey: ['', Validators.required],
    });

    this.providerData = this.providersService.mapProviderData(
      this.Provider.provider.toLowerCase() as SupportedProvider
    );

    this.hasApiKey = !!this.Provider.apiKey;
    this.providerId = this.Provider?.id;

    if (this.Provider.apiKey && this.providerData?.keyTemplate) {
      const visibleKey = this.providerData.keyTemplate + this.Provider.apiKey;
      this.lastValidValue = visibleKey;
      this.form.patchValue({ apiKey: visibleKey });
      this.disableInput();
    }

    this.form.get('apiKey')?.valueChanges.subscribe((value: string) => {
      if (value && this.providerData?.keyTemplate) {
        const lastPart = value.slice(-4);
        this.Provider.apiKey = lastPart;
      }
    });
  }

  navigateToProvider() {
    if (this.providerData?.apiKeyUrl) {
      window.open(this.providerData.apiKeyUrl, '_blank');
    } else {
      toast.error('Unable to open link', {
        description: 'No URL found for this provider.',
      });
    }
  }

  startEdition() {
    this.lastValidValue = this.form.get('apiKey')!.value;
    this.enableInput();
    this.isUpdating = true;
    this.form.setValue({ apiKey: '' });
  }

  finishApiKeyEdition(apiKey?: string) {
    this.isUpdating = false;
    if (apiKey) {
      const visibleKey = this.providerData?.keyTemplate + apiKey.slice(-4);
      this.form.setValue({
        apiKey: visibleKey,
      });
    } else {
      this.form.setValue({
        apiKey: this.lastValidValue,
      });
    }

    this.disableInput();
  }

  update() {
    const apiKey: string = this.form.get('apiKey')?.value;

    if (!apiKey || !this.form.valid) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });

      this.finishApiKeyEdition();
      return;
    }

    try {
      this.providersService
        .updateProvider(
          this.providerId,
          apiKey,
          this.Provider.provider.toLowerCase() as SupportedProvider
        )
        .subscribe({
          next: () => {
            toast.success('Provider updated successfully');
            this.finishApiKeyEdition(apiKey);
          },
          error: (msg) => {
            toast.error('Update failed', {
              description: `${msg.error}`,
            });
          },
        });
    } catch (err) {
      toast.error('Update failed', {
        description: `${err}`,
      });
    }
  }

  dismiss() {
    this.isUpdating = false;
    this.form.setValue({
      apiKey: this.lastValidValue,
    });

    this.disableInput();
  }

  delete() {
    this.providersService.deleteProvider(this.providerId).subscribe({
      next: () => {
        toast.success('Provider deleted successfully', {
          description: 'The provider has been deleted successfully.',
        });

        this.form.reset();
        this.hasApiKey = false;
        this.providerId = '';
        this.enableInput();
      },
      error: (msg) => {
        toast.error('An error occurred', {
          description: msg,
        });
      },
    });
  }

  add() {
    const apiKey = this.form.get('apiKey')?.value;

    if (!apiKey || !this.form.valid) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });
      return;
    }
    try {
      this.providersService
        .addProvider(
          this.Provider.provider.toLowerCase() as SupportedProvider,
          apiKey
        )
        .subscribe({
          next: (response) => {
            toast.success('Provider saved successfully', {
              description: 'The provider has been saved successfully.',
            });
            this.hasApiKey = true;
            this.providerId = response.id;
            this.finishApiKeyEdition(apiKey);
          },
          error: (msg) => {
            toast.error('An error occurred', {
              description: msg,
            });
          },
        });
    } catch (err) {
      toast.error('An error occurred', {
        description: `${err}`,
      });
    }
  }

  private disableInput() {
    this.form.get('apiKey')!.disable();
  }

  private enableInput() {
    this.form.get('apiKey')!.enable();
  }
}
