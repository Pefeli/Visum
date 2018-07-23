
namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Models;
    using PQXamarin.Attributes;
    using Services;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string password;
        private string confirmPassword;
        #endregion

        #region Properties
        [Required("Ingrese una Contraseña", Priority = 0)]
        [RangeLength(6, 16, "Ingrese una Contraseña entre 6 y 16 caracteres", Priority = 1)]
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        [Required("Confirme la Contraseña", Priority = 0)]
        [RangeLength(6, 16, "Ingrese una Contraseña entre 6 y 16 caracteres", Priority = 1)]
        public string ConfirmPassword
        {
            get { return this.confirmPassword; }
            set { SetValue(ref this.confirmPassword, value); }
        }
        #endregion

        #region Constructor
        public ChangePasswordViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand ChangeCommand
        {
            get
            {
                return new RelayCommand(Change);
            }
        }

        private async void Change()
        {
            ShowErrors = true;

            if (HasErrors)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Valida la información ingresada.",
                    "Aceptar");

                return;
            }

            if (this.Password != this.ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Las Contraseñas no coinciden.",
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

            User UpdateRequest = new User
            {
                Password = this.Password
            };

            string Token = MainViewModel.GetInstance().Token;
            string UserId = MainViewModel.GetInstance().UserId;

            var response = await this.apiService.Patch<BasicResponse, User>(
                Application.Current.Resources["APIVisum"].ToString(),
                "/usuarios",
                "/",
                Token,
                UserId,
                UpdateRequest);

            DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                return;
            }

            var updateResponse = (BasicResponse)response.Result;

            if (updateResponse.Complete && !updateResponse.Error)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Contraseña actualizada exitosamente.",
                    "Aceptar");
            }
            else
            {
                if (updateResponse.Error)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        updateResponse.Message,
                        "Aceptar");

                    return;
                }

                MainViewModel.GetInstance().Token = string.Empty;
                Settings.Token = string.Empty;

                MainViewModel.GetInstance().UserId = string.Empty;
                Settings.UserId = string.Empty;

                MainViewModel.GetInstance().Name = string.Empty;
                Settings.Name = string.Empty;

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion
    }
}
