namespace EduEquip;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Please enter your email.", "OK");
            return;
        }

        // Simulate password reset process
        await DisplayAlert("Success", "Password reset instructions have been sent to your email.", "OK");

        // Navigate back to the previous page
        await Navigation.PopAsync();
    }
}
