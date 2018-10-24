import "dart:async";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

import "../src/formats/floor.dart";

class FloorService {

  Future<List<Floor>> getAll(String projectNumber) async {
    fs.Firestore store = fb.firestore();
    fs.CollectionReference ref = store.collection("projects").doc(projectNumber).collection("floors");

    var allFloors = List<Floor>();

    print("Floor query started...");
    await ref.onSnapshot
      ..first
        .then((x) => allFloors.clear())
      ..first
        .then((x) => x.docs.forEach((y) => allFloors.add(Floor(projectNumber, y.id, y.get("svg")))))
        .catchError((x) => print(x.toString()))
        .whenComplete(() => print("Query complete! Found " + allFloors.length.toString() + " floors!"));

    return allFloors;
  } 

/*
  Future<String> get(String projectNumber, String floorNumber) async {
    String activeFloor;

    var allFloors = await getAll(projectNumber);
    
    print("Result received with " + activeFloor.length.toString() + " objects.");

    activeFloor = allFloors.firstWhere((floor) => floor == floorNumber);
      
    print("Fetched project " + activeFloor);

    return activeFloor;
  } 
  */
}