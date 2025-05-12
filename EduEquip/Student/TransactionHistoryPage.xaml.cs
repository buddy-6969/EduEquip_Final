using System.Collections.ObjectModel;

namespace EduEquip;

public partial class TransactionHistoryPage : ContentPage
{
    private string studentId;
    private ObservableCollection<Transaction> transactions;

    public TransactionHistoryPage(string studentId)
    {
        InitializeComponent();
        this.studentId = studentId;
        StudentInfoLabel.Text = $"Student: {studentId}";

        // Load transaction history
        LoadTransactions();

        // Setup refresh command
        TransactionRefreshView.Command = new Command(() => {
            LoadTransactions();
            TransactionRefreshView.IsRefreshing = false;
        });
    }

    private void LoadTransactions()
    {
        // In a real app, this would load from a database based on studentId
        transactions = new ObservableCollection<Transaction>
        {
            new Transaction
            {
                Id = "T001",
                EquipmentName = "Desktop Computer",
                Description = "Borrowed for Desktop App Development project",
                Date = "April 1, 2025",
                Type = "Borrow",
                TypeColor = Colors.Green,
                IconSource = "borrow_icon.png"
            },
            new Transaction
            {
                Id = "T002",
                EquipmentName = "Logic Gates Kit",
                Description = "Borrowed for Logic Circuit Simulator project",
                Date = "March 15, 2025",
                Type = "Borrow",
                TypeColor = Colors.Green,
                IconSource = "borrow_icon.png"
            },
            new Transaction
            {
                Id = "T003",
                EquipmentName = "USB Drive",
                Description = "Borrowed for Desktop App Development project",
                Date = "March 10, 2025",
                Type = "Borrow",
                TypeColor = Colors.Green,
                IconSource = "borrow_icon.png"
            },
            new Transaction
            {
                Id = "T004",
                EquipmentName = "Logic Gates Kit",
                Description = "Extended return date for Logic Circuit Simulator project",
                Date = "March 30, 2025",
                Type = "Extension",
                TypeColor = Colors.Blue,
                IconSource = "extension_icon.png"
            }
        };

        TransactionCollection.ItemsSource = transactions;
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        string searchTerm = SearchEntry.Text?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // Reset to show all transactions
            TransactionCollection.ItemsSource = transactions;
            return;
        }

        // Filter by search term
        var filteredItems = new ObservableCollection<Transaction>(
            transactions.Where(item =>
                item.EquipmentName.ToLower().Contains(searchTerm) ||
                item.Description.ToLower().Contains(searchTerm))
        );

        TransactionCollection.ItemsSource = filteredItems;
    }

    private void OnTypeFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void OnDateFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filteredItems = transactions;

        // Apply type filter
        if (TypePicker.SelectedIndex > 0)
        {
            string selectedType = TypePicker.SelectedItem.ToString();
            filteredItems = new ObservableCollection<Transaction>(
                filteredItems.Where(item => item.Type == selectedType)
            );
        }

        // Apply date filter
        if (DatePicker.SelectedIndex > 0)
        {
            // In a real app, you would calculate the date range based on selection
            // For this example, we'll just simulate the filtering
            string selectedDateRange = DatePicker.SelectedItem.ToString();

            // Just simulating the filter
            // In a real app, you'd compare actual dates
            if (selectedDateRange == "Last 7 Days")
            {
                filteredItems = new ObservableCollection<Transaction>(
                    filteredItems.Where(t => t.Id != "T003") // Just an example filter
                );
            }
        }

        TransactionCollection.ItemsSource = filteredItems;
    }

    private async void OnTransactionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Transaction selectedTransaction)
        {
            // Clear selection
            TransactionCollection.SelectedItem = null;

            // Show transaction details
            await DisplayAlert(selectedTransaction.Type,
                $"Equipment: {selectedTransaction.EquipmentName}\n" +
                $"Date: {selectedTransaction.Date}\n" +
                $"Details: {selectedTransaction.Description}",
                "OK");
        }
    }
}

// Transaction class to represent transaction data
public class Transaction
{
    public string Id { get; set; }
    public string EquipmentName { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }
    public Color TypeColor { get; set; }
    public string IconSource { get; set; }
}