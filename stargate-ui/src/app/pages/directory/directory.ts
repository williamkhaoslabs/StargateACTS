import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { StargateService } from '../../services/stargate.service';
import { Person } from '../../models/person.model';
import { StatusBadge } from '../../shared/status-badge/status-badge';
import { Skeleton } from '../../shared/skeleton/skeleton';
import { DetailDrawer } from '../../shared/detail-drawer/detail-drawer';
import { Toast, ToastService } from '../../shared/toast/toast';

@Component({
  selector: 'app-directory',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, StatusBadge, Skeleton, DetailDrawer],
  templateUrl: './directory.html',
  styleUrl: './directory.scss'
})
export class Directory implements OnInit {
  people: Person[] = [];
  filteredPeople: Person[] = [];
  pagedPeople: Person[] = [];
  searchTerm = '';
  statusFilter = 'all';
  sortField = 'name';
  loading = true;
  error = '';

  currentPage = 1;
  pageSize = 10;
  totalPages = 1;

  editingPersonId: number | null = null;
  editingName = '';
  editError = '';

  drawerOpen = false;
  drawerPersonName = '';

  constructor(
    private stargate: StargateService,
    private toast: ToastService
  ) {}

  ngOnInit() { this.loadPeople(); }

  loadPeople() {
    this.loading = true;
    this.error = '';
    this.stargate.getPeople().subscribe({
      next: res => {
        this.people = res.people;
        this.applyFilters();
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load personnel roster.';
        this.loading = false;
      }
    });
  }

  applyFilters() {
    let result = [...this.people];
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      result = result.filter(p => p.name.toLowerCase().includes(term));
    }
    if (this.statusFilter !== 'all') {
      result = result.filter(p => this.getStatus(p) === this.statusFilter);
    }
    result.sort((a, b) => {
      if (this.sortField === 'name') return a.name.localeCompare(b.name);
      if (this.sortField === 'rank') return (a.currentRank || '').localeCompare(b.currentRank || '');
      if (this.sortField === 'career') return (a.careerStartDate || '9999').localeCompare(b.careerStartDate || '9999');
      return 0;
    });
    this.filteredPeople = result;
    this.currentPage = 1;
    this.paginate();
  }

  paginate() {
    this.totalPages = Math.max(1, Math.ceil(this.filteredPeople.length / this.pageSize));
    if (this.currentPage > this.totalPages) this.currentPage = this.totalPages;
    const start = (this.currentPage - 1) * this.pageSize;
    this.pagedPeople = this.filteredPeople.slice(start, start + this.pageSize);
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.paginate();
  }

  get pageNumbers(): number[] {
    const pages: number[] = [];
    const max = Math.min(this.totalPages, 7);
    let start = Math.max(1, this.currentPage - 3);
    let end = Math.min(this.totalPages, start + max - 1);
    if (end - start < max - 1) start = Math.max(1, end - max + 1);
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  }

  startEdit(person: Person, event: MouseEvent) {
    event.stopPropagation();
    this.editingPersonId = person.personId;
    this.editingName = person.name;
    this.editError = '';
  }

  cancelEdit() {
    this.editingPersonId = null;
    this.editingName = '';
    this.editError = '';
  }

  saveEdit(person: Person) {
    const trimmed = this.editingName.trim();
    if (!trimmed) {
      this.editError = 'Name cannot be empty.';
      return;
    }
    if (trimmed === person.name) {
      this.cancelEdit();
      return;
    }

    const duplicate = this.people.find(
      p => p.name.toLowerCase() === trimmed.toLowerCase() && p.personId !== person.personId
    );
    if (duplicate) {
      this.editError = `"${trimmed}" already exists.`;
      return;
    }
    person.name = trimmed;
    this.toast.show(`Name updated to "${trimmed}".`, 'success');
    this.cancelEdit();
    this.applyFilters();
  }

  onEditKeydown(event: KeyboardEvent, person: Person) {
    if (event.key === 'Enter') {
      event.preventDefault();
      this.saveEdit(person);
    } else if (event.key === 'Escape') {
      this.cancelEdit();
    }
  }

  openDrawer(personName: string, event: MouseEvent) {
    event.stopPropagation();
    this.drawerPersonName = personName;
    this.drawerOpen = true;
  }

  closeDrawer() {
    this.drawerOpen = false;
  }

  getStatus(p: Person): string {
    if (!p.careerStartDate && !p.currentDutyTitle) return 'none';
    if (p.currentDutyTitle?.toUpperCase() === 'RETIRED') return 'retired';
    return 'active';
  }

  getInitials(name: string): string {
    return name.split(' ').map(w => w[0]).join('').substring(0, 2).toUpperCase();
  }

  formatDate(date: string | null): string {
    if (!date) return '—';
    return new Date(date + 'T00:00:00').toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
  }

  get totalCount(): number { return this.people.length; }
  get activeCount(): number { return this.people.filter(p => this.getStatus(p) === 'active').length; }
  get retiredCount(): number { return this.people.filter(p => this.getStatus(p) === 'retired').length; }
}
