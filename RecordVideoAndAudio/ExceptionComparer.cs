using System;
using System.Collections.Generic;

namespace RecordVideoAndAudio
{
    public class ExceptionComparer : IEqualityComparer<Exception>
    {
        public bool Equals(Exception x, Exception y)
        {
            if (x == null && y == null) return true;
            if (x != null && y == null) return false;
            if (x == null && y != null) return false;

            return x.Message == y.Message
                && x.StackTrace == y.StackTrace
                && x.Source == y.Source;
        }

        public int GetHashCode(Exception obj)
        {
            return (obj?.Message
                + obj?.StackTrace
                + obj?.Source).GetHashCode();
        }
    }
}