import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';

import { AuthComponent } from './auth/auth/auth.component';
import { AuthGuard } from './Services/Guards/auth.guard';
import { NoAuthGuard } from './Services/Guards/noauth.guard';
import { AdminGuard } from './Services/Guards/admin.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: async () => (await (import('./user/user.module'))).UserModule,
    canActivate: [AuthGuard],
  },
  {
    path: 'auth',
    component: AuthComponent,
    loadChildren: async () => (await (import('./auth/auth.module'))).AuthModule,
    canActivate: [NoAuthGuard],
  },
  {
    path: 'admin',
    loadChildren: async () => (await (import('./admin/admin.module'))).AdminModule,
    canActivate: [AdminGuard],
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
