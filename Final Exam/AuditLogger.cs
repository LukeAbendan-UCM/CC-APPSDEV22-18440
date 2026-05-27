using System;
using System.IO;

namespace StudentConsultationLogs
{
    class AuditLogger
    {
        public static void Log(
            string action,
            string details)
        {
            string log =
                DateTime.Now.ToString() +
                " | " +
                action +
                " | " +
                details;

            File.AppendAllText(
                FileRepository.auditFile,
                log + Environment.NewLine);
        }
    }
}
