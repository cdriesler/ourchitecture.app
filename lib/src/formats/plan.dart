class Plan {
  final String projectNumber;
  final String floorNumber;
  String svgData;

  Plan(this.projectNumber, this.floorNumber);
}

class SvgData {
  String viewBox;
  String drawingInfo;

  SvgData(this.viewBox, this.drawingInfo);
}