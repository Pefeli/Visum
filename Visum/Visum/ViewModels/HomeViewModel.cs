
namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Models;
    using Services;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class HomeViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string filter;
        private bool isShowingFilters;
        private string imageFilters;
        private int distanceFilter;
        private int priceFilterMin;
        private int priceFilterMax;
        #endregion

        #region Properties
        public string Filter
        {
            get { return this.filter; }
            set { SetValue(ref this.filter, value); }
        }

        public bool IsShowingFilters
        {
            get { return this.isShowingFilters; }
            set { SetValue(ref this.isShowingFilters, value); }
        }

        public string ImageFilters
        {
            get { return this.imageFilters; }
            set { SetValue(ref this.imageFilters, value); }
        }

        public int DistanceFilter
        {
            get { return this.distanceFilter; }
            set { SetValue(ref this.distanceFilter, value); }
        }

        public int PriceFilterMin
        {
            get { return this.priceFilterMin; }
            set { SetValue(ref this.priceFilterMin, value); }
        }

        public int PriceFilterMax
        {
            get { return this.priceFilterMax; }
            set { SetValue(ref this.priceFilterMax, value); }
        }
        #endregion

        #region Constructors
        public HomeViewModel()
        {
            this.Initialize();
        }

        public HomeViewModel(string Token)
        {
            this.Initialize();

            this.ValidateToken(Token);
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private async void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Debes ingresar un criterio para continuar con la búsqueda.",
                    "Aceptar");

                return;
            }

            if (this.PriceFilterMin > this.PriceFilterMax)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "El Rango Mínimo de Precios no puede ser superior al Rango Máximo.",
                    "Aceptar");

                return;
            }

            if (this.PriceFilterMax < this.PriceFilterMin)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "El Rango Máximo de Precios no puede ser inferior al Rango Mínimo.",
                    "Aceptar");

                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Atención",
                "Estás buscando..." + this.Filter,
                "Aceptar");
        }

        public ICommand ShowFiltersCommand
        {
            get
            {
                return new RelayCommand(ShowFilters);
            }
        }

        private void ShowFilters()
        {
            if (this.IsShowingFilters)
                this.ImageFilters = "arrow_drop_down.png";      
            else
                this.ImageFilters = "arrow_drop_up.png";

            this.IsShowingFilters = !this.IsShowingFilters;
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.apiService = new ApiService();

            this.ImageFilters = "arrow_drop_down.png";
            this.DistanceFilter = 5;
        }

        private async void ValidateToken(string Token)
        {
            //DependencyService.Get<ILoadingPageIndicator>().InitLoadingPage();
            //DependencyService.Get<ILoadingPageIndicator>().ShowLoadingPage();

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                //DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");

                MainViewModel.GetInstance().Token = Settings.Token;

                return;
            }

            var response = await this.apiService.ValidateUserToken<LoginResponse>(
                "https://pacific-taiga-76447.herokuapp.com",
                "/usuarios",
                "/me",
                Token);
        
            //DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                MainViewModel.GetInstance().Token = Settings.Token;

                return;
            }

            var loginResponse = (LoginResponse)response.Result;

            if (loginResponse.Complete && !loginResponse.Error)
            {
                MainViewModel.GetInstance().Token = Settings.Token;
            }
            else
            {
                Settings.Token = string.Empty;

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion
    }
}
