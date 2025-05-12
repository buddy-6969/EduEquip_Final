using System.Collections.ObjectModel;

namespace EduEquip;

public partial class TrackBorrowedEquipmentPage : ContentPage
{
    private string studentId;
    private ObservableCollection<BorrowedEquipment> borrowedItems;

    public TrackBorrowedEquipmentPage(string studentId)
    {
        InitializeComponent();
        this.studentId = studentId;
        StudentInfoLabel.Text = $"Student: {studentId}";

        // Load borrowed equipment
        LoadBorrowedEquipment();

        // Setup refresh command
        EquipmentRefreshView.Command = new Command(() => {
            LoadBorrowedEquipment();
            EquipmentRefreshView.IsRefreshing = false;
        });
    }

    private void LoadBorrowedEquipment()
    {
        // In a real app, this would load from a database based on studentId
        borrowedItems = new ObservableCollection<BorrowedEquipment>
        {
            new BorrowedEquipment
            {
                Id = "E001",
                Name = "Desktop Computer",
                ProjectName = "Desktop App Development",
                BorrowDate = "April 1, 2025",
                DueDate = "April 25, 2025",
                Status = "Active",
                StatusColor = Colors.Green
            },
            new BorrowedEquipment
            {
                Id = "E005",
                Name = "Logic Gates Kit",
                ProjectName = "Logic Circuit Simulator",
                BorrowDate = "March 15, 2025",
                DueDate = "April 30, 2025",
                Status = "Active",
                StatusColor = Colors.Green
            },
            new BorrowedEquipment
            {
                Id = "E010",
                Name = "USB Drive",
                ProjectName = "Desktop App Development",
                BorrowDate = "March 10, 2025",
                DueDate = "March 25, 2025",
                Status = "Overdue",
                StatusColor = Colors.Red
            }
        };

        BorrowedEquipmentCollection.ItemsSource = borrowedItems;
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        string searchTerm = SearchEntry.Text?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // Reset to show all borrowed equipment
            BorrowedEquipmentCollection.ItemsSource = borrowedItems;
            return;
        }

        // Filter by search term
        var filteredItems = new ObservableCollection<BorrowedEquipment>(
            borrowedItems.Where(item =>
                item.Name.ToLower().Contains(searchTerm) ||
                item.ProjectName.ToLower().Contains(searchTerm))
        );

        BorrowedEquipmentCollection.ItemsSource = filteredItems;
    }

    private void OnProjectFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void OnStatusFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filteredItems = borrowedItems;

        // Apply project filter
        if (ProjectPicker.SelectedIndex > 0)
        {
            string selectedProject = ProjectPicker.SelectedItem.ToString();
            filteredItems = new ObservableCollection<BorrowedEquipment>(
                filteredItems.Where(item => item.ProjectName == selectedProject)
            );
        }

        // Apply status filter
        if (StatusPicker.SelectedIndex > 0)
        {
            string selectedStatus = StatusPicker.SelectedItem.ToString();
            filteredItems = new ObservableCollection<BorrowedEquipment>(
                filteredItems.Where(item => item.Status == selectedStatus)
            );
        }

        BorrowedEquipmentCollection.ItemsSource = filteredItems;
    }

    private async void OnEquipmentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is BorrowedEquipment selectedItem)
        {
            // Clear selection
            BorrowedEquipmentCollection.SelectedItem = null;

            // Show equipment details
            await DisplayAlert(selectedItem.Name,
                $"Project: {selectedItem.ProjectName}\n" +
                $"Borrowed: {selectedItem.BorrowDate}\n" +
                $"Due: {selectedItem.DueDate}\n" +
                $"Status: {selectedItem.Status}",
                "OK");
        }
    }
}

// BorrowedEquipment class to represent borrowed equipment data
public class BorrowedEquipment
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ProjectName { get; set; }
    public string BorrowDate { get; set; }
    public string DueDate { get; set; }
    public string Status { get; set; }
    public Color StatusColor { get; set; }
}