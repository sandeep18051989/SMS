using System.Collections.Generic;

namespace SMS.Models.Widgets
{
    public partial class TeacherWidgetModel
    {
        public TeacherWidgetModel()
        {
            Subjects = new List<SubjectModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
		public string SystemName { get; set; }
        public string Description { get; set; }
        public string Qualification { get; set; }
        public int ProfilePictureId { get; set; }
        public string ProfilePicture { get; set; }
        public string FacebookLink { get; set; }
        public string TweeterLink { get; set; }
        public string InstagramLink { get; set; }
        public string GooglePlusLink { get; set; }
        public string PInterestLink { get; set; }
        public string LinkedInLink { get; set; }
        public string Hi5Link { get; set; }
        public IList<SubjectModel> Subjects { get; set; }
    }
}