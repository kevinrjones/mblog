using System;
using System.Runtime.Serialization;

namespace MBlogModel
{
    [Serializable]
    public class MBlogMediaNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MBlogMediaNotFoundException()
        {
        }

        public MBlogMediaNotFoundException(string message) : base(message)
        {
        }

        public MBlogMediaNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MBlogMediaNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}