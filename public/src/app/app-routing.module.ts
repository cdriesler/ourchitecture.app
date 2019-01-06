import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RootComponent } from './root/root.component';
import { CiceroComponent } from './cicero/cicero.component';

const routes: Routes = [
  { path: "", redirectTo: "/root", pathMatch: "full" },
  { path: "root", component: RootComponent},
  { path: "root/test", component: RootComponent},
  { path: "cicero", component: CiceroComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
