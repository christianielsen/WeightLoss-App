using SQLite;

namespace WeightLoss_App.Models;

public class WeightModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public double Weight { get; set; }
    public DateTime DateTime { get; set; }
}