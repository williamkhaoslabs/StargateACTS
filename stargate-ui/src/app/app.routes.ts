import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/directory/directory').then(m => m.Directory),
    data: { animation: 'directory' }
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard),
    data: { animation: 'dashboard' }
  },
  {
    path: 'duties/:name',
    loadComponent: () => import('./pages/profile/profile').then(m => m.Profile),
    data: { animation: 'profile' }
  },
  {
    path: 'add-person',
    loadComponent: () => import('./pages/add-person/add-person').then(m => m.AddPerson),
    data: { animation: 'addPerson' }
  },
  {
    path: 'add-duty',
    loadComponent: () => import('./pages/add-duty/add-duty').then(m => m.AddDuty),
    data: { animation: 'addDuty' }
  },
  {
    path: 'activity-log',
    loadComponent: () => import('./pages/activity-log/activity-log').then(m => m.ActivityLog),
    data: { animation: 'activityLog' }
  },
  { path: '**', redirectTo: '' }
];
