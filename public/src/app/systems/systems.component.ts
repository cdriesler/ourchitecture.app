import { Language, Dialect} from './../../models/language';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-systems',
  templateUrl: './systems.component.html',
  styleUrls: ['./systems.component.css']
})
export class SystemsComponent implements OnInit {
  languages: Language[] = [];

  selectedDialect: Dialect;
  previousSelectedDialect: Dialect;

  constructor(private http:HttpClient) { }

  onSelectDialect(selectedDialect:Dialect) {
    this.selectedDialect = selectedDialect;
    window.
  }

  ngOnInit() {
    this.http.get("https://ourchitecture.app/api/").subscribe((res)=>{

    for (let language of res["languages"]) {
      this.languages.push(new Language(language));
    }

    // for (let lang of this.languages) {
    //   console.log(lang.name);
    // }
    });
  }

}
