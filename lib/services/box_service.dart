import "dart:async";
import "package:firebase/firebase.dart" as fb;
import "package:firebase/firestore.dart" as fs;

//import "../src/formats/floor.dart";

class BoxService {

  Future<List<String>> getAllVerbs() async {
    //fs.Firestore store = fb.firestore();
    //fs.CollectionReference ref = store.collection("projects").doc(projectNumber).collection("floors");

    List<String> allVerbs = [
      "regulate",
      "capture",
      "partner",
      "hide",
      "thicken",
      "sheath",
      "flourish",
      "elevate",
      "focus"
    ];

    return allVerbs;
  } 

  Future<List<String>> getAllAdverbs() async {
    
    List<String> allAdverbs = [
      "paired",
      "adversarial",
      "decorated",
      "layered",
      "broken",
      "flattened",
      "fractal",
      "lazy",
      "twisted"
    ];

    return allAdverbs;
  }

}