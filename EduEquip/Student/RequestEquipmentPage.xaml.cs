using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace EduEquip;

public partial class RequestEquipmentPage : ContentPage
{
    private string studentId;
    private ObservableCollection<Equipment> equipmentItems;

    // Constructor for navigation from StudentDashboard
    public RequestEquipmentPage(string studentId)
    {
        InitializeComponent();
        this.studentId = studentId;
        StudentInfoLabel.Text = $"Student: {studentId}";

        LoadProjects();
    }

    // Constructor for navigation from ProjectDetailsPage
    public RequestEquipmentPage(string studentId, string projectId, string projectName)
    {
        InitializeComponent();
        this.studentId = studentId;
        StudentInfoLabel.Text = $"Student: {studentId}";

        LoadProjects();

        // Pre-select the project in the picker
        SelectProjectByName(projectName);
    }

    private void LoadProjects()
    {
        // Clear existing items
        ProjectPicker.Items.Clear();

        // Add default item
        ProjectPicker.Items.Add("Select a project");

        // Add projects
        ProjectPicker.Items.Add("Desktop App Development");
        ProjectPicker.Items.Add("Logic Circuit Simulator");
        ProjectPicker.Items.Add("CPU Architecture Analysis");

        // Select default
        ProjectPicker.SelectedIndex = 0;
    }

    private void SelectProjectByName(string projectName)
    {
        for (int i = 0; i < ProjectPicker.Items.Count; i++)
        {
            if (ProjectPicker.Items[i] == projectName)
            {
                ProjectPicker.SelectedIndex = i;
                return;
            }
        }
    }

    private void OnProjectSelectionChanged(object sender, EventArgs e)
    {
        if (ProjectPicker.SelectedIndex <= 0)
        {
            ProjectDetailsLabel.Text = "Project details will appear here";
            return;
        }

        string selectedProject = ProjectPicker.Items[ProjectPicker.SelectedIndex];
        ProjectDetailsLabel.Text = $"Project: {selectedProject}\nThis project requires various equipment items for development and testing.";

        // Load equipment for this project
        LoadEquipmentForProject(selectedProject);
    }

    private void LoadEquipmentForProject(string projectName)
    {
        // In a real app, this would load from a database based on the project
        equipmentItems = new ObservableCollection<Equipment>();

        // Add sample equipment based on project
        switch (projectName)
        {
            case "Desktop App Development":
                equipmentItems.Add(new Equipment { Name = "Desktop Computer", Category = "Computing Devices", InventoryStatus = "Available", StatusTextColor = Colors.Green });
                equipmentItems.Add(new Equipment { Name = "Development Software", Category = "Computing Devices", InventoryStatus = "Available", StatusTextColor = Colors.Green });
                equipmentItems.Add(new Equipment { Name = "USB Drive", Category = "Electronic Components", InventoryStatus = "Limited Stock", StatusTextColor = Colors.Orange });
                break;

            case "Logic Circuit Simulator":
                equipmentItems.Add(new Equipment { Name = "Logic Gates Kit", Category = "Electronic Components", InventoryStatus = "Available", StatusTextColor = Colors.Green });
                equipmentItems.Add(new Equipment { Name = "Breadboard", Category = "Lab Hardware", InventoryStatus = "Available", StatusTextColor = Colors.Green });
                equipmentItems.Add(new Equipment { Name = "Multimeter", Category = "Measurement Tools", InventoryStatus = "Limited Stock", StatusTextColor = Colors.Orange });
                break;

            case "CPU Architecture Analysis":
                equipmentItems.Add(new Equipment { Name = "CPU Development Board", Category = "Electronic Components", InventoryStatus = "Limited Stock", StatusTextColor = Colors.Orange });
                equipmentItems.Add(new Equipment { Name = "Oscilloscope", Category = "Measurement Tools", InventoryStatus = "Available", StatusTextColor = Colors.Green });
                equipmentItems.Add(new Equipment { Name = "Logic Analyzer", Category = "Measurement Tools", InventoryStatus = "Out of Stock", StatusTextColor = Colors.Red });
                break;
        }

        EquipmentCollection.ItemsSource = equipmentItems;
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        if (equipmentItems == null)
            return;

        string searchTerm = SearchEntry.Text?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // Reset to show all equipment for the current project
            if (ProjectPicker.SelectedIndex > 0)
            {
                LoadEquipmentForProject(ProjectPicker.Items[ProjectPicker.SelectedIndex]);
            }
            return;
        }

        // Filter by search term
        var filteredItems = new ObservableCollection<Equipment>(
            equipmentItems.Where(item =>
                item.Name.ToLower().Contains(searchTerm) ||
                item.Category.ToLower().Contains(searchTerm))
        );

        EquipmentCollection.ItemsSource = filteredItems;
    }

    private void OnCategoryChanged(object sender, EventArgs e)
    {
        if (equipmentItems == null || CategoryPicker.SelectedIndex < 0)
            return;

        string category = CategoryPicker.SelectedItem.ToString();

        if (category == "All Categories")
        {
            // Reset to show all equipment for the current project
            if (ProjectPicker.SelectedIndex > 0)
            {
                LoadEquipmentForProject(ProjectPicker.Items[ProjectPicker.SelectedIndex]);
            }
            return;
        }

        // Filter by category
        var filteredItems = new ObservableCollection<Equipment>(
            equipmentItems.Where(item => item.Category == category)
        );

        EquipmentCollection.ItemsSource = filteredItems;
    }

    private void OnEquipmentSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        int selectedCount = 0;

        // Count selected items
        if (EquipmentCollection.ItemsSource != null)
        {
            foreach (Equipment item in EquipmentCollection.ItemsSource)
            {
                if (item.IsSelected)
                    selectedCount++;
            }
        }

        SelectedCountLabel.Text = $"Selected: {selectedCount} items";
        SubmitButton.IsEnabled = selectedCount > 0;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSubmitRequestClicked(object sender, EventArgs e)
    {
        // In a real app, you would save the request to a database
        await DisplayAlert("Success", "Equipment request submitted successfully", "OK");
        await Navigation.PopAsync();
    }
}

// Equipment class to represent equipment data
public class Equipment
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string InventoryStatus { get; set; }
    public Color StatusTextColor { get; set; }
    public bool IsSelected { get; set; }
}