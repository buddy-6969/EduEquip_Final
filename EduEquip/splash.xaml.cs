using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace EduEquip
{
    public partial class splash : ContentPage
    {
        public splash()
        {
            InitializeComponent();
            StartSplash();
        }

        private async void StartSplash()
        {
            try
            {
                // Wait a bit to ensure the UI is fully loaded
                await Task.Delay(200);

                // Run animations on the UI thread
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    await LogoImage.FadeTo(1, 1300); // Fade in
                    await Task.Delay(900);          // Hold
                    await LogoImage.FadeTo(0, 900); // Fade out

                    // Navigate to MainPage
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Animation Error", ex.Message, "OK");
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
