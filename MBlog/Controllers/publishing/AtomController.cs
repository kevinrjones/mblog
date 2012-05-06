using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MBlog.Controllers.publishing
{
    public class AtomController : ApiController
    {
        // GET /api/atom
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/atom/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/atom
        public void Post(string value)
        {
        }

        // PUT /api/atom/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/atom/5
        public void Delete(int id)
        {
        }
    }
}
