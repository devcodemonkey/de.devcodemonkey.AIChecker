using de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces;
using System.Net;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester.Models
{
    public class ApiResult<TResponse> : IApiResult<TResponse>
    {
        public HttpStatusCode StatusCode { get; set; }
        public TResponse? Data { get; set; }
        public DateTime RequestStart { get; set; }
        public DateTime RequestEnd { get; set; }
    }

}
