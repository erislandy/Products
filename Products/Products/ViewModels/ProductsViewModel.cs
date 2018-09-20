using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Products.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Services;

namespace Products.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        List<Product> _productsList;
        ObservableCollection<Product> _products;
        bool _isRefreshing;
        #endregion

        #region Services

        NavigationService navigationService;
        DialogService dialogService;
        IApiService apiService;

        #endregion


        #region Properties
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Products))
                        );

                }
            }
        }

        public Category Category { get; set; }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing))
                        );
                }
            }
        }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {

        }
        public ProductsViewModel(List<Product> products)
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiServiceWithoutConnection();
            if(products == null)
            {
                Products = new ObservableCollection<Product>();
            }
            else
            {
                _productsList = new List<Product>(products);
                Products = new ObservableCollection<Product>(
                products.OrderBy(p => p.Description));
            }
            

        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(RefreshProductsMethod);
            }

        }

        private async void RefreshProductsMethod()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message);
                IsRefreshing = false;
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var apiServiceWithoutConection = (ApiServiceWithoutConnection)apiService;

            var response = await apiServiceWithoutConection.GetCategory(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                Category.CategoryId);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                IsRefreshing = false;
                return;
            }
            IsRefreshing = false;
            var category = (Category)response.Result;

            Category = category;
            _productsList = category.Products;
            Products = new ObservableCollection<Product>(_productsList.OrderBy(p => p.Description));
        }


        #endregion

        #region Method

        public void AddProduct(Product product)
        {
            _productsList.Add(product);
            Products = new ObservableCollection<Product>(_productsList.OrderBy(p => p.Description));
        }

        public void UpdateProduct(Product product)
        {
            var productExisting = _productsList.Where(p => p.ProductId == product.ProductId).FirstOrDefault();
            productExisting.Description = product.Description;
            productExisting.Price = product.Price;
            productExisting.IsActive = product.IsActive;
            productExisting.LastPurchase = product.LastPurchase;
            productExisting.Remarks = product.Remarks;
            productExisting.Stock = product.Stock;

            Products = new ObservableCollection<Product>(_productsList.OrderBy(p => p.Description));
        }

        public async Task DeleteProduct(Product product)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var apiServiceWithoutConection = (ApiServiceWithoutConnection)apiService;

            var response = await apiServiceWithoutConection.DeleteProduct(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                     "Error",
                     response.Message);
                IsRefreshing = false;
                return;

            }
            IsRefreshing = false;
            var productsList = (List<Product>)response.Result;
            Category.Products = productsList;
            Products = new ObservableCollection<Product>(productsList);

        }

    }
        #endregion

    
}
