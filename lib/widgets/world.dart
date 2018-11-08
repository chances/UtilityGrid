import 'package:flutter/widgets.dart';
import 'package:utility_grid/engine/ecs.dart' as ecs;
import 'package:utility_grid/game.dart';

class World extends InheritedWidget {
  final ecs.World world = new ecs.World();

  static ecs.World of(BuildContext context) {
    final World worldWidget = context.inheritFromWidgetOfExactType(World);
    if (worldWidget != null) {
      return worldWidget.world;
    }

    throw FlutterError(
      'World.of() called with a context that does not contain a World.',
    );
  }

  World({Widget child}) : super(child: child) {
    for (var i = 0; i < 5; i += 1) {
      var home = ecs.Entity();
      home.addComponent(House());
      home.addComponent(Power());
      world.entities.add(home);
    }
  }

  @override
  bool updateShouldNotify(World old) => false;
}
