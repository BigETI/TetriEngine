using System;
using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Game manager class
    /// </summary>
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Block rotation count
        /// </summary>
        private static readonly int blockRotationCount = Enum.GetValues(typeof(EBlockRotation)).Length;

        /// <summary>
        /// Line block cells
        /// </summary>
        private static readonly ECell[,] lineBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Square block cells
        /// </summary>
        private static readonly ECell[,] squareBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Left L block cells
        /// </summary>
        private static readonly ECell[,] leftLBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Right L block cells
        /// </summary>
        private static readonly ECell[,] rightLBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Right Z block cells
        /// </summary>
        private static readonly ECell[,] rightZBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Left Z block cells
        /// </summary>
        private static readonly ECell[,] leftZBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Half cross block cells
        /// </summary>
        private static readonly ECell[,] halfCrossBlockCells = new ECell[,]
        {
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing },
            { ECell.Nothing, ECell.Nothing, ECell.Nothing }
        };

        /// <summary>
        /// Blocks
        /// </summary>
        private static ECell[][][,] blocksCells = new ECell[][][,]
        {
            // Nothing
            new ECell[blockRotationCount][,],

            // Line
            new ECell[][,]
            {
                RotateBlock(lineBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(lineBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(lineBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(lineBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Square
            new ECell[][,]
            {
                RotateBlock(squareBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(squareBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(squareBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(squareBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Left L
            new ECell[][,]
            {
                RotateBlock(leftLBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(leftLBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(leftLBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(leftLBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Right L
            new ECell[][,]
            {
                RotateBlock(rightLBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(rightLBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(rightLBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(rightLBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Right Z
            new ECell[][,]
            {
                RotateBlock(rightZBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(rightZBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(rightZBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(rightZBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Left Z
            new ECell[][,]
            {
                RotateBlock(leftZBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(leftZBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(leftZBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(leftZBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            },
            
            // Half cross
            new ECell[][,]
            {
                RotateBlock(halfCrossBlockCells, EBlockRotation.ZeroDegree),
                RotateBlock(halfCrossBlockCells, EBlockRotation.NinetyDegree),
                RotateBlock(halfCrossBlockCells, EBlockRotation.OneHundredEightyDegree),
                RotateBlock(halfCrossBlockCells, EBlockRotation.TwoHundredSeventyDegree)
            }
        };

        /// <summary>
        /// Users
        /// </summary>
        private Pool<IUser> users;

        /// <summary>
        /// Field
        /// </summary>
        private User user;

        /// <summary>
        /// Chat messages
        /// </summary>
        private List<string> chatMessages;

        /// <summary>
        /// Last block step state
        /// </summary>
        private EBlockStepState lastBlockStepState = EBlockStepState.Nothing;

        /// <summary>
        /// Last block step time
        /// </summary>
        private DateTime lastBlockStepTime;

        /// <summary>
        /// Field
        /// </summary>
        public IUser User => user;

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyPool<IUser> Users => users;

        /// <summary>
        /// Chat messages
        /// </summary>
        public IReadOnlyList<string> ChatMessages => chatMessages;

        /// <summary>
        /// Can perform action
        /// </summary>
        public bool CanPerformAction { get; internal set; }

        /// <summary>
        /// Game options
        /// </summary>
        public GameOptions GameOptions { get; private set; }

        /// <summary>
        /// Is paused
        /// </summary>
        public bool IsPaused { get; internal set; }

        /// <summary>
        /// Winlist
        /// </summary>
        public Winlist Winlist
        {
            get
            {
                Winlist ret = new Winlist();
                Dictionary<string, long> team_scores = new Dictionary<string, long>();
                foreach (IUser user in users)
                {
                    if (user.TeamName.Length > 0)
                    {
                        if (team_scores.ContainsKey(user.TeamName))
                        {
                            team_scores[user.TeamName] += user.Score;
                        }
                    }
                    else
                    {
                        Winlist.AppendUser(user);
                    }
                }
                foreach (KeyValuePair<string, long> team_score in team_scores)
                {
                    Winlist.AppendTeam(new Team(team_score.Key, team_score.Value));
                }
                team_scores.Clear();
                return ret;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="gameOptions">Game options</param>
        internal GameManager(User user, GameOptions gameOptions)
        {
            this.user = user;
            GameOptions = gameOptions;
            lastBlockStepTime = DateTime.Now;
        }

        /// <summary>
        /// Get block cells
        /// </summary>
        /// <param name="block">Block</param>
        /// <param name="blockRotation">Block rotation</param>
        /// <returns>Block cells</returns>
        internal static ECell[,] GetBlockCells(EBlock block, EBlockRotation blockRotation) => blocksCells[(int)block][(int)blockRotation];

        /// <summary>
        /// Check cells
        /// </summary>
        /// <param name="fieldCells">Field cells</param>
        /// <param name="blockCells">Block cells</param>
        /// <param name="xOrigin">X origin </param>
        /// <param name="yOrigin">Y origin</param>
        /// <returns>"true" if block cells fit into field cells, otherwise "false"</returns>
        private bool CheckCells(ECell[] fieldCells, ECell[,] blockCells, int xOrigin, int yOrigin)
        {
            bool ret = true;
            Field field = user.FieldInternal;
            for (int x = 0, y, x_len = blockCells.GetLength(0), y_len = blockCells.GetLength(1); x < x_len; x++)
            {
                int field_x = x + xOrigin;
                if ((field_x < 0) || (field_x >= Field.width))
                {
                    ret = false;
                    break;
                }
                else
                {
                    for (y = 0; y < y_len; y++)
                    {
                        int field_y = y + yOrigin;
                        if ((field_y < 0) || (field_y >= Field.height))
                        {
                            ret = false;
                            break;
                        }
                        else
                        {
                            int field_index = field_x + (field_y * Field.width);
                            if ((fieldCells[field_index] != ECell.Nothing) && (blockCells[x, y] != ECell.Nothing))
                            {
                                ret = false;
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Move block
        /// </summary>
        /// <param name="xOffset">X offset</param>
        /// <param name="yOffset">Y offset</param>
        /// <param name="landWhenFailed">Land when failed</param>
        /// <returns>"true" if possible, otherwise "false"</returns>
        private bool MoveBlock(int xOffset, int yOffset, bool landWhenFailed)
        {
            bool ret = false;
            Field field = user.FieldInternal;
            if (field.SelectedBlock != EBlock.Nothing)
            {
                ECell[] field_cells = new ECell[Field.width * Field.height];
                ECell[,] block_cells = GetBlockCells(field.SelectedBlock, field.SelectedBlockRotation);
                int x_origin = (int)(field.SelectedBlockPositionX) + xOffset;
                int y_origin = (int)(field.SelectedBlockPositionY) + yOffset;
                if (field.CopyCellsTo(field_cells, false))
                {
                    ret = CheckCells(field_cells, block_cells, x_origin, y_origin);
                    if (ret)
                    {
                        field.SelectedBlockPositionX = (uint)x_origin;
                        field.SelectedBlockPositionY = (uint)y_origin;
                    }
                    else if (landWhenFailed)
                    {
                        for (int x = 0, y, x_len = block_cells.GetLength(0), y_len = block_cells.GetLength(1); x < x_len; x++)
                        {
                            int field_x = x + x_origin;
                            if ((field_x >= 0) && (field_x < Field.width))
                            {
                                for (y = 0; y < y_len; y++)
                                {
                                    int field_y = y + y_origin;
                                    if ((field_y >= 0) && (field_y < Field.height))
                                    {
                                        int field_index = field_x + (field_y * Field.width);
                                        if ((field_cells[field_index] != ECell.Nothing) && (block_cells[x, y] != ECell.Nothing))
                                        {
                                            field_cells[field_index] = block_cells[x, y];
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        field.SelectedBlock = EBlock.Nothing;
                        field.SelectedBlockPositionX = 0U;
                        field.SelectedBlockPositionY = 0U;
                        field.UpdateCells(field_cells);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Drop block
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        public bool DropBlock()
        {
            bool ret = false;
            while (MoveBlockDown())
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Move block left
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        public bool MoveBlockLeft() => MoveBlock(-1, 0, false);

        /// <summary>
        /// Move block right
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        public bool MoveBlockRight() => MoveBlock(1, 0, false);

        /// <summary>
        /// Rotate block
        /// </summary>
        /// <param name="blockCells">Block cells</param>
        /// <param name="blockRotation">Block rotation</param>
        /// <returns>ROtated block cells</returns>
        private static ECell[,] RotateBlock(ECell[,] blockCells, EBlockRotation blockRotation)
        {
            ECell[,] ret = null;
            switch (blockRotation)
            {
                case EBlockRotation.ZeroDegree:
                    ret = blockCells;
                    break;
                case EBlockRotation.NinetyDegree:
                    ret = new ECell[blockCells.GetLength(1), blockCells.GetLength(0)];
                    for (int x = 0, y, x_len = ret.GetLength(0), y_len = ret.GetLength(1); x < x_len; x++)
                    {
                        for (y = 0; y < y_len; y++)
                        {
                            ret[x, y_len - y - 1] = blockCells[y, x];
                        }
                    }
                    break;
                case EBlockRotation.OneHundredEightyDegree:
                    ret = new ECell[blockCells.GetLength(0), blockCells.GetLength(1)];
                    for (int x = 0, y, x_len = ret.GetLength(0), y_len = ret.GetLength(1); x < x_len; x++)
                    {
                        for (y = 0; y < y_len; y++)
                        {
                            ret[x_len - x - 1, y_len - y - 1] = blockCells[x, y];
                        }
                    }
                    break;
                case EBlockRotation.TwoHundredSeventyDegree:
                    ret = new ECell[blockCells.GetLength(1), blockCells.GetLength(0)];
                    for (int x = 0, y, x_len = ret.GetLength(0), y_len = ret.GetLength(1); x < x_len; x++)
                    {
                        for (y = 0; y < y_len; y++)
                        {
                            ret[x_len - x - 1, y] = blockCells[y, x];
                        }
                    }
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Move block down
        /// </summary>
        /// <returns>"true" if possible, otherwise "false"</returns>
        public bool MoveBlockDown() => MoveBlock(0, 1, true);

        /// <summary>
        /// Turn block
        /// </summary>
        /// <param name="turnLeft">Turn left</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        private bool TurnBlock(bool turnLeft)
        {
            bool ret = false;
            Field field = user.FieldInternal;
            if (field.SelectedBlock != EBlock.Nothing)
            {
                EBlockRotation block_rotation = (EBlockRotation)(((int)(field.SelectedBlockRotation) + (turnLeft ? 1 : 3)) % blockRotationCount);
                ECell[] field_cells = new ECell[Field.width * Field.height];
                ECell[,] block_cells = GetBlockCells(field.SelectedBlock, block_rotation);
                int x_origin = (int)(field.SelectedBlockPositionX);
                int y_origin = (int)(field.SelectedBlockPositionY);
                if (field.CopyCellsTo(field_cells, false))
                {
                    ret = CheckCells(field_cells, block_cells, x_origin, y_origin);
                    if (ret)
                    {
                        field.SelectedBlockRotation = block_rotation;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Turn block left
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool TurnBlockLeft() => TurnBlock(true);

        /// <summary>
        /// Turn block right
        /// </summary>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool TurnBlockRight() => TurnBlock(false);

        /// <summary>
        /// Use special
        /// </summary>
        /// <param name="special">Special</param>
        /// <returns>"true" if possible, otherwise "false"</returns>
        public bool UseSpecial(ESpecial special) => user.InventoryInternal.UseSpecial(special);

        /// <summary>
        /// Block step
        /// </summary>
        /// <returns>"true" if block has landed, otherwise "false"</returns>
        internal EBlockStepState BlockStep()
        {
            EBlockStepState ret = ((lastBlockStepState == EBlockStepState.Loose) ? EBlockStepState.Loose : EBlockStepState.Wait);
            if ((!IsPaused) && (lastBlockStepState != EBlockStepState.Loose))
            {
                DateTime now = DateTime.Now;
                double seconds = (now - lastBlockStepTime).TotalSeconds;
                while (seconds >= 1.0)
                {
                    seconds -= 1.0;
                    switch (lastBlockStepState)
                    {
                        case EBlockStepState.Nothing:
                        case EBlockStepState.Wait:
                        case EBlockStepState.Move:
                            if (MoveBlockDown())
                            {
                                ret = EBlockStepState.Move;
                            }
                            else
                            {
                                ret = EBlockStepState.Land;
                            }
                            break;
                        case EBlockStepState.SelectNew:
                            if (MoveBlock(0, 0, true))
                            {
                                ret = EBlockStepState.Move;
                            }
                            else
                            {
                                ret = EBlockStepState.Loose;
                            }
                            break;
                        case EBlockStepState.Land:
                            lastBlockStepState = EBlockStepState.SelectNew;
                            ret = lastBlockStepState;
                            break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Add game chat message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="message">Message</param>
        internal bool AddGameChatMessage(IUser user, string message)
        {
            bool ret = users.IsIDValid(user.ID);
            if (ret)
            {
                IUser internal_user = users[user.ID];
                chatMessages.Add("<" + internal_user.Name + ((internal_user.TeamName.Length > 0) ? (" ** " + internal_user.TeamName) : "") + "> " + message);
            }
            return ret;
        }
    }
}
