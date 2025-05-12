using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace EduEquip.ViewModels
{
    public class Subject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
    }

    public class CreateSubjectViewModel : INotifyPropertyChanged
    {
        private string _subjectCode;
        private string _subjectName;
        private string _subjectDescription;
        private string _subjectUnits;

        public ObservableCollection<Subject> Subjects { get; set; } = new ObservableCollection<Subject>();

        public string SubjectCode
        {
            get => _subjectCode;
            set { _subjectCode = value; OnPropertyChanged(); }
        }

        public string SubjectName
        {
            get => _subjectName;
            set { _subjectName = value; OnPropertyChanged(); }
        }

        public string SubjectDescription
        {
            get => _subjectDescription;
            set { _subjectDescription = value; OnPropertyChanged(); }
        }

        public string SubjectUnits
        {
            get => _subjectUnits;
            set { _subjectUnits = value; OnPropertyChanged(); }
        }

        public ICommand CreateSubjectCommand { get; }
        public ICommand DeleteCommand { get; }

        public bool HasSubjects => Subjects.Count > 0;
        public bool HasNoSubjects => !HasSubjects;

        public CreateSubjectViewModel()
        {
            CreateSubjectCommand = new Command(AddSubject);
            DeleteCommand = new Command<Subject>(DeleteSubject);
        }

        private void AddSubject()
        {
            if (string.IsNullOrWhiteSpace(SubjectCode) || string.IsNullOrWhiteSpace(SubjectName))
                return;

            var newSubject = new Subject
            {
                Code = SubjectCode,
                Name = SubjectName,
                Description = SubjectDescription,
                Units = SubjectUnits
            };

            Subjects.Add(newSubject);
            OnPropertyChanged(nameof(HasSubjects));
            OnPropertyChanged(nameof(HasNoSubjects));

            // Clear input fields
            SubjectCode = string.Empty;
            SubjectName = string.Empty;
            SubjectDescription = string.Empty;
            SubjectUnits = string.Empty;
        }

        private void DeleteSubject(Subject subject)
        {
            if (Subjects.Contains(subject))
            {
                Subjects.Remove(subject);
                OnPropertyChanged(nameof(HasSubjects));
                OnPropertyChanged(nameof(HasNoSubjects));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
