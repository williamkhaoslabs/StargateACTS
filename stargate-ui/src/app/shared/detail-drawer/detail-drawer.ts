import { Component, Input, Output, EventEmitter, HostListener, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { StargateService } from '../../services/stargate.service';
import { Person } from '../../models/person.model';
import { AstronautDuty } from '../../models/astronaut-duty.model';
import { StatusBadge } from '../status-badge/status-badge';
import { LoadingSpinner } from '../loading-spinner/loading-spinner';

@Component({
  selector: 'app-detail-drawer',
  standalone: true,
  imports: [CommonModule, RouterLink, StatusBadge, LoadingSpinner],
  templateUrl: './detail-drawer.html',
  styleUrl: './detail-drawer.scss',
})
export class DetailDrawer implements OnChanges {
  @Input() personName = '';
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();

  person: Person | null = null;
  duties: AstronautDuty[] = [];
  loading = false;
  error = '';

  constructor(private stargate: StargateService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['personName'] && this.personName && this.isOpen) {
      this.loadData();
    }
    if (changes['isOpen'] && this.isOpen && this.personName) {
      this.loadData();
    }
  }

  private loadData() {
    this.loading = true;
    this.error = '';
    this.person = null;
    this.duties = [];
    this.stargate.getAstronautDutiesByName(this.personName).subscribe({
      next: res => {
        this.person = res.person;
        this.duties = res.astronautDuties;
        this.loading = false;
      },
      error: err => {
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
      month: 'short', day: 'numeric', year: 'numeric'
    });
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    if (this.isOpen) this.close.emit();
  }
}

