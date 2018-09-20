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
    public class Product
    {
       
        #region Properties
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
        public bool IsActive { get; set; }

        public double Stock { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Remarks { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "noImage";
                }
                else
                {
                    return string.Format(
                        "http://productszuluapi.azurewebsites.new/{0}",
                        Image.Substring(1));
                }
            }

        }
        public byte[] ImageArray { get; set; }
        #endregion

        #region Services

        NavigationService navigationService;
        DialogService dialogService;
        #endregion

        #region Constructors
        public Product()
        {
            
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(EditMethod);
            }
        }

        async void EditMethod()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await navigationService.Navigate("EditProductView");
        }

        
        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(DeleteMethod);
            }
        }

        public async void DeleteMethod()
        {
            var choice = await dialogService.ConfirmMessage(
                "Confirm",
                "Are you sure to delete this record");
            if (!choice)
            {
                return;
            }

            await MainViewModel.GetInstance().ProductsViewModel.DeleteProduct(this);
        }

        #endregion
    }
}
