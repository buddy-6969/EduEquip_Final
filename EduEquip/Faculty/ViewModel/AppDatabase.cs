using System;
using System.IO;
using System.Threading.Tasks;
using EduEquip.Models;

namespace EduEquip.Services
{
    public static class AppDatabase
    {
        // Called when the application starts to initialize stored data
        public static async Task InitializeAsync()
        {
            try
            {
                // Initialize subjects - this loads them into memory
                SubjectService.LoadSubjects();

                // If you need to use the model-specific service, use the renamed one
                // EduEquip.Models.SubjectDataManager.LoadSubjects();

                // Add other initialization steps here as needed

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Log any initialization errors
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        // Called during login to ensure user-specific data is loaded
        public static async Task LoadUserDataAsync(string userId, string userRole)
        {
            try
            {
                // Load appropriate data based on user role
                if (userRole.ToLower() == "faculty")
                {
                    // Make sure subjects are loaded for faculty members
                    SubjectService.LoadSubjects();

                    // Load other faculty-specific data
                    // e.g., await ProjectService.LoadProjectsAsync(userId);
                }
                else if (userRole.ToLower() == "student")
                {
                    // Load student-specific data
                    // e.g., await EnrollmentService.LoadEnrollmentsAsync(userId);
                }
                else if (userRole.ToLower() == "labassistant")
                {
                    // Load lab assistant-specific data
                    // e.g., await InventoryService.LoadInventoryAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Log any user data loading errors
                Console.WriteLine($"User data loading error: {ex.Message}");
            }
        }

        // Returns the path for application data files
        public static string GetDataFilePath(string fileName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }
    }
}