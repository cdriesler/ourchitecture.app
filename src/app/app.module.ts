import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AngularFirestoreModule } from 'angularfire2/firestore';
import { AngularFireModule } from 'angularfire2';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CiceroComponent } from './cicero/cicero.component';
import { ArgumentDetailComponent } from './argument-detail/argument-detail.component';
import { environment } from 'src/environments/environment';
import { ArgConceptComponent } from './arg-concept/arg-concept.component';
import { ArgAnalyticComponent } from './arg-analytic/arg-analytic.component';
import { ArgVernacularComponent } from './arg-vernacular/arg-vernacular.component';
import { ArgPrecedentsComponent } from './arg-precedents/arg-precedents.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    CiceroComponent,
    ArgumentDetailComponent,
    ArgConceptComponent,
    ArgAnalyticComponent,
    ArgVernacularComponent,
    ArgPrecedentsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFirestoreModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
