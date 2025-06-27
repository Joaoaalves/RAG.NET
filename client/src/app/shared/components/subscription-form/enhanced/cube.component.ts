import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-cube',
  templateUrl: './cube.component.html',
  standalone: true,
})
export class CubeComponent implements OnInit {
  @ViewChild('canvasRef', { static: true })
  canvasRef!: ElementRef<HTMLCanvasElement>;
  enhancedVisible = false;

  ngOnInit(): void {
    setTimeout(() => (this.enhancedVisible = true), 300);
    this.animateCube();
  }

  private animateCube() {
    const canvas = this.canvasRef.nativeElement;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    canvas.width = 400;
    canvas.height = 400;

    let rotation = 0;
    let pulsePhase = 0;

    const draw = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      const centerX = canvas.width / 2;
      const centerY = canvas.height / 2;
      const size = 80;
      const pulse = Math.sin(pulsePhase) * 0.3 + 1;
      pulsePhase += 0.01;

      const vertices = [
        [-size, -size, -size],
        [size, -size, -size],
        [size, size, -size],
        [-size, size, -size],
        [-size, -size, size],
        [size, -size, size],
        [size, size, size],
        [-size, size, size],
      ];

      const rotated = vertices.map(([x, y, z]) => {
        const cosR = Math.cos(rotation);
        const sinR = Math.sin(rotation);
        const newX = x * cosR - z * sinR;
        const newZ = x * sinR + z * cosR;
        const newY = y * cosR - newZ * sinR * 0.5;
        return [centerX + newX * pulse, centerY + newY * pulse, newZ];
      });

      const edges = [
        [0, 1],
        [1, 2],
        [2, 3],
        [3, 0],
        [4, 5],
        [5, 6],
        [6, 7],
        [7, 4],
        [0, 4],
        [1, 5],
        [2, 6],
        [3, 7],
      ];

      edges.forEach(([start, end]) => {
        const [x1, y1] = rotated[start];
        const [x2, y2] = rotated[end];
        const gradient = ctx.createLinearGradient(x1, y1, x2, y2);
        gradient.addColorStop(0, `rgba(217,70,239,${0.8 * pulse})`);
        gradient.addColorStop(0.5, `rgba(244,63,94,${1 * pulse})`);
        gradient.addColorStop(1, `rgba(14,165,233,${0.6 * pulse})`);
        ctx.strokeStyle = gradient;
        ctx.lineWidth = 2;
        ctx.shadowColor = 'rgba(217,70,239,0.6)';
        ctx.shadowBlur = 20;
        ctx.beginPath();
        ctx.moveTo(x1, y1);
        ctx.lineTo(x2, y2);
        ctx.stroke();
      });

      rotation += 0.005;
      requestAnimationFrame(draw);
    };

    draw();
  }
}
