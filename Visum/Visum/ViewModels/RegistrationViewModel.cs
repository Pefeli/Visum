namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class RegistrationViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string password;
        private string confirmPassword;
        #endregion

        #region Properties
        public string Name
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string CellPhone
        {
            get;
            set;
        }

        public DateTime Birthdate
        {
            get;
            set;
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

        public int Gender
        {
            get;
            set;
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public string ConfirmPassword
        {
            get { return this.confirmPassword; }
            set { SetValue(ref this.confirmPassword, value); }
        }
        #endregion

        #region Constructor
        public RegistrationViewModel()
        {
            this.apiService = new ApiService();

            this.Birthdate = DateTime.Now.AddYears(-11);
            this.MinBirthdate = DateTime.Now.AddYears(-100);
            this.MaxBirthdate = DateTime.Now.AddYears(-10);
        }
        #endregion

        #region Commands
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            //Activate loading

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    connection.Message,
                    "Aceptar");
            }

            //RegistrationRequest RegistrationRequest = new RegistrationRequest
            //{

            //}
        }
        #endregion

        #region Methods

        #endregion
    }
}
