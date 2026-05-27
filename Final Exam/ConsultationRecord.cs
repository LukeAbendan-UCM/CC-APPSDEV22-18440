using System;

namespace StudentConsultationLogs
{
    class ConsultationRecord
    {
        public int RecordId { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
        public string AdviserName { get; set; }
        public string Concern { get; set; }
        public string ConsultationDate { get; set; }

        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public int Checksum { get; set; }

        public int GenerateChecksum()
        {
            string data =
                StudentName +
                Course +
                AdviserName +
                Concern +
                ConsultationDate;

            int sum = 0;

            foreach (char c in data)
            {
                sum += c;
            }

            return sum;
        }

        public override string ToString()
        {
            return RecordId + "|" +
                   StudentName + "|" +
                   Course + "|" +
                   AdviserName + "|" +
                   Concern + "|" +
                   ConsultationDate + "|" +
                   CreatedAt + "|" +
                   UpdatedAt + "|" +
                   IsActive + "|" +
                   Checksum;
        }

        public static ConsultationRecord FromString(string line)
        {
            string[] parts = line.Split('|');

            ConsultationRecord r = new ConsultationRecord();

            r.RecordId = Convert.ToInt32(parts[0]);
            r.StudentName = parts[1];
            r.Course = parts[2];
            r.AdviserName = parts[3];
            r.Concern = parts[4];
            r.ConsultationDate = parts[5];
            r.CreatedAt = parts[6];
            r.UpdatedAt = parts[7];
            r.IsActive = Convert.ToBoolean(parts[8]);
            r.Checksum = Convert.ToInt32(parts[9]);

            return r;
        }
    }
}
