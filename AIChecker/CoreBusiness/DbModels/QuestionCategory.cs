using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels
{
    public class QuestionCategory
    {
        public Guid QuestionCategoryId { get; set; }

        public string? FileName { get; set; }

        public string Value { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = null!;
    }
}
