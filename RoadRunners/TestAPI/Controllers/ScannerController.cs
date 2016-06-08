using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestAPI.Controllers
{
    public class ScannerController : ApiController
    {
        // GET: api/Scanner
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Scanner/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Scanner
        public void Post([FromBody]object value)
        {
            var s = value;
        }

        // PUT: api/Scanner/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Scanner/5
        public void Delete(int id)
        {
        }
    }
}
