namespace Visum.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Interfaces;
    using Models;
    using PQXamarin.Attributes;
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
        [Required("Ingrese un Nombre")]
        [MinLength(2, "Ingrese un Nombre")]
        public string Name
        {
            get;
            set;
        }

        [Required("Ingrese un Apellido")]
        [MinLength(2, "Ingrese un Apellido")]
        public string LastName
        {
            get;
            set;
        }

        [Required("Ingrese un Correo Electrónico", Priority = 0)]
        [Email("Correo Electrónico no válido", Priority = 1)]
        public string Email
        {
            get;
            set;
        }

        [Required("Ingrese un Número de Celular", Priority = 0)]
        [MinLength(7, "Ingrese un Número de Celular", Priority = 1)]
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

        [Required("Seleccione un Género")]
        public string Gender
        {
            get;
            set;
        }


        [Required("Ingrese una Contraseña", Priority = 0)]
        [RangeLength(6, 16, "Ingrese una Contraseña entre 6 y 16 caracteres", Priority = 1)]
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        [Required("Confirme la Contraseña", Priority = 0)]
        //[Compare("Password", "Las Contraseñas no coinciden", Priority = 1)]
        [RangeLength(6, 16, "Ingrese una Contraseña entre 6 y 16 caracteres", Priority = 1)]
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

            //Loading

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

            RegistrationRequest RegistrationRequest = new RegistrationRequest
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Birthdate = this.Birthdate.ToString("yyyy-MM-dd") + " 00:00:00",
                Gender = this.Gender.Substring(0, 1),
                CellPhone = this.CellPhone,
                Status = 1,
                Type = 1
            };

            var response = await this.apiService.Post<RegistrationResponse, RegistrationRequest>(
                "https://pacific-taiga-76447.herokuapp.com",
                "/usuarios",
                "/",
                RegistrationRequest);

            DependencyService.Get<ILoadingPageIndicator>().HideLoadingPage();

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");

                return;
            }

            var registrationResponse = (RegistrationResponse)response.Result;

            if (!registrationResponse.Complete && !registrationResponse.Error)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "El Correo Electrónico ingresado ya se encuentra registrado.",
                    "Aceptar");

                return;
            }

            if (registrationResponse.Error)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    registrationResponse.Message,
                    "Aceptar");

                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Usuario registrado con éxito. Por favor valida tu Correo Electrónico para completar el registro.",
                    "Aceptar");

            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion

        #region Methods

        #endregion
    }
}
