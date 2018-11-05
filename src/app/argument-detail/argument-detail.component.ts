import { NavigationService } from './../../services/navigation-service.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ArgumentManifest, ArgumentPremise, ConceptStatementArguments, ProjectStatementArguments} from '../../models/argument';

@Component({
  selector: 'app-argument-detail',
  templateUrl: './argument-detail.component.html',
  styleUrls: ['./argument-detail.component.css']
})
export class ArgumentDetailComponent implements OnInit {

  @Input() arg: ArgumentManifest;
  @Output() makeReady = new EventEmitter<boolean>();
  selectedPremise: string;

  constructor(private _navigationService: NavigationService) { }

  onSelectPremise(premise: string) {
    this.selectedPremise = premise;
    //this.makeReady.emit(true);

    this._navigationService.emitChange(true);
  }

  ngOnInit() {
  }

}
