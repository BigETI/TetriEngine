using System;
using System.Collections.Generic;
using System.Text;

namespace TetriEngine.Networking
{
    public struct ChatMessageAction
    {
        public IUser User { get; private set; }

        public string Message { get; private set; }

        public bool IsAction { get; private set; }

        internal ChatMessageAction(IUser user, string message, bool isAction)
        {
            User = user;
            Message = message;
            IsAction = isAction;
        }

        public string ToString(string format)
        {
            return string.Format(format, User.ID, User.Name, Message);
        }

        public override string ToString()
        {
            return (IsAction ? ToString("({0}) {1} {2}") : ToString("({0}) {1}> {2}"));
        }
    }
}
