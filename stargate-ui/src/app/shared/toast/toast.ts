import { Injectable, Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Subscription, timer } from 'rxjs';

export interface ToastItem {
  id: number;
  message: string;
  type: 'success' | 'error' | 'warning';
  autodismiss: boolean;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private _toasts: ToastItem[] = [];
  private nextId = 1;
  toasts$ = new Subject<ToastItem[]>();

  show(message: string, type: 'success' | 'error' | 'warning' = 'success') {
    const autodismiss = type === 'success';
    const toast: ToastItem = {
      id: this.nextId++,
      message,
      type,
      autodismiss
    };

    this._toasts.push(toast);
    this.toasts$.next([...this._toasts]);

    if (autodismiss) {
      timer(4000).subscribe(() => this.dismiss(toast.id));
    }
  }

  dismiss(id: number) {
    this._toasts = this._toasts.filter(t => t.id !== id);
    this.toasts$.next([...this._toasts]);
  }
}

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.html',
  styleUrl: './toast.scss',
})
export class Toast implements OnDestroy {
  toasts: ToastItem[] = [];
  private sub: Subscription;

  constructor(private toastService: ToastService) {
    this.sub = this.toastService.toasts$.subscribe(t => this.toasts = t);
  }

  dismiss(id: number) {
    this.toastService.dismiss(id);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
