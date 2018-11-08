import 'package:flutter/material.dart';
import 'package:utility_grid/engine/ecs.dart';

class House implements Component {
  @override
  String name = 'house';

  Color color = Colors.green;
}
