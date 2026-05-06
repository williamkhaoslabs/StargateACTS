import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StargateService } from '../../services/stargate.service';
import { LoadingSpinner } from '../../shared/loading-spinner/loading-spinner';

@Component({
  selector: 'app-activity-log',
  standalone: true,
  imports: [CommonModule, LoadingSpinner],
  templateUrl: './activity-log.html',
  styleUrl: './activity-log.scss'
})
export class ActivityLog implements OnInit {
  logs: any[] = [];
  loading = true;

  constructor(private stargate: StargateService) {}

  ngOnInit() {
    this.stargate.getProcessLogs().subscribe({
      next: logs => {
        this.logs = logs;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  formatTimestamp(ts: string): string {
    return new Date(ts).toLocaleString('en-US', {
      month: 'short', day: 'numeric', year: 'numeric',
      hour: 'numeric', minute: '2-digit'
    });
  }

  getLevelClass(level: string): string {
    switch (level) {
      case 'Information': return 'info';
      case 'Warning': return 'warn';
      case 'Error': return 'error';
      default: return 'info';
    }
  }

  getLevelIcon(level: string): string {
    switch (level) {
      case 'Information': return '✓';
      case 'Warning': return '!';
      case 'Error': return '✕';
      default: return '·';
    }
  }
}
