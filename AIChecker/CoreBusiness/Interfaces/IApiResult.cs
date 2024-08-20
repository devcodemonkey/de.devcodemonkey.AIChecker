using System.Net;

namespace de.devcodemonkey.AIChecker.DataSource.APIRequester.Interfaces
{
    public interface IApiResult<TResponse>
    {
        TResponse Data { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}