using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.Models
{
    public class Menu
    {
        #region Services

        NavigationService navigation;
        DataService dataService;
       
        #endregion

        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }

        #endregion

        #region Constructors

        public Menu()
        {
            navigation = new NavigationService();
            dataService = new DataService();
          
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(NavigateMethod);
            }
            
        }

        async void NavigateMethod()
        {
            switch (PageName)
            {
                case "LoginView":
                    {
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.Token.IsRemembered = false;
                        dataService.Update(mainViewModel.Token);
                        mainViewModel.Login = new LoginViewModel();
                        navigation.SetMainPage(PageName);

                        break;
                    }
                case "SyncView":
                    {
                        var mainViewModel = MainViewModel.GetInstance();
                         mainViewModel.Sync = new SyncViewModel();
                        await navigation.NavigateOnMaster(PageName);
                        break;
                    }
                case "MyProfileView":
                    {
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.MyProfile = new MyProfileViewModel();
                        await navigation.NavigateOnMaster(PageName);
                        break;
                    }

            }
        }

        #endregion

        
    }

}
