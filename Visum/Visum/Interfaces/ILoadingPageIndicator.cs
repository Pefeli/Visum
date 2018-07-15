namespace Visum.Interfaces
{
    using Xamarin.Forms;

    public interface ILoadingPageIndicator
    {
        void InitLoadingPage(ContentPage loadingIndicatorPage = null);

        void ShowLoadingPage();

        void HideLoadingPage();
    }
}
