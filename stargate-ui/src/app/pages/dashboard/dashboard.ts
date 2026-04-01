import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StargateService } from '../../services/stargate.service';
import { Person } from '../../models/person.model';
import { Skeleton} from '../../shared/skeleton/skeleton';

interface ChartSegment {
  label: string;
  value: number;
  color: string;
  percent: number;
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, Skeleton],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  people: Person[] = [];
  loading = true;
  animated = false;

  // Stats
  totalCount = 0;
  activeCount = 0;
  retiredCount = 0;
  noRecordCount = 0;

  // Chart data
  statusSegments: ChartSegment[] = [];
  rankDistribution: { label: string; value: number; color: string; maxPercent: number }[] = [];
  careerLengths: { name: string; years: number; color: string }[] = [];

  constructor(private stargate: StargateService) {}

  ngOnInit() {
    this.stargate.getPeople().subscribe({
      next: res => {
        this.people = res.people;
        this.computeStats();
        this.loading = false;
        // Trigger animation after a frame
        requestAnimationFrame(() => {
          requestAnimationFrame(() => this.animated = true);
        });
      }
    });
  }

  private computeStats() {
    this.totalCount = this.people.length;
    this.activeCount = this.people.filter(p => this.isActive(p)).length;
    this.retiredCount = this.people.filter(p => this.isRetired(p)).length;
    this.noRecordCount = this.people.filter(p => this.isNone(p)).length;

    // Status distribution for donut
    const total = this.totalCount || 1;
    this.statusSegments = [
      { label: 'Active', value: this.activeCount, color: 'var(--chart-2)', percent: (this.activeCount / total) * 100 },
      { label: 'Retired', value: this.retiredCount, color: 'var(--chart-3)', percent: (this.retiredCount / total) * 100 },
      { label: 'No Record', value: this.noRecordCount, color: 'var(--text-faint)', percent: (this.noRecordCount / total) * 100 },
    ].filter(s => s.value > 0);

    // Rank distribution (horizontal bars)
    const ranks = new Map<string, number>();
    this.people.forEach(p => {
      if (p.currentRank) ranks.set(p.currentRank, (ranks.get(p.currentRank) || 0) + 1);
    });
    const sortedRanks = [...ranks.entries()].sort((a, b) => b[1] - a[1]).slice(0, 6);
    const maxRank = sortedRanks[0]?.[1] || 1;
    const colors = ['var(--chart-1)', 'var(--chart-2)', 'var(--chart-3)', 'var(--chart-4)', 'var(--chart-5)', 'var(--text-muted)'];
    this.rankDistribution = sortedRanks.map(([label, value], i) => ({
      label, value, color: colors[i % colors.length], maxPercent: (value / maxRank) * 100
    }));

    // Career lengths (top 8 longest)
    this.careerLengths = this.people
      .filter(p => p.careerStartDate)
      .map(p => {
        const start = new Date(p.careerStartDate!).getTime();
        const end = p.careerEndDate ? new Date(p.careerEndDate).getTime() : Date.now();
        const years = Math.round((end - start) / (365.25 * 24 * 60 * 60 * 1000));
        return { name: p.name.split(' ').pop()!, years, color: this.isRetired(p) ? 'var(--chart-3)' : 'var(--chart-1)' };
      })
      .sort((a, b) => b.years - a.years)
      .slice(0, 8);
  }

  get donutGradient(): string {
    let cumulative = 0;
    const stops: string[] = [];
    this.statusSegments.forEach(seg => {
      stops.push(`${seg.color} ${cumulative}%`);
      cumulative += seg.percent;
      stops.push(`${seg.color} ${cumulative}%`);
    });
    return `conic-gradient(${stops.join(', ')})`;
  }

  get maxCareerYears(): number {
    return Math.max(...this.careerLengths.map(c => c.years), 1);
  }

  private isActive(p: Person): boolean {
    return !!p.careerStartDate && p.currentDutyTitle?.toUpperCase() !== 'RETIRED';
  }
  private isRetired(p: Person): boolean {
    return p.currentDutyTitle?.toUpperCase() === 'RETIRED';
  }
  private isNone(p: Person): boolean {
    return !p.careerStartDate && !p.currentDutyTitle;
  }

  exportCSV() {
    const headers = ['Name', 'Current Rank', 'Current Duty', 'Career Start', 'Career End', 'Status'];
    const rows = this.people.map(p => [
      p.name,
      p.currentRank || '',
      p.currentDutyTitle || '',
      p.careerStartDate || '',
      p.careerEndDate || '',
      this.isRetired(p) ? 'Retired' : this.isActive(p) ? 'Active' : 'No Record'
    ]);
    const csv = [headers, ...rows].map(r => r.map(c => `"${c}"`).join(',')).join('\n');
    this.downloadFile(csv, 'acts-roster.csv', 'text/csv');
  }

  exportJSON() {
    const json = JSON.stringify(this.people, null, 2);
    this.downloadFile(json, 'acts-roster.json', 'application/json');
  }

  private downloadFile(content: string, filename: string, type: string) {
    const blob = new Blob([content], { type });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    URL.revokeObjectURL(url);
  }
}
