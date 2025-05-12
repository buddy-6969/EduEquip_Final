using System;
using System.Collections.ObjectModel;
using System.Linq;
using EduEquip.Models;
using EduEquip.Services;
using Microsoft.Maui.Controls;

namespace EduEquip
{
    public partial class CreateSubject : ContentPage
    {
        private CourseSubject _currentSubject;
        private bool _isEditMode = false;
        private string _currentUserId;

        public CreateSubject(string userId)
        {
            InitializeComponent();
            _currentUserId = userId;

            // Ensure we load subjects when the page is created
            LoadSubjects();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh the subjects list when returning to this page
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            // Ensure subjects are loaded from persistent storage
            EduEquip.Services.SubjectService.LoadSubjects();

            // Filter subjects by current faculty ID
            var facultySubjects = EduEquip.Services.SubjectService.GetSubjectsByFacultyId(_currentUserId);
            SubjectsCollection.ItemsSource = new ObservableCollection<CourseSubject>(facultySubjects);
        }

        private void ClearForm()
        {
            SubjectNameEntry.Text = string.Empty;
            SubjectCodeEntry.Text = string.Empty;
            SubjectUnitsEntry.Text = string.Empty;  // Clear the Units field
            SubjectDescriptionEditor.Text = string.Empty;
            _currentSubject = null;
            _isEditMode = false;

            // Find the save button by its Grid location and update text
            var saveButton = this.FindByName<Button>("SaveButton");
            if (saveButton != null)
            {
                saveButton.Text = "Create Subject";
            }
        }

        private async void OnSaveSubjectClicked(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(SubjectNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "Subject name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(SubjectCodeEntry.Text))
            {
                await DisplayAlert("Validation Error", "Subject code is required.", "OK");
                return;
            }

            // Validate units (must be a valid integer)
            int units = 0;
            if (string.IsNullOrWhiteSpace(SubjectUnitsEntry.Text) ||
                !int.TryParse(SubjectUnitsEntry.Text, out units))
            {
                await DisplayAlert("Validation Error", "Units must be a valid number.", "OK");
                return;
            }

            if (_isEditMode && _currentSubject != null)
            {
                // Update existing subject
                _currentSubject.Name = SubjectNameEntry.Text;
                _currentSubject.Code = SubjectCodeEntry.Text;
                _currentSubject.Units = units;
                _currentSubject.Description = SubjectDescriptionEditor.Text;

                await EduEquip.Services.SubjectService.UpdateSubjectAsync(_currentSubject);
                await DisplayAlert("Success", "Subject updated successfully!", "OK");
            }
            else
            {
                // Create new subject
                var newSubject = new CourseSubject
                {
                    Name = SubjectNameEntry.Text,
                    Code = SubjectCodeEntry.Text,
                    Units = units,
                    Description = SubjectDescriptionEditor.Text,
                    FacultyId = _currentUserId
                };

                await EduEquip.Services.SubjectService.AddSubjectAsync(newSubject);
                await DisplayAlert("Success", "Subject created successfully!", "OK");
            }

            // Clear form and reload subjects
            ClearForm();
            LoadSubjects();
        }

        private void OnEditSubjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string subjectId)
            {
                // Find the subject to edit
                _currentSubject = EduEquip.Services.SubjectService.GetSubjectById(subjectId);

                if (_currentSubject != null)
                {
                    // Populate form with subject data
                    SubjectNameEntry.Text = _currentSubject.Name;
                    SubjectCodeEntry.Text = _currentSubject.Code;
                    SubjectUnitsEntry.Text = _currentSubject.Units.ToString();
                    SubjectDescriptionEditor.Text = _currentSubject.Description;

                    // Set edit mode flag
                    _isEditMode = true;

                    // Change button text to indicate edit mode
                    var saveButton = this.FindByName<Button>("SaveButton");
                    if (saveButton != null)
                    {
                        saveButton.Text = "Update Subject";
                    }
                }
            }
        }

        private async void OnDeleteSubjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string subjectId)
            {
                // Confirm deletion
                bool confirm = await DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this subject? This action cannot be undone.",
                    "Yes", "No");

                if (confirm)
                {
                    await EduEquip.Services.SubjectService.DeleteSubjectAsync(subjectId);
                    LoadSubjects();
                    await DisplayAlert("Success", "Subject deleted successfully.", "OK");
                }
            }
        }

        private void OnSubjectSelected(object sender, SelectionChangedEventArgs e)
        {
            // Handle subject selection if needed
            if (e.CurrentSelection.FirstOrDefault() is CourseSubject selectedSubject)
            {
                // Deselect item
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void OnBackToDashboardClicked(object sender, EventArgs e)
        {
            // Navigate back to faculty dashboard
            await Navigation.PopAsync();
        }
    }
}