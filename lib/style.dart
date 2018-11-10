import 'package:flutter/material.dart';

class Style {
  static final theme = ThemeData(
    primarySwatch: Colors.blue,
    splashColor: Colors.blueAccent,
    scaffoldBackgroundColor: Color.fromRGBO(245, 245, 245, 1.0),
  );

  static const double margin = 8.0;
  static const double marginText = 4.0;

  static BorderRadius circularBorderRadius({double radius = margin}) =>
      BorderRadius.all(Radius.circular(radius));
}
