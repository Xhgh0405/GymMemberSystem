using System;

namespace GymMemberSystem
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public double HeightCm { get; set; }
        public string Goal { get; set; }
        public string Trainer { get; set; }
        public string PhotoPath { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public string Status
        {
            get { return ExpireDate.Date >= DateTime.Today ? "有效" : "過期"; }
        }

        public int RemainingDays
        {
            get { return Math.Max(0, (ExpireDate.Date - DateTime.Today).Days); }
        }

        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - Birthday.Year;
                if (Birthday.Date > DateTime.Today.AddYears(-age)) age--;
                return Math.Max(age, 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0:000}  {1}", Id, Name);
        }
    }

    public class BodyRecord
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime RecordDate { get; set; }
        public double WeightKg { get; set; }
        public double BodyFatPct { get; set; }
        public double MuscleKg { get; set; }
        public double VisceralFat { get; set; }
        public double WaistCm { get; set; }
        public double HipCm { get; set; }
    }

    public class PaymentRecord
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime PayDate { get; set; }
        public string PlanName { get; set; }
        public int Days { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Note { get; set; }
    }

    public class ActivityItem
    {
        public string Name { get; set; }
        public double Factor { get; set; }

        public ActivityItem(string name, double factor)
        {
            Name = name;
            Factor = factor;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class PlanItem
    {
        public string Name { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }

        public PlanItem(string name, int days, decimal price)
        {
            Name = name;
            Days = days;
            Price = price;
        }

        public override string ToString()
        {
            return string.Format("{0}｜{1} 天｜NT$ {2:N0}", Name, Days, Price);
        }
    }
}
