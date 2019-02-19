import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SystemsComponent } from './systems/systems.component';
import { BoxComponent } from './box/box.component';

const routes: Routes = [
  { path: "", redirectTo: "/systems", pathMatch: "full" },
  { path: "systems", component: SystemsComponent},
  { path: "box/:language/:dialect", component: BoxComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
