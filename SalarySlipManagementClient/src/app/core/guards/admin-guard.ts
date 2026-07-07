import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  if (typeof window !== 'undefined') {
    const role = localStorage.getItem('user_role');
    if (role === 'Admin') return true;

    router.navigate(['/employee']);
    return false;
  }
  return true;
};
