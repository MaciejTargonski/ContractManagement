using System;
using System.Runtime.Serialization;

namespace ContractManagement.Domain.Models
{
    [Serializable]
    public class ProffessionalExperianceTooLowException : Exception
    {
        public ProffessionalExperianceTooLowException()
        {
        }

        public ProffessionalExperianceTooLowException(string message) : base(message)
        {
        }

        public ProffessionalExperianceTooLowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProffessionalExperianceTooLowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}