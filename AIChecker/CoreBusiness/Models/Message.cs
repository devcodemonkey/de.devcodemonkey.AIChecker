using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.devcodemonkey.AIChecker.CoreBusiness.Interfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class Message : IMessage
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
