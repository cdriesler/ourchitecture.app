import { DashboardRouteService } from './../services/dashboard-route.service';
import { NavigationService } from './../services/navigation-service.service';
import { Component, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ NavigationService, DashboardRouteService]
})
export class AppComponent {
  title = 'boxboxbox';
  onReady = false;
  currentRoute: string = "/";

  constructor (private _navigationService: NavigationService, private _dashboardRouteService: DashboardRouteService) {
    _navigationService.changeEmitted$.subscribe(x => this.makeReady(x));
    _dashboardRouteService.changeEmitted$.subscribe(x => this.currentRoute = x);
   }

  makeReady(val: boolean) {
    this.onReady=val;
    console.log("Ready!");
  }

  setReady(val: boolean) {
    this.onReady = val;
  }
  
}
