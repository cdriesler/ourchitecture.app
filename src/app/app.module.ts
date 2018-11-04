import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CiceroComponent } from './cicero/cicero.component';
import { ArgumentDetailComponent } from './argument-detail/argument-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    CiceroComponent,
    ArgumentDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
