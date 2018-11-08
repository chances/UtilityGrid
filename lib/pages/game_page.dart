import 'package:flutter/material.dart';
import 'package:utility_grid/widgets.dart';

class GamePage extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => _GamePageState();
}

class _GamePageState extends State<GamePage> {
  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      body: new World(
        child: Homes(),
      ),
    );
  }
}
