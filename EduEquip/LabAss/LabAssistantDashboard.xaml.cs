using Microsoft.Maui.Controls;

namespace EduEquip;

public partial class LabAssistantDashboard : ContentPage
{
    private string _assistantId;

    public LabAssistantDashboard(string assistantId)
    {
        InitializeComponent();
        _assistantId = assistantId;
        WelcomeLabel.Text = $"Welcome, Lab Assistant {assistantId}";
    }

    private async void OnManageInventoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryManagement());
    }

    private async void OnViewRequestsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ApproveRequests());
    }

    private async void OnProcessReturnsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProcessReturns());
    }

    private async void OnGenerateReportsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GenerateReportsPage());
    }
}