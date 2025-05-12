using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace EduEquip
{
    public partial class ViewOverdueEquipment : ContentPage
    {
        // Observable collection to hold overdue equipment items
        private ObservableCollection<OverdueEquipmentItem> _overdueEquipment;
        private List<OverdueEquipmentItem> _allOverdueEquipment;

        public ViewOverdueEquipment()
        {
            InitializeComponent();

            // Initialize the collections
            _allOverdueEquipment = new List<OverdueEquipmentItem>();
            _overdueEquipment = new ObservableCollection<OverdueEquipmentItem>();

            // Load overdue equipment data
            LoadOverdueEquipment();

            // Set the collection as the binding context for the CollectionView
            OverdueEquipmentCollection.ItemsSource = _overdueEquipment;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh the data when the page appears
            LoadOverdueEquipment();
        }

        private void LoadOverdueEquipment()
        {
            // In a real application, you would load this data from a database or service
            // For demonstration purposes, we'll create sample data
            _allOverdueEquipment = new List<OverdueEquipmentItem>
            {
                new OverdueEquipmentItem
                {
                    Id = 1,
                    EquipmentName = "Oscilloscope",
                    EquipmentId = "OSC-001",
                    StudentName = "John Doe",
                    StudentId = "S12345",
                    ProjectName = "Digital Circuit Analysis",
                    SubjectCode = "EE201",
                    BorrowDate = DateTime.Now.AddDays(-15),
                    DueDate = DateTime.Now.AddDays(-5),
                    DaysOverdue = 5,
                    RemindersSent = 1
                },
                new OverdueEquipmentItem
                {
                    Id = 2,
                    EquipmentName = "Logic Analyzer",
                    EquipmentId = "LA-002",
                    StudentName = "Jane Smith",
                    StudentId = "S12346",
                    ProjectName = "FPGA Programming",
                    SubjectCode = "EE301",
                    BorrowDate = DateTime.Now.AddDays(-20),
                    DueDate = DateTime.Now.AddDays(-8),
                    DaysOverdue = 8,
                    RemindersSent = 2
                },
                new OverdueEquipmentItem
                {
                    Id = 3,
                    EquipmentName = "Function Generator",
                    EquipmentId = "FG-003",
                    StudentName = "Mike Johnson",
                    StudentId = "S12347",
                    ProjectName = "Analog Signal Processing",
                    SubjectCode = "EE202",
                    BorrowDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(-1),
                    DaysOverdue = 1,
                    RemindersSent = 0
                }
            };

            // Update the observable collection with all items
            UpdateObservableCollection(_allOverdueEquipment);
        }

        private void UpdateObservableCollection(List<OverdueEquipmentItem> items)
        {
            _overdueEquipment.Clear();
            foreach (var item in items)
            {
                _overdueEquipment.Add(item);
            }
        }

        private void FilterEquipment(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                UpdateObservableCollection(_allOverdueEquipment);
                return;
            }

            searchText = searchText.ToLower();
            var filteredItems = _allOverdueEquipment.Where(item =>
                item.EquipmentName.ToLower().Contains(searchText) ||
                item.EquipmentId.ToLower().Contains(searchText) ||
                item.StudentName.ToLower().Contains(searchText) ||
                item.StudentId.ToLower().Contains(searchText)
            ).ToList();

            UpdateObservableCollection(filteredItems);
        }

        private async void OnSendReminderClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int equipmentId)
            {
                var item = _allOverdueEquipment.FirstOrDefault(i => i.Id == equipmentId);
                if (item != null)
                {
                    item.RemindersSent++;
                    // In a real application, you'd send an actual notification or email here
                    await DisplayAlert("Reminder Sent",
                        $"A reminder has been sent to {item.StudentName} regarding the overdue {item.EquipmentName}.",
                        "OK");
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Real-time filtering as user types
            FilterEquipment(e.NewTextValue);
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            FilterEquipment(SearchEntry.Text);
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            SearchEntry.Text = string.Empty;
            UpdateObservableCollection(_allOverdueEquipment);
        }

        private void OnEquipmentSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is OverdueEquipmentItem selectedItem)
            {
                // Show detail view or perform action with selected item
                // For now, just display basic info
                DisplayAlert("Equipment Details",
                    $"Equipment: {selectedItem.EquipmentName}\nStudent: {selectedItem.StudentName}\nDays Overdue: {selectedItem.DaysOverdue}",
                    "OK");

                // Clear selection
                OverdueEquipmentCollection.SelectedItem = null;
            }
        }

        private async void OnGenerateReportClicked(object sender, EventArgs e)
        {
            // In a real application, this would generate a report file
            await DisplayAlert("Report Generation",
                "Overdue equipment report has been generated and saved to your documents folder.",
                "OK");
        }

        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            // Navigate back to the faculty dashboard
            await Navigation.PopAsync();
        }
    }

    // Model class for overdue equipment items
    public class OverdueEquipmentItem
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentId { get; set; }
        public string StudentName { get; set; }
        public string StudentId { get; set; }
        public string ProjectName { get; set; }
        public string SubjectCode { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public int RemindersSent { get; set; }
    }
}