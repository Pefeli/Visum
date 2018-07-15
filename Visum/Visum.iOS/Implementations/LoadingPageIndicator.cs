[assembly: Xamarin.Forms.Dependency(typeof(Visum.iOS.Implementations.LoadingPageIndicator))]
namespace Visum.iOS.Implementations
{
    using System;
    using UIKit;
    using Views;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;
    using VisumPlatform = Xamarin.Forms.Platform.iOS.Platform;

    public class LoadingPageIndicator
    {
        private UIView _nativeView;

        private bool _isInitialized;

        public void InitLoadingPage(ContentPage loadingIndicatorPage)
        {
            // check if the page parameter is available
            if (loadingIndicatorPage != null)
            {
                // build the loading page with native base
                loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

                loadingIndicatorPage.Layout(new Rectangle(0, 0,
                    Xamarin.Forms.Application.Current.MainPage.Width,
                    Xamarin.Forms.Application.Current.MainPage.Height));

                var renderer = loadingIndicatorPage.GetOrCreateRenderer();

                _nativeView = renderer.NativeView;

                _isInitialized = true;
            }
        }

        public void ShowLoadingPage()
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(new LoadingIndicatorPage()); // set the default page

            // showing the native loading page
            UIApplication.SharedApplication.KeyWindow.AddSubview(_nativeView);
        }

        public void HideLoadingPage()
        {
            // Hide the page
            _nativeView.RemoveFromSuperview();
        }
    }

    internal static class PlatformExtension
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = VisumPlatform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = VisumPlatform.CreateRenderer(bindable);
                VisumPlatform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }
    }
}