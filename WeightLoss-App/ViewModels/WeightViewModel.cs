using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    private ObservableCollection<WeightModel> _weights = new ObservableCollection<WeightModel>();
    
    WeightDatabase database;
    public WeightViewModel(WeightDatabase weightDatabase)
    {
        database = weightDatabase;
        LoadWeightsAsync();
    }

    private async Task LoadWeightsAsync()
    {
        var weights = await database.GetWeightsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Weights.Clear();
            foreach (var weight in weights)
            {
                Weights.Add(weight);
            }
        });
    }

    [RelayCommand]
    private async Task AddWeight()
    {
        if (Weight == 0)
        {
            await Shell.Current.DisplayAlert("Weight Required", "Please enter a valid weight", "OK");
            return;
        }

        var weightModel = new WeightModel { Weight = Weight, DateTime = DateTime.Now};
        var Id = await database.SaveWeightAsync(weightModel);
        Console.WriteLine(Id);
        Weights.Add(weightModel);
        
        Weight = 0;
    }
    
    [RelayCommand]
    private async Task DeleteWeight(WeightModel weight)
    {
        await database.DeleteWeightAsync(weight);

        await LoadWeightsAsync();
    }
}