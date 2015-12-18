using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANGenericRepository
{
    public class OperationResult
    {

        public static OperationResult GetInstance()
        {
            return new OperationResult { IsFailed = false, ErrorMessage = "" };
        }
        public string ErrorMessage { get; set; }
        public string ObjectId { get; set; }
        public bool IsFailed { get; set; }
        public object Item { get; set; }

        public OperationResult GetExceptionDetail(Exception exception)
        {
            OperationResult opResult = new OperationResult();
            opResult.IsFailed = true;
            opResult.ErrorMessage = "Ex Detail : " + exception.Message + exception.StackTrace;
            if (exception.InnerException != null)
            {
                opResult.ErrorMessage += ".Inner Ex Detail : " + exception.InnerException.Message + exception.InnerException.StackTrace;
            }//End if

            return opResult;

        }//End Method

    }
}
