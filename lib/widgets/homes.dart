import 'package:flutter/material.dart';
import 'package:utility_grid/engine/ecs.dart' as ecs;
import 'package:utility_grid/game.dart';
import 'package:utility_grid/style.dart';
import 'package:utility_grid/widgets.dart';

class Homes extends StatefulWidget {
  @override
  State<StatefulWidget> createState() => _HomesState();
}

class _HomesState extends State<Homes> {
  @override
  Widget build(BuildContext context) {
    var homes = World.of(context).entitiesWith([House, Power]);

    var gridCells = homes.map((home) => houseWidget(home)).toList();
    gridCells.add(Container(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Icon(Icons.add),
          Text('Add Home'),
        ],
      ),
    ));

    return GridView.count(
      primary: true,
      padding: const EdgeInsets.all(20.0),
      mainAxisSpacing: 10.0,
      crossAxisSpacing: 10.0,
      crossAxisCount: 2,
      children: gridCells,
    );
  }

  Widget houseWidget(ecs.Entity home) {
    House house = home.componentOfType(House);
    Power power = home.componentOfType(Power);

    return Material(
      color: Colors.white,
      elevation: 2.0,
      borderRadius: Style.circularBorderRadius(),
      child: InkWell(
          borderRadius: Style.circularBorderRadius(),
          onTap: () {},
          child: Padding(
            padding: EdgeInsets.all(Style.margin),
            child: Home(house, power),
          )),
    );
  }
}

class Home extends StatelessWidget {
  final House house;
  final Power housePower;

  const Home(this.house, this.housePower, {Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var stackChildren = <Widget>[
      Column(
        mainAxisSize: MainAxisSize.min,
        children: <Widget>[
          Icon(
            Icons.home,
            color: house.color,
            size: 48,
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.baseline,
            textBaseline: TextBaseline.ideographic,
            children: <Widget>[
              Icon(Icons.power),
              Text(
                '${housePower.power}',
                style: TextStyle(
                  fontSize: 20.0,
                  fontWeight: FontWeight.bold,
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(left: Style.marginText),
                child: Text('/ ${housePower.minPower}'),
              )
            ],
          ),
        ],
      ),
    ];

    if (housePower.criticallyLow) {
      stackChildren.add(
        Positioned(
          child: Opacity(
            opacity: 0.7,
            child: Icon(
              Icons.error_outline,
              color: Colors.red,
            ),
          ),
          top: 0,
          left: 0,
        ),
      );
    }

    return Stack(
      alignment: Alignment.center,
      children: stackChildren,
    );
  }
}
