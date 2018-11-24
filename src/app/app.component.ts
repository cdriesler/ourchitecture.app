import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ourchitecture';
  headerActive: boolean;

  onActivateHeader() {
    this.headerActive = !this.headerActive;
  }
}
