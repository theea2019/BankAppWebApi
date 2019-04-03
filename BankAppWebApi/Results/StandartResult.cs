using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BankAppWebApi.Models;

namespace BankAppWebApi.Results
{
    public class StandartResult<T> : IHttpActionResult where T : class
    {
        protected ResponseContent<T> _data;
        protected HttpRequestMessage _request;

        public StandartResult(ResponseContent<T> data, HttpRequestMessage request)
        {
            _data = data;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = ConvertStatusCode(_data.Result),
                Content = new ObjectContent<ResponseContent<T>>(_data, new JsonMediaTypeFormatter()),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }

        protected HttpStatusCode ConvertStatusCode(string result)
        {
            switch (result)
            {
                case "1":
                    return HttpStatusCode.OK;
                case "2":
                    return HttpStatusCode.NotFound;
                default:
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}