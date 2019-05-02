using System;

namespace Game
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for Utility Grid.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new UtilityGridGame())
            {
                #if DEBUG
                game.DebugMode = true;
                #endif

                game.Run();
                game.Dispose();
                Environment.Exit(0);
            }
        }
    }
}
