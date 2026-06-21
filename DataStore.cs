using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GymMemberSystem
{
    public static class DataStore
    {
        private static readonly string DataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string MemberFile = Path.Combine(DataDir, "members.csv");
        private static readonly string RecordFile = Path.Combine(DataDir, "body_records.csv");
        private static readonly string PaymentFile = Path.Combine(DataDir, "payments.csv");
        private static readonly string PhotoDir = Path.Combine(DataDir, "Photos");

        public static List<Member> LoadMembers()
        {
            EnsureDataDir();
            if (!File.Exists(MemberFile))
            {
                var demo = new List<Member>
                {
                    new Member
                    {
                        Id = 1, Name = "王小明", Phone = "0912-345-678", Gender = "男",
                        Birthday = new DateTime(2002, 5, 10), HeightCm = 175, Goal = "減脂",
                        Trainer = "Kevin", JoinDate = DateTime.Today.AddMonths(-2), ExpireDate = DateTime.Today.AddMonths(1)
                    },
                    new Member
                    {
                        Id = 2, Name = "林美美", Phone = "0988-111-222", Gender = "女",
                        Birthday = new DateTime(2001, 8, 20), HeightCm = 162, Goal = "增肌",
                        Trainer = "Mina", JoinDate = DateTime.Today.AddMonths(-1), ExpireDate = DateTime.Today.AddMonths(5)
                    },
                    new Member
                    {
                        Id = 3, Name = "陳健豪", Phone = "0977-222-333", Gender = "男",
                        Birthday = new DateTime(1999, 2, 3), HeightCm = 181, Goal = "維持健康",
                        Trainer = "Alex", JoinDate = DateTime.Today.AddMonths(-7), ExpireDate = DateTime.Today.AddDays(8)
                    }
                };
                SaveMembers(demo);
                return demo;
            }

            var list = new List<Member>();
            foreach (var line in File.ReadAllLines(MemberFile, Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 9) continue;
                list.Add(new Member
                {
                    Id = ParseInt(p[0]),
                    Name = p[1],
                    Phone = p[2],
                    Gender = p[3],
                    Birthday = ParseDate(p[4]),
                    HeightCm = ParseDouble(p[5]),
                    Goal = p[6],
                    JoinDate = ParseDate(p[7]),
                    ExpireDate = ParseDate(p[8]),
                    Trainer = p.Length > 9 ? p[9] : "未指定",
                    PhotoPath = p.Length > 10 ? p[10] : ""
                });
            }
            return list;
        }

        public static void SaveMembers(IEnumerable<Member> members)
        {
            EnsureDataDir();
            var lines = new List<string> { "Id|Name|Phone|Gender|Birthday|HeightCm|Goal|JoinDate|ExpireDate|Trainer|PhotoPath" };
            foreach (var m in members.OrderBy(x => x.Id))
            {
                lines.Add(string.Join("|", new[]
                {
                    m.Id.ToString(CultureInfo.InvariantCulture), Clean(m.Name), Clean(m.Phone), Clean(m.Gender),
                    m.Birthday.ToString("yyyy-MM-dd"), m.HeightCm.ToString(CultureInfo.InvariantCulture), Clean(m.Goal),
                    m.JoinDate.ToString("yyyy-MM-dd"), m.ExpireDate.ToString("yyyy-MM-dd"), Clean(m.Trainer), Clean(m.PhotoPath)
                }));
            }
            File.WriteAllLines(MemberFile, lines, Encoding.UTF8);
        }

        public static List<BodyRecord> LoadBodyRecords()
        {
            EnsureDataDir();
            if (!File.Exists(RecordFile))
            {
                var demo = new List<BodyRecord>
                {
                    new BodyRecord { Id = 1, MemberId = 1, RecordDate = DateTime.Today.AddDays(-42), WeightKg = 71.3, BodyFatPct = 24.2, MuscleKg = 47.8, VisceralFat = 8, WaistCm = 84, HipCm = 95 },
                    new BodyRecord { Id = 2, MemberId = 1, RecordDate = DateTime.Today.AddDays(-28), WeightKg = 70.5, BodyFatPct = 23.4, MuscleKg = 48.1, VisceralFat = 7, WaistCm = 82, HipCm = 94 },
                    new BodyRecord { Id = 3, MemberId = 1, RecordDate = DateTime.Today.AddDays(-14), WeightKg = 69.2, BodyFatPct = 22.1, MuscleKg = 48.6, VisceralFat = 7, WaistCm = 80, HipCm = 93 },
                    new BodyRecord { Id = 4, MemberId = 1, RecordDate = DateTime.Today, WeightKg = 68.4, BodyFatPct = 20.8, MuscleKg = 49.0, VisceralFat = 6, WaistCm = 78, HipCm = 92 },
                    new BodyRecord { Id = 5, MemberId = 2, RecordDate = DateTime.Today.AddDays(-30), WeightKg = 52.5, BodyFatPct = 25.8, MuscleKg = 33.8, VisceralFat = 5, WaistCm = 68, HipCm = 89 },
                    new BodyRecord { Id = 6, MemberId = 2, RecordDate = DateTime.Today.AddDays(-15), WeightKg = 52.8, BodyFatPct = 25.0, MuscleKg = 34.2, VisceralFat = 4, WaistCm = 67, HipCm = 88 },
                    new BodyRecord { Id = 7, MemberId = 2, RecordDate = DateTime.Today, WeightKg = 53.6, BodyFatPct = 24.1, MuscleKg = 35.1, VisceralFat = 4, WaistCm = 67, HipCm = 88 },
                    new BodyRecord { Id = 8, MemberId = 3, RecordDate = DateTime.Today.AddDays(-20), WeightKg = 82.0, BodyFatPct = 19.4, MuscleKg = 58.6, VisceralFat = 8, WaistCm = 88, HipCm = 98 },
                    new BodyRecord { Id = 9, MemberId = 3, RecordDate = DateTime.Today, WeightKg = 81.2, BodyFatPct = 18.6, MuscleKg = 59.0, VisceralFat = 8, WaistCm = 87, HipCm = 98 }
                };
                SaveBodyRecords(demo);
                return demo;
            }

            var list = new List<BodyRecord>();
            foreach (var line in File.ReadAllLines(RecordFile, Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 8) continue;
                list.Add(new BodyRecord
                {
                    Id = ParseInt(p[0]), MemberId = ParseInt(p[1]), RecordDate = ParseDate(p[2]),
                    WeightKg = ParseDouble(p[3]), BodyFatPct = ParseDouble(p[4]), MuscleKg = ParseDouble(p[5]),
                    VisceralFat = ParseDouble(p[6]), WaistCm = ParseDouble(p[7]), HipCm = p.Length > 8 ? ParseDouble(p[8]) : 0
                });
            }
            return list;
        }

        public static void SaveBodyRecords(IEnumerable<BodyRecord> records)
        {
            EnsureDataDir();
            var lines = new List<string> { "Id|MemberId|RecordDate|WeightKg|BodyFatPct|MuscleKg|VisceralFat|WaistCm|HipCm" };
            foreach (var r in records.OrderBy(x => x.MemberId).ThenBy(x => x.RecordDate))
            {
                lines.Add(string.Join("|", new[]
                {
                    r.Id.ToString(CultureInfo.InvariantCulture), r.MemberId.ToString(CultureInfo.InvariantCulture),
                    r.RecordDate.ToString("yyyy-MM-dd"), r.WeightKg.ToString(CultureInfo.InvariantCulture),
                    r.BodyFatPct.ToString(CultureInfo.InvariantCulture), r.MuscleKg.ToString(CultureInfo.InvariantCulture),
                    r.VisceralFat.ToString(CultureInfo.InvariantCulture), r.WaistCm.ToString(CultureInfo.InvariantCulture), r.HipCm.ToString(CultureInfo.InvariantCulture)
                }));
            }
            File.WriteAllLines(RecordFile, lines, Encoding.UTF8);
        }

        public static List<PaymentRecord> LoadPayments()
        {
            EnsureDataDir();
            if (!File.Exists(PaymentFile))
            {
                var demo = new List<PaymentRecord>
                {
                    new PaymentRecord { Id = 1, MemberId = 1, PayDate = DateTime.Today.AddMonths(-1), PlanName = "月卡", Days = 30, Amount = 1500, Method = "現金", Note = "初始示範" },
                    new PaymentRecord { Id = 2, MemberId = 2, PayDate = DateTime.Today.AddMonths(-1), PlanName = "半年卡", Days = 180, Amount = 7200, Method = "信用卡", Note = "初始示範" }
                };
                SavePayments(demo);
                return demo;
            }

            var list = new List<PaymentRecord>();
            foreach (var line in File.ReadAllLines(PaymentFile, Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 7) continue;
                list.Add(new PaymentRecord
                {
                    Id = ParseInt(p[0]), MemberId = ParseInt(p[1]), PayDate = ParseDate(p[2]),
                    PlanName = p[3], Days = ParseInt(p[4]), Amount = ParseDecimal(p[5]), Method = p[6], Note = p.Length > 7 ? p[7] : ""
                });
            }
            return list;
        }

        public static void SavePayments(IEnumerable<PaymentRecord> payments)
        {
            EnsureDataDir();
            var lines = new List<string> { "Id|MemberId|PayDate|PlanName|Days|Amount|Method|Note" };
            foreach (var p in payments.OrderByDescending(x => x.PayDate).ThenBy(x => x.Id))
            {
                lines.Add(string.Join("|", new[]
                {
                    p.Id.ToString(CultureInfo.InvariantCulture), p.MemberId.ToString(CultureInfo.InvariantCulture), p.PayDate.ToString("yyyy-MM-dd"),
                    Clean(p.PlanName), p.Days.ToString(CultureInfo.InvariantCulture), p.Amount.ToString(CultureInfo.InvariantCulture), Clean(p.Method), Clean(p.Note)
                }));
            }
            File.WriteAllLines(PaymentFile, lines, Encoding.UTF8);
        }

        public static string ExportReport(string fileName, string content)
        {
            EnsureDataDir();
            string file = Path.Combine(DataDir, fileName);
            File.WriteAllText(file, content, Encoding.UTF8);
            return file;
        }

        public static string SaveDietPlan(int memberId, string content)
        {
            EnsureDataDir();
            string file = Path.Combine(DataDir, string.Format("diet_plan_member_{0}_{1}.txt", memberId, DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            File.WriteAllText(file, content, Encoding.UTF8);
            return file;
        }


        public static string SaveMemberPhoto(int memberId, string sourcePath)
        {
            EnsureDataDir();
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath)) return string.Empty;

            string fullPhotoDir = Path.GetFullPath(PhotoDir).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string fullSource = Path.GetFullPath(sourcePath);
            if (fullSource.StartsWith(fullPhotoDir, StringComparison.OrdinalIgnoreCase)) return fullSource;

            string ext = Path.GetExtension(sourcePath).ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".bmp") ext = ".jpg";
            string target = Path.Combine(PhotoDir, string.Format("member_{0}{1}", memberId, ext));
            File.Copy(sourcePath, target, true);
            return target;
        }

        private static void EnsureDataDir()
        {
            if (!Directory.Exists(DataDir)) Directory.CreateDirectory(DataDir);
            if (!Directory.Exists(PhotoDir)) Directory.CreateDirectory(PhotoDir);
        }

        private static string Clean(string text)
        {
            if (text == null) return string.Empty;
            return text.Replace("|", " ").Replace("\r", " ").Replace("\n", " ").Trim();
        }

        private static int ParseInt(string text)
        {
            int value;
            return int.TryParse(text, out value) ? value : 0;
        }

        private static double ParseDouble(string text)
        {
            double value;
            return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : 0;
        }

        private static decimal ParseDecimal(string text)
        {
            decimal value;
            return decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : 0M;
        }

        private static DateTime ParseDate(string text)
        {
            DateTime value;
            return DateTime.TryParse(text, out value) ? value : DateTime.Today;
        }
    }
}
