import "package:angular/angular.dart";
import "package:angular_forms/angular_forms.dart";
import "package:angular_router/angular_router.dart";
import "dart:async";

import "./formats/project.dart";
import "../services/project_service.dart";
import "route_paths.dart";

@Component(
  selector: 'my-project',
  directives: [coreDirectives, formDirectives],
  templateUrl: "project_component.html",
)
class ProjectComponent implements OnActivate{
  final ProjectService _projectService;
  final Location _location;

  Project project;

  ProjectComponent(this._projectService, this._location);

  @override
  void onActivate(_, RouterState current) async {
    print("Routing to: " + current.toUrl());

    final id = current.toUrl().split("/").last;

    if (id != null) {
      project = await _projectService.get(id);
    } 

    //print(project == null ? "Null project!" : project.name);
  }

  String getId(Map<String, String> parameters) {
    final id = parameters[idParam];

    return id == null ? null : id;
  }

  void goBack() => _location.back();
}