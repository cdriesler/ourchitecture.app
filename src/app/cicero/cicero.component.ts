import { Component, OnInit } from '@angular/core';
import { AngularFirestore, DocumentChangeAction } from 'angularfire2/firestore';
import { Observable } from 'rxjs';
import { defineBase } from '@angular/core/src/render3';

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

  currentName: string = "test";

  output: String = "";

  db: AngularFirestore;

  public svgResponse: Observable<any[]>;

  constructor(db: AngularFirestore) {
    //this.svgResponse = db.collection('/results').doc('k')

    this.db = db;



    //console.log(item);
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

      //document.getElementById("output_data").textContent = this.output.toString();
    }

    if (this.selectedTool == "box") {
      var data = "#box_" + "null" + "/" + this.selectedVerb + ":((" + this.activeCornerPoints[0] + "),(" + this.activeCornerPoints[1] + "))";

      this.output = this.output + data;

      //document.getElementById("output_data").textContent = this.output.toString();
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

  onDeploy() {
    var id = this.db.createId();

    var data = {
      payload: this.output,
    }

    var newDoc = this.db.collection('/queue').doc(id).set(data);

    var ref = this.db.collection('/results').doc(id);

    var getDoc = ref.get().toPromise().then(doc => {
      console.log(doc.data());
    })


  }

  ngOnInit() {
    this.selectedTool = "none";
  }

}
