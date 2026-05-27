using System;
using System.Collections.Generic;

namespace StudentConsultationLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            FileRepository.InitializeStorage();

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine(" STUDENT CONSULTATION LOGS SYSTEM");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Add Record");
                Console.WriteLine("2. View Records");
                Console.WriteLine("3. Search Record");
                Console.WriteLine("4. Update Record");
                Console.WriteLine("5. Soft Delete Record");
                Console.WriteLine("6. Hard Delete Record");
                Console.WriteLine("7. Generate Report");
                Console.WriteLine("8. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddRecord();
                            break;

                        case "2":
                            ViewRecords();
                            break;

                        case "3":
                            SearchRecord();
                            break;

                        case "4":
                            UpdateRecord();
                            break;

                        case "5":
                            SoftDeleteRecord();
                            break;

                        case "6":
                            HardDeleteRecord();
                            break;

                        case "7":
                            ReportGenerator.GenerateReport();
                            break;

                        case "8":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    AuditLogger.Log("ERROR", ex.Message);
                    Pause();
                }
            }
        }

        static void AddRecord()
        {
            Console.Clear();

            ConsultationRecord record = new ConsultationRecord();

            record.RecordId = FileRepository.GenerateNextId();

            Console.Write("Student Name: ");
            record.StudentName = Console.ReadLine();

            Console.Write("Course: ");
            record.Course = Console.ReadLine();

            Console.Write("Adviser Name: ");
            record.AdviserName = Console.ReadLine();

            Console.Write("Concern: ");
            record.Concern = Console.ReadLine();

            Console.Write("Consultation Date (yyyy-MM-dd): ");
            record.ConsultationDate = Console.ReadLine();

            if (!Validator.ValidateRecord(record))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            record.CreatedAt = DateTime.Now.ToString();
            record.UpdatedAt = DateTime.Now.ToString();
            record.IsActive = true;
            record.Checksum = record.GenerateChecksum();

            FileRepository.AddRecord(record);

            Console.WriteLine("Record added successfully.");
            Pause();
        }

        static void ViewRecords()
        {
            Console.Clear();

            List<ConsultationRecord> records = FileRepository.GetAllRecords();

            Console.WriteLine("===== ACTIVE RECORDS =====");

            foreach (ConsultationRecord r in records)
            {
                if (r.IsActive)
                {
                    DisplayRecord(r);
                }
            }

            AuditLogger.Log("READ", "Viewed records");
            Pause();
        }

        static void SearchRecord()
        {
            Console.Clear();

            Console.Write("Enter student name to search: ");
            string keyword = Console.ReadLine().ToLower();

            List<ConsultationRecord> records = FileRepository.GetAllRecords();

            foreach (ConsultationRecord r in records)
            {
                if (r.IsActive &&
                    r.StudentName.ToLower().Contains(keyword))
                {
                    DisplayRecord(r);
                }
            }

            AuditLogger.Log("READ", "Searched records");
            Pause();
        }

        static void UpdateRecord()
        {
            Console.Clear();

            Console.Write("Enter Record ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<ConsultationRecord> records = FileRepository.GetAllRecords();

            ConsultationRecord target = null;

            foreach (ConsultationRecord r in records)
            {
                if (r.RecordId == id && r.IsActive)
                {
                    target = r;
                    break;
                }
            }

            if (target == null)
            {
                Console.WriteLine("Record not found.");
                Pause();
                return;
            }

            Console.Write("New Student Name: ");
            target.StudentName = Console.ReadLine();

            Console.Write("New Course: ");
            target.Course = Console.ReadLine();

            Console.Write("New Adviser Name: ");
            target.AdviserName = Console.ReadLine();

            Console.Write("New Concern: ");
            target.Concern = Console.ReadLine();

            Console.Write("New Consultation Date: ");
            target.ConsultationDate = Console.ReadLine();

            target.UpdatedAt = DateTime.Now.ToString();
            target.Checksum = target.GenerateChecksum();

            FileRepository.SaveAllRecords(records);

            AuditLogger.Log("UPDATE", "Updated record ID " + id);

            Console.WriteLine("Record updated.");
            Pause();
        }

        static void SoftDeleteRecord()
        {
            Console.Clear();

            Console.Write("Enter Record ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<ConsultationRecord> records = FileRepository.GetAllRecords();

            foreach (ConsultationRecord r in records)
            {
                if (r.RecordId == id)
                {
                    r.IsActive = false;
                    r.UpdatedAt = DateTime.Now.ToString();
                    r.Checksum = r.GenerateChecksum();

                    FileRepository.SaveAllRecords(records);

                    AuditLogger.Log("DELETE", "Soft deleted ID " + id);

                    Console.WriteLine("Record soft deleted.");
                    Pause();
                    return;
                }
            }

            Console.WriteLine("Record not found.");
            Pause();
        }

        static void HardDeleteRecord()
        {
            Console.Clear();

            Console.Write("Enter Record ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            List<ConsultationRecord> records = FileRepository.GetAllRecords();

            records.RemoveAll(r => r.RecordId == id);

            FileRepository.SaveAllRecords(records);

            AuditLogger.Log("DELETE", "Hard deleted ID " + id);

            Console.WriteLine("Record permanently deleted.");
            Pause();
        }

        static void DisplayRecord(ConsultationRecord r)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID: " + r.RecordId);
            Console.WriteLine("Student: " + r.StudentName);
            Console.WriteLine("Course: " + r.Course);
            Console.WriteLine("Adviser: " + r.AdviserName);
            Console.WriteLine("Concern: " + r.Concern);
            Console.WriteLine("Date: " + r.ConsultationDate);
            Console.WriteLine("Created: " + r.CreatedAt);
            Console.WriteLine("Updated: " + r.UpdatedAt);
            Console.WriteLine("--------------------------------");
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
