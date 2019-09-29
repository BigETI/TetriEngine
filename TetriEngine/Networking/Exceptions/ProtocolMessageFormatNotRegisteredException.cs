/// <summary>
/// TetriEngine networking namespace
/// </summary>
namespace TetriEngine.Networking
{
    /// <summary>
    /// Protocol message format not registered exception class
    /// </summary>
    public class ProtocolMessageFormatNotRegisteredException : ProtocolMessageNotRegisteredException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="protocolMessage">Protocol message</param>
        /// <param name="rawContent">Raw content</param>
        internal ProtocolMessageFormatNotRegisteredException(string protocolMessage, string rawContent) : base(protocolMessage, rawContent)
        {
            // ...
        }
    }
}
