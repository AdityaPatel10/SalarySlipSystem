import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window === 'undefined') {
    return true;
  }

  const token = localStorage.getItem('jwt_token');
  if (token) {
    return true;
  }

  router.navigate(['/']);
  return false;
};
