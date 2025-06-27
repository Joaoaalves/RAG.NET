import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-orbital-system',
  templateUrl: './orbital-system.component.html',
  standalone: true,
})
export class OrbitalSystemComponent implements OnInit {
  @ViewChild('orbitalCanvasRef', { static: true })
  orbitalCanvasRef!: ElementRef<HTMLCanvasElement>;

  ascendVisible = false;
  ngOnInit(): void {
    setTimeout(() => (this.ascendVisible = true), 600);
    this.animateOrbitalSystem();
  }
  private animateOrbitalSystem() {
    const canvas = this.orbitalCanvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    canvas.width = 600;
    canvas.height = 600;

    let time = 0;
    const particles = Array.from({ length: 12 }).map((_, i) => ({
      angle: (i / 12) * Math.PI * 2,
      radius: 80 + Math.random() * 180,
      speed: 0.01 + Math.random() * 0.005,
      size: 3 + Math.random() * 4,
      color: i % 3 === 0 ? '#f43f5e' : i % 3 === 1 ? '#d946ef' : '#0ea5e9',
    }));

    const draw = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      const centerX = canvas.width / 2;
      const centerY = canvas.height / 2;

      const core = ctx.createRadialGradient(
        centerX,
        centerY,
        0,
        centerX,
        centerY,
        50
      );
      core.addColorStop(0, 'rgba(217,70,239,1)');
      core.addColorStop(0.5, 'rgba(244,63,94,0.8)');
      core.addColorStop(1, 'rgba(14,165,233,0.3)');
      ctx.fillStyle = core;
      ctx.shadowColor = 'rgba(217,70,239,0.8)';
      ctx.shadowBlur = 20;
      ctx.beginPath();
      ctx.arc(centerX, centerY, 15 + Math.sin(time * 2) * 3, 0, Math.PI * 2);
      ctx.fill();

      particles.forEach((p) => {
        p.angle += p.speed;
        const x = centerX + Math.cos(p.angle) * p.radius;
        const y = centerY + Math.sin(p.angle) * p.radius * 0.6;

        const gradient = ctx.createRadialGradient(x, y, 0, x, y, p.size * 2);
        gradient.addColorStop(0, p.color);
        gradient.addColorStop(1, 'transparent');

        ctx.fillStyle = gradient;
        ctx.shadowColor = p.color;
        ctx.shadowBlur = 15;
        ctx.beginPath();
        ctx.arc(x, y, p.size, 0, Math.PI * 2);
        ctx.fill();

        ctx.strokeStyle = `${p.color}20`;
        ctx.lineWidth = 1;
        ctx.beginPath();
        ctx.ellipse(
          centerX,
          centerY,
          p.radius,
          p.radius * 0.6,
          0,
          0,
          Math.PI * 2
        );
        ctx.stroke();
      });

      time += 0.02;
      requestAnimationFrame(draw);
    };

    draw();
  }
}
