using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class Message : IMessage
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
