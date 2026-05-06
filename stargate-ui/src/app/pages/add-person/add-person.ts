import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { StargateService } from '../../services/stargate.service';
import { ToastService } from '../../shared/toast/toast';

@Component({
  selector: 'app-add-person',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './add-person.html',
  styleUrl: './add-person.scss'
})
export class AddPerson {
  name = '';
  submitting = false;
  nameError = '';

  constructor(
    private stargate: StargateService,
    private toast: ToastService,
    private router: Router
  ) {}

  validate(): boolean {
    this.nameError = '';
    if (!this.name.trim()) {
      this.nameError = 'Name is required.';
      return false;
    }
    return true;
  }

  onSubmit() {
    if (!this.validate() || this.submitting) return;
    this.submitting = true;

    this.stargate.createPerson(this.name.trim()).subscribe({
      next: () => {
        this.toast.show(`${this.name.trim()} was added successfully.`, 'success');
        this.submitting = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.nameError = err?.message || 'Failed to create person.';
        this.toast.show(this.nameError, 'error');
        this.submitting = false;
      }
    });
  }
}
