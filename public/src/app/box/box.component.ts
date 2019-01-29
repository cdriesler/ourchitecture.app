import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from "@angular/common/http";

import { DialectManifest, DialectInputStep } from './../../models/dialect_manifest';

@Component({
  selector: 'app-box',
  templateUrl: './box.component.html',
  styleUrls: ['./box.component.css']
})
export class BoxComponent implements OnInit, OnDestroy {
  private sub: any;

  activeLanguageName: string;
  activeDialectName: string;

  activeDialect: DialectManifest;

  activeStep: number = -1;
  
  started: boolean;
  sessionId: string;
  defaultSessionId: string;

  constructor(private route: ActivatedRoute, private http:HttpClient) { }

  onStart() {
    this.sessionId = (<HTMLInputElement>document.getElementById("session-id")).value;
    this.started = true;
    this.activeStep = 0;
  }

  ngOnInit() {
    this.defaultSessionId = "abcdef";

    this.sub = this.route.params.subscribe(params => {
      this.activeLanguageName = params["language"];
      this.activeDialectName = params["dialect"];      
    });

    this.http.get("https://ourchitecture.app/api/box/" + this.activeLanguageName + "/" + this.activeDialectName).subscribe((res)=>{
      this.activeDialect = new DialectManifest(res);
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
