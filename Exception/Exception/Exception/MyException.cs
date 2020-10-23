using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyException.Exception
{
    public class MyException : System.Exception, IKnowException
    {
        public MyException(int code, string message, params object[] errorData) : base(message)
        {
            this.ErrorCode = code;
            this.ErrorData = errorData;
        }

        public int ErrorCode { get; private set; }

        public object[] ErrorData { get; private set; }
    }
}
