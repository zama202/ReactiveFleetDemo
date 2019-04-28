using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Service.Tracking.Domain
{
    public class ResponseDomain
    {
        public HttpStatusCode HttpStatus { get; internal set; }
        public object Body { get; internal set; }
    }
}
