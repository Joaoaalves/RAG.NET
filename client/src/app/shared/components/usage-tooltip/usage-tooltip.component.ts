import {
  Component,
  Input,
  ViewChild,
  ElementRef,
  TemplateRef,
  ViewContainerRef,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideCircleHelp } from '@ng-icons/lucide';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-usage-tooltip',
  standalone: true,
  imports: [CommonModule, NgIcon],
  providers: [provideIcons({ lucideCircleHelp })],
  templateUrl: './usage-tooltip.component.html',
  styles: [
    `
      :host {
        display: inline-block;
        position: relative;
      }
      .trigger {
        cursor: pointer;
      }
    `,
  ],
})
export class UsageTooltipComponent implements AfterViewInit, OnDestroy {
  @ViewChild('tooltipTrigger', { static: true }) triggerRef!: ElementRef;
  @ViewChild('tooltipTemplate') tooltipTemplateRef!: TemplateRef<any>;

  private overlayRef!: OverlayRef;

  constructor(private overlay: Overlay, private vcr: ViewContainerRef) {}

  ngAfterViewInit(): void {
    const positionStrategy = this.overlay
      .position()
      .flexibleConnectedTo(this.triggerRef)
      .withPositions([
        {
          originX: 'center',
          originY: 'top',
          overlayX: 'center',
          overlayY: 'bottom',
          offsetY: -8,
        },
      ]);

    this.overlayRef = this.overlay.create({
      positionStrategy,
      hasBackdrop: false,
      scrollStrategy: this.overlay.scrollStrategies.reposition(),
    });
  }

  showTooltip() {
    if (!this.overlayRef.hasAttached()) {
      const tooltipPortal = new TemplatePortal(
        this.tooltipTemplateRef,
        this.vcr
      );
      this.overlayRef.attach(tooltipPortal);
    }
  }

  hideTooltip() {
    if (this.overlayRef.hasAttached()) {
      this.overlayRef.detach();
    }
  }

  ngOnDestroy(): void {
    this.overlayRef?.dispose();
  }
}
