
namespace Visum.ViewModels
{
    using System;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }
        #endregion

        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }

        public HomeViewModel Home
        {
            get;
            set;
        }

        public RegistrationViewModel Registration
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            this.Login = new LoginViewModel();

            this.LoadMenu();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "person_black.png",
                PageName = "MyProfilePage",
                Title = "Mi Pefil"
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "settings_black.png",
                PageName = "SettingsPage",
                Title = "Configuración"
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "lock_black.png",
                PageName = "ChangePasswordPage",
                Title = "Cambiar Contraseña"
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "help_black.png",
                PageName = "HelpPage",
                Title = "Ayuda"
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "exit_black.png",
                PageName = "LoginPage",
                Title = "Cerrar Sesión"
            });
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
