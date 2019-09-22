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
        /// Get cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Tetri cell</returns>
        ECell GetCell(int x, int y);

        /// <summary>
        /// Copy cells to
        /// </summary>
        /// <param name="cells">Destination cells</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool CopyCellsTo(ECell[] destinationCells);
    }
}
