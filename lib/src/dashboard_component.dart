import "package:angular/angular.dart";
import "package:angular_router/angular_router.dart";

import "./formats/project.dart";
import "../services/project_service.dart";
import "route_paths.dart";
import "routes.dart";

@Component(
  selector: "my-dashboard",
  templateUrl: "dashboard_component.html",
  directives: [coreDirectives, routerDirectives],
  styleUrls: ["dashboard_component.css"],
  exports: [RoutePaths, Routes],
)
class DashboardComponent implements OnInit{
  List<Project> projects = List<Project>();

  final ProjectService _projectService;

  DashboardComponent(this._projectService);

  String projectUrl(String id) => RoutePaths.project.toUrl(parameters: {idParam: "$id"});

  void TypeMessages() {

  }

  @override
  void ngOnInit() async {
    TypeMessages();
  }
}