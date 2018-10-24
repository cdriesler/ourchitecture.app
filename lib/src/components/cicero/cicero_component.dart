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
  selector: "cicero",
  templateUrl: "cicero_component.html",
  directives: [coreDirectives, routerDirectives],
  styleUrls: ["cicero_component.css"],
)
class CiceroComponent implements OnInit {
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

  CiceroComponent(this._boxService, this._submissionService);

  //Input selection methods.
  void onInputSelect(String input) {
    activeInput = input;
  }

  @override
  void ngOnInit() async {

  }
}