import "dart:async";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

import "../src/formats/program.dart";

class ProgramService {

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