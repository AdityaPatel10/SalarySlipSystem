import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { authGuard } from './core/guards/auth-guard';
import { Dashboard } from './features/admin/dashboard/dashboard';
import { HireEmployee } from './features/admin/hire-employee/hire-employee';
import { MasterData } from './features/admin/master-data/master-data';
import { EmployeePayroll } from './features/admin/employee-payroll/employee-payroll';

export const routes: Routes = [
  { path: '', component: Login },
  {
    path: 'dashboard',
    component: AdminLayout,
    canActivate: [authGuard],
    children: [
      { path: '', component: Dashboard },
      { path: 'hire', component: HireEmployee },
      { path: 'settings', component: MasterData },
      { path: 'payroll/:id', component: EmployeePayroll },
    ],
  },
  { path: '**', redirectTo: '' },
];
