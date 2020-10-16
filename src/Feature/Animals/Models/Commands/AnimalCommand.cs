namespace Animals.Models.Commands
{
    public abstract class AnimalCommand
    {
        public string UserId { get; set; }

        public bool IsNull()
        {
            return this == null ? true : false;
        }
    }
}
