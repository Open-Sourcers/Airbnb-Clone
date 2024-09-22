namespace Airbnb.Domain
{
    public class BaseEntity<T>
    {
       public T Id { get; set; }
       public string Name { get; set; }
    }
}
