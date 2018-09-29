using EF.Services;

namespace SMS.Models
{
    public partial class OptionModel : BaseEntityModel
    {
        public OptionModel()
        {
            Question = new QuestionModel();
        }
        public string Name { get; set; }
        public string CorrectAnswer { get; set; }
        public int DisplayOrder { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
    }
}