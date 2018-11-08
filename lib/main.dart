import 'package:flutter/material.dart';
import 'package:utility_grid/pages.dart';

void main() => runApp(UtilityGridGame());

class UtilityGridGame extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Utility Grid',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        splashColor: Colors.blueAccent,
      ),
      home: GamePage(),
    );
  }
}
