using System;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Field class
    /// </summary>
    internal class Field : IField
    {
        /// <summary>
        /// Width
        /// </summary>
        public static readonly int width = 12;

        /// <summary>
        /// Width
        /// </summary>
        public static readonly int height = 22;

        /// <summary>
        /// Blocks
        /// </summary>
        private ECell[] cells = new ECell[width * height];

        /// <summary>
        /// Field width
        /// </summary>
        public int Width => cells.GetLength(0);

        /// <summary>
        /// Field height
        /// </summary>
        public int Height => cells.GetLength(1);

        /// <summary>
        /// Default constructor
        /// </summary>
        internal Field()
        {
            // ...
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="field">Field</param>
        internal Field(Field field)
        {
            Array.Copy(field.cells, cells, cells.Length);
        }

        /// <summary>
        /// Get cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Tetri cell</returns>
        public ECell GetCell(int x, int y) => (((x >= 0) && (x < cells.GetLength(0)) && (x >= 0) && (x < cells.GetLength(0))) ? cells[x + (y * width)] : ECell.Nothing);

        /// <summary>
        /// Set cell
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        internal bool SetCell(ECell cell, int x, int y)
        {
            bool ret = false;
            if ((x >= 0) && (x < width) && (y >= 0) && (y < height))
            {
                cells[x + (y * width)] = cell;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Copy cells to
        /// </summary>
        /// <param name="cells">Destination cells</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool CopyCellsTo(ECell[] destinationCells)
        {
            bool ret = false;
            if (destinationCells != null)
            {
                if (destinationCells.Length == cells.Length)
                {
                    Array.Copy(cells, destinationCells, cells.Length);
                }
            }
            return ret;
        }

        /// <summary>
        /// Update cells
        /// </summary>
        /// <param name="cells">Cells</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        internal bool UpdateCells(ECell[] cells)
        {
            bool ret = false;
            if (cells != null)
            {
                if (cells.Length == this.cells.Length)
                {
                    Array.Copy(cells, this.cells, cells.Length);
                    ret = true;
                }
            }
            return ret;
        }
    }
}
