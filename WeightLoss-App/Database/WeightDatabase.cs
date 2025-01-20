using SQLite;
using WeightLoss_App.Models;

namespace WeightLoss_App.Database;

public class WeightDatabase
{
    private SQLiteAsyncConnection Database;

    public WeightDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;
        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        await Database.CreateTableAsync<WeightModel>();
    }

    public async Task<List<WeightModel>> GetWeightsAsync()
    {
        await Init();
        return await Database.Table<WeightModel>().OrderBy(w => w.Id).ToListAsync();
    }

    public async Task<int> SaveWeightAsync(WeightModel weight)
    {
        await Init();
        return await Database.InsertAsync(weight);
    }

    public async Task<int> DeleteWeightAsync(WeightModel weight)
    {
        await Init();
        return await Database.DeleteAsync(weight);
    }
}