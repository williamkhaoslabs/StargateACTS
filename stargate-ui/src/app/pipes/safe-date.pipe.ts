import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({ name: 'safeDate', standalone: true })
export class SafeDatePipe implements PipeTransform {
  private datePipe = new DatePipe('en-US');

  transform(value: string | Date | null | undefined, format: string = 'mediumDate', fallback: string = '—'): string {
    if (!value) return fallback;

    const parsed = new Date(value);
    if (isNaN(parsed.getTime())) return fallback;

    return this.datePipe.transform(parsed, format) ?? fallback;
  }
}
