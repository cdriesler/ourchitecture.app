import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RootComponent } from './root/root.component';
import { CiceroComponent } from './cicero/cicero.component';
import { SystemsComponent } from './systems/systems.component';

const routes: Routes = [
  { path: "", redirectTo: "/systems", pathMatch: "full" },
  { path: "systems", component: SystemsComponent},
  { path: "root", component: RootComponent},
  { path: "root/test", component: RootComponent},
  { path: "cicero", component: CiceroComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
