using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login { get; set; }
        public CategoriesViewModel CategoriesViewModel { get; set; }
        public TokenResponse Token { get; set; }
        public ProductsViewModel ProductsViewModel { get; set; }
        public NewCategoryViewModel NewCategory { get; set; }
        public EditCategoryViewModel EditCategory { get; set; }

        public NewProductViewModel NewProduct { get; set; }
        public EditProductViewModel EditProduct { get; internal set; }


        #endregion

        #region Services

        NavigationService navigationService;

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Login = new LoginViewModel();
            navigationService = new NavigationService();
        }
        #endregion

        #region Singleton
        static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if(instance == null)
            {
                return new MainViewModel();
            }
            else
            {
                return instance;
            }
        }
        #endregion

        #region Commands
        public ICommand NewCategoryCommand
        {
            get
            {
                return new RelayCommand(NewCategoryMethod);
            }
        }
        async void NewCategoryMethod()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.Navigate("NewCategoryView");
        }
        public ICommand NewProductCommand
        {
            get
            {
                return new RelayCommand(NewProductMethod);
            }
           
        }
        
        async void NewProductMethod()
        {
            NewProduct = new NewProductViewModel();
            await navigationService.Navigate("NewProductView");
        }

        #endregion

        
    }
}
