using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.UseCases
{
    public class ObjectCreationForApi
    {
        public static List<IMessage> CreateMessageForApi(string systemPrompt, string message)
        {
            return new List<IMessage> {
                new Message
                {
                    Role = "user",
                    Content = message
                },
                new Message
                {
                    Role = "system",
                    Content = systemPrompt
                }
            };
        }
    }
}
