using System.IO;
using System.Threading.Tasks;

namespace CrossPuzzleClient.GameDataService
{
    public interface IPuzzleWebApiService
    {
        Task<Stream> GetPuzzleDataFromApi();
    }
}