
namespace Products.ViewModels
{

    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class SyncViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services

        DialogService dialogService;
        IApiService apiService;
        NavigationService navigationService;
        DataService dataService;
        #endregion

        #region Attributes

        string _message;
        bool _isEnabled;
        bool _isRunning;

        #endregion

        #region Properties

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
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Message)));
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

        #endregion

        #region Constructors
        public SyncViewModel()
        {
            IsEnabled = true;
          
            apiService = new ApiServiceWithoutConnection();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();

            Message = "Press sync button to start";
            
        }
        #endregion

        #region Comands
        public ICommand SyncCommand
        {
            get
            {
                return new RelayCommand(Sync);
            }
        }

        async void Sync()
        {
            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", connection.Message);
            }
            var products = dataService.Get<Product>(false)
                .Where(p => p.PendingToSave)
                .ToList();

            if (products.Count == 0)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Message",
                    "There are not products to sync");
                return;
            }
            var apiServiceWithoutConection = (ApiServiceWithoutConnection)apiService;
            var mainViewModel = MainViewModel.GetInstance();
            foreach (var product in products)
            {
                var response = await apiServiceWithoutConection.PostProduct(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

                if (response.IsSuccess)
                {
                    product.PendingToSave = false;
                    dataService.Delete(product);
                }
            }
            IsRunning = false;
            IsEnabled = true;
            await dialogService.ShowMessage("Confirm", "Sync operation Ok");

           await navigationService.BackOnMaster();

        }

        #endregion
    }
}
