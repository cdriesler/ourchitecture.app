import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cicero',
  templateUrl: './cicero.component.html',
  styleUrls: ['./cicero.component.css']
})
export class CiceroComponent implements OnInit {

  ciceroTools: String[] = ["line", "box"];
  selectedTool: String;

  allVerbs: String[] = ["regulate", "concentrate"]
  selectedVerb: String = '';

  activeEndPoints: String[] = [];
  activeCornerPoints: String[] = [];

  output: String = "";

  constructor() {

  }

  onEndPointSelect(coord: String) {

    var coords = coord.split(',');
    var coordX = coords[0];
    var coordY = coords[1];

    //Confirm points are not on the same edge.
    if (this.activeEndPoints.length == 1) {
      var currentCoords = this.activeEndPoints[0].split(',');
      var currentX = currentCoords[0];
      var currentY = currentCoords[1];

      if ((coordX == "0" && currentX == coordX) || (coordX == "6" && currentX == coordX) || (coordY == "0" && currentY == coordY) || (coordY == "6" && currentY == coordY)) {
        //warning = "Please do not select points on the same edge.";
        return;
      }
    }
    if (this.activeEndPoints.length == 2) {
      var currentCoords = this.activeEndPoints[0].split(',');
      var currentX = currentCoords[0];
      var currentY = currentCoords[1];

      if ((coordX == "0" && currentX == coordX) || (coordX == "6" && currentX == coordX) || (coordY == "0" && currentY == coordY) || (coordY == "6" && currentY == coordY)) {
        //warning = "Please do not select points on the same edge.";
        return;
      }
    }

      //Add end points to list.
      if (this.activeEndPoints.length < 2) {
        this.activeEndPoints.push(coord);
      }
      else {
        this.activeEndPoints[1] = this.activeEndPoints[0];
        this.activeEndPoints[0] = coord;
      }
  }

  onCornerPointSelect(coord: String) {

    var coords = coord.split(',');
    var coordX = coords[0];
    var coordY = coords[1];

    //Confirm points are not colinear.
    if (this.activeCornerPoints.length >= 1) {
      var current = this.activeCornerPoints[0].split(',');
      var currentX = current[0];
      var currentY = current[1];

      if (coordX == currentX || coordY == currentY) {
        //warning = "Please do not select colinear corner points.";
        return;
      }
    }

    //Add corner points to list.
    if (this.activeCornerPoints.length < 2) {
      this.activeCornerPoints.push(coord);
    }
    else {
      this.activeCornerPoints[1] = this.activeCornerPoints[0];
      this.activeCornerPoints[0] = coord;
    }

  }

  onCommit() {
    if (this.selectedTool == "line") {
      var data = "#input_line:((" + this.activeEndPoints[0] + "),(" + this.activeEndPoints[1] + "))";

      this.output = this.output + data;

      document.getElementById("output").textContent = this.output.toString();
    }

    if (this.selectedTool == "box") {
      var data = "#box_" + "null" + "/" + this.selectedVerb + ":((" + this.activeCornerPoints[0] + "),(" + this.activeCornerPoints[1] + "))";

      this.output = this.output + data;

      document.getElementById("output").textContent = this.output.toString();
    }

    this.clearCache();
  }

  onSelectTool(tool: String) {
    this.selectedTool = tool;

    if (this.selectedTool == "line") {
      this.selectedVerb == '';
    }
  }

  onSelectVerb(verb: String) {
    this.selectedVerb = verb;
  }

  clearCache() {
    this.activeCornerPoints[0] = '';
    this.activeCornerPoints[1] = '';
    this.activeEndPoints[0] = '';
    this.activeEndPoints[1] = '';
  }

  ngOnInit() {
    this.selectedTool = "none";
  }

}
