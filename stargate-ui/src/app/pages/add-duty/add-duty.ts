import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { StargateService } from '../../services/stargate.service';
import { Person } from '../../models/person.model';
import { ToastService } from '../../shared/toast/toast';
import {ConfirmModal} from '../../shared/confirm-modal/confirm-modal';


@Component({
  selector: 'app-add-duty',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ConfirmModal],
  templateUrl: './add-duty.html',
  styleUrl: './add-duty.scss'
})
export class AddDuty implements OnInit {
  // Form fields
  personName = '';
  rank = '';
  dutyTitle = '';
  dutyStartDate = '';

  // Typeahead
  people: Person[] = [];
  filteredPeople: Person[] = [];
  showSuggestions = false;

  // State
  submitting = false;
  errors: Record<string, string> = {};
  showRetireConfirm = false;

  constructor(
    private stargate: StargateService,
    private toast: ToastService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // Pre-fill name from query param
    const qName = this.route.snapshot.queryParamMap.get('name');
    if (qName) this.personName = qName;

    // Load people for typeahead
    this.stargate.getPeople().subscribe({
      next: res => this.people = res.people
    });
  }

  onNameInput() {
    this.errors['name'] = '';
    const term = this.personName.toLowerCase().trim();
    if (term.length > 0) {
      this.filteredPeople = this.people.filter(p =>
        p.name.toLowerCase().includes(term)
      ).slice(0, 6);
      this.showSuggestions = this.filteredPeople.length > 0;
    } else {
      this.showSuggestions = false;
    }
  }

  selectPerson(name: string) {
    this.personName = name;
    this.showSuggestions = false;
  }

  hideSuggestions() {
    setTimeout(() => this.showSuggestions = false, 200);
  }

  validate(): boolean {
    this.errors = {};
    if (!this.personName.trim()) this.errors['name'] = 'Person name is required.';
    if (!this.rank.trim()) this.errors['rank'] = 'Rank is required.';
    if (!this.dutyTitle.trim()) this.errors['title'] = 'Duty title is required.';
    if (!this.dutyStartDate) this.errors['date'] = 'Start date is required.';
    return Object.keys(this.errors).length === 0;
  }

  get isRetired(): boolean {
    return this.dutyTitle.trim().toUpperCase() === 'RETIRED';
  }

  get summaryText(): string {
    const parts: string[] = [];
    if (this.rank.trim()) parts.push(this.rank.trim());
    if (this.dutyTitle.trim()) parts.push(this.dutyTitle.trim());
    const title = parts.length > 0 ? parts.join(' / ') : 'Untitled assignment';
    const person = this.personName.trim() || 'unselected person';
    const date = this.dutyStartDate
      ? new Date(this.dutyStartDate + 'T00:00:00').toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })
      : 'no date selected';
    return `${title} for ${person}, starting ${date}.`;
  }

  onSubmit() {
    if (!this.validate() || this.submitting) return;

    // Show confirmation modal for RETIRED duties
    if (this.isRetired) {
      this.showRetireConfirm = true;
      return;
    }

    this.submitDuty();
  }

  onRetireConfirmed() {
    this.showRetireConfirm = false;
    this.submitDuty();
  }

  onRetireCancelled() {
    this.showRetireConfirm = false;
  }

  private submitDuty() {
    this.submitting = true;

    this.stargate.createAstronautDuty({
      name: this.personName.trim(),
      rank: this.rank.trim(),
      dutyTitle: this.dutyTitle.trim(),
      dutyStartDate: this.dutyStartDate
    }).subscribe({
      next: () => {
        this.toast.show(
          `Duty assigned: ${this.rank.trim()} / ${this.dutyTitle.trim()} for ${this.personName.trim()}.`,
          'success'
        );
        this.submitting = false;
        this.router.navigate(['/duties', this.personName.trim()]);
      },
      error: (err) => {
        const msg = err?.message || 'Failed to create duty.';
        this.toast.show(msg, 'error');
        this.submitting = false;
      }
    });
  }
}
