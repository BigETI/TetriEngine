using System;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Malformed message exception
    /// </summary>
    public class MalformedMessageException : Exception
    {
        /// <summary>
        /// Received message
        /// </summary>
        public string ReceivedMessage { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="receivedMessage">Received message</param>
        internal MalformedMessageException(string receivedMessage) : base("Malformed message:" + Environment.NewLine + Environment.NewLine + receivedMessage)
        {
            ReceivedMessage = receivedMessage;
        }
    }
}
