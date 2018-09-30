namespace EF.Core.Data
{
    public partial class CustomPageUrl : BaseEntity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Slug { get; set; }

        public bool IsActive { get; set; }

    }
}
