import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { StargateService } from '../../services/stargate.service';
import { Person } from '../../models/person.model';
import { AstronautDuty } from '../../models/astronaut-duty.model';
import { StatusBadge } from '../../shared/status-badge/status-badge';
import { Skeleton } from '../../shared/skeleton/skeleton';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterLink, StatusBadge, Skeleton],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  person: Person | null = null;
  duties: AstronautDuty[] = [];
  loading = true;
  error = '';
  personName = '';

  constructor(
    private route: ActivatedRoute,
    private stargate: StargateService
  ) {}

  ngOnInit() {
    this.personName = decodeURIComponent(this.route.snapshot.paramMap.get('name') || '');
    this.loadProfile();
  }

  loadProfile() {
    this.loading = true;
    this.error = '';
    this.stargate.getAstronautDutiesByName(this.personName).subscribe({
      next: res => {
        this.person = res.person;
        this.duties = res.astronautDuties;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message || 'Failed to load profile.';
        this.loading = false;
      }
    });
  }

  getInitials(name: string): string {
    return name.split(' ').map(w => w[0]).join('').substring(0, 2).toUpperCase();
  }

  formatDate(date: string | null): string {
    if (!date) return '—';
    return new Date(date + 'T00:00:00').toLocaleDateString('en-US', {
      month: 'long', day: 'numeric', year: 'numeric'
    });
  }

  getStatus(): string {
    if (!this.person) return 'none';
    if (!this.person.careerStartDate && !this.person.currentDutyTitle) return 'none';
    if (this.person.currentDutyTitle?.toUpperCase() === 'RETIRED') return 'retired';
    return 'active';
  }

  get dutyCount(): number { return this.duties.length; }

  get yearSpan(): string {
    if (!this.person?.careerStartDate) return '—';
    const start = new Date(this.person.careerStartDate).getFullYear();
    const end = this.person.careerEndDate
      ? new Date(this.person.careerEndDate).getFullYear()
      : 'Present';
    return `${start} – ${end}`;
  }
}
