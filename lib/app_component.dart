import "dart:html";
import "dart:async";
import "package:angular/angular.dart";
import "package:angular_router/angular_router.dart";
import "package:angular_forms/angular_forms.dart";
//import "package:firebase/firebase.dart" as fb;

import "services/project_service.dart";
import "services/floor_service.dart";
import "services/program_service.dart";
import "services/submission_service.dart";
import "services/box_service.dart";

import "services/database_service.dart" as db;
import "src/routes.dart";

@Component(
  selector: "my-app",
  templateUrl: "app_component.html",
  directives: [routerDirectives, NgStyle, NgClass, NgModel, NgClass],
  providers: [ClassProvider(ProjectService), ClassProvider(FloorService), ClassProvider(ProgramService), ClassProvider(SubmissionService), ClassProvider(db.DatabaseService), ClassProvider(BoxService)],
  exports: [RoutePaths, Routes],
  styles: ['.active {border-color: black; outline-color: black; color: black}']
)
class AppComponent implements OnInit{
  var firebase = new db.DatabaseService();

  final title = "box/box/box";
  //&#9632; &#9632; &#9632;

  Future pause(Duration d) => new Future.delayed(d);
  
  var name = "Angular";

  Future TypeMessages() async {
    var message = document.getElementById("temp");
    message.setInnerHtml("");

    var messages = List<String>();
    messages.add("boxboxbox is currently getting redesigned");
    messages.add("functionality will return soon!");
    messages.add("last updated 19 OCT 2018");

    for (int i = 0; i < messages.length; i++) {
      for (int j = 0; j < messages[i].length; j++) {
        message.setInnerHtml(message.innerHtml + messages[i][j]);

        await pause(const Duration(milliseconds: 20));
        
      }

      message.setInnerHtml(message.innerHtml + "<br>");
    }
  }

  @override 
  void ngOnInit() async {
    TypeMessages();
  }
}