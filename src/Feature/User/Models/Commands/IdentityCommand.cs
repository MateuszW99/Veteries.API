namespace User.Models.Commands
{
    public class IdentityCommand
    {
        public bool IsNull()
        {
            return this == null ? true : false;
        }
    }
}
