import "dart:async";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

import "../src/formats/project.dart";

class ProjectService {

  Future<List<Project>> getAll() async {
    fs.Firestore store = fb.firestore();
    fs.CollectionReference ref = store.collection("projects");

    var projects = List<Project>();;

    print("Query started...");
    await ref.onSnapshot
      ..first
        .then((x) => projects.clear())
      ..first
        .then((x) => x.docs.forEach((y) => projects.add(Project(y.id, y.data()["name"]))))
        .catchError((x) => print(x.toString()))
        .whenComplete(() => print("Query complete! Found " + projects.length.toString() + " projects!"));

    return projects;
  } 

  Future<Project> get(String id) async {
    Project proj;

    var allProjects = await getAll();
    
    print("Result received with " + allProjects.length.toString() + " objects.");
    proj = allProjects.firstWhere((project) => project.id == id);
      
    print("Fetched project " + proj.id);

    return proj;
  } 

  Future<void> addProject(String projectNumber, String projectName, List<String> floors) async {
    fs.Firestore store = fb.firestore();
    fs.CollectionReference ref = store.collection("projects");

    var doc = ref.doc(projectNumber);
    
    var data = {
      "name":projectName
    };

    doc.set(data);

    var floorCollection = doc.collection("floors");

    floors.forEach((floor) {
      var floorDoc = floorCollection.doc(floor);
      
      var floorData = {
        "name":"blank"
      };

      floorDoc.set(floorData);

    });  
  }
}