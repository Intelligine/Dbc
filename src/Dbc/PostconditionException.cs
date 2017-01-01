#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Dbc
{
    /// <summary>
    ///     Exception raised when a postcondition fails.
    /// </summary>
    [Serializable]
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException()
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PostconditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
