import 'package:flutter/material.dart';
import 'package:utility_grid/style.dart';

class Meter extends StatelessWidget {
  final Icon icon;
  final int value;
  final int minValue;

  const Meter(
      {Key key, this.icon, @required this.value, @required this.minValue})
      : super(key: key);

  @override
  Widget build(BuildContext context) => Row(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.baseline,
        textBaseline: TextBaseline.ideographic,
        children: <Widget>[
          icon,
          Text(
            '$value',
            style: TextStyle(
              fontSize: 20.0,
              fontWeight: FontWeight.bold,
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(left: Style.marginText),
            child: Text('/ $minValue'),
          )
        ],
      );
}
