import { Component, OnInit } from '@angular/core';
import { ArgumentManifest, ArgumentPremise, ConceptStatementArguments, ProjectStatementArguments} from '../../models/argument';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  intentActive: boolean;

  conceptStatementArguments: ArgumentManifest[] = ConceptStatementArguments;
  selectedArgument: ArgumentManifest;

  constructor() { }



  onToggleIntent() {
    this.intentActive = !this.intentActive;
  }

  onSelectArg(arg: ArgumentManifest) {
    this.selectedArgument = arg;

    console.log(this.selectedArgument.premises.length)
  }

  ngOnInit() {
    this.intentActive = false;
  }

}
