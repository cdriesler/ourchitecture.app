import "dart:async";
import "dart:html";
import "dart:math";
import "package:angular/angular.dart";
import "package:angular_router/angular_router.dart";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

import "../src/formats/project.dart";
import "../src/formats/program.dart";
import "../src/formats/request.dart";
import "../src/formats/floor.dart";
import "../services/project_service.dart";
import "../services/floor_service.dart";
import "../services/program_service.dart";
import "../services/submission_service.dart";
import "route_paths.dart" as paths;

@Component(
  selector: 'my-projects',
  templateUrl: "project_list_component.html",
  directives: [coreDirectives],
  styleUrls: ["project_list_component.css", "request_manifest_zone.css"],
  pipes: [UpperCasePipe],
)
class ProjectListComponent implements OnInit {
  final ProjectService _projectService;
  final FloorService _floorService;
  final ProgramService _programService;
  final SubmissionService _submissionService;

  final Router _router;

  ProjectListComponent(this._projectService, this._floorService, this._programService, this._submissionService, this._router);

  final title = "Desk Jockey";
  List<Project> projects;

  Future<void> _getProjects() async {
    projects = await _projectService.getAll();
  }

  void ngOnInit() => _getProjects();

  String name = "Angular";

  Project selected;
  Floor activeFloor;
  var allFloors = List<Floor>();

  Future<void> _getFloors(Project selectedProject) async {
    allFloors = await _floorService.getAll(selectedProject.id);
  }

  void onSelect(Project project) {
    selected = project;
    pendingRequest = null;
    activeFloor = null;

    //Collect floors for project in floors.
    allFloors.clear();

    _getFloors(selected);

    //allFloors.add("L" + project.id.substring(5, 8));
    //allFloors.add("TYP" + project.id.substring(4, 5));
  } 

  var metadataFields = List<String>();
  var allPrograms = List<Program>();
  var plans = List<Floor>();

  Future<void> _getPrograms(String projectNumber) async {
    allPrograms = await _programService.getAll(projectNumber);
  }

    final NodeValidatorBuilder _htmlValidator = NodeValidatorBuilder.common()
    ..allowSvg();

  void updateSvg(Floor plan) {
    if (pendingRequest != null) {
      return;
    }

    if (plan.svgData == null) {
      querySelector(".plan_svg").setInnerHtml("no drawing available");
      return;
    }

    querySelector(".plan_svg").setInnerHtml(plan.svgData, validator: _htmlValidator);
  }

