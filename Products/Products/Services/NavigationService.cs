using Products.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Products.Services
{
    public class NavigationService
    {
        public async Task Navigate (string pageName)
        {
            switch (pageName)
            {
                case  "CategoriesView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new CategoriesView());
                        break;
                    }
                case "ProductsView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
                        break;
                    }
                case "NewCategoryView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new NewCategoryView());
                        break;
                    }
                case "EditCategoryView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new EditCategoryView());
                        break;
                    }
                case "NewProductView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new NewProductView());
                        break;
                    }
                case "EditProductView":
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new EditProductView());
                        break;
                    }
                default:
                    break;
            }
            
        }

        public async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
