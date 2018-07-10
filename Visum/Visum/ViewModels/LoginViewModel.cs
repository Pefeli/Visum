namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System;
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
        private bool isEnabled;
        private bool isRunning;
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

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();

            this.IsRememberme = true;
            this.IsEnabled = true;

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

            this.IsRunning = true;
            this.IsEnabled = false;

            LoginAccess LoginAccess = new LoginAccess();

            LoginAccess.Email = this.Email;
            LoginAccess.Password = this.Password;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");

                this.IsRunning = false;
                this.IsEnabled = true;

                return;
            }

            var response = await this.apiService.Login<LoginResponse>(
                "https://pacific-taiga-76447.herokuapp.com",
                "/usuarios",
                "/login",
                LoginAccess);

            this.IsRunning = false;
            this.IsEnabled = true;

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

            MainViewModel.GetInstance().Home = new HomeViewModel();
            Application.Current.MainPage = new NavigationPage(new HomePage());
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
