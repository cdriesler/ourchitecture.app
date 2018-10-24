import "dart:async";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

import "../src/formats/program.dart";
import "../src/formats/request.dart";

class SubmissionService {

  Future<String> submitToQueue(Request request) async {
    fs.Firestore store = fb.firestore();
    fs.DocumentReference ref = store.collection("queue").doc();

    //print("Submitting a request for project " + request.projectNumber);

    var data = {
      "project_number":request.projectNumber,
      "floor":request.floorNumber,
      "quotas":request.quotas
    };

    ref.set(data);

    return ref.id;
  }

  Future<String> deployPayload(String payload) async {
    fs.Firestore store = fb.firestore();
    fs.DocumentReference ref = store.collection("queue").doc();

    print("Document " + ref.id + " created.");

    var data = {
      "payload":payload,
    };

    ref.set(data);

    return ref.id;
  }

  Future<List<Program>> getAll(String projectNumber) async {
    fs.Firestore store = fb.firestore();
    fs.CollectionReference ref = store.collection("projects").doc(projectNumber).collection("programs");

    var programs = List<Program>();;

    print("Program query started...");
    await ref.onSnapshot
      ..first
        .then((x) => programs.clear())
      ..first
        .then((x) => x.docs.forEach((y) => programs.add(Program(y.get("name"), y.get("quota")))))
        .catchError((x) => print(x.toString()))
        .whenComplete(() => print("Query complete! Found " + programs.length.toString() + " programs!"));

    return programs;
  } 

}