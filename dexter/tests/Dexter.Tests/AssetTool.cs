namespace Dexter.Tests;


/// <summary>
/// Info to make available assistant - similar to view model in idea
/// </summary>
public class AssetInfo
{
    /// <summary>
    /// Asset Name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Asset amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime RenewsAt { get; set; }
}

/// <summary>
/// Provide asset data to assistants
/// </summary>
public class AssetsTool
{
    /// <summary>
    /// get assets
    /// </summary>
    public async Task<List<AssetInfo>> GetAssets()
    {
        await Task.Yield();

        return
        [
            new AssetInfo
            {
                Name = "Gusto",
                Amount = 200.00m,
                RenewsAt = DateTime.Today
            },
            new AssetInfo
            {
                Name = "GitHub",
                Amount = 40.00m,
                RenewsAt = DateTime.Today
            },
            new AssetInfo
            {
                Name = "Elastic",
                Amount = 50_000,
                RenewsAt = DateTime.Today
            }
        ];
    }


    /// <summary>
    /// get asset
    /// </summary>
    public async Task<List<AssetInfo>> GetAsset(string name)
    {
        await Task.Yield();

        return
        [
            new AssetInfo
            {
                Name = name,
                Amount = 200.00m,
                RenewsAt = DateTime.Today
            },
        ];
    }
}
