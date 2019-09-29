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
        /// Selected block
        /// </summary>
        public EBlock SelectedBlock { get; internal set; }

        /// <summary>
        /// Selected block position X
        /// </summary>
        public uint SelectedBlockPositionX { get; internal set; }

        /// <summary>
        /// Selected block position Y
        /// </summary>
        public uint SelectedBlockPositionY { get; internal set; }

        /// <summary>
        /// Selected block rotation
        /// </summary>
        public EBlockRotation SelectedBlockRotation { get; internal set; }

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
        internal Field(IField field)
        {
            if (field == null)
            {
                cells = new ECell[width * height];
            }
            else
            {
                SelectedBlock = field.SelectedBlock;
                SelectedBlockPositionX = field.SelectedBlockPositionX;
                SelectedBlockPositionY = field.SelectedBlockPositionY;
                SelectedBlockRotation = field.SelectedBlockRotation;
                cells = new ECell[field.Width * field.Height];
                field.CopyCellsTo(cells, false);
            }
        }

        /// <summary>
        /// Get cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="copySelectedBlock">Copy selected block</param>
        /// <returns>Tetri cell</returns>
        public ECell GetCell(int x, int y, bool copySelectedBlock)
        {
            ECell ret = ECell.Nothing;
            if ((x >= 0) && (x < cells.GetLength(0)) && (x >= 0) && (x < cells.GetLength(0)))
            {
                ret = cells[x + (y * width)];
                if (copySelectedBlock)
                {
                    ECell[,] block_cells = GameManager.GetBlockCells(SelectedBlock, SelectedBlockRotation);
                    int block_x = x - (int)SelectedBlockPositionX;
                    int block_y = y - (int)SelectedBlockPositionY;
                    if ((block_x >= 0) && (block_x < block_cells.GetLength(0)) && (block_y >= 0) && (block_y < block_cells.GetLength(1)))
                    {
                        ret = block_cells[block_x, block_y];
                    }
                }
            }
            return ret;
        }

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
        /// <param name="destinationCells">Destination cells</param>
        /// <param name="copySelectedBlock">Copy selected block</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool CopyCellsTo(ECell[] destinationCells, bool copySelectedBlock)
        {
            bool ret = false;
            if (destinationCells != null)
            {
                if (destinationCells.Length == cells.Length)
                {
                    Array.Copy(cells, destinationCells, cells.Length);
                    if (copySelectedBlock)
                    {
                        ECell[,] block_cells = GameManager.GetBlockCells(SelectedBlock, SelectedBlockRotation);
                        int x_origin = (int)SelectedBlockPositionX;
                        int y_origin = (int)SelectedBlockPositionY;
                        for (int x = 0, y, x_len = block_cells.GetLength(0), y_len = block_cells.GetLength(1); x < x_len; x++)
                        {
                            for (y = 0; y < y_len; y++)
                            {
                                int field_x = x + x_origin;
                                int field_y = y + y_origin;
                                if ((field_x >= 0) && (field_x < width) && (field_y >= 0) && (field_y < height))
                                {
                                    destinationCells[field_x + (field_y * width)] = block_cells[x, y];
                                }
                            }
                        }
                    }
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
