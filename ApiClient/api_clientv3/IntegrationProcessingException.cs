using System;
using System.Runtime.Serialization;

namespace core.logic.mapping_woo_to_biro.document_insertion
{
    [Serializable]
    internal class IntegrationProcessingException : Exception
    {
        public IntegrationProcessingException() {
        }

        public IntegrationProcessingException(string message) : base(message) {
        }

        public IntegrationProcessingException(string message, Exception innerException) : base(message, innerException) {
        }

        protected IntegrationProcessingException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}