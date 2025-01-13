using System.Runtime.InteropServices.JavaScript;
using SQLite;

namespace WeightLoss_App.Models;

public class WeightModel
{
    [AutoIncrement]
    public int Id { get; set; }
    public double Weight { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}