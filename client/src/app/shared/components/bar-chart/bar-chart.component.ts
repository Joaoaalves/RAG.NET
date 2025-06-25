import {
  Component,
  ViewChild,
  ChangeDetectionStrategy,
  Input,
  ElementRef,
  AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import {
  ChartConfiguration,
  ChartOptions,
  TooltipItem,
  ChartDataset,
} from 'chart.js';

@Component({
  selector: 'app-bar-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './bar-chart.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [
    `
      :host {
        display: block;
        height: 100%;
      }
    `,
  ],
})
export class BarChartComponent implements AfterViewInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;
  @ViewChild('canvasRef') canvasRef?: ElementRef<HTMLCanvasElement>;

  @Input() set labels(value: string[]) {
    this.chartData.labels = value;
    this.chart?.update();
  }

  @Input() set datasets(value: ChartDataset<'bar'>[]) {
    this._datasets = value;
    this.applyGradients();
  }

  private _datasets: ChartDataset<'bar'>[] = [];

  public chartType: 'bar' = 'bar';

  public chartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [],
  };

  public chartOptions: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,

    scales: {
      x: {
        ticks: {
          display: true,
          color: '#aaa',
          font: { weight: 500 },
        },
        grid: { display: false },
        border: {
          display: false,
        },
      },
      y: {
        beginAtZero: true,
        ticks: { display: false },
        grid: { display: false },
        border: {
          display: false,
        },
      },
    },
    plugins: {
      legend: { display: false },
      tooltip: {
        backgroundColor: '#1e1e2f',
        titleColor: '#fff',
        bodyColor: '#ccc',
        cornerRadius: 6,
        callbacks: {
          label: (context: TooltipItem<'bar'>) => {
            const label = context.dataset.label ?? '';
            const value = context.raw as number;
            return `${label}: ${value.toFixed(1)}`;
          },
          title: (items) => items[0].label ?? '',
        },
      },
    },
  };

  ngAfterViewInit(): void {
    this.applyGradients();
  }

  private applyGradients(): void {
    if (!this.canvasRef) return;
    const ctx = this.canvasRef.nativeElement.getContext('2d');
    if (!ctx) return;

    const palette = [
      [
        'rgba(0, 198, 255, 0.6)',
        'rgba(0, 114, 255, 0.1)',
        'rgba(0, 198, 255, 1)',
      ],
      [
        'rgba(255, 117, 140, 0.6)',
        'rgba(255, 126, 179, 0.1)',
        'rgba(255, 117, 140, 1)',
      ],
      [
        'rgba(161, 140, 209, 0.6)',
        'rgba(251, 194, 235, 0.1)',
        'rgba(161, 140, 209, 1)',
      ],
      [
        'rgba(67, 233, 123, 0.6)',
        'rgba(56, 249, 215, 0.1)',
        'rgba(67, 233, 123, 1)',
      ],
    ];

    const datasets = this._datasets.map((ds, i) => {
      const [from, to, border] = palette[i % palette.length];
      const gradient = ctx.createLinearGradient(0, 0, 0, 400);
      gradient.addColorStop(0, from);
      gradient.addColorStop(1, to);

      return {
        ...ds,
        backgroundColor: gradient,
        borderColor: border,
        borderWidth: 1,
        borderRadius: 6,
        barPercentage: 0.6,
      };
    });

    this.chartData.datasets = datasets;
    this.chart?.update();
  }
}
