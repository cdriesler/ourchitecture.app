import { NavigationService } from './../services/navigation-service.service';
import { Component, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ NavigationService ]
})
export class AppComponent {
  title = 'boxboxbox';
  onReady = false;

  constructor (private _navigationService: NavigationService) {
    _navigationService.changeEmitted$.subscribe(x => this.makeReady(x));
   }

  makeReady(val: boolean) {
    this.onReady=val;
    console.log("Ready!");
  }

  setReady(val: boolean) {
    this.onReady = val;
  }
}
