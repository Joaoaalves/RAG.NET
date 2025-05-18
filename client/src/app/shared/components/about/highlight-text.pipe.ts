import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlightText',
  standalone: true,
})
export class HighlightTextPipe implements PipeTransform {
  transform(value: string): string {
    return value.replace(/\*\*(.*?)\*\*/g, (_, match) => {
      return `
        <span class="relative font-semibold">
          <span class="bg-gradient-to-r from-rose-500 via-fuchsia-500 to-sky-500 bg-clip-text text-transparent">
            ${match}
          </span>
          <span class="absolute -left-0.5 bottom-0 w-full h-0.5 bg-gradient-to-r from-rose-500 via-fuchsia-500 to-sky-500"></span>
        </span>
      `;
    });
  }
}
