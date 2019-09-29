/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Field interface
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Field width
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Field height
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Selected block
        /// </summary>
        EBlock SelectedBlock { get; }

        /// <summary>
        /// Selected block position X
        /// </summary>
        uint SelectedBlockPositionX { get; }

        /// <summary>
        /// Selected block position Y
        /// </summary>
        uint SelectedBlockPositionY { get; }

        /// <summary>
        /// Selected block rotation
        /// </summary>
        EBlockRotation SelectedBlockRotation { get; }

        /// <summary>
        /// Get cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="copySelectedBlock">Copy selected block</param>
        /// <returns>Tetri cell</returns>
        ECell GetCell(int x, int y, bool copySelectedBlock);

        /// <summary>
        /// Copy cells to
        /// </summary>
        /// <param name="destinationCells">Destination cells</param>
        /// <param name="copySelectedBlock">Copy selected block</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool CopyCellsTo(ECell[] destinationCells, bool copySelectedBlock);
    }
}
