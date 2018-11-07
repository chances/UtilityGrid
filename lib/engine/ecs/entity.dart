import 'package:utility_grid/engine/ecs/component.dart';
import 'package:utility_grid/engine/ecs/world.dart';

class Entity {
  String id = World.generateId();
  Map<String, Component> components;

  void addComponent(Component component) {
    components[component.name] = component;
  }

  Component componentOfType(Type type) =>
      components.values.where((c) => c.runtimeType == type).first;
}
