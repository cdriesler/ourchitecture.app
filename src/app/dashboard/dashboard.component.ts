import { NavigationService } from './../../services/navigation-service.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ArgumentManifest, ArgumentPremise, ConceptStatementArguments, ProjectStatementArguments} from '../../models/argument';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  @Output() sendReady = new EventEmitter<boolean>();

  intentActive: boolean;

  conceptStatementArguments: ArgumentManifest[] = ConceptStatementArguments;
  selectedArgument: ArgumentManifest;

  constructor(private _navigationService: NavigationService) { }

  onToggleIntent() {
    this.intentActive = !this.intentActive;
  }

  onSelectArg(arg: ArgumentManifest) {
    this.selectedArgument = arg;

    var sel = Number(arg.number[0]);

    if (isNaN(sel)) {
      this._navigationService.emitChange(true);
    }
  }

  ngOnInit() {
    this.intentActive = false;
  }

  makeReady(val: boolean) {
    this.sendReady.emit(true);
  }

}
