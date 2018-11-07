import 'package:utility_grid/engine/ecs/entity.dart';
import 'package:utility_grid/engine/ecs/world.dart';

abstract class System {
  final World world;
  final List<Type> operableComponentTypes = [];

  System(this.world);

  bool canOperateOn(Entity e) {
    var componentTypes = e.components.values.map((c) => c.runtimeType);
    var componentsAreOperable =
    componentTypes.map((t) => operableComponentTypes.contains(t));
    /// A [System] may operate on a given Entity IFF the entity contains all
    /// specified operable components
    return componentsAreOperable.fold(true, (a, b) => a && b);
  }

  void operate();
}
