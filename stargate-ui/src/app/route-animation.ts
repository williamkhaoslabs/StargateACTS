import { trigger, transition, style, animate, query, group } from '@angular/animations';

export const routeAnimation = trigger('routeAnimation', [
  transition('* <=> *', [
    query(':enter, :leave', [
      style({
        position: 'absolute',
        top: 0,
        left: 0,
        width: '100%'
      })
    ], { optional: true }),
    group([
      query(':leave', [
        animate('200ms ease-out', style({
          opacity: 0,
          transform: 'translateY(8px)'
        }))
      ], { optional: true }),
      query(':enter', [
        style({
          opacity: 0,
          transform: 'translateY(-8px)'
        }),
        animate('300ms 100ms ease-out', style({
          opacity: 1,
          transform: 'translateY(0)'
        }))
      ], { optional: true })
    ])
  ])
]);
