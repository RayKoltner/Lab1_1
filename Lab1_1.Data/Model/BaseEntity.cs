namespace Lab1_1.Data.Model
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public string Comments { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public int DeletedUserId { get; set; }

        public DateTime DeletedDate { get; set; }
    }
}
