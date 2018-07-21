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

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string PageName
        {
            get;
            set;
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navegate);
            }
        }

        private async void Navegate()
        {
            if (this.PageName == "LoginPage")
            {
                ApiService apiService = new ApiService();

                DependencyService.Get<ILoadingPageIndicator>().InitLoadingPage();
                DependencyService.Get<ILoadingPageIndicator>().ShowLoadingPage();

                var connection = await apiService.CheckConnection();

                if (!connection.IsSuccess)
                {
                    DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        connection.Message,
                        "Aceptar");

                    return;
                }

                string Token = MainViewModel.GetInstance().Token;

                var response = await apiService.Logout<LogoutResponse>(
                    "https://pacific-taiga-76447.herokuapp.com",
                    "/usuarios",
                    "/me/token",
                    Token);

                DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        response.Message,
                        "Aceptar");

                    return;
                }

                var logoutResponse = (LogoutResponse)response.Result;

                if (!logoutResponse.Complete && !logoutResponse.Error)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Atención",
                        logoutResponse.Message,
                        "Aceptar");

                    return;
                }

                if (logoutResponse.Error)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        logoutResponse.Message,
                        "Aceptar");

                    return;
                }

                MainViewModel.GetInstance().Token = string.Empty;
                Settings.Token = string.Empty;

                Application.Current.MainPage = new LoginPage();
            }
        }
        #endregion
    }
}
