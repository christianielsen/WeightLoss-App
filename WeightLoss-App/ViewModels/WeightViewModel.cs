using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeightLoss_App.Database;
using WeightLoss_App.Models;

namespace WeightLoss_App.ViewModels;

public partial class WeightViewModel : ObservableObject
{
    [ObservableProperty]
    private double _weight;
    
    [ObservableProperty]
    private ObservableCollection<WeightModel> _weights;

    [ObservableProperty] private double _change;
    
    private readonly WeightDatabase database;

    public WeightViewModel(WeightDatabase weightDatabase)
    {
        database = weightDatabase;
        _weights = new ObservableCollection<WeightModel>();
        LoadWeightsAsync();
    }

    private async Task LoadWeightsAsync()
    {
        var weights = await database.GetWeightsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Weights.Clear();
            foreach (var weight in weights.OrderBy(w => w.DateTime))
            {
                Weights.Add(weight);
            }

            CalculateChange();
        });
    }

    [RelayCommand]
    private async Task AddWeight()
    {
        if (Weight <= 0)
        {
            await Shell.Current.DisplayAlert("Invalid Weight", "Please enter a valid weight greater than 0", "OK");
            return;
        }

        var weightModel = new WeightModel 
        { 
            Weight = Weight, 
            DateTime = DateTime.Now 
        };

        var id = await database.SaveWeightAsync(weightModel);
        
        // Insert the new weight in the correct position to maintain ordering
        var insertIndex = Weights.ToList().FindIndex(w => w.DateTime > weightModel.DateTime);
        if (insertIndex == -1)
            Weights.Add(weightModel);
        else
            Weights.Insert(insertIndex, weightModel);
        
        Weight = 0;
    }
    
    [RelayCommand]
    private async Task DeleteWeight(WeightModel weight)
    {
        bool answer = await Shell.Current.DisplayAlert(
            "Delete Weight",
            "Are you sure you want to delete this weight entry?",
            "Yes",
            "No");

        if (answer)
        {
            await database.DeleteWeightAsync(weight);
            Weights.Remove(weight);
            CalculateChange();
        }
    }

    private void CalculateChange()
    {
        if (Weights.Count == 0)
        {
            Change = 0;
            return;
        }
        
        var mostRecentWeight = Weights.OrderByDescending(w => w.DateTime).First();

        var sevenDaysAgo = DateTime.Now.AddDays(-7);
        var closestWeight = Weights
            .Where(w => w.DateTime <= sevenDaysAgo)
            .OrderByDescending(w => w.DateTime)
            .FirstOrDefault();

        // If no weight exists for 7 days ago, fallback to the oldest weight
        if (closestWeight == null)
        {
            closestWeight = Weights.OrderBy(w => w.DateTime).First();
        }
        
        Change = Math.Round(mostRecentWeight.Weight - closestWeight.Weight, 2);

    }
}