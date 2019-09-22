using System;
using System.Net.Sockets;

/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Unsupported addresss family exception class
    /// </summary>
    public class UnsupportedAddressFamilyException : Exception
    {
        /// <summary>
        /// Address family
        /// </summary>
        public AddressFamily AddressFamily { get; private set; }

        /// <summary>
        /// Unsupported address family exception
        /// </summary>
        internal UnsupportedAddressFamilyException(AddressFamily addressFamily) : base((addressFamily == AddressFamily.Unspecified) ? "Address family has not been specified." : ((addressFamily == AddressFamily.Unknown) ? "The unknown address family used in the connection is not supported" : ("The address family \"" + addressFamily + "\" used in the connection is not supported.")))
        {
            AddressFamily = addressFamily;
        }
    }
}
