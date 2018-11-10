import 'package:flutter/material.dart';
import 'package:utility_grid/pages.dart';
import 'package:utility_grid/style.dart';

void main() => runApp(UtilityGridGame());

class UtilityGridGame extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Utility Grid',
      theme: Style.theme,
      home: GamePage(),
    );
  }
}
