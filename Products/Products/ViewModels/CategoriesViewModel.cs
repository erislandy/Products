using GalaSoft.MvvmLight.Command;
using Products.Models;
using Products.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.ViewModels
{
    public class CategoriesViewModel :INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Attributes
        List<Category> categories;
        bool _isRefreshing;
        ObservableCollection<Category> _categories;
        string _filter;
        #endregion

        #region Services

        IApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;
        
        #endregion
        
        #region Properties
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if(_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Categories))
                        );

                }
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if(_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing))
                        );
                }
            }
        }
        public string  Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if(_filter != value)
                {
                    _filter = value;
                    Search();
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }
        #endregion

        #region Constructors
        public CategoriesViewModel()
        {
            instance = this;
            apiService = new ApiServiceWithoutConnection();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();
            LoadCategories();
        }
        #endregion

        #region Singleton
        static CategoriesViewModel instance;
        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategoriesViewModel();
            }
            else
            {
                return instance;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }

        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }
        void Search()
        {
            IsRefreshing = true;

            if (string.IsNullOrEmpty(Filter))
            {
                Categories = new ObservableCollection<Category>(
                    categories.OrderBy(c => c.Description));
            }
            else
            {
                Categories = new ObservableCollection<Category>(categories
                    .Where(c => c.Description.ToLower().Contains(Filter.ToLower()))
                    .OrderBy(c => c.Description));
            }

            IsRefreshing = false;

        }
        #endregion

        #region Methods
        async void LoadCategories()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                categories = dataService.Get<Category>(true);
                if(categories.Count == 0)
                {
                    await dialogService.ShowMessage(
                    "Error",
                   "Dear user YAPE");
                    IsRefreshing = false;
                    return;
                }
                
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();
                var response = await apiService.GetList<Category>(
                    "http://productszuluapi.azurewebsites.net",
                    "/api",
                    "/Categories",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken);

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        response.Message);
                    IsRefreshing = false;
                    return;
                }

                categories = (List<Category>)response.Result;
                SaveCategoriesOnDB();
            }


            Search();
            IsRefreshing = false;
        }

        void SaveCategoriesOnDB()
        {
            dataService.DeleteAll<Category>();
            dataService.DeleteAll<Product>();

            foreach (var category in categories)
            {
                dataService.Insert(category);
                dataService.Save(category.Products);
            }
        }


        public void AddCategory(Category category)
        {
            IsRefreshing = true;
            Categories = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description)
                );
            IsRefreshing = false;

        }
        
        public void UpdateCategory(Category category)
        {
            IsRefreshing = true;
            var oldCategory = Categories.Where(c => c.CategoryId == category.CategoryId).FirstOrDefault();
            oldCategory = category;
            Categories = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description)
                );
            IsRefreshing = false;
        }

        public async Task DeleteCategory(Category category)
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

            var response = await apiServiceWithoutConection.DeleteCategory(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            if (!response.IsSuccess)
            {
               await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                IsRefreshing = false;
                return;

            }
            IsRefreshing = false;
            Categories = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description)
                );
           
        }
        #endregion



    }
}
