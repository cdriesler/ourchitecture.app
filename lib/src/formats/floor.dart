class Floor {
  final String projectNumber;
  final String floorNumber;
  String svgData;

  Floor(this.projectNumber, this.floorNumber, this.svgData);
}

class FloorSolution {
  final Floor baseFloor;
  String svgData;

  FloorSolution(this.baseFloor);
}