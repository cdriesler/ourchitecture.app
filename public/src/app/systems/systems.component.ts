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
  selectedDialectName: string;

  constructor(private http:HttpClient) { }

  onSelectDialect(lang: Language, name:string) {
    if (!this.isDialectLoaded(lang, name)) return;
    
    this.selectedDialectName = name;
    this.languageContainsSelectedDialect(lang);
  }

  getDialectByName(lang:Language, name:string) : DialectManifest {
    if (lang.dialects != undefined) {
      return lang.dialects.find(x => x.name == name);
    }
    
    return undefined;
  }

  languageContainsSelectedDialect(lang:Language) : boolean {
    let contains = false;

    if (this.selectedDialectName == undefined) { return false } 

    for (let dialect of lang.dialects) {
      if (dialect.name == this.selectedDialectName) {
        this.selectedDialect = dialect;
        return true;
      }
    }
    
    return false;
  }

  isFirstDialect(lang:Language, dialectName:string) : boolean {
    if (!this.isDialectLoaded(lang, dialectName)) return false;

    if (lang.dialects[0].name == dialectName) return true;

    return false;
  }

  isDialectLoaded(lang:Language, dialectName:string) : boolean {
    return lang.dialects.find(x => x.name == dialectName) != undefined;
  }

  ngOnInit() {
    this.http.get("https://ourchitecture.app/api/").subscribe((res)=>{

    for (let language of res["languages"]) {
      let lang = new Language(language);

      this.languages.push(lang);    

      for (let dialect of lang.dialectNames) {
        this.http.get("https://ourchitecture.app/api/box/" + lang.name + "/" + dialect).subscribe((result) => {
          lang.dialects.push(new DialectManifest(result));
          //console.log(dialect + " loaded")
        })
      }
    }
    });
  }

}
