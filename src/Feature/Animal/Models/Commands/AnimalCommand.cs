namespace Animal.Models.Commands
{
    public abstract class AnimalCommand
    {
        public bool IsNull()
        {
            return this == null ? true : false;
        }
    }
}
