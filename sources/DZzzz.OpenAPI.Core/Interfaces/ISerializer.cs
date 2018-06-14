namespace DZzzz.OpenAPI.Core.Interfaces
{
    public interface ISerializer<T>
    {
        T Serialize<TK>(TK @object);
        TK Deserialize<TK>(T value);
    }
}