import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:utility_grid/pages.dart';

void main() => runApp(UtilityGridGame());

class UtilityGridGame extends StatelessWidget {
  UtilityGridGame() {
    // Hide OS UI overlays so the game is fullscreen
    SystemChrome.setEnabledSystemUIOverlays([]);
  }

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Utility Grid',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: GamePage(),
    );
  }
}
