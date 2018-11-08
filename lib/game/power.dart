import 'package:utility_grid/engine/ecs.dart';

class Power implements Component {
  @override
  String name = 'power';

  int power = 0;
  int minPower = 10;

  bool get criticallyLow => power < minPower;

  void consume() {
    power -= minPower;
    if (power < 0) {
      power = 0;
    }
  }
}

class PowerGenerator extends System {
  PowerGenerator(World world) : super(world, [Power]);

  static const int generationRate = 5;

  @override
  void operate() {
    List<Power> powerConsumers =
      world.operableComponentsFor(this, Power).cast();
    for (var powerConsumer in powerConsumers) {
      powerConsumer.power += generationRate;
    }
  }
}

class PowerUsage extends System {
  PowerUsage(World world) : super(world, [Power]);

  @override
  void operate() {
    List<Power> powerConsumers =
        world.operableComponentsFor(this, Power).cast();
    for (var powerConsumer in powerConsumers) {
      powerConsumer.consume();
    }
  }
}
