using System;
using System.Collections.Generic;

namespace StudentConsultationLogs
{
    class ReportGenerator
    {
        public static void GenerateReport()
        {
            Console.Clear();

            List<ConsultationRecord> records =
                FileRepository.GetAllRecords();

            Dictionary<string, int> courseCount =
                new Dictionary<string, int>();

            int total = 0;

            foreach (ConsultationRecord r in records)
            {
                if (r.IsActive)
                {
                    total++;

                    if (!courseCount.ContainsKey(r.Course))
                    {
                        courseCount[r.Course] = 0;
                    }

                    courseCount[r.Course]++;
                }
            }

            Console.WriteLine(
                "===== CONSULTATION REPORT =====");

            Console.WriteLine(
                "Total Active Records: " + total);

            Console.WriteLine();

            Console.WriteLine("Consultations Per Course:");

            foreach (KeyValuePair<string, int> item
                in courseCount)
            {
                Console.WriteLine(
                    item.Key + " : " + item.Value);
            }

            AuditLogger.Log(
                "REPORT",
                "Generated report");

            Console.WriteLine();
            Console.WriteLine(
                "Press any key to continue...");
            Console.ReadKey();
        }
    }
}
