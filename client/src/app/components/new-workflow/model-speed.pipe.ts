import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'modelSpeed',
})
export class ModelSpeedPipe implements PipeTransform {
  transform(speed: number): string[] {
    const maxStars = 5;
    const normalized = (speed / 10) * maxStars;

    const fullStars = Math.floor(normalized);
    const hasHalfStar = normalized - fullStars >= 0.5;
    const icons: string[] = [];

    for (let i = 0; i < fullStars; i++) {
      icons.push('ionStar');
    }

    if (hasHalfStar && icons.length < maxStars) {
      icons.push('ionStarHalf');
    }

    while (icons.length < maxStars) {
      icons.push('ionStarOutline');
    }

    return icons;
  }
}
