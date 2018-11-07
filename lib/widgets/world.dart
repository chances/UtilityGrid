import 'package:flutter/widgets.dart';
import 'package:utility_grid/engine/ecs.dart' as ecs;

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

  World({Widget child}) : super(child: child);

  @override
  bool updateShouldNotify(World old) => false;
}
