using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossPuzzleClient.GameDataService
{
    public class FakePuzzleWebApiService : IPuzzleWebApiService
    {
        private const string GameJsonData = "[{\"PuzzleGroupId\":1,\"Name\":\"Science\",\"Puzzles\":[{\"Title\":\"KS2 Science\",\"PuzzleSubGroupId\":1,\"PuzzleGames\":[{\"PuzzleGameId\":1,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}},{\"PuzzleGameId\":2,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}}]},{\"Title\":\"KS3 Science\",\"PuzzleSubGroupId\":2,\"PuzzleGames\":[{\"PuzzleGameId\":3,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}}]}]},{\"PuzzleGroupId\":2,\"Name\":\"English\",\"Puzzles\":[]}]";
        public async Task<Stream> GetPuzzleDataFromApi()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
                               {
                                   Content = new StringContent(GameJsonData)
                               };
            return await response.Content.ReadAsStreamAsync();
        }
    }
}