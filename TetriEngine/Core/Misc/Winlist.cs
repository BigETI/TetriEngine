using System.Collections.Generic;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Winlist class
    /// </summary>
    public class Winlist
    {
        /// <summary>
        /// Users
        /// </summary>
        private List<IUser> users = new List<IUser>();

        /// <summary>
        /// Teams
        /// </summary>
        private List<Team> teams = new List<Team>();

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyList<IUser> Users => users;

        /// <summary>
        /// Teams
        /// </summary>
        public IReadOnlyList<Team> Teams => teams;

        /// <summary>
        /// Default constructor
        /// </summary>
        internal Winlist()
        {
            // ...
        }

        /// <summary>
        /// Append user
        /// </summary>
        /// <param name="user">User</param>
        internal void AppendUser(IUser user)
        {
            users.Add(user);
        }

        /// <summary>
        /// Append team
        /// </summary>
        /// <param name="team">Team</param>
        internal void AppendTeam(Team team)
        {
            teams.Add(team);
        }
    }
}
