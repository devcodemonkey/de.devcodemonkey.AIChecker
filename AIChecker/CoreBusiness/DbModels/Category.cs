using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string FileName { get; set; }
        public string CategoryName { get; set; }
    }
}
