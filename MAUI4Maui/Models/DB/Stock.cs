namespace MAUI4Maui.Models.DB;

public class Stock
{
    public string Id { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public List<TimeValue> Values { get; set; } = new();
}