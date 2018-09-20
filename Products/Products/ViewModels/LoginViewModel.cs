

namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using System.ComponentModel;
    using System.Windows.Input;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        string _email;
        string _password;
        bool _isToggled;
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if(_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsToggled)));
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
        #endregion

        #region Services

        DialogService dialogService;
        IApiService apiService;
        NavigationService navigationService;

        #endregion
       
        #region Constructors
        public LoginViewModel()
        {
            IsEnabled = true;
            IsToggled = true;

            apiService = new ApiServiceWithoutConnection();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Email = "erislandy.cabrales@gmail.com";
            Password = "123456";
        }
        #endregion

        #region Comands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(LoginMethod);
            }
        }

        async void LoginMethod()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "You must enter an email...");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Error", "You must enter a password...");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();

            
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.GetToken(
                "http://productszuluapi.azurewebsites.net", 
                Email, 
                Password);

            if(response == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error", 
                    "The service is not available");
                return;

            }
            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error", 
                    response.ErrorDescription);
                return;

            }

            IsRunning = false;
            IsEnabled = true;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = response;
            mainViewModel.CategoriesViewModel = new CategoriesViewModel();
            await navigationService.Navigate("CategoriesView");
            
            Email = null;
            Password = null;
            

        }

        #endregion
    }
}
