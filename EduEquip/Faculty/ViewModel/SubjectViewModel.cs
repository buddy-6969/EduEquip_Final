using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace EduEquip
{
    public class Subject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Units { get; set; }
    }

    public class SubjectViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Subject> _subjects;
        private string _facultyId;
        private Subject _selectedSubject;

        public SubjectViewModel(string facultyId)
        {
            _facultyId = facultyId;
            Subjects = new ObservableCollection<Subject>();

            // Setup commands
            DeleteCommand = new Command<Subject>(DeleteSubject);
            EditCommand = new Command<Subject>(EditSubject);

            // Load initial data (in a real app, this would come from a database)
            LoadSampleData();
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSubjects));
                OnPropertyChanged(nameof(HasNoSubjects));
            }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged();
            }
        }

        public bool HasSubjects => Subjects != null && Subjects.Count > 0;
        public bool HasNoSubjects => !HasSubjects;

        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        private void LoadSampleData()
        {
            // Sample data for demonstration
            Subjects.Add(new Subject
            {
                Code = "CPE101",
                Name = "Introduction to Computer Engineering",
                Description = "Basic concepts and principles of computer engineering",
                Units = 3
            });

            Subjects.Add(new Subject
            {
                Code = "CPE205",
                Name = "Digital Logic Design",
                Description = "Fundamentals of digital electronics and logic circuits",
                Units = 4
            });
        }

        public async void AddSubject(Subject subject)
        {
            // In a real app, you would save to a database here
            Subjects.Add(subject);
            OnPropertyChanged(nameof(HasSubjects));
            OnPropertyChanged(nameof(HasNoSubjects));

            // Show confirmation (this will be handled in the view)
            // We'll return true to indicate success
        }

        public async void DeleteSubject(Subject subject)
        {
            // In a real app, you would delete from a database here
            if (Subjects.Contains(subject))
            {
                Subjects.Remove(subject);
                OnPropertyChanged(nameof(HasSubjects));
                OnPropertyChanged(nameof(HasNoSubjects));

                // Confirmation will be handled in the view
            }
        }

        public async void EditSubject(Subject subject)
        {
            // In a real app, you would update the database here
            // For now, we'll just select the subject
            SelectedSubject = subject;

            // The actual editing will be handled in the view
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}