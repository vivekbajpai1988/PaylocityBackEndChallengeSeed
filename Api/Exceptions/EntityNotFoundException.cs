namespace Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(String message) : base(message){ }

        public EntityNotFoundException(int id) : base($"Entity with Id {id} not found!") { }

    }
}
