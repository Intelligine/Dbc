#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Dbc
{
    /// <summary>
    ///     Exception raised when a precondition fails.
    /// </summary>
    [Serializable]
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException()
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PreconditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
