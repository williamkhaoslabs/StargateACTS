import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-skeleton',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './skeleton.html',
  styleUrl: './skeleton.scss',
})
export class Skeleton {
  @Input() type: 'table' | 'card' | 'profile' | 'chart' = 'table';
  @Input() count = 6;

  get rows(): number[] { return Array.from({ length: this.count }, (_, i) => i); }
  chartBars = [45, 70, 55, 85, 40, 65, 75, 50];
}
