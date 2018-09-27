using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Products
{
    using Products.Models;
    using Products.Services;
    using Products.ViewModels;
    using Views;
    public partial class App : Application
    {

        #region Services

        DataService dataService;
        NavigationService navigationService;
        #endregion
        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }
        public static MasterView Master
        {
            get;
            internal set;
        } 
        #endregion

        public App()
        {
            InitializeComponent();

            dataService = new DataService();
            navigationService = new NavigationService();

            var token = dataService.First<TokenResponse>(false);
            if (token != null &&
                token.IsRemembered &&
                token.Expires > DateTime.Now)
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = token;
                mainViewModel.CategoriesViewModel = new CategoriesViewModel();
                navigationService.SetMainPage("MasterView");
            }
            else
            {
                navigationService.SetMainPage("LoginView");
            }

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
