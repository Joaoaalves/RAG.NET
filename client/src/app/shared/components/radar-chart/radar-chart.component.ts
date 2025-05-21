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
        borderColor: '#e12afb00',
        borderWidth: 1,
        pointBackgroundColor: '#e12afb',
        pointBorderColor: '#e12afb',
        pointRadius: 5,
        backgroundColor: [] as any,
      },
    ],
  };

  public chartOptions: ChartOptions<'radar'> = {
    responsive: true,
    maintainAspectRatio: false,
    aspectRatio: 1,
    layout: {
      padding: 0,
    },
    scales: {
      r: {
        angleLines: { display: false },
        min: 0,
        max: 100,
        grid: { color: '#444' },
        pointLabels: {
          color: '#999',
          font: { size: 12 },
          padding: 0,
        },
        ticks: {
          display: false,
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

      const ctx = chartRef.ctx;
      const gradientBackground = ctx.createLinearGradient(
        0,
        0,
        chartRef.width,
        chartRef.height
      );
      const gradientBorder = ctx.createLinearGradient(
        0,
        0,
        chartRef.width,
        chartRef.height
      );
      console.log(chartRef.width, chartRef.height);

      gradientBackground.addColorStop(0, '#ff205650');
      gradientBackground.addColorStop(0.5, '#e12afb50');
      gradientBackground.addColorStop(0.8, '#00a6f450');

      gradientBorder.addColorStop(0, '#ff2056');
      gradientBorder.addColorStop(0.5, '#e12afb');
      gradientBorder.addColorStop(0.8, '#00a6f4');

      this.chartData.datasets[0].backgroundColor = gradientBackground;
      this.chartData.datasets[0].borderColor = gradientBorder;
      this.chart.update();
    });
  }
}
