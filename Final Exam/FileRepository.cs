using System;
using System.Collections.Generic;
using System.IO;

namespace StudentConsultationLogs
{
    class FileRepository
    {
        public static string dataFolder = "Data";
        public static string recordsFile = "Data/consultations.txt";
        public static string auditFile = "Data/audit.txt";

        public static void InitializeStorage()
        {
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            if (!File.Exists(recordsFile))
            {
                File.Create(recordsFile).Close();
            }

            if (!File.Exists(auditFile))
            {
                File.Create(auditFile).Close();
            }
        }

        public static int GenerateNextId()
        {
            List<ConsultationRecord> records = GetAllRecords();

            int max = 0;

            foreach (ConsultationRecord r in records)
            {
                if (r.RecordId > max)
                {
                    max = r.RecordId;
                }
            }

            return max + 1;
        }

        public static void AddRecord(ConsultationRecord record)
        {
            File.AppendAllText(recordsFile,
                record.ToString() + Environment.NewLine);

            AuditLogger.Log("ADD",
                "Added record ID " + record.RecordId);
        }

        public static List<ConsultationRecord> GetAllRecords()
        {
            List<ConsultationRecord> records =
                new List<ConsultationRecord>();

            string[] lines = File.ReadAllLines(recordsFile);

            foreach (string line in lines)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        records.Add(
                            ConsultationRecord.FromString(line));
                    }
                }
                catch
                {
                    AuditLogger.Log("ERROR",
                        "Malformed record skipped.");
                }
            }

            return records;
        }

        public static void SaveAllRecords(
            List<ConsultationRecord> records)
        {
            List<string> lines = new List<string>();

            foreach (ConsultationRecord r in records)
            {
                lines.Add(r.ToString());
            }

            File.WriteAllLines(recordsFile, lines.ToArray());
        }
    }
}
