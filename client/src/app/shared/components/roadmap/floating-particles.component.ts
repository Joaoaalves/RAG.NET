import {
  Component,
  OnInit,
  AfterViewInit,
  OnDestroy,
  ViewChild,
  ElementRef,
  HostListener,
} from '@angular/core';

@Component({
  selector: 'app-floating-particles',
  templateUrl: './floating-particles.component.html',
  styleUrls: ['./floating-particles.component.css'],
})
export class FloatingParticlesComponent implements AfterViewInit, OnDestroy {
  @ViewChild('canvasRef', { static: true })
  canvasRef!: ElementRef<HTMLCanvasElement>;

  private ctx!: CanvasRenderingContext2D;
  private particles: Particle[] = [];
  private animationId!: number;
  private particleCount = 50;

  ngAfterViewInit(): void {
    const canvas = this.canvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) {
      console.error('Canvas 2D context não disponível');
      return;
    }
    this.ctx = ctx;
    this.setCanvasDimensions();
    this.initParticles();
    this.animate();
  }

  @HostListener('window:resize')
  onResize(): void {
    this.setCanvasDimensions();
  }

  ngOnDestroy(): void {
    cancelAnimationFrame(this.animationId);
  }

  private setCanvasDimensions(): void {
    const canvas = this.canvasRef.nativeElement;
    const dpr = window.devicePixelRatio || 1;
    canvas.width = canvas.offsetWidth * dpr;
    canvas.height = canvas.offsetHeight * dpr;
    this.ctx.scale(dpr, dpr);
  }

  private initParticles(): void {
    this.particles = [];
    for (let i = 0; i < this.particleCount; i++) {
      this.particles.push(new Particle(this.canvasRef.nativeElement, this.ctx));
    }
  }

  private animate = (): void => {
    const canvas = this.canvasRef.nativeElement;
    const width = canvas.width / (window.devicePixelRatio || 1);
    const height = canvas.height / (window.devicePixelRatio || 1);

    this.ctx.clearRect(0, 0, width, height);

    this.particles.forEach((p) => {
      p.update();
      p.draw();
    });

    this.animationId = requestAnimationFrame(this.animate);
  };
}

class Particle {
  x!: number;
  y!: number;
  size!: number;
  speedX!: number;
  speedY!: number;
  color!: string;
  opacity!: number;
  life = 0;
  maxLife!: number;
  private ctx: CanvasRenderingContext2D;
  private canvas: HTMLCanvasElement;

  constructor(canvas: HTMLCanvasElement, ctx: CanvasRenderingContext2D) {
    this.canvas = canvas;
    this.ctx = ctx;
    this.reset();
  }

  reset(): void {
    const dpr = window.devicePixelRatio || 1;
    const w = this.canvas.width / dpr;
    const h = this.canvas.height / dpr;

    this.x = Math.random() * w;
    this.y = Math.random() * h;
    this.size = Math.random() * 1.5 + 0.1;
    this.speedX = Math.random() * 0.3 - 0.15;
    this.speedY = Math.random() * 0.3 - 0.15;
    this.opacity = Math.random() * 0.5 + 0.1;
    this.life = 0;
    this.maxLife = Math.random() * 100 + 50;

    const colors = [
      'rgba(244,114,182,0.8)',
      'rgba(217,70,239,0.8)',
      'rgba(14,165,233,0.8)',
    ];
    this.color = colors[Math.floor(Math.random() * colors.length)];
  }

  update(): void {
    this.x += this.speedX;
    this.y += this.speedY;
    this.life++;

    if (this.life > this.maxLife * 0.7) {
      this.opacity *= 0.98;
    }

    const dpr = window.devicePixelRatio || 1;
    const w = this.canvas.width / dpr;
    const h = this.canvas.height / dpr;
    if (
      this.life >= this.maxLife ||
      this.x < 0 ||
      this.x > w ||
      this.y < 0 ||
      this.y > h
    ) {
      this.reset();
    }
  }

  draw(): void {
    this.ctx.beginPath();
    this.ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);

    this.ctx.fillStyle = this.color.replace(/0\.8\)$/, `${this.opacity})`);

    this.ctx.shadowBlur = 5;
    this.ctx.shadowColor = this.color;
    this.ctx.fill();

    this.ctx.shadowBlur = 0;
  }
}
