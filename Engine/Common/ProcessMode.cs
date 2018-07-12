namespace UtilityGrid.Engine.Common
{
    public enum ProcessMode
    {
        /// <summary>
        /// Disable automatic Node processing.
        /// </summary>
        Disable,
        /// <summary>
        /// Process the Node on every physics step.
        /// </summary>
        Physics,
        /// <summary>
        /// Process the Node on every drawn frame.
        /// </summary>
        Idle
    }
}
