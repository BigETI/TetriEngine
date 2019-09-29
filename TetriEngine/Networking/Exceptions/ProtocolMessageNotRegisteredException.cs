using System;

/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Protocol message is not registered exception class
    /// </summary>
    public class ProtocolMessageNotRegisteredException : Exception
    {
        /// <summary>
        /// Protocol message
        /// </summary>
        public string ProtocolMessage { get; private set; }

        /// <summary>
        /// Raw content
        /// </summary>
        public string RawContent { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocolMessage">Protocol message</param>
        /// <param name="rawContent">Raw content</param>
        internal ProtocolMessageNotRegisteredException(string protocolMessage, string rawContent) : base("Message \"" + protocolMessage + "\" is not registered. Content:" + Environment.NewLine + Environment.NewLine + rawContent + Environment.NewLine)
        {
            ProtocolMessage = protocolMessage;
            RawContent = rawContent;
        }
    }
}
