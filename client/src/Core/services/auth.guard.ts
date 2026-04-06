import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account-service';

export const authGuard = () => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if (accountService.currentUser()) {
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};