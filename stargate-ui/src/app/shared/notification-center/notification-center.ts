import {
  Injectable, Component, ElementRef, HostListener,
  ViewChild, OnInit, OnDestroy
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Subscription } from 'rxjs';

// ─── Notification Model ───
export interface AppNotification {
  id: number;
  message: string;
  type: 'success' | 'error' | 'info';
  timestamp: Date;
  read: boolean;
  link?: string;
}

// ─── Service ───
@Injectable({ providedIn: 'root' })
export class NotificationService {
  private _notifications: AppNotification[] = [];
  private nextId = 1;
  notifications$ = new Subject<AppNotification[]>();

  get unreadCount(): number {
    return this._notifications.filter(n => !n.read).length;
  }

  get all(): AppNotification[] {
    return [...this._notifications];
  }

  push(message: string, type: 'success' | 'error' | 'info' = 'info', link?: string) {
    this._notifications.unshift({
      id: this.nextId++,
      message,
      type,
      timestamp: new Date(),
      read: false,
      link
    });
    // Keep max 50
    if (this._notifications.length > 50) {
      this._notifications = this._notifications.slice(0, 50);
    }
    this.notifications$.next(this.all);
  }

  markRead(id: number) {
    const n = this._notifications.find(x => x.id === id);
    if (n) n.read = true;
    this.notifications$.next(this.all);
  }

  markAllRead() {
    this._notifications.forEach(n => n.read = true);
    this.notifications$.next(this.all);
  }

  clear() {
    this._notifications = [];
    this.notifications$.next(this.all);
  }
}

