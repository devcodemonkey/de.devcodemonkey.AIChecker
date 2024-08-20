namespace de.devcodemonkey.AIChecker.CoreBusiness.Interfaces
{
    public interface IMessage
    {
        string Content { get; set; }
        string Role { get; set; }
    }
}