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

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string password;
        #endregion

        #region Properties
        public string Email
        {
            get;
            set;
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRememberme
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();

            this.IsRememberme = true;

            this.Email = "anfelipeloal@gmail.com";
            this.Password = "andres1";
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Debes ingresar un Correo Electrónico.",
                    "Aceptar");

                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Debes ingresar una Contraseña.",
                    "Aceptar");

                return;
            }

            DependencyService.Get<ILoadingPageIndicator>().InitLoadingPage();
            DependencyService.Get<ILoadingPageIndicator>().ShowLoadingPage();

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");

                return;
            }

            LoginRequest LoginAccess = new LoginRequest
            {
                Email = this.Email,
                Password = this.Password
            };

            var response = await this.apiService.Login<LoginResponse>(
                "https://pacific-taiga-76447.herokuapp.com",
                "/usuarios",
                "/login",
                LoginAccess);

            DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                return;
            }

            var loginResponse = (LoginResponse)response.Result;

            if (!loginResponse.Complete && !loginResponse.Error)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Correo Electrónico o Contraseña incorrectos.",
                    "Aceptar");

                this.Password = string.Empty;

                return;
            }

            if (loginResponse.Error)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    loginResponse.Message,
                    "Aceptar");

                this.Password = string.Empty;

                return;
            }

            string Token = loginResponse.User.Tokens[loginResponse.User.Tokens.Count - 1].Value;

            MainViewModel.GetInstance().Token = Token;

            if (this.IsRememberme)
                Settings.Token = Token;

            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new MasterPage();
        }

        public ICommand RegistrationCommand
        {
            get
            {
                return new RelayCommand(Registration);
            }
        }

        private async void Registration()
        {
            MainViewModel.GetInstance().Registration = new RegistrationViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());

        }
        #endregion

        #region Methods

        #endregion
    }
}
