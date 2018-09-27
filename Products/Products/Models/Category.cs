

namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;

    public class Category
    {
       
        #region Services

        NavigationService navigationService;
        DialogService dialogService;
        #endregion

        #region Properties
        [PrimaryKey]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Product> Products { get; set; }
        #endregion
        
        #region Constructors
        public Category()
        {
            Products = new List<Product>();
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand SelectCategory
        {
            get
            {
                return new RelayCommand(SelectCategoryMethod);
            }

        }
        async void SelectCategoryMethod()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.ProductsViewModel = new ProductsViewModel(Products);
            mainViewModel.ProductsViewModel.Category = this;
            await navigationService.NavigateOnMaster("ProductsView");
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(EditMethod);
            }
        }

        async void EditMethod()
        {
            MainViewModel.GetInstance().EditCategory = new EditCategoryViewModel(this);
            await navigationService.NavigateOnMaster("EditCategoryView");
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(DeleteMethod);
            }
        }

        async void DeleteMethod()
        {
            var choice = await dialogService.ConfirmMessage(
                "Confirm", 
                "Are you sure to delete this record");
            if (!choice)
            {
                return;
            }

            await MainViewModel.GetInstance().CategoriesViewModel.DeleteCategory(this);
        }

        #endregion
        
        #region Methods
        public override int GetHashCode()
        {
            return CategoryId;
        }
        #endregion
    }
}
