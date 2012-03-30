using System;
using System.Threading;

namespace MBlogNlogService
{
    internal static class ExceptionHelper
    {
        public static bool MustBeRethrown(this Exception exception)
        {
            if (exception is StackOverflowException || exception is ThreadAbortException ||
                exception is OutOfMemoryException)
                return true;
            else
                return false;
        }
    }
}