using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyException.Exception
{
    public interface IKnowException
    {
        public int ErrorCode { get; }
        public string Message { get; }
        public object[] ErrorData { get; }
    }
}
