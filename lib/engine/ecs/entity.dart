import 'package:utility_grid/engine/ecs/component.dart';
import 'package:utility_grid/engine/ecs/world.dart';

class Entity {
  String id = World.generateId();
  Map<String, Component> components;

  void addComponent(Component component) {
    components[component.name] = component;
  }

  bool hasComponentOfType(Type type) => components.values
      .map((c) => c.runtimeType == type)
      .fold(false, (a, b) => a || b);

  Component componentOfType(Type type) =>
      components.values.where((c) => c.runtimeType == type).first;
}
