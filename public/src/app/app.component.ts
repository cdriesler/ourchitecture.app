import { Language } from './../models/language';
import { HttpClient } from "@angular/common/http";
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ourchitecture.app';
  headerActive: boolean;

  languages: Language[] = [];

  constructor(private http:HttpClient) {}

  onActivateHeader() {
    this.headerActive = !this.headerActive;
  }

  ngOnInit() {
    this.http.get("https://ourchitecture.app/api/").subscribe((res)=>{

    //(<any>Object).values(res);

    for (let language of res["languages"]) {
      this.languages.push(new Language(language));
    }

    // for (let lang of this.languages) {
    //   console.log(lang.name);
    // }
  });
  }
}
