import { Component, Input, Output, EventEmitter, HostListener, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="overlay" (click)="onOverlayClick($event)" role="dialog" aria-modal="true" [attr.aria-label]="title">
      <div class="modal" #modalEl>
        <div class="modal-icon" [ngClass]="variant">
          {{ variant === 'danger' ? '⚠' : variant === 'warning' ? '!' : '?' }}
        </div>
        <h2 class="modal-title">{{ title }}</h2>
        <p class="modal-message">{{ message }}</p>
        <div class="modal-actions">
          <button class="btn-cancel" (click)="cancel.emit()" #cancelBtn>{{ cancelText }}</button>
          <button class="btn-confirm" [ngClass]="variant" (click)="confirm.emit()">{{ confirmText }}</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .overlay {
      position: fixed; inset: 0; z-index: 9000;
      background: rgba(15, 23, 42, 0.45);
      backdrop-filter: blur(4px);
      display: grid; place-items: center;
      padding: 24px;
      animation: fadeIn 0.2s ease-out;
    }
    .modal {
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 16px;
      padding: 32px 28px 24px;
      max-width: 440px; width: 100%;
      box-shadow: var(--shadow-lg);
      text-align: center;
      animation: scaleIn 0.25s cubic-bezier(0.22, 1, 0.36, 1);
    }
    .modal-icon {
      width: 56px; height: 56px; border-radius: 50%;
      display: inline-grid; place-items: center;
      font-size: 24px; font-weight: 800;
      margin-bottom: 16px;
    }
    .modal-icon.danger  { background: var(--error-soft); color: var(--error); }
    .modal-icon.warning { background: var(--warning-soft); color: var(--warning); }
    .modal-icon.info    { background: var(--info-bg); color: var(--info-text); }
    .modal-title {
      margin: 0 0 8px; font-size: 20px; font-weight: 800; color: var(--text-primary);
    }
    .modal-message {
      margin: 0 0 24px; font-size: 14px; line-height: 1.6; color: var(--text-muted);
    }
    .modal-actions {
      display: flex; gap: 12px; justify-content: center;
    }
    .btn-cancel, .btn-confirm {
      height: 44px; padding: 0 24px; border-radius: 8px;
      font-size: 14px; font-weight: 700; cursor: pointer;
      font-family: inherit; border: 1px solid; transition: all 0.15s;
    }
    .btn-cancel {
      background: var(--bg-surface); color: var(--text-body);
      border-color: var(--border-input);
    }
    .btn-cancel:hover { background: var(--bg-hover); }
    .btn-confirm {
      color: #fff; border-color: transparent;
    }
    .btn-confirm.danger  { background: #dc2626; }
    .btn-confirm.danger:hover { background: #b91c1c; }
    .btn-confirm.warning { background: #d97706; }
    .btn-confirm.warning:hover { background: #b45309; }
    .btn-confirm.info    { background: #2563eb; }
    .btn-confirm.info:hover { background: #1d4ed8; }

    @keyframes fadeIn {
      from { opacity: 0; } to { opacity: 1; }
    }
    @keyframes scaleIn {
      from { opacity: 0; transform: scale(0.9); }
      to   { opacity: 1; transform: scale(1); }
    }
  `]
})
export class ConfirmModal implements AfterViewInit {
  @Input() title = 'Are you sure?';
  @Input() message = '';
  @Input() confirmText = 'Confirm';
  @Input() cancelText = 'Cancel';
  @Input() variant: 'danger' | 'warning' | 'info' = 'warning';

  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  @ViewChild('cancelBtn') cancelBtn!: ElementRef<HTMLButtonElement>;

  ngAfterViewInit() {
    // Focus the cancel button by default for safety
    setTimeout(() => this.cancelBtn?.nativeElement?.focus(), 50);
  }

  @HostListener('document:keydown.escape')
  onEscape() { this.cancel.emit(); }

  onOverlayClick(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('overlay')) {
      this.cancel.emit();
    }
  }
}
