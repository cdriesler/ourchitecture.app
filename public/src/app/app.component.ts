import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ourchitecture.app';
  headerActive: boolean;

  constructor() {}

  onActivateHeader() {
    this.headerActive = !this.headerActive;
  }

  ngOnInit() {
  }
}

//(<any>Object).values(res);
