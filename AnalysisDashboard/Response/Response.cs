using AnalysisDashboard.Helper;
using System.Net;

namespace AnalysisDashboard.Response
{
    public class Response<ResponseData> where ResponseData : class, new()
    {
        public bool result { get; set; }
        public String message { get; set; }
        public int error_code { get; set; }
        public ResponseData data { get; set; }

        public Response()
        {
            result = false;
            message = Constants.NotFound;
            error_code = (int)HttpStatusCode.NotFound;

            data = new ResponseData();
        }
    }

    public class Response
    {
        public bool result { get; set; }
        public String message { get; set; }
        public int error_code { get; set; }

        public Response()
        {
            result = false;
            message = Constants.NotFound;
            error_code = (int)HttpStatusCode.NotFound;
        }
    }
}
