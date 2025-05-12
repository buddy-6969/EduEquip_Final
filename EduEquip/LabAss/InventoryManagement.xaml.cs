using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EduEquip
{
    public partial class InventoryManagement : ContentPage
    {
        // Observable collection for the equipment list
        private ObservableCollection<EquipmentItem> equipmentList;

        // Track if we're editing an existing item
        private bool isEditing = false;
        private string currentEditId = string.Empty;

        public InventoryManagement()
        {
            InitializeComponent();

            // Initialize equipment list with sample data
            equipmentList = new ObservableCollection<EquipmentItem>
            {
                new EquipmentItem { Id = "EQ001", Name = "Microscope", Category = "Electronic Equipment", Quantity = 10, Condition = "Good", Location = "Cabinet 3", Notes = "Last maintenance: Jan 2025" },
                new EquipmentItem { Id = "EQ002", Name = "Test Tubes (100ml)", Category = "Laboratory Glassware", Quantity = 50, Condition = "Good", Location = "Drawer B2", Notes = "" },
                new EquipmentItem { Id = "EQ003", Name = "Digital Scale", Category = "Measuring Instruments", Quantity = 5, Condition = "Fair", Location = "Shelf 4", Notes = "Needs calibration" },
                new EquipmentItem { Id = "EQ004", Name = "Safety Goggles", Category = "Safety Equipment", Quantity = 30, Condition = "New", Location = "Box S1", Notes = "" },
                new EquipmentItem { Id = "EQ005", Name = "Beakers (250ml)", Category = "Laboratory Glassware", Quantity = 25, Condition = "Good", Location = "Cabinet 2", Notes = "" }
            };

            // Set alternating row colors
            ApplyRowColors();

            // Set the collection view's item source
            EquipmentCollectionView.ItemsSource = equipmentList;
        }

        // Apply alternating row colors to the equipment list
        private void ApplyRowColors()
        {
            bool isAlternate = false;
            foreach (var item in equipmentList)
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
                EquipmentCollectionView.ItemsSource = equipmentList;
                return;
            }

            // Filter the equipment list based on search text
            var filteredList = new ObservableCollection<EquipmentItem>(
                equipmentList.Where(item =>
                    item.Id.ToLower().Contains(searchText) ||
                    item.Name.ToLower().Contains(searchText) ||
                    item.Category.ToLower().Contains(searchText))
            );

            EquipmentCollectionView.ItemsSource = filteredList;
        }

        // Handler for the clear form button
        private void OnClearFormClicked(object sender, EventArgs e)
        {
            // Clear all form fields
            EquipmentIdEntry.Text = string.Empty;
            EquipmentNameEntry.Text = string.Empty;
            CategoryPicker.SelectedIndex = -1;
            QuantityEntry.Text = string.Empty;
            ConditionPicker.SelectedIndex = -1;
            LocationEntry.Text = string.Empty;
            NotesEditor.Text = string.Empty;

            // Reset editing state
            isEditing = false;
            currentEditId = string.Empty;
        }

        // Handler for the add equipment button
        private async void OnAddEquipmentClicked(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(EquipmentIdEntry.Text) ||
                string.IsNullOrWhiteSpace(EquipmentNameEntry.Text) ||
                CategoryPicker.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(QuantityEntry.Text) ||
                ConditionPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Validation Error", "Please fill in all required fields (ID, Name, Category, Quantity, and Condition).", "OK");
                return;
            }

            // Validate quantity is a number
            if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity < 0)
            {
                await DisplayAlert("Validation Error", "Quantity must be a positive number.", "OK");
                return;
            }

            // Check if ID already exists (for new items)
            if (!isEditing && equipmentList.Any(item => item.Id == EquipmentIdEntry.Text))
            {
                await DisplayAlert("Duplicate ID", "An equipment item with this ID already exists.", "OK");
                return;
            }

            try
            {
                if (isEditing)
                {
                    // Update existing item
                    var itemToUpdate = equipmentList.FirstOrDefault(item => item.Id == currentEditId);
                    if (itemToUpdate != null)
                    {
                        // Remove the old item
                        equipmentList.Remove(itemToUpdate);

                        // Add the updated item (preserving the original position is complex with ObservableCollection)
                        AddNewEquipmentItem();

                        await DisplayAlert("Success", "Equipment updated successfully.", "OK");
                    }
                }
                else
                {
                    // Add new item
                    AddNewEquipmentItem();
                    await DisplayAlert("Success", "Equipment added successfully.", "OK");
                }

                // Clear the form
                OnClearFormClicked(sender, e);

                // Refresh the list with alternating colors
                ApplyRowColors();
                EquipmentCollectionView.ItemsSource = null;
                EquipmentCollectionView.ItemsSource = equipmentList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        // Helper method to add a new equipment item from form data
        private void AddNewEquipmentItem()
        {
            var newItem = new EquipmentItem
            {
                Id = EquipmentIdEntry.Text,
                Name = EquipmentNameEntry.Text,
                Category = CategoryPicker.SelectedItem.ToString(),
                Quantity = int.Parse(QuantityEntry.Text),
                Condition = ConditionPicker.SelectedItem.ToString(),
                Location = LocationEntry.Text ?? "",
                Notes = NotesEditor.Text ?? "",
                RowColor = Colors.White // Will be updated by ApplyRowColors
            };

            equipmentList.Add(newItem);
        }

        // Handler for the edit equipment button
        private void OnEditEquipmentClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string equipmentId = button?.CommandParameter?.ToString();

            if (!string.IsNullOrEmpty(equipmentId))
            {
                var itemToEdit = equipmentList.FirstOrDefault(item => item.Id == equipmentId);
                if (itemToEdit != null)
                {
                    // Populate form with item data
                    EquipmentIdEntry.Text = itemToEdit.Id;
                    EquipmentNameEntry.Text = itemToEdit.Name;
                    CategoryPicker.SelectedItem = itemToEdit.Category;
                    QuantityEntry.Text = itemToEdit.Quantity.ToString();
                    ConditionPicker.SelectedItem = itemToEdit.Condition;
                    LocationEntry.Text = itemToEdit.Location;
                    NotesEditor.Text = itemToEdit.Notes;

                    // Set editing state
                    isEditing = true;
                    currentEditId = equipmentId;
                }
            }
        }

        // Handler for the delete equipment button
        private async void OnDeleteEquipmentClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string equipmentId = button?.CommandParameter?.ToString();

            if (!string.IsNullOrEmpty(equipmentId))
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this equipment item? This action cannot be undone.",
                    "Delete", "Cancel");

                if (confirm)
                {
                    var itemToRemove = equipmentList.FirstOrDefault(item => item.Id == equipmentId);
                    if (itemToRemove != null)
                    {
                        equipmentList.Remove(itemToRemove);

                        // Refresh the list with alternating colors
                        ApplyRowColors();
                        EquipmentCollectionView.ItemsSource = null;
                        EquipmentCollectionView.ItemsSource = equipmentList;

                        await DisplayAlert("Success", "Equipment deleted successfully.", "OK");
                    }
                }
            }
        }

        // Handler for the back to dashboard button
        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    // Class to represent an equipment item
    public class EquipmentItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public Color RowColor { get; set; }
    }
}