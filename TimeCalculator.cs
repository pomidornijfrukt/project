using System;

namespace Project
{
    public class TimeCalculator
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DataName { get; set; }

        public TimeCalculator(DateTime startDate, DateTime endDate, string dataName)
        {
            StartDate = startDate;
            EndDate = endDate;
            DataName = dataName;
        }

        public string GetDifferenceAsString()
        {
            // Using long not to get overflowed with big numbers
            long totalSeconds = (long)GetDifference().TotalSeconds; // Get the total number of seconds
            long months = totalSeconds / (30 * 24 * 60 * 60); // Approximate number of seconds in a month
            totalSeconds -= months * (30 * 24 * 60 * 60); // Subtract the months
            long weeks = totalSeconds / (7 * 24 * 60 * 60); // Calculate the remaining weeks
            totalSeconds -= weeks * (7 * 24 * 60 * 60); // Subtract the weeks
            long days = totalSeconds / (24 * 60 * 60); // Calculate the remaining days
            totalSeconds -= days * (24 * 60 * 60); // Subtract the days
            long hours = totalSeconds / (60 * 60); // Calculate the remaining hours
            totalSeconds -= hours * (60 * 60); // Subtract the hours
            long minutes = totalSeconds / 60; // Calculate the remaining minutes
            totalSeconds -= minutes * 60; // Subtract the minutes
            long seconds = totalSeconds; // The remaining seconds

            var parts = new List<string>();

            if (months > 0)
            {
                parts.Add($"{months} month(s)");
            }
            if (weeks > 0)
            {
                parts.Add($"{weeks} week(s)");
            }
            if (days > 0)
            {
                parts.Add($"{days} day(s)");
            }
            if (hours > 0)
            {
                parts.Add($"{hours} hour(s)");
            }
            if (minutes > 0)
            {
                parts.Add($"{minutes} minute(s)");
            }
            if (seconds > 0)
            {
                parts.Add($"{seconds} second(s)");
            }

            return string.Join(", ", parts);
        }

        public TimeSpan GetDifference()
        {
            return EndDate - StartDate;
        }

        public double GetDifferenceInDays()
        {
            return GetDifference().TotalDays;
        }

        public double GetDifferenceInHours()
        {
            return GetDifference().TotalHours;
        }

        public double GetDifferenceInMinutes()
        {
            return GetDifference().TotalMinutes;
        }

        public double GetDifferenceInSeconds()
        {
            return GetDifference().TotalSeconds;
        }

        public double GetDifferenceInWeeks()
        {
            return GetDifference().TotalDays / 7;
        }

        public double GetDifferenceInMonths()
        {
            return GetDifference().TotalDays / 31;
        }
    }
}