using System.Collections.Generic;
using System.Net;

namespace Api.Models.ResponseModel
{
    public class Result<T>
    {
        public HttpStatusCode Status { get; set; }
        public T Entity { get; set; }
        public IEnumerable<Information> Infos { get; set; }
    }

    public struct Information
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}