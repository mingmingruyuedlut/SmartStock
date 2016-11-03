using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock.Core.Models
{
    public class ResponseResult
    {
        public ResponseResult()
        {
        }

        public ResponseResult(bool successful)
        {
            Successful = successful;
        }

        public ResponseResult(bool successful, string message)
        {
            Successful = successful;
            Message = message;
        }

        public ResponseResult(bool successful, object data)
        {
            Successful = successful;
            Data = data;
        }

        public ResponseResult(bool successful, string message, object data)
        {
            Successful = successful;
            Message = message;
            Data = data;
        }

        public bool Successful { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
