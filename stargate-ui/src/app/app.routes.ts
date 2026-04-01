import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/directory/directory').then(m => m.Directory),
    data: { animation: 'directory' }
  },
  { path: '**', redirectTo: '' }
];
