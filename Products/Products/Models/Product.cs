using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Products.Models
{
    public class Product
    {
       
        #region Properties

        [PrimaryKey, AutoIncrement]
        public int ProductId { get; set; }

        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }

        [ManyToOne]
        public Category Category { get; set; }

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
                    /* Desde el API
                    return string.Format(
                        "http://productszuluapi.azurewebsites.new/{0}",
                        Image.Substring(1));*/

                    return Image;
                }
            }

        }
        public byte[] ImageArray { get; set; }

        public bool PendingToSave { get; set; }

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
            await navigationService.NavigateOnMaster("EditProductView");
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
