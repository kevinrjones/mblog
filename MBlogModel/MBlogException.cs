using System;
using System.Runtime.Serialization;

namespace MBlogModel
{
    [Serializable]
    public class MBlogException : Exception
    {
        public MBlogException()
        {
        }

        public MBlogException(string message) : base(message)
        {
        }

        public MBlogException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MBlogException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}