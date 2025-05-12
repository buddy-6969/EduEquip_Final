using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EduEquip.Models;

namespace EduEquip.Services
{
    public static class SubjectService
    {
        private static string SubjectsFileName => Path.Combine(FileSystem.AppDataDirectory, "subjects.json");
        private static List<CourseSubject> _subjects;

        public static List<CourseSubject> Subjects => _subjects ?? (_subjects = new List<CourseSubject>());

        // Load all subjects from storage
        public static void LoadSubjects()
        {
            try
            {
                if (File.Exists(SubjectsFileName))
                {
                    string json = File.ReadAllText(SubjectsFileName);
                    _subjects = JsonSerializer.Deserialize<List<CourseSubject>>(json);
                }
                else
                {
                    _subjects = new List<CourseSubject>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading subjects: {ex.Message}");
                _subjects = new List<CourseSubject>();
            }
        }

        // Save all subjects to storage
        private static void SaveSubjects()
        {
            try
            {
                string json = JsonSerializer.Serialize(_subjects);
                File.WriteAllText(SubjectsFileName, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving subjects: {ex.Message}");
            }
        }

        // Get all subjects
        public static List<CourseSubject> GetAllSubjects()
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }
            return _subjects;
        }

        // Get subjects by faculty ID
        public static List<CourseSubject> GetSubjectsByFacultyId(string facultyId)
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }
            return _subjects.Where(s => s.FacultyId == facultyId).ToList();
        }

        // Get subject by ID
        public static CourseSubject GetSubjectById(string id)
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }
            return _subjects.FirstOrDefault(s => s.Id == id);
        }

        // Add a new subject
        public static Task<bool> AddSubjectAsync(CourseSubject subject)
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }

            // Ensure the subject has an ID
            if (string.IsNullOrEmpty(subject.Id))
            {
                subject.Id = Guid.NewGuid().ToString();
            }

            _subjects.Add(subject);
            SaveSubjects(); // Save changes immediately
            return Task.FromResult(true);
        }

        // Update an existing subject
        public static Task<bool> UpdateSubjectAsync(CourseSubject updatedSubject)
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }

            int index = _subjects.FindIndex(s => s.Id == updatedSubject.Id);
            if (index != -1)
            {
                _subjects[index] = updatedSubject;
                SaveSubjects(); // Save changes immediately
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // Delete a subject
        public static Task<bool> DeleteSubjectAsync(string subjectId)
        {
            if (_subjects == null)
            {
                LoadSubjects();
            }

            int index = _subjects.FindIndex(s => s.Id == subjectId);
            if (index != -1)
            {
                _subjects.RemoveAt(index);
                SaveSubjects(); // Save changes immediately
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}