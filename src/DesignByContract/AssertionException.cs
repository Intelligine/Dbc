#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Intelligine.DesignByContract
{
    /// <summary>
    ///     Exception raised when an assertion fails.
    /// </summary>
    [Serializable]
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException()
        {
        }

        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected AssertionException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
