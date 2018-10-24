import "dart:html" as window;
import "dart:async";
import "package:angular/angular.dart";
import "package:angular_router/angular_router.dart";
import "package:angular_forms/angular_forms.dart";
import "package:firebase/firebase.dart";
import 'package:firebase/firestore.dart' as fs;

import "../../formats/box.dart";
import "../../../services/box_service.dart";
import "../../../services/submission_service.dart";
//import "../services/drawing_service.dart";
import "../../route_paths.dart" as paths;

@Component(
  selector: "my-blackbox",
  templateUrl: "blackbox_component.html",
  directives: [coreDirectives, routerDirectives],
  styleUrls: ["blackbox_component.css", "canvas_styles.css"],
)
class BlackBoxComponent implements OnInit {
  List<String> allInputs = List<String>();
  List<String> boxAdverbs = List<String>();
  List<String> boxVerbs = List<String>();

  List<String> activeEndPoints = List<String>();
  List<String> activeCornerPoints = List<String>();

  String mode = "";

  String warning = "";
  bool resultReceived = false;

  String activeInput = "";
  String activeAdverb = "";
  String activeVerb = "";

  final BoxService _boxService;
  final SubmissionService _submissionService;
  //final DrawingService _drawingService;

  BlackBoxComponent(this._boxService, this._submissionService);

  //Input selection methods.
  void onInputSelect(String input) {
    activeInput = input;
  }

  void onAdverbSelect(String adverb) {
    activeAdverb = adverb;
  }

  void onVerbSelect(String verb) {
    activeVerb = verb;
  }


  //Input submission methods.
  void onSubmitInput() {
    if (activeInput == "") {
      warning = "Please select an input!";
      return;
    }

    //Show line guides.
    mode = activeInput;
  }

  void onCommitInput() {
    if (activeEndPoints.length < 2) {
      warning = "Please select two points.";
      return;
    }

    var data = "";

    var div = window.document.getElementById("main-data");

    if (activeInput == "line") {

    data = "#input_line:((" + activeEndPoints[0] + "),(" + activeEndPoints[1] + "))";
    }
    else if (activeInput == "polyline") {

      for (int i = 0; i < activeEndPoints.length - 1; i++) {

          data = data + "#input_line:((" + activeEndPoints[i] + "),(" + activeEndPoints[i+1] + "))";
      }
    }

    div.text = div.text + data;

    mode = "";
    activeEndPoints.clear();
    onClearSelection();

    updateCurrentData();
  }

  void onSubmitBox() {
    if (activeAdverb == "" || activeVerb == "") {
      warning = "Please select both a verb and adverb!";
      return;
    }

    //Show box guides.
    mode = "box";
  }

  void onCommitBox() {
    if (activeCornerPoints.length < 2) {
      warning = "Please select two corner points.";
      return;
    }

    var div = window.document.getElementById("main-data");

    var data = "#box_" + activeAdverb + "/" + activeVerb + ":((" + activeCornerPoints[0] + "),(" + activeCornerPoints[1] + "))";

    div.text = div.text + data;

    mode = "";
    activeCornerPoints.clear();
    onClearSelection;  
  }

  void onEndPointSelect(String coord) {

    var coords = coord.split(',');
    var coordX = coords[0];
    var coordY = coords[1];

    if(activeInput == "polyline") {
      activeEndPoints.add(coord);
      return;
    }

    //Confirm points are not on the same edge.
    if (activeEndPoints.length == 1) {
      var currentCoords = activeEndPoints[0].split(',');
      var currentX = currentCoords[0];
      var currentY = currentCoords[1];

      if ((coordX == "0" && currentX == coordX) || (coordX == "6" && currentX == coordX) || (coordY == "0" && currentY == coordY) || (coordY == "6" && currentY == coordY)) {
        warning = "Please do not select points on the same edge.";
        return;
      }
    }
    if (activeEndPoints.length == 2) {
      var currentCoords = activeEndPoints[0].split(',');
      var currentX = currentCoords[0];
      var currentY = currentCoords[1];

      if ((coordX == "0" && currentX == coordX) || (coordX == "6" && currentX == coordX) || (coordY == "0" && currentY == coordY) || (coordY == "6" && currentY == coordY)) {
        warning = "Please do not select points on the same edge.";
        return;
      }
    }
    
    //Add end points to list.
    if (activeEndPoints.length < 2) {
      activeEndPoints.add(coord);
    }
    else {
      activeEndPoints[1] = activeEndPoints[0];
      activeEndPoints[0] = coord;
    }
    
  }

  void onCornerPointSelect(String coord) {

    var coords = coord.split(',');
    var coordX = coords[0];
    var coordY = coords[1];

    //Confirm points are not colinear.
    if (activeCornerPoints.length >= 1) {
      var current = activeCornerPoints[0].split(',');
      var currentX = current[0];
      var currentY = current[1];

      if (coordX == currentX || coordY == currentY) {
        warning = "Please do not select colinear corner points.";
        return;
      }
    }

    //Add corner points to list.
    if (activeCornerPoints.length < 2) {
      activeCornerPoints.add(coord);
    }
    else {
      activeCornerPoints[1] = activeCornerPoints[0];
      activeCornerPoints[0] = coord;
    }

  }

