using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EduEquip
{
    public partial class ProcessReturns : ContentPage
    {
        // Observable collections for the returns lists
        private ObservableCollection<ReturnItem> pendingReturns;
        private ObservableCollection<ProcessedReturn> processedReturns;

        // Current return being processed
        private ReturnItem currentReturn;

        // Property for binding form visibility
        public bool IsReturnFormVisible { get; set; }

        public ProcessReturns()
        {
            InitializeComponent();

            // Initialize with sample data for pending returns
            pendingReturns = new ObservableCollection<ReturnItem>
            {
                new ReturnItem {
                    BorrowId = "BR001",
                    StudentName = "John Smith",
                    StudentId = "ST12345",
                    EquipmentName = "Microscope",
                    EquipmentId = "EQ001",
                    BorrowDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(2),
                    Status = "Due Soon",
                    StatusColor = Color.FromArgb("#ff9800")
                },
                new ReturnItem {
                    BorrowId = "BR002",
                    StudentName = "Maria Garcia",
                    StudentId = "ST12346",
                    EquipmentName = "Digital Scale",
                    EquipmentId = "EQ003",
                    BorrowDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(-1),
                    Status = "Overdue",
                    StatusColor = Color.FromArgb("#f44336")
                },
                new ReturnItem {
                    BorrowId = "BR003",
                    StudentName = "David Lee",
                    StudentId = "ST12347",
                    EquipmentName = "Safety Goggles",
                    EquipmentId = "EQ004",
                    BorrowDate = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(4),
                    Status = "Active",
                    StatusColor = Color.FromArgb("#4caf50")
                }
            };

            // Sample data for processed returns
            processedReturns = new ObservableCollection<ProcessedReturn>
            {
                new ProcessedReturn {
                    BorrowId = "BR004",
                    StudentName = "Emma Wilson",
                    EquipmentName = "Test Tubes (100ml)",
                    ReturnDate = DateTime.Now.AddDays(-1),
                    Condition = "Good",
                    RowColor = Colors.White
                },
                new ProcessedReturn {
                    BorrowId = "BR005",
                    StudentName = "James Brown",
                    EquipmentName = "Beakers (250ml)",
                    ReturnDate = DateTime.Now.AddDays(-2),
                    Condition = "Fair",
                    RowColor = Color.FromArgb("#f5f5f5")
                }
            };

            // Apply alternating row colors
            ApplyRowColors();

            // Set the collection view's item sources
            ReturnsCollectionView.ItemsSource = pendingReturns;
            ProcessedReturnsCollectionView.ItemsSource = processedReturns;

            // Initially hide the return form
            IsReturnFormVisible = false;
            ReturnForm.BindingContext = this;
        }

        // Apply alternating row colors to the lists
        private void ApplyRowColors()
        {
            bool isAlternate = false;
            foreach (var item in pendingReturns)
            {
                item.RowColor = isAlternate ? Color.FromArgb("#f5f5f5") : Colors.White;
                isAlternate = !isAlternate;
            }

            isAlternate = false;
            foreach (var item in processedReturns)
            {
                item.RowColor = isAlternate ? Color.FromArgb("#f5f5f5") : Colors.White;
                isAlternate = !isAlternate;
            }
        }

        // Handler for the search button
        private void OnSearchClicked(object sender, EventArgs e)
        {
            string searchText = SearchEntry.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // If search is empty, show all items
                ReturnsCollectionView.ItemsSource = pendingReturns;
                return;
            }

            // Filter the pending returns based on search text
            var filteredList = new ObservableCollection<ReturnItem>(
                pendingReturns.Where(item =>
                    item.BorrowId.ToLower().Contains(searchText) ||
                    item.StudentName.ToLower().Contains(searchText) ||
                    item.EquipmentName.ToLower().Contains(searchText))
            );

            ReturnsCollectionView.ItemsSource = filteredList;
        }

        // Handler for the process return button
        private void OnProcessReturnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string borrowId = button?.CommandParameter?.ToString();

            if (!string.IsNullOrEmpty(borrowId))
            {
                currentReturn = pendingReturns.FirstOrDefault(item => item.BorrowId == borrowId);
                if (currentReturn != null)
                {
                    // Populate form with return data
                    BorrowIdEntry.Text = currentReturn.BorrowId;
                    StudentNameEntry.Text = currentReturn.StudentName;
                    EquipmentNameEntry.Text = currentReturn.EquipmentName;
                    ReturnDatePicker.Date = DateTime.Today;
                    ConditionPicker.SelectedIndex = 1; // Default to "Good"

                    // Set return status based on due date
                    if (currentReturn.DueDate < DateTime.Today)
                    {
                        ReturnStatusPicker.SelectedItem = "Returned late";
                    }
                    else
                    {
                        ReturnStatusPicker.SelectedItem = "Returned on time";
                    }

                    // Show the return form
                    IsReturnFormVisible = true;
                    OnPropertyChanged(nameof(IsReturnFormVisible));
                }
            }
        }

        // Handler for the cancel button
        private void OnCancelProcessClicked(object sender, EventArgs e)
        {
            // Hide the return form
            IsReturnFormVisible = false;
            OnPropertyChanged(nameof(IsReturnFormVisible));
            currentReturn = null;
        }

        // Handler for the complete return button
        private async void OnCompleteReturnClicked(object sender, EventArgs e)
        {
            // Validate required fields
            if (ConditionPicker.SelectedIndex == -1 ||
                ReturnStatusPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Validation Error", "Please select equipment condition and return status.", "OK");
                return;
            }

            try
            {
                // Create a new processed return
                var newProcessedReturn = new ProcessedReturn
                {
                    BorrowId = currentReturn.BorrowId,
                    StudentName = currentReturn.StudentName,
                    EquipmentName = currentReturn.EquipmentName,
                    ReturnDate = ReturnDatePicker.Date,
                    Condition = ConditionPicker.SelectedItem.ToString(),
                    Notes = NotesEditor.Text ?? "",
                    RowColor = Colors.White // Will be updated by ApplyRowColors
                };

                // Add to processed returns and remove from pending
                processedReturns.Insert(0, newProcessedReturn);
                pendingReturns.Remove(currentReturn);

                // Refresh the lists with alternating colors
                ApplyRowColors();
                ReturnsCollectionView.ItemsSource = null;
                ReturnsCollectionView.ItemsSource = pendingReturns;
                ProcessedReturnsCollectionView.ItemsSource = null;
                ProcessedReturnsCollectionView.ItemsSource = processedReturns;

                // Hide the form
                IsReturnFormVisible = false;
                OnPropertyChanged(nameof(IsReturnFormVisible));
                currentReturn = null;

                await DisplayAlert("Success", "Return processed successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        // Handler for the back to dashboard button
        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    // Class to represent a pending return item
    public class ReturnItem
    {
        public string BorrowId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public Color StatusColor { get; set; }
        public Color RowColor { get; set; }
    }

    // Class to represent a processed return
    public class ProcessedReturn
    {
        public string BorrowId { get; set; }
        public string StudentName { get; set; }
        public string EquipmentName { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public Color RowColor { get; set; }
    }
}