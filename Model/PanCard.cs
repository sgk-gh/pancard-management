using System;
namespace Model
{
    public class PanCard
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CustomerName { get; set; }
        public string FatherName { get; set; }
        public DateTime PanEntryDate { get; set; }
        public string CouponNumber { get; set; }
        public string FilePath { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; }
    }
}
