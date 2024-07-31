namespace OAuth2.Service.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found!")
        {
        }

        public NotFoundException(string key, string value) 
            : base($"Attribute {key}={value} not found!")
        {

        }
    }
}
