/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Block step state enumerator
    /// </summary>
    public enum EBlockStepState
    {
        /// <summary>
        /// Nothing
        /// </summary>
        Nothing,

        /// <summary>
        /// Wait
        /// </summary>
        Wait,

        /// <summary>
        /// Move
        /// </summary>
        Move,

        /// <summary>
        /// Land
        /// </summary>
        Land,

        /// <summary>
        /// Select new
        /// </summary>
        SelectNew,

        /// <summary>
        /// Loose
        /// </summary>
        Loose
    }
}
