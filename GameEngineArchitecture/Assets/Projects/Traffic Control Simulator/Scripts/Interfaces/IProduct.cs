/// <summary>
/// Common interface for all objects that can
/// be instantiated by a factory.
/// </summary>
public interface IProduct
{
    public string Name { get; }
    public void Initialize();
}
