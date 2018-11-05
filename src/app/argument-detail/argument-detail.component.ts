import { DashboardRouteService } from './../../services/dashboard-route.service';
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
  selectedPremise: ArgumentPremise;

  constructor(private _navigationService: NavigationService, private _dashboardRouteService: DashboardRouteService) { }

  onSelectPremise(premise: ArgumentPremise) {
    this.selectedPremise = premise;
    //this.makeReady.emit(true);

    this._navigationService.emitChange(true);
    this._dashboardRouteService.emitChange("/intent/" + this.arg.number[0] + "/" + premise.number)
  }

  ngOnInit() {
  }

}
