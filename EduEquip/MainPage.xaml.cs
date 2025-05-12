using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace EduEquip
{
    public partial class MainPage : ContentPage
    {
        // Dictionary to store user accounts (ID and Password)
        private Dictionary<string, string> studentAccounts = new Dictionary<string, string>
        {
            { "2022008114", "admin123" },
            { "2022008115", "admin123" },
            { "2022008116", "admin123" },
            { "2022008117", "admin123" },
            { "2022008118", "admin123" },
            { "2022008119", "admin123" },
            { "2022008120", "admin123" },
            { "2022008121", "admin123" },
            { "2022008122", "admin123" },
            { "2022008123", "admin123" }
        };

        // Add faculty accounts with usernames
        private Dictionary<string, (string password, string username)> facultyAccounts = new Dictionary<string, (string password, string username)>
        {
            { "FAC001", ("FAC123", "Professor Smith") },
            { "FAC002", ("FAC123", "Professor Johnson") }
        };

        // Add lab assistant accounts
        private Dictionary<string, string> labAssistantAccounts = new Dictionary<string, string>
        {
            { "LAB001", "lab123" },
            { "LAB002", "lab123" }
        };

        public MainPage()
        {
            InitializeComponent();
        }

        // Handle Login Button Click
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string schoolId = SchoolIdEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(schoolId) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both School ID and Password.", "OK");
                return;
            }

            // Check student accounts
            if (studentAccounts.TryGetValue(schoolId, out string studentPassword) && studentPassword == password)
            {
                await Navigation.PushAsync(new StudentDashboard(schoolId));
                return;
            }

            // Check faculty accounts
            if (facultyAccounts.TryGetValue(schoolId, out var facultyInfo) && facultyInfo.password == password)
            {
                await Navigation.PushAsync(new FacultyDashboard(schoolId, facultyInfo.username));
                return;
            }

            // Check lab assistant accounts
            if (labAssistantAccounts.TryGetValue(schoolId, out string labPassword) && labPassword == password)
            {
                await Navigation.PushAsync(new LabAssistantDashboard(schoolId));
                return;
            }

            // If no match found
            await DisplayAlert("Login Failed", "Invalid School ID or Password.", "OK");
        }

        // Handle Forgot Password Click
        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
    }
}