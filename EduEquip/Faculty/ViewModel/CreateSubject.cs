using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EduEquip.Models
{
    public class Subject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Units { get; set; } // Added Units property as integer
        public DateTime CreatedDate { get; set; }
        public string FacultyId { get; set; }

        public Subject()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }

    // Renamed from SubjectService to SubjectDataManager to avoid conflicts
    public static class SubjectDataManager
    {
        private static string SubjectsFileName => Path.Combine(FileSystem.AppDataDirectory, "subjects.json");
        private static ObservableCollection<Subject> _subjects;

        public static ObservableCollection<Subject> Subjects
        {
            get
            {
                if (_subjects == null)
                {
                    LoadSubjects();
                }
                return _subjects;
            }
        }

        public static void LoadSubjects()
        {
            if (_subjects == null)
            {
                _subjects = new ObservableCollection<Subject>();
            }
            else
            {
                _subjects.Clear();
            }

            if (File.Exists(SubjectsFileName))
            {
                try
                {
                    string json = File.ReadAllText(SubjectsFileName);
                    var subjects = JsonSerializer.Deserialize<List<Subject>>(json);
                    foreach (var subject in subjects)
                    {
                        _subjects.Add(subject);
                    }
                }
                catch (Exception ex)
                {
                    // Log error or handle exception
                    Console.WriteLine($"Error loading subjects: {ex.Message}");
                }
            }
        }

        public static async Task SaveSubjectsAsync()
        {
            try
            {
                string json = JsonSerializer.Serialize(_subjects);
                await File.WriteAllTextAsync(SubjectsFileName, json);
            }
            catch (Exception ex)
            {
                // Log error or handle exception
                Console.WriteLine($"Error saving subjects: {ex.Message}");
            }
        }

        public static async Task AddSubjectAsync(Subject subject)
        {
            _subjects.Add(subject);
            await SaveSubjectsAsync();
        }

        public static async Task UpdateSubjectAsync(Subject subject)
        {
            int index = _subjects.IndexOf(_subjects.FirstOrDefault(s => s.Id == subject.Id));
            if (index != -1)
            {
                _subjects[index] = subject;
                await SaveSubjectsAsync();
            }
        }

        public static async Task DeleteSubjectAsync(string subjectId)
        {
            var subject = _subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject != null)
            {
                _subjects.Remove(subject);
                await SaveSubjectsAsync();
            }
        }

        public static List<Subject> GetSubjectsByFacultyId(string facultyId)
        {
            return _subjects.Where(s => s.FacultyId == facultyId).ToList();
        }
    }
}