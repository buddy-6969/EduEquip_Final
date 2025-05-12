using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace EduEquip
{
    public partial class GenerateReportsPage : ContentPage
    {
        public GenerateReportsPage()
        {
            // Remove the SS prefix if it was accidentally added
            InitializeComponent();
            
            SetupReportContent();
        }
        
        private void SetupReportContent()
        {
            try
            {
                // Make sure these elements exist in the XAML
                if (ReportTitleLabel != null)
                    ReportTitleLabel.Text = "Equipment Inventory Report";
                
                if (ReportDateLabel != null)
                    ReportDateLabel.Text = $"Generated on: {DateTime.Now.ToShortDateString()}";
                
                PopulateReportContent();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error setting up report: {ex.Message}", "OK");
            }
        }
        
        private void PopulateReportContent()
        {
            if (ReportContentLayout == null)
                return;
                
            try
            {
                Label headerLabel = new Label
                {
                    Text = "Equipment Summary",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                
                Label contentLabel = new Label
                {
                    Text = "Total equipment: 42\nAvailable: 35\nChecked out: 7\n\nMost frequently used: Laptops (15 checkouts)"
                };
                
                ReportContentLayout.Children.Clear();
                ReportContentLayout.Children.Add(headerLabel);
                ReportContentLayout.Children.Add(contentLabel);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error populating report: {ex.Message}", "OK");
            }
        }
    }
}