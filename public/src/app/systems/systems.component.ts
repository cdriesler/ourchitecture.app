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
  }

  languageContainsSelectedDialect(lang:Language) : boolean {
    let contains = false;

    if (this.selectedDialect == undefined) { return false } 

    for (let dialect of lang.dialects) {
      if (dialect.name == this.selectedDialect.name) {
        return true;
      }
    }

    return false;
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
