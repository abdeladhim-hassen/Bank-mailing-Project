import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanActivateFn, CanActivateChildFn } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthService } from '../auth.service';

export const AuthGuard: CanActivateFn  = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree => {
  const authService = inject(AuthService); // Use inject() for dependency injection
  const router = inject(Router);

  return authService.isUserAuthenticated().pipe(
    map((authenticated: boolean) => {
      if (authenticated) {
        return true; // User is authenticated, allow access
      } else {
        // User is not authenticated, redirect to login page
        return router.createUrlTree(['/auth'], {
          queryParams: { returnUrl: state.url }, // Save the original URL
        });
      }
    }),
    catchError(() => {
      // Handle any errors (optional)
      return of(false); // Deny access
    })
  );
};
