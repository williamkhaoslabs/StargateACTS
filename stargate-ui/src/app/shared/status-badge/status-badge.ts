import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.scss',
})
export class StatusBadge {
  @Input() dutyTitle: string = '';
  @Input() careerStartDate: string | null = null;

  get statusClass(): string {
    if (!this.careerStartDate && !this.dutyTitle) return 'none';
    if (this.dutyTitle?.toUpperCase() === 'RETIRED') return 'retired';
    return 'active';
  }

  get label(): string {
    if (!this.careerStartDate && !this.dutyTitle) return 'No Record';
    if (this.dutyTitle?.toUpperCase() === 'RETIRED') return 'Retired';
    return 'Active';
  }
}