  void onFloorSelect(Floor floor) {
    activeFloor = floor;
    pendingRequest = null;
    print(floor.floorNumber + " is active.");

    //Load drawing elements and interaction information.
    plans.clear();

    //var activePlan = Plan(projectNumber, floor);
    //activePlan.svgData = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 1561.6 1082.6" style="enable-background:new 0 0 1561.6 1082.6;" xml:space="preserve"> <style type="text/css"> .st0{fill:none;stroke:#000000;stroke-linecap:round;stroke-linejoin:round;} .st1{fill:none;stroke:#3F3836;stroke-linecap:round;stroke-linejoin:round;stroke-dasharray:14.1732,14.1732;} .st2{fill:none;stroke:#CECAC9;stroke-linecap:round;stroke-linejoin:round;} .st3{fill:none;stroke:#97908E;stroke-width:1.7008;stroke-linecap:round;stroke-linejoin:round;} </style> <g id="floor_plan_input"> <polygon class="st0" points="91.6,179.9 246,950.8 579.1,950.8 579.1,901.8 641.9,901.8 641.9,977 1339.1,977 1339.1,780.9 1399.4,780.9 1399.4,348.5 1338.8,348.5 1338.8,152.6 641.5,152.6 641.5,227.9 578.8,227.9 578.8,179.9 "/> </g> <g id="floor_plan_input::profile"> <polygon class="st0" points="101.6,188.1 252.7,942.6 570.9,942.6 570.9,893.6 650.2,893.6 650.2,968.8 1330.9,968.8 1330.9,772.7 1391.1,772.7 1391.1,356.7 1330.6,356.7 1330.6,160.9 649.7,160.9 649.7,236.1 570.6,236.1 570.6,188.1 "/> </g> <g id="floor_plan_input::structure"> <rect x="306.2" y="743.5" class="st0" width="13.4" height="13.4"/> <rect x="303.8" y="931.3" class="st0" width="15.8" height="19.6"/> <rect x="306.2" y="620.1" class="st0" width="13.4" height="13.4"/> <rect x="306.2" y="493.2" class="st0" width="13.4" height="13.4"/> <rect x="306.2" y="373.4" class="st0" width="13.4" height="13.4"/> <rect x="303.8" y="184.4" class="st0" width="15.8" height="13.4"/> <rect x="180.3" y="184.4" class="st0" width="15.8" height="13.4"/> <rect x="427.2" y="184.4" class="st0" width="15.8" height="13.4"/> <rect x="550.6" y="184.4" class="st0" width="15.8" height="13.4"/> <rect x="920.9" y="155.2" class="st0" width="15.8" height="19.6"/> <rect x="797.5" y="154.8" class="st0" width="15.8" height="19.6"/> <rect x="1380.5" y="617.7" class="st0" width="20.7" height="13.4"/> <rect x="1380.5" y="490.8" class="st0" width="20.7" height="13.4"/> <rect x="585.7" y="618.4" class="st0" width="13.4" height="13.4"/> <rect x="585.7" y="491.5" class="st0" width="13.4" height="13.4"/> <rect x="106.9" y="248.3" transform="matrix(0.9805 -0.1966 0.1966 0.9805 -48.1128 27.9457)" class="st0" width="19.6" height="15.8"/> <rect x="131.4" y="371.8" transform="matrix(0.9805 -0.1966 0.1966 0.9805 -71.9057 35.1839)" class="st0" width="19.6" height="15.8"/> <rect x="156" y="495.2" transform="matrix(0.9805 -0.1967 0.1967 0.9805 -95.7015 42.4237)" class="st0" width="19.6" height="15.8"/> <rect x="180.5" y="618.6" transform="matrix(0.9805 -0.1967 0.1967 0.9805 -119.4951 49.6621)" class="st0" width="19.6" height="15.8"/> <rect x="205.1" y="742.1" transform="matrix(0.9805 -0.1966 0.1966 0.9805 -143.2845 56.8984)" class="st0" width="19.6" height="15.8"/> <rect x="229.6" y="865.5" transform="matrix(0.9805 -0.1967 0.1967 0.9805 -167.0824 64.1391)" class="st0" width="19.6" height="15.8"/> <rect x="1044.3" y="155.2" class="st0" width="15.8" height="19.6"/> <rect x="427.2" y="931.3" class="st0" width="15.8" height="19.6"/> <rect x="550.6" y="931.3" class="st0" width="15.8" height="19.6"/> <rect x="920.9" y="954.9" class="st0" width="15.8" height="19.6"/> <rect x="797.5" y="954.6" class="st0" width="15.8" height="19.6"/> <rect x="1044.3" y="954.9" class="st0" width="15.8" height="19.6"/> <rect x="1167.8" y="954.9" class="st0" width="15.8" height="19.6"/> <rect x="1167.8" y="155.2" class="st0" width="15.8" height="19.6"/> </g> <g id="floor_plan_input::circulation"> <line class="st1" x1="0" y1="286.3" x2="1561.6" y2="286.3"/> <line class="st1" x1="1274.3" y1="1079.6" x2="1274.3" y2="0"/> <line class="st1" x1="336.1" y1="1079.6" x2="336.1" y2="0"/> <line class="st1" x1="1493.9" y1="851" x2="100.6" y2="851"/> <line class="st1" x1="543.9" y1="1082.6" x2="543.9" y2="3"/> </g> <g id="floor_plan_input::access"> <line class="st2" x1="634.3" y1="746.1" x2="634.3" y2="726.9"/> <line class="st2" x1="940.4" y1="799.6" x2="978.8" y2="799.6"/> <line class="st2" x1="634.3" y1="400.5" x2="634.3" y2="381.3"/> <line class="st2" x1="940.4" y1="330.1" x2="978.8" y2="330.1"/> <line class="st2" x1="634.3" y1="632.9" x2="634.3" y2="491.1"/> </g> <g id="floor_plan_input::exemptions"> <polygon class="st3" points="939.1,905.7 939.1,887.1 995.8,872.4 1022.7,887.1 1022.7,905.7 1022.7,924.4 995.8,939.1 939.1,924.4 "/> <polygon class="st3" points="157.9,619.3 311.7,619.3 311.7,507.5 135.9,507.5 "/> <polygon class="st3" points="1041.1,221.4 1041.1,202.8 1097.8,188.1 1124.7,202.8 1124.7,221.4 1124.7,240.1 1097.8,254.8 1041.1,240.1 "/> </g> <g id="floor_plan_input::core"> <polygon class="st0" points="634.3,317.5 634.3,811.9 775.2,811.9 775.2,799.6 1216.4,799.6 1216.4,330.1 775.2,330.1 775.2,317.5 "/> </g> </svg>';
    plans.add(activeFloor);    

    //Load request manifest information.

    //Metadata
    metadataFields.clear();
    metadataFields.add("description");
    metadataFields.add("notes");

    //Programs
    allPrograms.clear();
    _getPrograms(floor.projectNumber);

    //querySelector("#mainCanvas").setInnerHtml(activePlan.svgData);
  }

