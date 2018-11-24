import { AllBoxes, BoxManifest, BoxManifestCategory } from './../../models/box';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent implements OnInit {

  boxes: BoxManifestCategory[];
  selectedBox: BoxManifest;

  constructor() { }

  ngOnInit() {
    this.boxes = AllBoxes;
  }

  updateCategory() {

  }

  onSelectBox(box: BoxManifest) {
    if (this.selectedBox == box) {
      this.selectedBox = null;
    }
    else {
      this.selectedBox = box;
    }
  }

}
