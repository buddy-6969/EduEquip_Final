using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EduEquip.Services
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In a real app, this should be hashed
        public string Name { get; set; }
        public string Role { get; set; } // Faculty, Student, LabAssistant
    }

    public static class UserService
    {
        private static string UsersFileName => Path.Combine(FileSystem.AppDataDirectory, "users.json");
        private static string CurrentUserFileName => Path.Combine(FileSystem.AppDataDirectory, "currentUser.json");
        private static List<User> _users;
        private static User _currentUser;

        public static User CurrentUser => _currentUser;

        // Load all users from storage
        public static void LoadUsers()
        {
            if (File.Exists(UsersFileName))
            {
                string json = File.ReadAllText(UsersFileName);
                _users = JsonSerializer.Deserialize<List<User>>(json);
            }
            else
            {
                _users = new List<User>();
                // You might want to create default users here
            }
        }

        // Save all users to storage
        public static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(UsersFileName, json);
        }

        // Save current user to storage
        public static void SaveCurrentUser()
        {
            if (_currentUser != null)
            {
                string json = JsonSerializer.Serialize(_currentUser);
                File.WriteAllText(CurrentUserFileName, json);
            }
        }

        // Load current user from storage
        public static void LoadCurrentUser()
        {
            if (File.Exists(CurrentUserFileName))
            {
                string json = File.ReadAllText(CurrentUserFileName);
                _currentUser = JsonSerializer.Deserialize<User>(json);
            }
        }

        // Clear current user (for logout)
        public static void ClearCurrentUser()
        {
            _currentUser = null;
            if (File.Exists(CurrentUserFileName))
            {
                File.Delete(CurrentUserFileName);
            }
        }

        // Login with username and password
        public static bool Login(string username, string password)
        {
            if (_users == null)
            {
                LoadUsers();
            }

            _currentUser = _users.Find(u => u.Username == username && u.Password == password);
            if (_currentUser != null)
            {
                SaveCurrentUser();
                return true;
            }
            return false;
        }

        // Register a new user
        public static bool Register(User newUser)
        {
            if (_users == null)
            {
                LoadUsers();
            }

            // Check if username already exists
            if (_users.Exists(u => u.Username == newUser.Username))
            {
                return false;
            }

            newUser.Id = Guid.NewGuid().ToString();
            _users.Add(newUser);
            SaveUsers();
            return true;
        }

        // Get all users
        public static List<User> GetAllUsers()
        {
            if (_users == null)
            {
                LoadUsers();
            }
            return _users;
        }

        // Get user by ID
        public static User GetUserById(string id)
        {
            if (_users == null)
            {
                LoadUsers();
            }
            return _users.Find(u => u.Id == id);
        }

        // Update user
        public static bool UpdateUser(User updatedUser)
        {
            if (_users == null)
            {
                LoadUsers();
            }

            int index = _users.FindIndex(u => u.Id == updatedUser.Id);
            if (index != -1)
            {
                _users[index] = updatedUser;
                SaveUsers();

                // Update current user if it's the same user
                if (_currentUser?.Id == updatedUser.Id)
                {
                    _currentUser = updatedUser;
                    SaveCurrentUser();
                }
                return true;
            }
            return false;
        }

        // Delete user
        public static bool DeleteUser(string userId)
        {
            if (_users == null)
            {
                LoadUsers();
            }

            int index = _users.FindIndex(u => u.Id == userId);
            if (index != -1)
            {
                _users.RemoveAt(index);
                SaveUsers();
                return true;
            }
            return false;
        }
    }
}