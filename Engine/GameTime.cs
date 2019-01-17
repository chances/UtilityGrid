using System;

namespace Engine
{
    public class GameTime
    {
        public GameTime()
        {
            TotalGameTime = TimeSpan.Zero;
            ElapsedGameTime = TimeSpan.Zero;
            IsRunningSlowly = false;
        }

        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = false;
        }

        private GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = isRunningSlowly;
        }

        /// <summary>
        /// The amount of game time since the start of the game.
        /// </summary>
        /// <value>
        /// Game time since the start of the game.
        /// </value>
        /// <remarks>
        /// Fixed-step clocks update by a fixed time span upon every clock step. This results in uniform clock steps
        /// that may not actually track the wall clock time. Fixed step clocks were popular on console systems where
        /// one had tight control over code and a fixed system performance. Fixed-step clocks are also useful when
        /// trying to achieve deterministic updates for debugging, offline rendering, or other such scenarios.
        /// </remarks>
        public TimeSpan TotalGameTime { get; }

        /// <summary>
        /// The amount of elapsed game time since the last update.
        /// </summary>
        /// <remarks>
        /// Fixed-step clocks update by a fixed time span upon every clock step. This results in uniform clock steps
        /// that may not actually track the wall clock time. Fixed-step clocks were popular on console systems where
        /// one had tight control over code and a fixed system performance. Fixed-step clocks are also useful when
        /// trying to achieve deterministic updates for debugging, offline rendering, or other such scenarios.
        /// </remarks>
        public TimeSpan ElapsedGameTime { get; }

        /// <summary>
        /// Gets a value indicating that the game loop is taking longer than its TargetElapsedTime. In this case, the
        /// game loop can be considered to be running too slowly and should do something to "catch up."
        /// </summary>
        /// <value>
        /// true if the game loop is taking too long; false otherwise.
        /// </value>
        public bool IsRunningSlowly { get; }

        public static GameTime RunningSlowly(GameTime gameTime)
        {
            return new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime, true);
        }
    }
}
