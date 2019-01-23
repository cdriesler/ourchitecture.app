import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from "@angular/common/http";

import { DialectInputStep } from './../../models/dialect_manifest';

@Component({
  selector: 'app-box',
  templateUrl: './box.component.html',
  styleUrls: ['./box.component.css']
})
export class BoxComponent implements OnInit, OnDestroy {
  private sub: any;

  activeLanguage: string;
  activeDialect: string;
  activeDialectVersion: string = "";
  activeDialectDescription: string;
  activeDialectSteps: DialectInputStep[] = [];
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
      this.activeLanguage = params["language"];
      this.activeDialect = params["dialect"];      
    });

    this.http.get("https://ourchitecture.app/api/box/" + this.activeLanguage + "/" + this.activeDialect).subscribe((res)=>{
      this.activeDialectVersion = res["version"];
      this.activeDialectDescription = res["description"];

      let steps = res["inputSteps"];

      for (let step of steps) {
        console.log(step);
          this.activeDialectSteps.push(new DialectInputStep(step));
      }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
