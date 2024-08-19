using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester
{
    public class ApiResult<TResponse> : IApiResult<TResponse>
    {
        public HttpStatusCode StatusCode { get; set; }
        public TResponse Data { get; set; }
    }

}
