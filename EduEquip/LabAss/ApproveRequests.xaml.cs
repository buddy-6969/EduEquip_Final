using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EduEquip;

public partial class ApproveRequests : ContentPage
{
    // Observable collection to store equipment requests
    private ObservableCollection<EquipmentRequest> _allRequests;
    private ObservableCollection<EquipmentRequest> _filteredRequests;

    public ApproveRequests()
    {
        InitializeComponent();

        // Initialize the requests collection
        LoadEquipmentRequests();

        // Set the initial filtered collection to all requests
        _filteredRequests = new ObservableCollection<EquipmentRequest>(_allRequests);
        RequestsCollectionView.ItemsSource = _filteredRequests;
    }

    private void LoadEquipmentRequests()
    {
        // This would normally load from a database or service
        // For now, using mock data
        _allRequests = new ObservableCollection<EquipmentRequest>
        {
            new EquipmentRequest
            {
                RequestId = "REQ-001",
                EquipmentName = "Oscilloscope",
                BorrowerName = "John Smith",
                DateNeeded = "2025-04-20",
                Status = "Pending",
                StatusColor = "#ff9800",
                IsPending = true,
                RowColor = "#ffffff"
            },
            new EquipmentRequest
            {
                RequestId = "REQ-002",
                EquipmentName = "Arduino Kit",
                BorrowerName = "Maria Garcia",
                DateNeeded = "2025-04-21",
                Status = "Approved",
                StatusColor = "#4caf50",
                IsPending = false,
                RowColor = "#f5f5f5"
            },
            new EquipmentRequest
            {
                RequestId = "REQ-003",
                EquipmentName = "Digital Multimeter",
                BorrowerName = "Alex Johnson",
                DateNeeded = "2025-04-19",
                Status = "Rejected",
                StatusColor = "#f44336",
                IsPending = false,
                RowColor = "#ffffff"
            },
            new EquipmentRequest
            {
                RequestId = "REQ-004",
                EquipmentName = "Raspberry Pi",
                BorrowerName = "Sarah Lee",
                DateNeeded = "2025-04-22",
                Status = "Pending",
                StatusColor = "#ff9800",
                IsPending = true,
                RowColor = "#f5f5f5"
            }
        };
    }

    private void OnBackToDashboardClicked(object sender, EventArgs e)
    {
        // Navigate back to dashboard
        Navigation.PopAsync();
    }

    private void OnFilterAllClicked(object sender, EventArgs e)
    {
        _filteredRequests.Clear();
        foreach (var request in _allRequests)
        {
            _filteredRequests.Add(request);
        }
    }

    private void OnFilterPendingClicked(object sender, EventArgs e)
    {
        _filteredRequests.Clear();
        foreach (var request in _allRequests)
        {
            if (request.Status == "Pending")
            {
                _filteredRequests.Add(request);
            }
        }
    }

    private void OnFilterApprovedClicked(object sender, EventArgs e)
    {
        _filteredRequests.Clear();
        foreach (var request in _allRequests)
        {
            if (request.Status == "Approved")
            {
                _filteredRequests.Add(request);
            }
        }
    }

    private void OnFilterRejectedClicked(object sender, EventArgs e)
    {
        _filteredRequests.Clear();
        foreach (var request in _allRequests)
        {
            if (request.Status == "Rejected")
            {
                _filteredRequests.Add(request);
            }
        }
    }

    private async void OnViewRequestClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var requestId = button.CommandParameter.ToString();

        // Find the request with the matching ID
        var request = _allRequests.FirstOrDefault(r => r.RequestId == requestId);

        if (request != null)
        {
            // Display request details in a popup
            await DisplayAlert("Request Details",
                $"Request ID: {request.RequestId}\n" +
                $"Equipment: {request.EquipmentName}\n" +
                $"Borrower: {request.BorrowerName}\n" +
                $"Date Needed: {request.DateNeeded}\n" +
                $"Status: {request.Status}",
                "Close");
        }
    }

    private async void OnApproveRequestClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var requestId = button.CommandParameter.ToString();

        // Find the request with the matching ID
        var request = _allRequests.FirstOrDefault(r => r.RequestId == requestId);

        if (request != null)
        {
            // Confirm approval
            bool answer = await DisplayAlert("Confirm Approval",
                $"Are you sure you want to approve request {request.RequestId} for {request.EquipmentName}?",
                "Yes", "No");

            if (answer)
            {
                // Update request status
                request.Status = "Approved";
                request.StatusColor = "#4caf50";
                request.IsPending = false;

                // Refresh the list
                var index = _filteredRequests.IndexOf(request);
                if (index >= 0)
                {
                    _filteredRequests[index] = request;
                }

                await DisplayAlert("Success", $"Request {request.RequestId} has been approved.", "OK");
            }
        }
    }

    private async void OnRejectRequestClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var requestId = button.CommandParameter.ToString();

        // Find the request with the matching ID
        var request = _allRequests.FirstOrDefault(r => r.RequestId == requestId);

        if (request != null)
        {
            // Confirm rejection
            bool answer = await DisplayAlert("Confirm Rejection",
                $"Are you sure you want to reject request {request.RequestId} for {request.EquipmentName}?",
                "Yes", "No");

            if (answer)
            {
                // Update request status
                request.Status = "Rejected";
                request.StatusColor = "#f44336";
                request.IsPending = false;

                // Refresh the list
                var index = _filteredRequests.IndexOf(request);
                if (index >= 0)
                {
                    _filteredRequests[index] = request;
                }

                await DisplayAlert("Success", $"Request {request.RequestId} has been rejected.", "OK");
            }
        }
    }
}

// Model class for equipment requests
public class EquipmentRequest
{
    public string RequestId { get; set; }
    public string EquipmentName { get; set; }
    public string BorrowerName { get; set; }
    public string DateNeeded { get; set; }
    public string Status { get; set; }
    public string StatusColor { get; set; }
    public bool IsPending { get; set; }
    public string RowColor { get; set; }
}