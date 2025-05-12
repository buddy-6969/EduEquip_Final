using System.Collections.ObjectModel;

namespace EduEquip;

public partial class ViewProjectsPage : ContentPage
{
    private string studentId;
    private ObservableCollection<Project> projects;

    public ViewProjectsPage(string studentId)
    {
        InitializeComponent();
        this.studentId = studentId;
        StudentInfoLabel.Text = $"Student: {studentId}";

        // Initialize and load projects
        LoadProjects();

        ProjectsRefreshView.Command = new Command(() => {
            LoadProjects();
            ProjectsRefreshView.IsRefreshing = false;
        });

        // Initial values for pickers
        SubjectPicker.SelectedIndex = 0;
        StatusPicker.SelectedIndex = 0;

        // Add change handlers for filters
        SubjectPicker.SelectedIndexChanged += (s, e) => FilterProjects();
        StatusPicker.SelectedIndexChanged += (s, e) => FilterProjects();
    }

    private void LoadProjects()
    {
        // In a real app, this would load from a database
        projects = new ObservableCollection<Project>
        {
            new Project
            {
                ProjectId = "P001",
                ProjectName = "Desktop App Development",
                SubjectName = "CPE SD1",
                Deadline = "April 30, 2025",
                Status = "Ongoing",
                StatusColor = Color.FromArgb("#FFA500"), // Orange
                Description = "Create a desktop application for laboratory equipment management",
                Requirements = "CRUD operations, User authentication, Equipment tracking",
                AssignedBy = "Engr. Jane Evangelista"
            },
            new Project
            {
                ProjectId = "P002",
                ProjectName = "Logic Circuit Simulator",
                SubjectName = "Digital Logic",
                Deadline = "May 15, 2025",
                Status = "Not Started",
                StatusColor = Color.FromArgb("#FF0000"), // Red
                Description = "Design and implement a logic circuit simulator",
                Requirements = "GUI interface, Circuit validation, Truth table generation",
                AssignedBy = "Engr. John Smith"
            },
            new Project
            {
                ProjectId = "P003",
                ProjectName = "CPU Architecture Analysis",
                SubjectName = "Computer Architecture",
                Deadline = "March 20, 2025",
                Status = "Completed",
                StatusColor = Color.FromArgb("#008000"), // Green
                Description = "Analyze and document modern CPU architectures",
                Requirements = "Research paper, Performance analysis, Comparative study",
                AssignedBy = "Dr. Maria Garcia"
            }
        };

        ProjectsCollection.ItemsSource = projects;
    }

    private void FilterProjects()
    {
        var filteredProjects = projects;

        // Apply subject filter
        if (SubjectPicker.SelectedIndex > 0)
        {
            string selectedSubject = SubjectPicker.SelectedItem.ToString();
            filteredProjects = new ObservableCollection<Project>(
                filteredProjects.Where(p => p.SubjectName == selectedSubject)
            );
        }

        // Apply status filter
        if (StatusPicker.SelectedIndex > 0)
        {
            string selectedStatus = StatusPicker.SelectedItem.ToString();
            filteredProjects = new ObservableCollection<Project>(
                filteredProjects.Where(p => p.Status == selectedStatus)
            );
        }

        ProjectsCollection.ItemsSource = filteredProjects;
    }

    private async void OnProjectSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Project selectedProject)
        {
            // Clear selection
            ProjectsCollection.SelectedItem = null;

            // Show project details - now passing studentId
            await Navigation.PushAsync(new ProjectDetailsPage(selectedProject, studentId));
        }
    }
}

// Project class to represent project data
public class Project
{
    public string ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string SubjectName { get; set; }
    public string Deadline { get; set; }
    public string Status { get; set; }
    public Color StatusColor { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public string AssignedBy { get; set; }
}

// Project details page to show when a project is selected
public class ProjectDetailsPage : ContentPage
{
    private string studentId;

    public ProjectDetailsPage(Project project, string studentId)
    {
        Title = project.ProjectName;
        BackgroundColor = Colors.White;
        this.studentId = studentId;

        // Create project details layout
        var content = new VerticalStackLayout
        {
            Padding = new Thickness(20),
            Spacing = 15
        };

        // Add header
        content.Children.Add(new Image { Source = "logo.png", HeightRequest = 60, WidthRequest = 60, HorizontalOptions = LayoutOptions.Start });
        content.Children.Add(new Label { Text = project.ProjectName, FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = Colors.DarkBlue });
        content.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Colors.LightGray });

        // Project info
        var infoGrid = new Grid { ColumnSpacing = 10, RowSpacing = 15 };
        infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // Add project details
        AddInfoRow(infoGrid, 0, "Subject:", project.SubjectName);
        AddInfoRow(infoGrid, 1, "Deadline:", project.Deadline);
        AddInfoRow(infoGrid, 2, "Status:", project.Status);
        AddInfoRow(infoGrid, 3, "Assigned By:", project.AssignedBy);

        content.Children.Add(infoGrid);
        content.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Colors.LightGray });

        // Description
        content.Children.Add(new Label { Text = "Description", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.DarkBlue });
        content.Children.Add(new Label { Text = project.Description, FontSize = 16 });

        // Requirements
        content.Children.Add(new Label { Text = "Requirements", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.DarkBlue, Margin = new Thickness(0, 10, 0, 0) });
        content.Children.Add(new Label { Text = project.Requirements, FontSize = 16 });

        // Request Equipment button
        var requestButton = new Button
        {
            Text = "Request Equipment for this Project",
            BackgroundColor = Colors.DarkBlue,
            TextColor = Colors.White,
            CornerRadius = 5,
            Margin = new Thickness(0, 20, 0, 0)
        };

        // Updated to pass three parameters
        requestButton.Clicked += async (s, e) =>
            await Navigation.PushAsync(new RequestEquipmentPage(studentId, project.ProjectId, project.ProjectName));

        content.Children.Add(requestButton);

        // Set page content
        Content = new ScrollView { Content = content };
    }

    private void AddInfoRow(Grid grid, int row, string label, string value)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

        grid.Add(new Label
        {
            Text = label,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center
        }, 0, row);

        grid.Add(new Label
        {
            Text = value,
            VerticalOptions = LayoutOptions.Center
        }, 1, row);
    }
}