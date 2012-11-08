using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossPuzzleClient.GameDataService
{
    class PuzzleWebApiService : IPuzzleWebApiService
    {
        public async Task<Stream> GetPuzzleDataFromApi()
        {
            Stream responseStream;
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:51425/api/") };
            HttpResponseMessage response = await client.GetAsync("PuzzleGames");
            response.EnsureSuccessStatusCode();

            responseStream = await response.Content.ReadAsStreamAsync();

            if (response.Content.Headers.ContentType.MediaType != "application/json") return null;

            return responseStream;
        }
    }
}