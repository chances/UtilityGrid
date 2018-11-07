import 'package:utility_grid/engine/ecs/entity.dart';
import 'package:utility_grid/engine/ecs/component.dart';
import 'package:utility_grid/engine/ecs/system.dart';
import 'package:uuid/uuid.dart';

class World {
  static Uuid _uuid = new Uuid();
  static String generateId() => _uuid.v4();

  List<Entity> entities;

  List<Entity> entitiesWith(Type componentType) {
    var typedEntities =
        entities.where((e) => e.hasComponentOfType(componentType));
    if (typedEntities.isEmpty) {
      return [];
    }

    return typedEntities.toList(growable: false);
  }

  List<Entity> operableEntitiesFor(System system) {
    var operableEntities = entities.where(system.canOperateOn);
    if (operableEntities.isEmpty) {
      return [];
    }

    return operableEntities.toList(growable: false);
  }

  List<Component> operableComponentsFor(System system, Type componentType) {
    var operableComponents = operableEntitiesFor(system)
        .map((e) => e.componentOfType(componentType));
    if (operableComponents.isEmpty) {
      return [];
    }

    return operableComponents.toList(growable: false);
  }
}
