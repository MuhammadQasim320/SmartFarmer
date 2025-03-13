using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Model
{
    public class TimeSheetViewModel : TimeSheetRequestViewModel
    {
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class TimeSheetRequestViewModel
    {
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchKey { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

    }
    public class TimeSheetAppRequestViewModel
    {
        public int? Month { get; set; }
        public int? Year { get; set; }

    }

    public class TimeSheetWebRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate
        {
            get; set;

        }
      
        public class TimeSheetSearchViewModel
        {
            public DateTime CreatedDate { get; set; }
            public int? EventTypeId { get; set; }
            public string EventTypeName { get; set; }
        }

        public class TimeSheetListSearchResponseViewModel
        {
            public int TotalCount { get; set; }
            public IEnumerable<TimeSheetListSearchViewModel> List { get; set; }
        }
        public class TimeSheetSearchResponseViewModel
        {
            //public int TotalCount { get; set; }
            public IEnumerable<TimeSheetListSearchViewModel> List { get; set; }
        }
        public class TimeSheetListSearchViewModel
        {
            public DateTime CreatedDate { get; set; }
            public string Date { get; set; }
            public string CreatedDay { get; set; }
            public int? EventTypeId { get; set; }
            //public string EventTypeName { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedByName { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public string Hours { get; set; }
        }
        public class TimeSheetListViewModel
        {
            public DateTime CreatedDate { get; set; }
            public string Date { get; set; }
            public string CreatedDay { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedByName { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public string Hours { get; set; }
            public int? EventTypeId { get; set; }
        }

      

        public class TimeSheetResponseViewModel
        {
            public List<TimeSheetMonthViewModel> TimeSheetSummary { get; set; }
        }

        public class TimeSheetMonthViewModel
        {
            public string Month { get; set; } 
            public string TotalMonthlyHours { get; set; } 
            public List<TimeSheetWeekViewModel> Weeks { get; set; }
        }

        public class TimeSheetWeekViewModel
        {
            public string WeekNumber { get; set; }
            public string TotalWeeklyHours { get; set; } 
            public List<TimeSheetEntryViewModel> Entries { get; set; }
        }

        public class TimeSheetEntryViewModel
        {
            public DateTime? Date { get; set; }
            public string Day { get; set; }
            public DateTime? ClockIn { get; set; }
            public DateTime? ClockOut { get; set; } 
            public string Duration { get; set; } 
        }
    }
}
