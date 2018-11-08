import 'package:utility_grid/engine/ecs/component.dart';
import 'package:utility_grid/engine/ecs/world.dart';

class Entity {
  String _id = World.generateId();
  Map<String, Component> _components = {};

  String get id => _id;
  Map<String, Component> get components => _components;

  void addComponent(Component component) {
    components.putIfAbsent(component.name, () => component);
  }

  bool hasComponentOfType(Type type) => components.values
      .map((c) => c.runtimeType == type)
      .fold(false, (a, b) => a || b);

  Component componentOfType(Type type) =>
      components.values.where((c) => c.runtimeType == type).first;
}
