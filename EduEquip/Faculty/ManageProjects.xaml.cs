using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EduEquip
{
    // Renamed classes to avoid conflicts with existing classes
    public class ProjectSubject
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class AcademicProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public string SubjectCode { get; set; }
        public ObservableCollection<ProjectEquipment> RequiredEquipment { get; set; }

        public AcademicProject()
        {
            RequiredEquipment = new ObservableCollection<ProjectEquipment>();
        }
    }

    public class ProjectEquipment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public partial class ManageProjects : ContentPage
    {
        private string _facultyId;
        private ObservableCollection<ProjectSubject> _subjects;
        private ObservableCollection<AcademicProject> _allProjects;
        private ObservableCollection<AcademicProject> _filteredProjects;
        private AcademicProject _selectedProject;

        public ManageProjects(string facultyId)
        {
            InitializeComponent();
            _facultyId = facultyId;

            // Initialize collections
            _subjects = new ObservableCollection<ProjectSubject>();
            _allProjects = new ObservableCollection<AcademicProject>();
            _filteredProjects = new ObservableCollection<AcademicProject>();

            // Load data
            LoadSubjects();
            LoadProjects();

            // Bind data
            SubjectsListView.ItemsSource = _subjects;
            ProjectsListView.ItemsSource = _filteredProjects;

            // Update welcome label
            WelcomeLabel.Text = $"Welcome, Professor {facultyId}";
        }

        private void LoadSubjects()
        {
            // In a real app, this would load from a database
            _subjects.Add(new ProjectSubject { Code = "CPE101", Name = "Introduction to Computer Engineering" });
            _subjects.Add(new ProjectSubject { Code = "CPE205", Name = "Digital Logic Design" });
            _subjects.Add(new ProjectSubject { Code = "CPE305", Name = "Microprocessors" });
        }

        private void LoadProjects()
        {
            // In a real app, this would load from a database
            var project1 = new AcademicProject
            {
                Id = "PROJ001",
                Name = "Digital Clock Design",
                Description = "Design and implement a digital clock using logic gates",
                Deadline = DateTime.Now.AddDays(30),
                Status = "Active",
                SubjectCode = "CPE205"
            };

            project1.RequiredEquipment.Add(new ProjectEquipment { Id = "EQ001", Name = "Logic Analyzer", Quantity = 2 });
            project1.RequiredEquipment.Add(new ProjectEquipment { Id = "EQ002", Name = "Digital Multimeter", Quantity = 5 });
            project1.RequiredEquipment.Add(new ProjectEquipment { Id = "EQ003", Name = "Breadboard", Quantity = 10 });

            var project2 = new AcademicProject
            {
                Id = "PROJ002",
                Name = "Simple CPU Design",
                Description = "Develop a simple CPU architecture using microprocessors",
                Deadline = DateTime.Now.AddDays(45),
                Status = "Active",
                SubjectCode = "CPE305"
            };

            project2.RequiredEquipment.Add(new ProjectEquipment { Id = "EQ004", Name = "Arduino Kit", Quantity = 8 });
            project2.RequiredEquipment.Add(new ProjectEquipment { Id = "EQ005", Name = "Oscilloscope", Quantity = 4 });

            var project3 = new AcademicProject
            {
                Id = "PROJ003",
                Name = "Hello World Program",
                Description = "Create a simple Hello World program in C",
                Deadline = DateTime.Now.AddDays(10),
                Status = "Active",
                SubjectCode = "CPE101"
            };

            _allProjects.Add(project1);
            _allProjects.Add(project2);
            _allProjects.Add(project3);
        }

        private void OnSubjectSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedSubject = e.SelectedItem as ProjectSubject;
            FilterProjects(selectedSubject.Code);

            // Reset project details
            ProjectDetailsFrame.IsVisible = false;
        }

        private void FilterProjects(string subjectCode)
        {
            _filteredProjects.Clear();

            foreach (var project in _allProjects.Where(p => p.SubjectCode == subjectCode))
            {
                _filteredProjects.Add(project);
            }
        }

        private void OnProjectSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            _selectedProject = e.SelectedItem as AcademicProject;
            DisplayProjectDetails(_selectedProject);
        }

        private void DisplayProjectDetails(AcademicProject project)
        {
            // Set project details
            ProjectNameLabel.Text = project.Name;
            ProjectSubjectLabel.Text = _subjects.FirstOrDefault(s => s.Code == project.SubjectCode)?.Name ?? "";
            ProjectDescriptionLabel.Text = project.Description;
            ProjectDeadlineLabel.Text = project.Deadline.ToString("MMMM dd, yyyy");
            ProjectStatusLabel.Text = project.Status;

            // Set equipment
            EquipmentListView.ItemsSource = project.RequiredEquipment;

            // Show details frame
            ProjectDetailsFrame.IsVisible = true;
        }

        private async void OnCreateProjectClicked(object sender, EventArgs e)
        {
            // In a full implementation, we would navigate to a create project page
            await DisplayAlert("Create Project", "This feature is not implemented in this demo.", "OK");
        }

        private async void OnAddEquipmentClicked(object sender, EventArgs e)
        {
            if (_selectedProject == null)
                return;

            // In a full implementation, we would show a dialog to add equipment
            await DisplayAlert("Add Equipment", "This feature is not implemented in this demo.", "OK");
        }

        private async void OnEditProjectClicked(object sender, EventArgs e)
        {
            if (_selectedProject == null)
                return;

            // In a full implementation, we would navigate to an edit project page
            await DisplayAlert("Edit Project", "This feature is not implemented in this demo.", "OK");
        }

        private async void OnDeleteProjectClicked(object sender, EventArgs e)
        {
            if (_selectedProject == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete project '{_selectedProject.Name}'?",
                "Yes", "No");

            if (confirm)
            {
                _allProjects.Remove(_selectedProject);

                // Refresh filtered projects
                var subjectCode = _selectedProject.SubjectCode;
                _selectedProject = null;

                FilterProjects(subjectCode);
                ProjectDetailsFrame.IsVisible = false;

                await DisplayAlert("Success", "Project has been deleted.", "OK");
            }
        }
    }
}