import { Component, OnInit, Input } from '@angular/core';
import { ArgumentManifest, ArgumentPremise, ConceptStatementArguments, ProjectStatementArguments} from '../../models/argument';

@Component({
  selector: 'app-argument-detail',
  templateUrl: './argument-detail.component.html',
  styleUrls: ['./argument-detail.component.css']
})
export class ArgumentDetailComponent implements OnInit {

  @Input() arg: ArgumentManifest;

  constructor() { }

  ngOnInit() {
  }

}
