using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Products.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "Accept");
        }

        public async Task<bool> ConfirmMessage(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "Yes",
                "No");
        }

        public async Task<string> ShowImageOptions()
        {
            return await Application.Current.MainPage.DisplayActionSheet(
                "Where do you take the image?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");
        }

    }
}
