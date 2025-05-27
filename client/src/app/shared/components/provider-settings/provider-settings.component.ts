import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { ionStar, ionStarHalf, ionStarOutline } from '@ng-icons/ionicons';
import {
  lucideChevronDown,
  lucideClock,
  lucideCode,
  lucideDatabase,
  lucideDollarSign,
  lucideInfo,
  lucideMessageCircle,
  lucideStar,
  lucideZap,
} from '@ng-icons/lucide';
import { Observable, startWith, switchMap, tap } from 'rxjs';
import { ConversationModel } from 'src/app/models/chat';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ProviderSelectService } from 'src/app/services/provider-select.service';
import { SelectComponent } from '../select/select.component';

@Component({
  selector: 'app-provider-settings',
  templateUrl: './provider-settings.component.html',
  imports: [NgIcon, CommonModule, ReactiveFormsModule, SelectComponent],
  providers: [
    provideIcons({
      lucideZap,
      lucideDatabase,
      lucideInfo,
      lucideDollarSign,
      lucideClock,
      lucideStar,
      ionStar,
      ionStarHalf,
      ionStarOutline,
      lucideChevronDown,
      lucideCode,
      lucideMessageCircle,
    }),
  ],
  standalone: true,
})
export class ProviderSettingsComponent implements OnInit {
  @Input() currentEmbedding!: { provider: number; model: string };
  @Input() currentConversation!: { provider: number; model: string };

  embeddingForm!: FormGroup;
  conversationForm!: FormGroup;

  embeddingOptions$!: Observable<{ value: string | number; label: string }[]>;
  conversationOptions$!: Observable<
    { value: string | number; label: string }[]
  >;
  embeddingModels$!: Observable<EmbeddingModel[]>;
  conversationModels$!: Observable<ConversationModel[]>;

  showConfirmEmbedding = false;
  pendingEmbeddingValue: number | null = null;

  showConfirmConversation = false;
  pendingConversationValue: number | null = null;

  constructor(private fb: FormBuilder, private ps: ProviderSelectService) {}

  ngOnInit(): void {
    this.initForms();
    this.setupOptionsAndModels();
  }

  private initForms(): void {
    this.embeddingForm = this.fb.group({
      provider: [this.currentEmbedding.provider, Validators.required],
      model: [this.currentEmbedding.model, Validators.required],
    });
    this.conversationForm = this.fb.group({
      provider: [this.currentConversation.provider, Validators.required],
      model: [this.currentConversation.model, Validators.required],
    });
  }

  private setupOptionsAndModels(): void {
    this.embeddingOptions$ = this.ps.getEmbeddingProviders();
    this.conversationOptions$ = this.ps.getConversationProviders();

    const embProv = this.embeddingForm.get('provider')!;
    this.embeddingModels$ = embProv.valueChanges.pipe(
      startWith(this.currentEmbedding.provider),
      switchMap((id) => this.ps.getEmbeddingModels(id)),
      tap((models) => {
        if (models && models.length > 0) {
          const initialModel = models.find(
            (m) => m.value === this.currentEmbedding.model
          );
          if (initialModel) {
            this.embeddingForm.get('model')!.setValue(initialModel.value);
          } else {
            this.embeddingForm.get('model')!.reset();
          }
        }
      })
    );

    const convProv = this.conversationForm.get('provider')!;
    this.conversationModels$ = convProv.valueChanges.pipe(
      startWith(this.currentConversation.provider),
      switchMap((id) => this.ps.getConversationModels(id)),
      tap((models) => {
        if (models && models.length > 0) {
          const initialModel = models.find(
            (m) => m.value === this.currentConversation.model
          );
          if (initialModel) {
            this.conversationForm.get('model')!.setValue(initialModel.value);
          } else {
            this.conversationForm.get('model')!.reset();
          }
        }
      })
    );
  }

  changeEmbeddingProvider(): void {
    const selectedProvider = this.embeddingForm.get('provider')!.value;
    if (selectedProvider !== this.currentEmbedding) {
      this.pendingEmbeddingValue = selectedProvider;
      this.showConfirmEmbedding = true;
    } else {
      this.applyEmbeddingChange();
    }
  }

  confirmEmbedding(): void {
    if (this.pendingEmbeddingValue !== null) {
      this.currentEmbedding.provider = this.pendingEmbeddingValue;
      this.embeddingForm
        .get('provider')!
        .setValue(this.currentEmbedding.provider);
      this.applyEmbeddingChange();
    }
    this.showConfirmEmbedding = false;
    this.pendingEmbeddingValue = null;
  }

  cancelEmbedding(): void {
    this.showConfirmEmbedding = false;
    this.pendingEmbeddingValue = null;
  }

  private applyEmbeddingChange(): void {
    const provider = this.embeddingForm.get('provider')!.value;
    const model = this.embeddingForm.get('model')!.value;
  }

  changeConversationProvider(): void {
    const selectedProvider = this.conversationForm.get('provider')!.value;
    if (selectedProvider !== this.currentConversation) {
      this.pendingConversationValue = selectedProvider;
      this.showConfirmConversation = true;
    } else {
      this.applyConversationChange();
    }
  }

  confirmConversation(): void {
    if (this.pendingConversationValue !== null) {
      this.currentConversation.provider = this.pendingConversationValue;
      this.conversationForm
        .get('provider')!
        .setValue(this.currentConversation.provider);
      this.applyConversationChange();
    }
    this.showConfirmConversation = false;
    this.pendingConversationValue = null;
  }

  cancelConversation(): void {
    this.showConfirmConversation = false;
    this.pendingConversationValue = null;
  }

  private applyConversationChange(): void {
    const provider = this.conversationForm.get('provider')!.value;
    const model = this.conversationForm.get('model')!.value;
  }
}
