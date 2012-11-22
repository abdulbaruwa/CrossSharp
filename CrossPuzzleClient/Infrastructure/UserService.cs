using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Media.Imaging;

namespace CrossPuzzleClient.Infrastructure
{
    public class UserService : IUserService
    {

        public async Task<string> GetCurrentUser()
        {
            return await UserInformation.GetFirstNameAsync();
        }

        public async Task<BitmapImage> LoadUserImage()
        {
            var bitmapImage = new BitmapImage();

            var image = UserInformation.GetAccountPicture(AccountPictureKind.SmallImage) as StorageFile;
            if (image != null)
            {
                var imageStream = await image.OpenReadAsync();
                bitmapImage.SetSource(imageStream);
            }
            return bitmapImage;
        }
    }
}