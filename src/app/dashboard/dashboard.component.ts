import { DashboardRouteService } from './../../services/dashboard-route.service';
import { SystemProfiles, System } from './../../models/system';
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
  systemActive: boolean;

  conceptStatementArguments: ArgumentManifest[] = ConceptStatementArguments;
  selectedArgument: ArgumentManifest;

  systemProfiles: System[] = SystemProfiles;
  selectedSystem: System;

  constructor(private _navigationService: NavigationService, private _dashboardRouteService: DashboardRouteService) { }

  onToggleIntent() {
    this.intentActive = !this.intentActive;
    this.systemActive = false;
  }

  onSelectArg(arg: ArgumentManifest) {
    this.selectedArgument = arg;

    var sel = Number(arg.number[0]);

    if (isNaN(sel)) {
      this._navigationService.emitChange(true);
      this._dashboardRouteService.emitChange("/intent/" + arg.number[0]);
    }
  }

  onToggleSystem() {
    this.systemActive = !this.systemActive;
    this.intentActive = false;
  }

  onSelectSys(sys: System) {
    this.selectedSystem = sys;

    this._navigationService.emitChange(true);
    this._dashboardRouteService.emitChange("/system/" + sys.name.toLowerCase());
  }

  ngOnInit() {
    this.intentActive = false;
    this.systemActive = false;
  }

  makeReady(val: boolean) {
    this.sendReady.emit(true);
  }

}
