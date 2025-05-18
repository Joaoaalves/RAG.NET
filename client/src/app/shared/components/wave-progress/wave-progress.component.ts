import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  OnInit,
  OnDestroy,
  AfterViewInit,
  NgZone,
  ChangeDetectorRef,
} from '@angular/core';

@Component({
  selector: 'app-wave-progress',
  templateUrl: './wave-progress.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class WaveProgressComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() progress = 0;
  @Input() width: number | string = '100%';
  @Input() height: number | string = 60;
  @Input() strandCount = 7;
  @Input() waveAmplitude = 3;
  @Input() waveLength = 30;
  @Input() animationSpeed = 10;
  @Input() glowIntensity = 2;
  @Input() className = '';

  phase = 0;
  private requestId!: number;
  private previousTime = 0;

  get actualWidth(): number {
    return typeof this.width === 'number' ? this.width : 500;
  }
  get actualHeight(): number {
    return typeof this.height === 'number' ? this.height : 60;
  }
  get responsiveStrandCount(): number {
    return this.actualWidth < 300
      ? Math.max(3, Math.floor(this.strandCount * 0.7))
      : this.strandCount;
  }
  get responsiveAmplitude(): number {
    return this.actualWidth < 300
      ? Math.max(1, this.waveAmplitude * 0.7)
      : this.waveAmplitude;
  }
  get strandThickness(): number {
    return 1.5;
  }
  get gap(): number {
    return (
      (this.actualHeight - this.responsiveStrandCount * this.strandThickness) /
      (this.responsiveStrandCount - 1)
    );
  }

  constructor(private ngZone: NgZone, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.ngZone.runOutsideAngular(() => this.animate(0));
  }

  ngOnDestroy(): void {
    cancelAnimationFrame(this.requestId);
  }

  private animate(time: number) {
    if (this.previousTime) {
      const delta = time - this.previousTime;
      this.phase =
        (this.phase + delta * 0.001 * this.animationSpeed) % (Math.PI * 2);
      this.cd.detectChanges();
    }
    this.previousTime = time;
    this.requestId = requestAnimationFrame((t) => this.animate(t));
  }

  wavePath(index: number): string {
    const yOffset =
      index * (this.strandThickness + this.gap) + this.strandThickness / 2;
    const segments = Math.ceil(this.actualWidth / this.waveLength) + 1;
    let path = `M 0 ${yOffset}`;
    for (let i = 0; i <= segments; i++) {
      const x = i * this.waveLength;
      const y =
        yOffset +
        Math.sin(i + this.phase + index * 0.3) * this.responsiveAmplitude;
      path += ` L ${x} ${y}`;
    }
    return path;
  }

  pathLength(): number {
    return this.actualWidth * 1.1;
  }
}
