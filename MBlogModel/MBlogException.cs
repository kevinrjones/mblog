using System;
using System.Runtime.Serialization;

namespace MBlogModel
{
    [Serializable]
    public class MBlogException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

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