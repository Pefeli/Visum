using Visum.Helpers;
using Visum.ViewModels;
using Visum.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Visum
{
	public partial class App : Application
	{
        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }
        #endregion

        #region Constructor
        public App ()
		{
			InitializeComponent();

            if (string.IsNullOrEmpty(Settings.Token) || string.IsNullOrEmpty(Settings.UserId))
            {
			    MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainViewModel.GetInstance().Home = new HomeViewModel(Settings.Token);
                MainPage = new MasterPage();
            }
		}
        #endregion

        #region Methods
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
        #endregion
    }
}
