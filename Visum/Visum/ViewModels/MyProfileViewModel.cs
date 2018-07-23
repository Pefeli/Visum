namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Models;
    using PQXamarin.Attributes;
    using Services;
    using System;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MyProfileViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string name;
        private string lastName;
        private string email;
        private string cellPhone;
        private DateTime birthdate;
        private string gender;
        #endregion

        #region Properties
        [Required("Ingrese un Nombre")]
        [MinLength(2, "Ingrese un Nombre")]
        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        [Required("Ingrese un Apellido")]
        [MinLength(2, "Ingrese un Apellido")]
        public string LastName
        {
            get { return this.lastName; }
            set { SetValue(ref this.lastName, value); }
        }

        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        [Required("Ingrese un Número de Celular", Priority = 0)]
        [MinLength(7, "Ingrese un Número de Celular", Priority = 1)]
        public string CellPhone
        {
            get { return this.cellPhone; }
            set { SetValue(ref this.cellPhone, value); }
        }

        public DateTime Birthdate
        {
            get { return this.birthdate; }
            set { SetValue(ref this.birthdate, value); }
        }

        public DateTime MinBirthdate
        {
            get;
            set;
        }

        public DateTime MaxBirthdate
        {
            get;
            set;
        }

        [Required("Seleccione un Género")]
        public string Gender
        {
            get { return this.gender; }
            set { SetValue(ref this.gender, value); }
        }
        #endregion

        #region Constructor
        public MyProfileViewModel()
        {
            this.apiService = new ApiService();

            this.MinBirthdate = DateTime.Now.AddYears(-100);
            this.MaxBirthdate = DateTime.Now.AddYears(-10);

            this.LoadUserData();
        }
        #endregion

        #region Commands
        public ICommand UpdateCommand
        {
            get
            {
                return new RelayCommand(Update);
            }
        }

        private async void Update()
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
                Name = this.Name,
                LastName = this.LastName,
                CellPhone = this.CellPhone,
                Birthdate = this.Birthdate.ToString("yyyy-MM-dd") + " 00:00:00",
                Gender = this.Gender.Substring(0, 1)
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
                MainViewModel.GetInstance().Name = this.Name;
                Settings.Name = this.Name;

                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Datos actualizados exitosamente.",
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

        #region Methods
        private async void LoadUserData()
        {
            string Token = MainViewModel.GetInstance().Token;

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

            var response = await this.apiService.ValidateUserDataByToken<UserResponse>(
                Application.Current.Resources["APIVisum"].ToString(),
                "/usuarios",
                "/me",
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

            var loginResponse = (UserResponse)response.Result;

            if (loginResponse.Complete && !loginResponse.Error)
            {
                this.Email = loginResponse.User.Email;
                this.Name = loginResponse.User.Name;
                this.LastName = loginResponse.User.LastName;
                this.CellPhone = loginResponse.User.CellPhone;
                this.Birthdate = new DateTime(1970, 1, 1).AddSeconds(double.Parse(loginResponse.User.Birthdate));

                if (loginResponse.User.Gender == "O")
                    this.Gender = "Otro";
                else if (loginResponse.User.Gender == "F")
                    this.Gender = "Femenino";
                else if (loginResponse.User.Gender == "M")
                    this.Gender = "Masculino";
            }
            else
            {
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
