namespace Animals.Models.Results
{
    public interface IAnimalResult
    { 
        bool Success { get; set; }
        string Message { get; set; }
    }
}