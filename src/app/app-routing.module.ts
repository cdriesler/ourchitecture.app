import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { CiceroComponent } from './cicero/cicero.component';
import { ArgConceptComponent } from './arg-concept/arg-concept.component';
import { ArgVernacularComponent } from './arg-vernacular/arg-vernacular.component';
import { ArgAnalyticComponent } from './arg-analytic/arg-analytic.component';
import { ArgPrecedentsComponent } from './arg-precedents/arg-precedents.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'},
  { path: 'dashboard', component: DashboardComponent },
  { path: 'system/cicero', component: CiceroComponent },
  { path: 'intent/1/1', component: ArgAnalyticComponent},
  { path: 'intent/3/1', component: ArgVernacularComponent},
  { path: 'intent/B', component: ArgConceptComponent },
  { path: 'intent/C', component: ArgPrecedentsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