  /*
  void onSelectEdge(dynamic event) {
    var div = window.document.getElementById("canvas").getBoundingClientRect();

    var point = event.page;

    if (activeLinePoints.length < 2) {
      activeLinePoints.add(point);
    }
    else {
      activeLinePoints[1] = activeLinePoints[0];
      activeLinePoints[0] = point;

      window.DivElement line = new window.DivElement();
    }

    print("Page point: " + event.page.toString());
    print("Normalized point: (" + ((event.page.x - div.topLeft.x)/div.width).toString().substring(0,4) + "," + ((event.page.y - div.topLeft.y)/div.height).toString().substring(0,4) + ")");

    //print(window.document.getElementById("canvas").getBoundingClientRect().topLeft.x.toString());
  }
  */

  //Alert methods.
  void onClearAlert() {
    warning = "";
  }

  void onClearSelection() {
    activeInput = "";
    activeVerb = "";
    activeAdverb = "";

    mode = "";
  }

  void onClearData() {
    var div = window.document.getElementById("main-data");
    div.text = "";

    var data = window.document.getElementById("current-data");
    data.innerHtml = "";
  }

  //SVG utils
  String makeLine(String coordA, String coordB) {
    var coordsA = coordA.split(',');
    var coordsB = coordB.split(',');

    var vals = List<int>();

    var x1 = int.parse(coordsA[0]) * 11;
    var y1 = int.parse(coordsA[1]) * 11;
    var x2 = int.parse(coordsB[0]) * 11;
    var y2 = int.parse(coordsB[1]) * 11;

    vals.add(x1);
    vals.add(y1);
    vals.add(x2);
    vals.add(y2);

    for (int i = 0; i < vals.length; i++) {
      if (vals[0] > 65) {
        vals[0] = 65;
      }
      if (vals[0] == 0) {
        vals[1] = 1;
      }
    }

    var svgData = "<line class='svgdata' x1='" + vals[0].toString() + "vh' y1='" + vals[1].toString() + "' x2='" + vals[2].toString() + "' y2='" + vals[3].toString() + "'/>";

    return svgData;
  }

  void updateCurrentData() {
    var current_data = window.document.getElementById("current-data");

    print(current_data.toString());
    //current_data.text = current_data.text + makeLine(activeEndPoints[0], activeEndPoints[1]);
  }

  final window.NodeValidatorBuilder _htmlValidator = window.NodeValidatorBuilder.common()
  ..allowSvg();

  void SolutionTimeout() {
    if (!resultReceived) {
      warning = "Server request timed out. It may be offline or your request may not be valid. Please try again.";

      onClearData();
      onClearSelection();
    }
  }

  Future<void> onDeployPayload() async {
    resultReceived = false;

    var div = window.document.getElementById("main-data");

    //print(div.text);

    if (div.text == "" || div.text[0]=="g") {
      div.text = "";
      print("Bad input, ignoring.");
      warning = "Payload format was incorrect. Please try again.";
      return;
    }

    var id = await _submissionService.deployPayload(div.text);

    onClearData();
    onClearSelection();

    div.text = "generating solution for request #" + id + ".";

    fs.Firestore store = firestore();
    fs.CollectionReference ref = store.collection("results");

    ref.onSnapshot.listen((snapshot) {  
        snapshot.docs.forEach((x) {
          print(x.id);
          if (x.id == id) {
            var tryGetData = x.get("svg");
            //var tryGetStatus = x.get("status");

            //if (tryGetStatus != null) {
            //  querySelector(".plan_svg").setInnerHtml(x.get("svg"), validator: _htmlValidator);
            //}

            if (tryGetData != null) {
              if (tryGetData.toString().contains("failed")) {
                warning = tryGetData;
                div.text = "";
                mode = "";

                resultReceived = true;
              }
              else {
                window.document.getElementById("current-data").setInnerHtml(x.get("svg"), validator: _htmlValidator);
                div.text = "";

                resultReceived = true;
              }
              //window.querySelector(".plan_svg").setInnerHtml(x.get("svg"), validator: _htmlValidator);
            }
            else {
              warning = "The server failed to deliver a result, please try again.";

              resultReceived = true;
            }
          }
        });

        new Timer(new Duration(seconds: 25), SolutionTimeout);
    });

  }

  @override
  void ngOnInit() async {
    boxAdverbs = (await _boxService.getAllAdverbs());
    boxVerbs = (await _boxService.getAllVerbs());
    //edgeGuides = (await _drawingService.getEdgeGuides());
    //innerGuides = (await _drawingService.getInnerGuides());

    allInputs.add("line");
    allInputs.add("polyline");
  }
}