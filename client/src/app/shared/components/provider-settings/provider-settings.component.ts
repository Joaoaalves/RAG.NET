import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
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
import {
  debounceTime,
  map,
  Observable,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { ConversationModel } from 'src/app/models/chat';
import { EmbeddingModel } from 'src/app/models/embedding';
import { ProviderSelectService } from 'src/app/services/provider-select.service';
import { SelectComponent } from '../select/select.component';
import { ConfirmationComponent } from './confirmation.component';
import { ModelSpeedPipe } from 'src/app/components/new-workflow/model-speed.pipe';

@Component({
  selector: 'app-provider-settings',
  templateUrl: './provider-settings.component.html',
  imports: [
    NgIcon,
    CommonModule,
    ReactiveFormsModule,
    SelectComponent,
    ConfirmationComponent,
    ModelSpeedPipe,
  ],
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
export class ProviderSettingsComponent implements OnInit, OnChanges {
  @Input() currentEmbedding!: { provider: number; model: string };
  @Input() currentConversation!: { provider: number; model: string };

  @Output() saveEmbeddingEvent = new EventEmitter<{
    embeddingProvider: {
      provider: number;
      model: string;
    };
  }>();
  @Output() saveConversationEvent = new EventEmitter<{
    conversationProvider: {
      provider: number;
      model: string;
    };
  }>();

  embeddingForm!: FormGroup;
  conversationForm!: FormGroup;

  embeddingOptions$!: Observable<{ value: string | number; label: string }[]>;
  conversationOptions$!: Observable<
    { value: string | number; label: string }[]
  >;
  embeddingModels$!: Observable<EmbeddingModel[]>;
  conversationModels$!: Observable<ConversationModel[]>;

  selectedEmbeddingModel$!: Observable<EmbeddingModel | null>;
  selectedConversationModel$!: Observable<ConversationModel | null>;

  showConfirmEmbedding = false;
  pendingEmbeddingValue: { provider: number; model: string } | null = null;

  showConfirmConversation = false;
  pendingConversationValue: { provider: number; model: string } | null = null;

  constructor(private fb: FormBuilder, private ps: ProviderSelectService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['currentEmbedding'] &&
      !changes['currentEmbedding'].firstChange
    ) {
      this.embeddingForm.patchValue({
        provider: this.currentEmbedding.provider,
        model: this.currentEmbedding.model,
      });
    }
    if (
      changes['currentConversation'] &&
      !changes['currentConversation'].firstChange
    ) {
      this.conversationForm.patchValue({
        provider: this.currentConversation.provider,
        model: this.currentConversation.model,
      });
    }
  }

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
      debounceTime(10),
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
      debounceTime(10),
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

    const embModel = this.embeddingForm.get('model')!;

    this.selectedEmbeddingModel$ = embModel?.valueChanges.pipe(
      startWith(embModel.value),
      withLatestFrom(this.embeddingModels$),
      map(([val, list]) => list.find((m) => m.value === val) ?? null)
    );

    const convModel = this.conversationForm.get('model')!;

    this.selectedConversationModel$ = convModel?.valueChanges.pipe(
      startWith(convModel.value),
      withLatestFrom(this.conversationModels$),
      map(([val, list]) => list.find((m) => m.value === val) ?? null)
    );
  }

  changeEmbeddingProvider(): void {
    const selectedProvider = this.embeddingForm.get('provider')!.value;
    const selectedModel = this.embeddingForm.get('model')!.value;

    if (
      selectedProvider !== this.currentEmbedding.provider ||
      selectedModel !== this.currentEmbedding.model
    ) {
      this.pendingEmbeddingValue = {
        provider: selectedProvider,
        model: selectedModel,
      };
      this.showConfirmEmbedding = true;
    } else {
      this.applyEmbeddingChange();
    }
  }

  confirmEmbedding(): void {
    if (this.pendingEmbeddingValue !== null) {
      this.embeddingForm
        .get('provider')!
        .setValue(this.pendingEmbeddingValue.provider);

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

    this.saveEmbeddingEvent.emit({
      embeddingProvider: {
        provider,
        model,
      },
    });
  }

  changeConversationProvider(): void {
    const selectedProvider = this.conversationForm.get('provider')!.value;
    const selectedModel = this.conversationForm.get('model')!.value;

    if (
      selectedProvider !== this.currentConversation.provider ||
      selectedModel !== this.currentConversation.model
    ) {
      this.pendingConversationValue = {
        provider: selectedProvider,
        model: selectedModel,
      };

      this.showConfirmConversation = true;
    } else {
      this.applyConversationChange();
    }
  }

  confirmConversation(): void {
    if (this.pendingConversationValue !== null) {
      this.conversationForm
        .get('provider')!
        .setValue(this.pendingConversationValue.provider);

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

    this.saveConversationEvent.emit({
      conversationProvider: {
        provider,
        model,
      },
    });
  }
}