// ─── Component ───
@Component({
  selector: 'app-notification-center',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notif-wrap">
      <button
        class="bell-btn"
        (click)="toggle()"
        [attr.aria-expanded]="open"
        aria-label="Notifications"
        aria-haspopup="true"
      >
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/>
          <path d="M13.73 21a2 2 0 0 1-3.46 0"/>
        </svg>
        @if (unreadCount > 0) {
          <span class="badge">{{ unreadCount > 9 ? '9+' : unreadCount }}</span>
        }
      </button>

      @if (open) {
        <div class="dropdown" role="menu" #dropdownEl>
          <div class="dropdown-header">
            <span class="dropdown-title">Notifications</span>
            @if (unreadCount > 0) {
              <button class="mark-read-btn" (click)="markAllRead()">Mark all read</button>
            }
          </div>
          <div class="dropdown-body">
            @if (notifications.length === 0) {
              <div class="empty">No notifications yet.</div>
            } @else {
              @for (n of notifications; track n.id) {
                <div
                  class="notif-item"
                  [class.unread]="!n.read"
                  (click)="onItemClick(n)"
                  role="menuitem"
                >
                  <div class="notif-dot" [ngClass]="n.type"></div>
                  <div class="notif-content">
                    <div class="notif-msg">{{ n.message }}</div>
                    <div class="notif-time">{{ timeAgo(n.timestamp) }}</div>
                  </div>
                </div>
              }
            }
          </div>
          @if (notifications.length > 0) {
            <div class="dropdown-footer">
              <button class="clear-btn" (click)="clearAll()">Clear all</button>
            </div>
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .notif-wrap { position: relative; }

    .bell-btn {
      position: relative;
      background: rgba(255,255,255,0.12);
      border: 1px solid rgba(255,255,255,0.2);
      border-radius: 8px;
      width: 38px; height: 38px;
      display: grid; place-items: center;
      cursor: pointer; color: #fff;
      transition: all 0.2s;
    }
    .bell-btn:hover { background: rgba(255,255,255,0.22); }

    .badge {
      position: absolute; top: -4px; right: -4px;
      min-width: 18px; height: 18px;
      background: #ef4444; color: #fff;
      font-size: 10px; font-weight: 800;
      border-radius: 999px;
      display: grid; place-items: center;
      padding: 0 4px;
      line-height: 1;
      animation: pop 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
    }

    .dropdown {
      position: absolute; top: calc(100% + 10px); right: 0;
      width: 360px; max-height: 440px;
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 12px;
      box-shadow: var(--shadow-lg);
      overflow: hidden; z-index: 2000;
      animation: slideDown 0.2s ease-out;
    }

    .dropdown-header {
      display: flex; align-items: center; justify-content: space-between;
      padding: 14px 16px;
      border-bottom: 1px solid var(--border);
    }
    .dropdown-title { font-weight: 800; font-size: 14px; color: var(--text-primary); }
    .mark-read-btn {
      background: none; border: none; color: var(--chart-1);
      font-size: 12px; font-weight: 700; cursor: pointer;
      font-family: inherit;
    }
    .mark-read-btn:hover { text-decoration: underline; }

    .dropdown-body {
      max-height: 320px; overflow-y: auto;
    }

    .notif-item {
      display: flex; gap: 10px; align-items: flex-start;
      padding: 12px 16px; cursor: pointer;
      border-bottom: 1px solid var(--border-light);
      transition: background 0.1s;
    }
    .notif-item:hover { background: var(--bg-hover); }
    .notif-item:last-child { border-bottom: none; }
    .notif-item.unread { background: var(--bg-elevated); }

    .notif-dot {
      width: 8px; height: 8px; border-radius: 50%;
      flex-shrink: 0; margin-top: 6px;
    }
    .notif-dot.success { background: var(--success); }
    .notif-dot.error   { background: var(--error); }
    .notif-dot.info    { background: var(--chart-1); }

    .notif-msg {
      font-size: 13px; font-weight: 600; color: var(--text-body);
      line-height: 1.4;
    }
    .notif-time {
      font-size: 11px; color: var(--text-faint); margin-top: 2px;
    }

    .empty {
      padding: 32px 16px; text-align: center;
      color: var(--text-faint); font-size: 13px;
    }

    .dropdown-footer {
      padding: 10px 16px; border-top: 1px solid var(--border);
      text-align: center;
    }
    .clear-btn {
      background: none; border: none;
      color: var(--text-muted); font-size: 12px;
      font-weight: 700; cursor: pointer;
      font-family: inherit;
    }
    .clear-btn:hover { color: var(--error); }

    @keyframes pop {
      0% { transform: scale(0); }
      100% { transform: scale(1); }
    }
    @keyframes slideDown {
      from { opacity: 0; transform: translateY(-8px); }
      to   { opacity: 1; transform: translateY(0); }
    }
  `]
})
export class NotificationCenterComponent implements OnInit, OnDestroy {
  open = false;
  notifications: AppNotification[] = [];
  unreadCount = 0;
  private sub!: Subscription;

  @ViewChild('dropdownEl') dropdownEl!: ElementRef;

  constructor(
    private notifService: NotificationService,
    private elRef: ElementRef
  ) {}

  ngOnInit() {
    this.sub = this.notifService.notifications$.subscribe(list => {
      this.notifications = list;
      this.unreadCount = this.notifService.unreadCount;
    });
    // Seed with initial data
    this.notifications = this.notifService.all;
    this.unreadCount = this.notifService.unreadCount;
  }

  ngOnDestroy() { this.sub?.unsubscribe(); }

  toggle() { this.open = !this.open; }

  onItemClick(n: AppNotification) {
    this.notifService.markRead(n.id);
  }

  markAllRead() { this.notifService.markAllRead(); }
  clearAll() { this.notifService.clear(); this.open = false; }

  timeAgo(date: Date): string {
    const seconds = Math.floor((Date.now() - date.getTime()) / 1000);
    if (seconds < 60) return 'Just now';
    const minutes = Math.floor(seconds / 60);
    if (minutes < 60) return `${minutes}m ago`;
    const hours = Math.floor(minutes / 60);
    if (hours < 24) return `${hours}h ago`;
    return `${Math.floor(hours / 24)}d ago`;
  }

  @HostListener('document:click', ['$event'])
  onDocClick(event: MouseEvent) {
    if (this.open && !this.elRef.nativeElement.contains(event.target)) {
      this.open = false;
    }
  }

  @HostListener('document:keydown.escape')
  onEscape() { this.open = false; }
}
