using Microsoft.Maui.Controls;
using System;

namespace EduEquip;

public partial class StudentDashboard : ContentPage
{
    private string studentId;

    public StudentDashboard(string studentId)
    {
        InitializeComponent();
        this.studentId = studentId;
        WelcomeLabel.Text = $"Welcome, Student {studentId}";

       
    }

    protected async void OnViewProjectsClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ViewProjectsPage(studentId));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }

    protected async void OnRequestEquipmentClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new RequestEquipmentPage(studentId));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }

    protected async void OnTrackBorrowedEquipmentClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new TrackBorrowedEquipmentPage(studentId));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }

    protected async void OnTransactionHistoryClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new TransactionHistoryPage(studentId));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }
}