using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace CrossPuzzleClient.Infrastructure
{
    public interface IUserService
    {
        Task<BitmapImage> LoadUserImageAsync();
        Task<string> GetCurrentUserAsync();
    }
}