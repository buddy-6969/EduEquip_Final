using System;
using EduEquip.Models;
using Microsoft.Maui.Controls;
using EduEquip.Services; // Add this using directive to access SubjectService

namespace EduEquip
{
    public partial class FacultyDashboard : ContentPage
    {
        private readonly string _userId;
        private readonly string _userName;

        public FacultyDashboard(string userId, string userName)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;

            // Set the welcome message with the faculty name
            WelcomeLabel.Text = $"Welcome, {_userName}";

            // Preload subjects to ensure they're available throughout the app
            SubjectService.LoadSubjects();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data if needed when returning to dashboard
        }

        private async void OnCreateSubjectClicked(object sender, EventArgs e)
        {
            // Navigate to Create Subject page, passing the faculty ID
            await Navigation.PushAsync(new CreateSubject(_userId));
        }

        private async void OnManageProjectsClicked(object sender, EventArgs e)
        {
            // Navigate to Manage Projects page
            await Navigation.PushAsync(new ManageProjects(_userId));
        }

        private async void OnViewBorrowedEquipmentClicked(object sender, EventArgs e)
        {
            // Navigate to View Borrowed Equipment page
            await Navigation.PushAsync(new ViewBorrowedEquipment(_userId));
        }

        private async void OnViewOverdueEquipmentClicked(object sender, EventArgs e)
        {
            // Navigate to View Overdue Equipment page
            await Navigation.PushAsync(new ViewOverdueEquipment());
        }
    }
}