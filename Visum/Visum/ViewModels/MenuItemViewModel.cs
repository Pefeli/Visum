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

                var response = await apiService.Logout<BasicResponse>(
                    Application.Current.Resources["APIVisum"].ToString(),
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

                var logoutResponse = (BasicResponse)response.Result;

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

                MainViewModel.GetInstance().UserId = string.Empty;
                Settings.UserId = string.Empty;

                MainViewModel.GetInstance().Name = string.Empty;
                Settings.Name = string.Empty;

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                await App.Navigator.PushAsync(new MyProfilePage());
            }
            else if (this.PageName == "ChangePasswordPage")
            {
                MainViewModel.GetInstance().ChangePassword = new ChangePasswordViewModel();
                await App.Navigator.PushAsync(new ChangePasswordPage());
            }
        }
        #endregion
    }
}
