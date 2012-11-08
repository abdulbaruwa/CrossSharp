using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrossSharp.Api.Controllers
{
    public class PuzzleGamesController : ApiController
    {
        //// GET api/puzzlegame
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/puzzlegame/5
        public string Get()
        {
            var gameDataSerializer = new GameDataSerializer();
            var resultTask = gameDataSerializer.GetPuzzleGroupJson();
            var result = resultTask.Result;
            return result;
        }

        // POST api/puzzlegame
        public void Post([FromBody]string value)
        {
        }

        // PUT api/puzzlegame/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/puzzlegame/5
        public void Delete(int id)
        {
        }
    }
}
