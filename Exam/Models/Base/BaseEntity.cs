namespace Exam.Models.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int IsDeleted { get; set; }
    }
}
