#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Intelligine.DesignByContract
{
    /// <summary>
    ///     Exception raised when a contract is broken.
    ///     Catch this exception type if you wish to differentiate between
    ///     any DesignByContract exception and other runtime exceptions.
    /// </summary>
    [Serializable]
    public class DesignByContractException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        protected DesignByContractException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        protected DesignByContractException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        protected DesignByContractException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DesignByContractException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
