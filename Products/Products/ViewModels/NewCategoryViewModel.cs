
namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.ComponentModel;
    using System.Windows.Input;

    public class NewCategoryViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        string _description;
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Services

        DialogService dialogService;
        IApiService apiService;
        NavigationService navigationService;

        #endregion

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if(_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Description))
                        );
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

        #region Constructors
        public NewCategoryViewModel()
        {
            IsEnabled = true;


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
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a category description... ");
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

            Category category = new Category()
            {
                Description = Description
            };
            var mainViewModel = MainViewModel.GetInstance();

            var apiServiceWithoutConection = (ApiServiceWithoutConnection)apiService;
            var response = await apiServiceWithoutConection.PostCategory(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;

            }

            category = (Category)response.Result;
            var categoryViewModel = CategoriesViewModel.GetInstance();
            categoryViewModel.AddCategory(category);
            await navigationService.Back();


            IsRunning = false;
            IsEnabled = true;

        }
        #endregion

       


    }
}
