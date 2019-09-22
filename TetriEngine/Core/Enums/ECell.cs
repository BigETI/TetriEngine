/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Cell enumerator
    /// </summary>
    public enum ECell
    {
        /// <summary>
        /// Nothing
        /// </summary>
        Nothing,

        /// <summary>
        /// Blue
        /// </summary>
        Blue,

        /// <summary>
        /// Yellow
        /// </summary>
        Yellow,

        /// <summary>
        /// Green
        /// </summary>
        Green,

        /// <summary>
        /// Purple
        /// </summary>
        Purple,

        /// <summary>
        /// Red
        /// </summary>
        Red,

        /// <summary>
        /// Add line (attack special)
        /// </summary>
        AddLine,

        /// <summary>
        /// Clear line (defence special)
        /// </summary>
        ClearLine,

        /// <summary>
        /// Clear special blocks (attack special)
        /// </summary>
        ClearSpecialBlocks,

        /// <summary>
        /// Random blocks clear (attack special)
        /// </summary>
        RandomBlocksClear,

        /// <summary>
        /// Block bomb (attack special)
        /// </summary>
        BlockBomb,

        /// <summary>
        /// Quake field (attack special)
        /// </summary>
        QuakeField,

        /// <summary>
        /// Gravity (defence special)
        /// </summary>
        Gravity,

        /// <summary>
        /// Switch fields (attack special)
        /// </summary>
        SwitchFields,

        /// <summary>
        /// Nuke field (defence special)
        /// </summary>
        NukeField,

        /// <summary>
        /// Immunity (defence special, TetriNET2 only)
        /// </summary>
        Immunity,

        /// <summary>
        /// Clear column (defence special, TetriNET2 only)
        /// </summary>
        ClearColumn,

        /// <summary>
        /// Mutate pieces (attack special, TetriNET2 only)
        /// </summary>
        MutatePieces,

        /// <summary>
        /// Darkness (attack special, TetriNET2 only)
        /// </summary>
        Darkness,

        /// <summary>
        /// Confusion (attack special, TetriNET2 only)
        /// </summary>
        Confusion,

        /// <summary>
        /// Left gravity (defence special, Blocktrix only)
        /// </summary>
        LeftGravity,

        /// <summary>
        /// Piece change (attack special, Blocktrix only)
        /// </summary>
        PieceChange,

        /// <summary>
        /// Zebra change (defence special, Blocktrix only)
        /// </summary>
        ZebraField
    }
}
