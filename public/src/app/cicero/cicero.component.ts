import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cicero',
  templateUrl: './cicero.component.html',
  styleUrls: ['./cicero.component.css']
})
export class CiceroComponent implements OnInit {

  description$: Observable<string>;

  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.http.get("https://ourchitecture.app/api/cicero").subscribe((res)=>{
      console.log(res);
  });
  }

  

}
