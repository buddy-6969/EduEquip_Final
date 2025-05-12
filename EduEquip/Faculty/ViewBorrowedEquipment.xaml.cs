using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EduEquip
{
    public partial class ViewBorrowedEquipment : ContentPage
    {
        private string _facultyId;
        private ObservableCollection<EquipmentLoan> _allEquipment;
        private EquipmentLoan _selectedEquipment;

        public ViewBorrowedEquipment(string facultyId)
        {
            InitializeComponent();
            _facultyId = facultyId;

            // Initialize collections
            _allEquipment = new ObservableCollection<EquipmentLoan>();

            // Populate filter pickers
            PopulateSubjectPicker();

            // Load data
            LoadBorrowedEquipmentData();

            // Set total items
            UpdateTotalItemsLabel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when page appears
            LoadBorrowedEquipmentData();
        }

        private void PopulateSubjectPicker()
        {
            // This would typically load from a database
            // For demonstration, we'll use sample data
            List<string> subjects = new List<string>
            {
                "Computer Programming",
                "Electronics",
                "Circuit Analysis",
                "Digital Logic Design",
                "Computer Architecture"
            };

            foreach (var subject in subjects)
            {
                SubjectPicker.Items.Add(subject);
            }
        }

        private void PopulateProjectPicker()
        {
            // This would be filtered based on selected subject
            // For demonstration, clear existing items and add sample data
            ProjectPicker.Items.Clear();

            if (SubjectPicker.SelectedIndex != -1)
            {
                List<string> projects = new List<string>
                {
                    "Final Project",
                    "Midterm Project",
                    "Laboratory Exercise 1",
                    "Laboratory Exercise 2",
                    "Group Assignment"
                };

                foreach (var project in projects)
                {
                    ProjectPicker.Items.Add(project);
                }
            }
        }

        private void LoadBorrowedEquipmentData()
        {
            // In a real application, this would load from a database
            // We'll use sample data for demonstration
            _allEquipment.Clear();

            // Sample data
            _allEquipment.Add(new EquipmentLoan
            {
                EquipmentId = "EQ001",
                EquipmentName = "Arduino Uno",
                StudentName = "John Smith",
                ProjectName = "Digital Clock",
                SubjectName = "Electronics",
                BorrowDate = DateTime.Now.AddDays(-5),
                ReturnDate = DateTime.Now.AddDays(10)
            });

            _allEquipment.Add(new EquipmentLoan
            {
                EquipmentId = "EQ002",
                EquipmentName = "Oscilloscope",
                StudentName = "Emily Johnson",
                ProjectName = "Signal Analysis",
                SubjectName = "Circuit Analysis",
                BorrowDate = DateTime.Now.AddDays(-3),
                ReturnDate = DateTime.Now.AddDays(7)
            });

            _allEquipment.Add(new EquipmentLoan
            {
                EquipmentId = "EQ003",
                EquipmentName = "Raspberry Pi 4",
                StudentName = "Michael Brown",
                ProjectName = "IoT Home System",
                SubjectName = "Computer Architecture",
                BorrowDate = DateTime.Now.AddDays(-7),
                ReturnDate = DateTime.Now.AddDays(14)
            });

            _allEquipment.Add(new EquipmentLoan
            {
                EquipmentId = "EQ004",
                EquipmentName = "Multimeter",
                StudentName = "Sarah Wilson",
                ProjectName = "Voltage Divider",
                SubjectName = "Electronics",
                BorrowDate = DateTime.Now.AddDays(-2),
                ReturnDate = DateTime.Now.AddDays(5)
            });

            _allEquipment.Add(new EquipmentLoan
            {
                EquipmentId = "EQ005",
                EquipmentName = "Logic Analyzer",
                StudentName = "David Miller",
                ProjectName = "Digital Circuit",
                SubjectName = "Digital Logic Design",
                BorrowDate = DateTime.Now.AddDays(-4),
                ReturnDate = DateTime.Now.AddDays(11)
            });

            // Apply current filters and search
            ApplyFiltersAndSearch();
        }

        private void ApplyFiltersAndSearch()
        {
            // Start with all equipment
            var filteredEquipment = _allEquipment.ToList();

            // Apply subject filter if selected
            if (SubjectPicker.SelectedIndex != -1)
            {
                string selectedSubject = SubjectPicker.Items[SubjectPicker.SelectedIndex];
                filteredEquipment = filteredEquipment.Where(e => e.SubjectName == selectedSubject).ToList();
            }

            // Apply project filter if selected
            if (ProjectPicker.SelectedIndex != -1)
            {
                string selectedProject = ProjectPicker.Items[ProjectPicker.SelectedIndex];
                filteredEquipment = filteredEquipment.Where(e => e.ProjectName == selectedProject).ToList();
            }

            // Apply search text if any
            if (!string.IsNullOrWhiteSpace(SearchEntry.Text))
            {
                string searchText = SearchEntry.Text.ToLower();
                filteredEquipment = filteredEquipment.Where(e =>
                    e.EquipmentId.ToLower().Contains(searchText) ||
                    e.EquipmentName.ToLower().Contains(searchText) ||
                    e.StudentName.ToLower().Contains(searchText) ||
                    e.ProjectName.ToLower().Contains(searchText)
                ).ToList();
            }

            // Update the list view
            BorrowedEquipmentListView.ItemsSource = new ObservableCollection<EquipmentLoan>(filteredEquipment);

            // Update the total items label
            UpdateTotalItemsLabel();
        }

        private void UpdateTotalItemsLabel()
        {
            int count = BorrowedEquipmentListView.ItemsSource != null ?
                ((ObservableCollection<EquipmentLoan>)BorrowedEquipmentListView.ItemsSource).Count : 0;
            TotalItemsLabel.Text = $"Total Items: {count}";
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (sender == SubjectPicker)
            {
                // Update project picker based on selected subject
                PopulateProjectPicker();
            }

            ApplyFiltersAndSearch();
        }

        private void OnResetFilters(object sender, EventArgs e)
        {
            // Clear all filters
            SubjectPicker.SelectedIndex = -1;
            ProjectPicker.SelectedIndex = -1;
            ProjectPicker.Items.Clear();
            SearchEntry.Text = string.Empty;

            // Reset to show all equipment
            BorrowedEquipmentListView.ItemsSource = _allEquipment;
            UpdateTotalItemsLabel();
        }

        private void OnEquipmentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedEquipment = e.SelectedItem as EquipmentLoan;
            ViewDetailsButton.IsEnabled = _selectedEquipment != null;
        }

        private async void OnViewDetailsClicked(object sender, EventArgs e)
        {
            if (_selectedEquipment != null)
            {
                // In a real app, you'd navigate to a details page
                // For now, just show a simple alert
                await DisplayAlert("Equipment Details",
                    $"ID: {_selectedEquipment.EquipmentId}\n" +
                    $"Name: {_selectedEquipment.EquipmentName}\n" +
                    $"Student: {_selectedEquipment.StudentName}\n" +
                    $"Project: {_selectedEquipment.ProjectName}\n" +
                    $"Subject: {_selectedEquipment.SubjectName}\n" +
                    $"Borrowed: {_selectedEquipment.BorrowDate:MM/dd/yyyy}\n" +
                    $"Return by: {_selectedEquipment.ReturnDate:MM/dd/yyyy}",
                    "Close");
            }
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            // In a real app, this would export data to a CSV file
            await DisplayAlert("Export", "Data exported to CSV successfully.", "OK");
        }
    }

    // Model class for borrowed equipment - renamed to avoid namespace conflict
    public class EquipmentLoan
    {
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string StudentName { get; set; }
        public string ProjectName { get; set; }
        public string SubjectName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}