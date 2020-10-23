using System;
using System.Collections.Generic;
using System.Text;

namespace Dennis.Core
{
    public class KnownException : IKnownException
    {
        public string Message { get; private set; }
        public int ErrorCode { get; private set; }
        public object[] ErrorData { get; private set; }

        public readonly static IKnownException UnknowException = new KnownException { Message = "Unknow", ErrorCode = 9999 };
        public static IKnownException FromKnownException(IKnownException knownException)
        {
            return new KnownException { Message = knownException.Message, ErrorCode = knownException.ErrorCode, ErrorData = knownException.ErrorData };
        }
    }
}