  bool addProject = false;

  void onAddRequest() {
    addProject = true;

    print("Add requested!");
  }

  void onCancel() {
    addProject = false;

    _getProjects();
  }

  Future<void> onAdd(String projectNumber, String projectName, String allFloors) async {

    if (projectNumber.length != 11) {
      print("Project number '" + projectNumber + "' improperly formatted.");
      return;
    }

    if (projectName.isEmpty) {
      return;
    }

    if (allFloors.isEmpty) {
      return;
    }

    List<String> floors = List<String>();
    floors.clear();

    var floorData = allFloors.split('/');

    floorData.forEach((x) => floors.add(x.replaceAll(' ', '')));

    await _projectService.addProject(projectNumber, projectName, floors);

    addProject = false;

    window.location.assign(window.location.href);
  }

  var allRequests = List<String>();
  String pendingRequest = null;

  void submitRequest(Floor floor, List<Program> programs) {
    var req = Request(floor.projectNumber, floor.floorNumber);

    var quotas = List<String>();
    var numQuotas = List<int>();

    programs.forEach((x) => quotas.add(x.name + ":" + (querySelector("." + x.name + "_quota") as InputElement).value));

    quotas.forEach((x) => x.split(":")[1] == "" ? numQuotas.add(-1) : numQuotas.add(int.parse(x.split(":")[1])));

    if (numQuotas.contains(-1)) {
      window.alert("Please input a quota for each program.");
      return;
    }

    if (!numQuotas.contains(0)) {
      var indexToChange = numQuotas.indexOf(numQuotas.reduce(max));

      var current = quotas[indexToChange];

      var data = current.split(":");

      var reconstructed = data[0] + ":0";

      quotas[indexToChange] = reconstructed;
    }

    req.quotas = quotas;

    var doc = _submissionService.submitToQueue(req);

    doc.then((x) {
      pendingRequest = x;
      allRequests.add(pendingRequest);
      print(pendingRequest);
      querySelector(".plan_svg").setInnerHtml("calculating solution");
    });

    fs.Firestore store = fb.firestore();
    fs.CollectionReference ref = store.collection("consumed");

    ref.onSnapshot.listen((snapshot) {  
        snapshot.docs.forEach((x) {
          print(x.id);
          if (x.id == pendingRequest) {
            var tryGetData = x.get("svg");
            //var tryGetStatus = x.get("status");

            //if (tryGetStatus != null) {
            //  querySelector(".plan_svg").setInnerHtml(x.get("svg"), validator: _htmlValidator);
            //}

            if (tryGetData != null) {
              querySelector(".plan_svg").setInnerHtml(x.get("svg"), validator: _htmlValidator);
            }
          }
        });
    });
  }

  void listenForSolution(String pendingRequest) {

  }

  String _projectUrl(String id) =>
    paths.RoutePaths.project.toUrl(parameters: {paths.idParam: "$id"});

  Future<NavigationResult> gotoDetail() =>
    _router.navigate(_projectUrl(selected.id));

}


