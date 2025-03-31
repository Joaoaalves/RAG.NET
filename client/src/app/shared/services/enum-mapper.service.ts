import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EnumMapperService {
  mapEnumToOptions<T extends object>(
    enumObject: T
  ): { label: string; value: number }[] {
    return Object.keys(enumObject)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({
        label: key.replace(/_/g, ' '),
        value: (enumObject as any)[key],
      }));
  }
}
