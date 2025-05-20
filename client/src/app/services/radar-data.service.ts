import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface RadarAxis {
  name: string;
  min: number;
  max: number;
  current: number;
}

@Injectable({ providedIn: 'root' })
export class RadarDataService {
  private axesSubject = new BehaviorSubject<RadarAxis[]>([]);
  axes$ = this.axesSubject.asObservable();

  setAxes(axes: RadarAxis[]) {
    this.axesSubject.next(axes);
  }

  updateAxis(name: string, current: number) {
    const axes = this.axesSubject
      .getValue()
      .map((ax) => (ax.name === name ? { ...ax, current } : ax));
    this.axesSubject.next(axes);
  }
}
