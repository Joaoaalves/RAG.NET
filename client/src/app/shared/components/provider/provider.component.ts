import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { toast } from 'ngx-sonner';
import { Provider } from 'src/app/models/provider';
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
import { getProviderImage } from '../../utils/providers-utils';

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
export class ProviderComponent implements OnInit {
  @Input() provider!: Provider;

  providerId!: number;
  form!: FormGroup;

  hasApiKey: boolean = false;
  isUpdating: boolean = false;
  lastValidValue: string = '';

  constructor(
    private fb: FormBuilder,
    private providersService: ProvidersService
  ) {}

  get providerImage(): string {
    return getProviderImage(this.provider.providerId);
  }

  get providerTemplate(): string {
    return this.provider.prefix + '**********';
  }

  ngOnInit() {
    this.form = this.fb.group({
      apiKey: ['', Validators.required],
    });

    this.hasApiKey = !!this.provider.apiKey;
    this.providerId = this.provider.providerId;
    if (this.provider.apiKey) {
      const visibleKey =
        this.provider.prefix + '**********' + this.provider.apiKey;

      this.lastValidValue = visibleKey;

      this.form.patchValue({ apiKey: visibleKey });

      this.disableInput();
    }

    this.form.get('apiKey')?.valueChanges.subscribe((value: string) => {
      if (value) {
        const lastPart = value.slice(-4);
        this.provider.apiKey = lastPart;
      }
    });
  }

  navigateToProvider() {
    window.open(this.provider.url, '_blank');
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
      const visibleKey = this.provider.prefix + '**********' + apiKey.slice(-4);
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

    if (!apiKey || !this.provider.id || !this.form.valid) {
      toast.error('Invalid API Key', {
        description: 'You should provide a valid API Key.',
      });

      this.finishApiKeyEdition();
      return;
    }

    try {
      this.providersService.updateProvider(this.provider, apiKey).subscribe({
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
    if (!this.provider.id)
      toast.error('An error occurred', {
        description: 'You cant delete this provider.',
      });

    this.providersService.deleteProvider(this.provider.id as string).subscribe({
      next: () => {
        toast.success('Provider deleted successfully', {
          description: 'The provider has been deleted successfully.',
        });

        this.form.reset();
        this.hasApiKey = false;

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
      this.providersService.addProvider(this.provider, apiKey).subscribe({
        next: (response) => {
          toast.success('Provider saved successfully', {
            description: 'The provider has been saved successfully.',
          });
          this.hasApiKey = true;

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
