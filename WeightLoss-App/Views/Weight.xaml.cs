using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightLoss_App.ViewModels;

namespace WeightLoss_App.Views;

public partial class Weight : ContentPage
{
    public Weight(WeightViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}