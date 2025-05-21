import {
  Component,
  AfterViewInit,
  ViewChild,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import {
  RadarDataService,
  RadarAxis,
} from '../../../services/radar-data.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-radar-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './radar-chart.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: `
  :host{
    display: contents;
    overflow: visible;
  }
  `,
})
export class RadarChartComponent implements AfterViewInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  @Input() set axes(value: RadarAxis[]) {
    if (value) this.dataService.setAxes(value);
  }

  public chartType: ChartType = 'radar';

  public chartData: ChartConfiguration<'radar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        borderWidth: 1,
        pointRadius: 3,
      },
    ],
  };

  public chartOptions: ChartOptions<'radar'> = {
    responsive: false,
    maintainAspectRatio: false,
    aspectRatio: 1,
    layout: {
      padding: 0,
    },
    elements: {
      line: {
        tension: 0.15,
      },
    },
    scales: {
      r: {
        angleLines: { display: false },
        min: 0,
        max: 100,
        grid: { color: '#333' },
        pointLabels: {
          color: '#999',
          font: { size: 12 },
          padding: 0,
        },
        ticks: {
          display: false,
          stepSize: 20,
          color: '#333',
        },
      },
    },
    plugins: { legend: { display: false } },
  };

  constructor(private dataService: RadarDataService) {}

  ngAfterViewInit(): void {
    this.dataService.axes$.subscribe((axes) => {
      if (!this.chart || !this.chart.chart) {
        return;
      }

      const chartRef = this.chart.chart;

      this.chartData.labels = axes.map((ax) => ax.name);
      const normalized = axes.map(
        (ax) => ((ax.current - ax.min) / (ax.max - ax.min)) * 100
      );
      this.chartData.datasets[0].data = normalized;

      const createGradient = () => {
        const ctx = chartRef.ctx;
        const { width, height } = chartRef;
        const centerX = width / 2;
        const centerY = height / 2;
        const radius = Math.min(width, height) / 2;

        const transparentGradient = ctx.createRadialGradient(
          centerX,
          centerY,
          0,
          centerX,
          centerY,
          radius
        );
        transparentGradient.addColorStop(0.6, '#ff205650');
        transparentGradient.addColorStop(0.3, '#e12afb50');
        transparentGradient.addColorStop(0, '#00a6f450');

        const solidGradient = ctx.createRadialGradient(
          centerX,
          centerY,
          0,
          centerX,
          centerY,
          radius
        );
        solidGradient.addColorStop(0.6, '#ff2056');
        solidGradient.addColorStop(0.3, '#e12afb');
        solidGradient.addColorStop(0, '#00a6f4');

        return { transparentGradient, solidGradient };
      };

      const { transparentGradient, solidGradient } = createGradient();

      this.chartData.datasets[0].backgroundColor = transparentGradient;
      this.chartData.datasets[0].borderColor = solidGradient;
      this.chartData.datasets[0].pointBorderColor = solidGradient;
      this.chartData.datasets[0].pointBackgroundColor = solidGradient;

      this.chart.update();
    });
  }
}
