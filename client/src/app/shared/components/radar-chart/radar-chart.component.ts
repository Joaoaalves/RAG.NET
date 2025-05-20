import {
  Component,
  AfterViewInit,
  ViewChild,
  ChangeDetectionStrategy,
  Input,
} from '@angular/core';
import { BaseChartDirective, provideCharts } from 'ng2-charts';
import { ChartConfiguration, ChartOptions } from 'chart.js';
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
})
export class RadarChartComponent implements AfterViewInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  @Input() set axes(value: RadarAxis[]) {
    if (value) this.dataService.setAxes(value);
  }

  public chartData: ChartConfiguration<'radar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        borderColor: 'transparent',
        pointBackgroundColor: '#d946ef',
        pointBorderColor: '#d946ef',
        pointRadius: 5,
        backgroundColor: [],
      },
    ],
  };

  public chartOptions: ChartOptions<'radar'> = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      r: {
        angleLines: { display: false },
        grid: { color: '#444' },
        pointLabels: {
          color: '#999',
          font: { size: 12 },
        },
        ticks: {
          display: false,
          stepSize: 25,
          z: 1,
        },
        suggestedMin: 0,
        suggestedMax: 100,
      },
    },
    plugins: { legend: { display: false } },
  };

  constructor(private dataService: RadarDataService) {}

  ngAfterViewInit(): void {
    this.dataService.axes$.subscribe((axes) => {
      if (!this.chart || !this.chart.chart) return;
      const chartRef = this.chart.chart;

      this.chartData.labels = axes.map((ax) => ax.name);

      const normalized = axes.map(
        (ax) => ((ax.current - ax.min) / (ax.max - ax.min)) * 100
      );
      this.chartData.datasets[0].data = normalized;

      const ctx = chartRef.ctx;
      const gradient = ctx.createLinearGradient(0, 0, chartRef.width!, 0);
      gradient.addColorStop(0, '#f43f5e');
      gradient.addColorStop(0.5, '#d946ef');
      gradient.addColorStop(1, '#0ea5e9');

      this.chartData.datasets[0].backgroundColor = gradient as any;

      // Trigger update
      this.chart.update();
    });
  }
}
