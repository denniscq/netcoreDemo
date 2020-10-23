using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyException.Exception
{
    public class KnowException : IKnowException
    {
        public int ErrorCode { get; private set; }
        public string Message { get; private set; }
        public object[] ErrorData { get; private set; }

        public static readonly KnowException UnKnowException = new KnowException { ErrorCode = 9999, Message = "UnKnow" };

        public static KnowException FromKnowException(IKnowException exception)
        {
            return new KnowException { ErrorCode = exception.ErrorCode, Message = exception.Message, ErrorData = exception.ErrorData };
        }
    }
}
