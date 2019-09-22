/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Cell position structure
    /// </summary>
    public struct CellPosition
    {
        /// <summary>
        /// Cell
        /// </summary>
        public ECell Cell { get; private set; }

        /// <summary>
        /// X
        /// </summary>
        public uint X { get; private set; }

        /// <summary>
        /// Y
        /// </summary>
        public uint Y { get; private set; }

        /// <summary>
        /// Cell position
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public CellPosition(ECell cell, uint x, uint y)
        {
            Cell = cell;
            X = x;
            Y = y;
        }
    }
}
