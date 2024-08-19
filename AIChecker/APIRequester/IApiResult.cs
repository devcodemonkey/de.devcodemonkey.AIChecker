using System.Net;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester
{
    public interface IApiResult<TResponse>
    {
        TResponse Data { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}