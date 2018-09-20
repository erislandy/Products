
namespace Products.ViewModels
{
    using System;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class EditProductViewModel : INotifyPropertyChanged
    {

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        DialogService dialogService;
        IApiService apiService;
        NavigationService navigationService;

        #endregion

        #region Attributes
        Product _product;
        string _categoryDescription;
        string _productDescription;
        string _price;
        string _image;
        bool _isActive;
        string _stock;
        DateTime _lastPurchace;
        string _remarks;
        bool _isRunning;
        bool _isEnabled;
        ImageSource _imageSource;
        MediaFile file;
        #endregion

        #region Properties
        public string CategoryDescription
        {
            get
            {
                return _categoryDescription;
            }
            set
            {
                if (_categoryDescription != value)
                {
                    _categoryDescription = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoryDescription)));
                }
            }
        }

        public string ProductDescription
        {
            get
            {
                return _productDescription;
            }
            set
            {
                if (_productDescription != value)
                {
                    _productDescription = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ProductDescription)));
                }
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Image)));
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }
        public string Stock
        {
            get
            {
                return _stock;
            }
            set
            {
                if (_stock != value)
                {
                    _stock = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Stock)));
                }
            }
        }
        public DateTime LastPurchase
        {
            get
            {
                return _lastPurchace;
            }
            set
            {
                if (_lastPurchace != value)
                {
                    _lastPurchace = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(LastPurchase)));
                }
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Remarks)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public ImageSource ImageSource
        {
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
            get
            {
                return _imageSource;
            }
        }

        #endregion

        #region Constructors
        public EditProductViewModel(Product product)
        {
            IsEnabled = true;
            _product = product;
            CategoryDescription = product.Description;
            ProductDescription = product.Description;
            Price = product.Price.ToString();
            IsActive = product.IsActive;
            Stock = product.Stock.ToString();
            LastPurchase = product.LastPurchase;
            Remarks = product.Remarks;
            ImageSource = product.ImageFullPath;

            apiService = new ApiServiceWithoutConnection();
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(SaveCommandMethod);
            }
        }

        async void SaveCommandMethod()
        {
            
            if (string.IsNullOrEmpty(ProductDescription))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product description... ");
                return;
            }

            _product.Description = ProductDescription;

            try
            {
                if (string.IsNullOrEmpty(Price))
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "You must enter a product price... ");
                    return;
                }
                _product.Price = decimal.Parse(Price);
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage(
                        "Error",
                        "You must enter a decimal product price... ");
                return;
            }

            try
            {
                _product.Stock = double.Parse(Stock);
            }

            catch (Exception ex)
            {
                await dialogService.ShowMessage(
                        "Error",
                        "You must enter a numeric product stock... ");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            _product.IsActive = IsActive;
            _product.Remarks = Remarks;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
           
            var apiServiceWithoutConection = (ApiServiceWithoutConnection)apiService;
            var response = await apiServiceWithoutConection.PutProduct(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                _product);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;

            }

            var productViewModel = mainViewModel.ProductsViewModel;
            productViewModel.UpdateProduct(_product);
            await navigationService.Back();


            IsRunning = false;
            IsEnabled = true;

        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await dialogService.ShowImageOptions();

                if (source == "Cancel")
                {
                    file = null;
                    return;
                }

                if (source == "From Camera")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        #endregion

    }
}
