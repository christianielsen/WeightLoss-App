using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeightLoss_App.Database;
using WeightLoss_App.Models;

namespace WeightLoss_App.ViewModels;

public partial class WeightViewModel : ObservableObject
{
    [ObservableProperty]
    private double _weight;
    
    private WeightDatabase database;
    public WeightViewModel(WeightDatabase weightDatabase)
    {
        database = weightDatabase;
    }

    [RelayCommand]
    private async Task AddWeight()
    {
        if (Weight == 0)
        {
            await Shell.Current.DisplayAlert("Weight Required", "Please enter a valid weight", "OK");
            return;
        }

        await database.SaveWeightAsync(new WeightModel { Weight = Weight });
        
    }
    
}