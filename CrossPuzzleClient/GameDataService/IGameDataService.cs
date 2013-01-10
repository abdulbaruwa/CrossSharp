using System.Threading.Tasks;

namespace CrossPuzzleClient.GameDataService
{
    public interface IGameDataService
    {
        Task GetGameDataAndStoreInLocalDb(string filePath);
        Task ProcessNewWordsIfAny(string filePath,string dbPath);
    }
}