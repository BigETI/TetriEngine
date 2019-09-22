/// <summary>
/// TetriEngine namespace
/// </summary>
namespace TetriEngine
{
    /// <summary>
    /// Send result class
    /// </summary>
    public class SendResult
    {
        /// <summary>
        /// Error
        /// </summary>
        private string error = string.Empty;

        /// <summary>
        /// Error
        /// </summary>
        public EResult Result { get; private set; }

        /// <summary>
        /// Error
        /// </summary>
        public string Error
        {
            get => error;
            private set
            {
                if (value != null)
                {
                    error = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result</param>
        /// <param name="error">Error</param>
        internal SendResult(EResult result, string error)
        {
            Result = result;
            Error = error;
        }
    }
}
