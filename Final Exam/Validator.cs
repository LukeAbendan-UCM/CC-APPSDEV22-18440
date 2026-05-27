using System;

namespace StudentConsultationLogs
{
    class Validator
    {
        public static bool ValidateRecord(
            ConsultationRecord record)
        {
            if (string.IsNullOrWhiteSpace(
                record.StudentName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                record.Course))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                record.AdviserName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                record.Concern))
            {
                return false;
            }

            DateTime tempDate;

            if (!DateTime.TryParse(
                record.ConsultationDate,
                out tempDate))
            {
                return false;
            }

            return true;
        }
    }
}
