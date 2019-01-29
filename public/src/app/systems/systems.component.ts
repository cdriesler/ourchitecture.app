import { Language } from 'src/models/language';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { DialectManifest } from 'src/models/dialect_manifest';

@Component({
  selector: 'app-systems',
  templateUrl: './systems.component.html',
  styleUrls: ['./systems.component.css']
})
export class SystemsComponent implements OnInit {
  languages: Language[] = [];

  selectedDialect: DialectManifest;

  constructor(private http:HttpClient) { }

  onSelectDialect(selectedDialect:DialectManifest) {
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
      let lang = new Language(language);

      for (let dialect of lang.dialectNames) {
        this.http.get("https://ourchitecture.app/api/box/" + lang.name + "/" + dialect).subscribe((result) => {
          lang.dialects.push(new DialectManifest(result));
        })
      }

      this.languages.push(lang);
    }
    });
  }

}
