using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public class Node : Godot.Node, IDisposableCollector, IGameLoopAware
    {
        [Export, UsedImplicitly]
        public ProcessMode ProcessMode { get; protected set; } = ProcessMode.Disable;

        public IObservable<float> OnLoop => _onLoop;
        private readonly Subject<float> _onLoop = new Subject<float>();

        private IList<IDisposable> _disposables;

        public Node()
        {
        }

        public Node([NotNull] string name)
        {
            Guard.AgainstNullArgument(nameof(name), name);

            Name = name;
        }

        public override void _Ready()
        {
            base._Ready();

            // Add disposable children to this disposable collector
            foreach (var disposable in this.GetChildren<IDisposable>())
            {
                disposable.AddTo(this);
            }

            SetProcess(ProcessMode == ProcessMode.Idle);
            SetPhysicsProcess(ProcessMode == ProcessMode.Physics);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            if (ProcessMode == ProcessMode.Idle)
            {
                ProcessLoop(delta);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            if (ProcessMode == ProcessMode.Physics)
            {
                ProcessLoop(delta);
            }
        }

        protected virtual void ProcessLoop(float delta) => _onLoop.OnNext(delta);

        public void Collect(IDisposable disposable)
        {
            Guard.AgainstNullArgument(nameof(disposable), disposable);

            if (_disposables == null)
            {
                _disposables = new List<IDisposable>();
            }
            else if (_disposables.Contains(disposable))
            {
                return;
            }

            _disposables.Add(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            _onLoop?.OnCompleted();
            _onLoop?.Dispose();

            _disposables?.Where(d => d != null).Reverse().ToList().ForEach(d => d.Dispose());
            _disposables = null;

            base.Dispose(disposing);
        }
    }
}
