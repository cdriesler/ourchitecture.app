import "dart:html";
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
  selector: "rhinotest",
  templateUrl: "rhinotest_component.html",
  directives: [coreDirectives, routerDirectives],
  styleUrls: ["rhinotest_component.css"],
)
class RhinotestComponent implements OnInit {
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

  RhinotestComponent(this._boxService, this._submissionService);

  //Input selection methods.
  void onInputSelect(String input) {
    activeInput = input;
  }

  void onButton() {
    print(rhino3dm().Point3dList(1, 2, 3));
    /*
    CustomEvent event = new CustomEvent("dog", detail: {
      "X" : 2,
      "Y" : 2,
      "Z" : 3,
    });

    document.dispatchEvent(event);

    //print(event.detail["info"]);
    */
  }

  @override
  void ngOnInit() async {

  }
}