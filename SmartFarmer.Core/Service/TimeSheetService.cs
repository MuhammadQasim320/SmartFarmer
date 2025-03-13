using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Domain.Model.TimeSheetWebRequestViewModel;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace SmartFarmer.Core.Service
{
    public class TimeSheetService : ITimeSheetService
    {
        private ITimeSheetRepository _timeSheetRepository;
        public TimeSheetService(ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetRepository = timeSheetRepository;
        }

        /// <summary>
        /// Get TimeSheet List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public TimeSheetListSearchResponseViewModel GetTimeSheetListBySearchWithPagination(string masterAdminId,TimeSheetRequestViewModel model)
        //{
        //    TimeSheetListSearchResponseViewModel timeSheetSearchResponse = new();
        //    timeSheetSearchResponse.List = _timeSheetRepository.GetTimeSheetListBySearchFilter(model.PageNumber, model.PageSize, masterAdminId, model.Month)?.Select(a => Mapper.MapTimeSheetToTimeSheetViewModel(a))?.ToList();
        //    timeSheetSearchResponse.TotalCount = _timeSheetRepository.GetTimeSheetCount(model.Month, model.Year, model.ToDate, model.FromDate);
        //    return timeSheetSearchResponse;
        //}

        /// <summary>
        /// get TimeSheet details List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TimeSheetSearchResponseViewModel GetTimeSheetList(string masterAdminId, TimeSheetRequestViewModel model)
        {
            TimeSheetSearchResponseViewModel timeSheetListViewModel = new();
            timeSheetListViewModel.List = _timeSheetRepository.GetTimeSheetListBySearchFilter(masterAdminId, /*model.PageNumber, model.PageSize,*/ model.SearchKey, model.Month, model.FromDate, model.ToDate, model.Year).Select(a => Mapper.MapTimeSheetListToTimeSheetViewModelList(a))?.ToList();
            //timeSheetListViewModel.TotalCount = _timeSheetRepository.GetTimeSheetCount(masterAdminId,model.SearchKey, model.Month, model.FromDate, model.ToDate, model.Year);

            var lastClockInByUser = new Dictionary<string, DateTime>();
            List<TimeSheetListSearchViewModel> timesheets = new();

            foreach (var evt in timeSheetListViewModel.List)
            {
                if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In)
                {
                    // Track the last Clock_In for the user
                    if (lastClockInByUser.ContainsKey(evt.CreatedBy))
                    {
                        var clockInTime = lastClockInByUser[evt.CreatedBy];
                        // If there's an unmatched Clock_In, pair it with this one as a single incomplete record
                        timesheets.Add(new TimeSheetListSearchViewModel
                        {

                            CreatedDate = clockInTime,
                            Date = clockInTime.Date.ToString("yyyy-MM-dd"),
                            CreatedDay = clockInTime.DayOfWeek.ToString(),
                            CreatedBy = evt.CreatedBy,
                            CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
                            StartTime = lastClockInByUser[evt.CreatedBy],
                            EndTime = null,
                            Hours = null
                        });
                    }

                    lastClockInByUser[evt.CreatedBy] = evt.CreatedDate;
                }
                else if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)
                {
                    if (lastClockInByUser.ContainsKey(evt.CreatedBy))
                    {
                        // Pair the Clock_Out with the last Clock_In
                        var clockIn = lastClockInByUser[evt.CreatedBy];
                        var clockOut = evt.CreatedDate;

                        TimeSpan duration = clockOut - clockIn;

                        var TotalHours = duration.TotalHours;


                        timesheets.Add(new TimeSheetListSearchViewModel
                        {
                            CreatedDate = clockIn,
                            Date = clockIn.ToString("yyyy-MM-dd"),
                            CreatedDay = clockIn.DayOfWeek.ToString(),
                            CreatedBy = evt.CreatedBy,
                            CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
                            StartTime = clockIn,
                            EndTime = clockOut,
                           // Hours = $"{duration.Days:D2}:{duration.Hours:D2}:{duration.Minutes:D2}",
                            Hours = $"{duration.TotalHours.ToString("0")} : {duration.Minutes.ToString("00")}"
                        });

                        // Remove the paired Clock_In
                        lastClockInByUser.Remove(evt.CreatedBy);
                    }
                    else
                    {
                        // No matching Clock_In, create an incomplete pair
                        timesheets.Add(new TimeSheetListSearchViewModel
                        {
                            CreatedDate = evt.CreatedDate,
                            Date = evt.CreatedDate.Date.ToString("yyyy-MM-dd"),
                            CreatedDay = evt.CreatedDate.DayOfWeek.ToString(),
                            CreatedBy = evt.CreatedBy,
                            CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
                            StartTime = null,
                            EndTime = evt.CreatedDate,
                            Hours = null
                        });
                    }

                  

                }
            }

           // Handle any remaining unmatched Clock_In events
            foreach (var key in lastClockInByUser)
            {
                var user = key.Key;
                var clockIn = key.Value;
                timesheets.Add(new TimeSheetListSearchViewModel
                {
                    CreatedDate = clockIn,
                    Date = clockIn.ToString("yyyy-MM-dd"),
                    CreatedDay = clockIn.DayOfWeek.ToString(),
                    CreatedBy = user,
                    CreatedByName = _timeSheetRepository.GetCreatedByName(user),
                    StartTime = clockIn,
                    EndTime = null,
                    Hours = null
                });
            }
            timeSheetListViewModel.List = timesheets.OrderByDescending(a=>a.CreatedDate);
            return timeSheetListViewModel;
        }



        ///// <summary>
        ///// get TimeSheet details List
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public TimeSheetResponseViewModel GetTimeSheetListAPP(string loginUserId, TimeSheetAppRequestViewModel model)
        //{
        //    TimeSheetSearchResponseViewModel timeSheetListViewModel = new();
        //    timeSheetListViewModel.List = _timeSheetRepository
        //        .GetTimeSheetListAPP(loginUserId, model.Month, model.Year)
        //        .Select(a => Mapper.MapTimeSheetListToTimeSheetViewModelList(a))?.ToList();

        //    var lastClockInByUser = new Dictionary<string, DateTime>();
        //    List<TimeSheetListSearchViewModel> timesheets = new();

        //    foreach (var evt in timeSheetListViewModel.List)
        //    {
        //        if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In)
        //        {
        //            if (lastClockInByUser.ContainsKey(evt.CreatedBy))
        //            {
        //                timesheets.Add(new TimeSheetListSearchViewModel
        //                {
        //                    CreatedDate = evt.CreatedDate.Date,
        //                    CreatedBy = evt.CreatedBy,
        //                    Date = evt.CreatedDate.Date.ToString("yyyy-MM-dd"),
        //                    CreatedDay = evt.CreatedDate.DayOfWeek.ToString(),
        //                    CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
        //                    StartTime = lastClockInByUser[evt.CreatedBy],
        //                    EndTime = null,
        //                    Hours = null
        //                });
        //            }

        //            lastClockInByUser[evt.CreatedBy] = evt.CreatedDate;
        //        }
        //        else if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)
        //        {
        //            if (lastClockInByUser.ContainsKey(evt.CreatedBy))
        //            {
        //                var clockIn = lastClockInByUser[evt.CreatedBy];
        //                var clockOut = evt.CreatedDate;

        //                TimeSpan duration = clockOut - clockIn;
        //                string formattedDuration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}";

        //                timesheets.Add(new TimeSheetListSearchViewModel
        //                {
        //                    CreatedDate = evt.CreatedDate.Date,
        //                    CreatedBy = evt.CreatedBy,
        //                    Date = evt.CreatedDate.Date.ToString("yyyy-MM-dd"),
        //                    CreatedDay = evt.CreatedDate.DayOfWeek.ToString(),
        //                    CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
        //                    StartTime = clockIn,
        //                    EndTime = clockOut,
        //                    Hours = formattedDuration
        //                });

        //                lastClockInByUser.Remove(evt.CreatedBy);
        //            }
        //            else
        //            {
        //                timesheets.Add(new TimeSheetListSearchViewModel
        //                {
        //                    CreatedDate = evt.CreatedDate.Date,
        //                    CreatedBy = evt.CreatedBy,
        //                    Date = evt.CreatedDate.Date.ToString("yyyy-MM-dd"),
        //                    CreatedDay = evt.CreatedDate.DayOfWeek.ToString(),
        //                    CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
        //                    StartTime = null,
        //                    EndTime = evt.CreatedDate,
        //                    Hours = null
        //                });
        //            }
        //        }
        //    }

        //    foreach (var key in lastClockInByUser)
        //    {
        //        var user = key.Key;
        //        var clockIn = key.Value;
        //        timesheets.Add(new TimeSheetListSearchViewModel
        //        {
        //            CreatedDate = clockIn.Date,
        //            CreatedBy = user,
        //            Date = clockIn.ToString("yyyy-MM-dd"),
        //            CreatedDay = clockIn.DayOfWeek.ToString(),
        //            CreatedByName = _timeSheetRepository.GetCreatedByName(user),
        //            StartTime = clockIn,
        //            EndTime = null,
        //            Hours = null
        //        });
        //    }

        //    var timeSheetSummary = timesheets
        //        .GroupBy(t => new { t.CreatedDate.Year, t.CreatedDate.Month })
        //        .Select(monthGroup =>
        //        {
        //            var monthName = new DateTime(monthGroup.Key.Year, monthGroup.Key.Month, 1).ToString("MMMM yyyy");

        //            // Calculate total monthly minutes
        //            var totalMonthlyMinutes = monthGroup
        //                .Where(t => !string.IsNullOrEmpty(t.Hours))
        //                .Sum(t => TryParseHoursToMinutes(t.Hours));

        //            // Convert totalMonthlyMinutes to hours and minutes
        //            var totalMonthlyHours = totalMonthlyMinutes / 60;
        //            var totalMonthlyRemainingMinutes = totalMonthlyMinutes % 60;

        //            var weeks = monthGroup.GroupBy(t =>
        //            {
        //                var firstDayOfMonth = new DateTime(t.CreatedDate.Year, t.CreatedDate.Month, 1);
        //                var weekNumber = ((t.CreatedDate.Day - 1) / 7) + 1;
        //                //var weekNumber = ((t.StartTime?.Day - 1) / 7) + 1;


        //                return weekNumber;
        //            })
        //            .Select(weekGroup =>
        //            {
        //                var totalWeeklyMinutes = weekGroup
        //                    .Where(t => !string.IsNullOrEmpty(t.Hours))
        //                    .Sum(t => TryParseHoursToMinutes(t.Hours));

        //                var totalWeeklyHours = totalWeeklyMinutes / 60;
        //                var totalWeeklyRemainingMinutes = totalWeeklyMinutes % 60;

        //                var entries = weekGroup.Select(t => new TimeSheetEntryViewModel
        //                {
        //                    Date = t.StartTime != null ? t.StartTime : t.EndTime,
        //                    Day = t.StartTime != null ? t.StartTime.Value.DayOfWeek.ToString() : t.EndTime?.DayOfWeek.ToString(),
        //                    ClockIn = t.StartTime,
        //                    ClockOut = t.EndTime,
        //                    Duration = t.Hours ?? "00:00"
        //                }).ToList();

        //                return new TimeSheetWeekViewModel
        //                {
        //                    WeekNumber = $"Week {weekGroup.Key}",
        //                    TotalWeeklyHours = $"{totalWeeklyHours:D2}:{totalWeeklyRemainingMinutes:D2}",
        //                    Entries = entries
        //                };
        //            }).ToList();

        //            return new TimeSheetMonthViewModel
        //            {
        //                Month = monthName,
        //                TotalMonthlyHours = $"{totalMonthlyHours:D2}:{totalMonthlyRemainingMinutes:D2}",
        //                Weeks = weeks
        //            };
        //        }).ToList();

        //    return new TimeSheetResponseViewModel
        //    {
        //        TimeSheetSummary = timeSheetSummary
        //    };

        //    int TryParseHoursToMinutes(string hours)
        //    {
        //        if (string.IsNullOrEmpty(hours)) return 0;

        //        var parts = hours.Split(':').Select(p => int.TryParse(p.Trim(), out var result) ? result : 0).ToArray();

        //        return parts.Length == 2
        //            ? (parts[0] * 60) + parts[1]
        //            : 0;
        //    }
        //}








        ///// <summary>
        ///// get TimeSheet details List
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        public TimeSheetResponseViewModel GetTimeSheetListAPP(string loginUserId, TimeSheetAppRequestViewModel model)
        {
            // Step 1: Get all timesheet data first
            var allTimeSheets = _timeSheetRepository.GetTimeSheetListAPP(loginUserId, model.Month, model.Year)
                .Select(a => Mapper.MapTimeSheetListToTimeSheetViewModelList(a))?.ToList();

            List<TimeSheetListSearchViewModel> timesheets = new();
            var lastClockInByUser = new Dictionary<string, DateTime>();

            // Step 2: Process and store all records
            foreach (var evt in allTimeSheets)
            {
                if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In)
                {
                    lastClockInByUser[evt.CreatedBy] = evt.CreatedDate;
                }
                else if (evt.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)
                {
                    if (lastClockInByUser.TryGetValue(evt.CreatedBy, out var clockIn))
                    {
                        var clockOut = evt.CreatedDate;
                        TimeSpan duration = clockOut - clockIn;
                        string formattedDuration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}";

                        timesheets.Add(new TimeSheetListSearchViewModel
                        {
                            CreatedDate = clockOut.Date,
                            CreatedBy = evt.CreatedBy,
                            Date = clockOut.ToString("yyyy-MM-dd"),
                            CreatedDay = clockOut.DayOfWeek.ToString(),
                            CreatedByName = _timeSheetRepository.GetCreatedByName(evt.CreatedBy),
                            StartTime = clockIn,
                            EndTime = clockOut,
                            Hours = formattedDuration
                        });

                        lastClockInByUser.Remove(evt.CreatedBy);
                    }
                }
            }

            // Step 3: Filter and group data at the end
            var groupedWeeks = timesheets.GroupBy(t => ((t.StartTime?.Day - 1) / 7) + 1)
                .Select(weekGroup => new TimeSheetWeekViewModel
                {
                    WeekNumber = $"Week {weekGroup.Key}",
                    TotalWeeklyHours = CalculateTotalHours(weekGroup),
                    Entries = weekGroup.Select(t => new TimeSheetEntryViewModel
                    {
                        Date = t.StartTime ?? t.EndTime,
                        Day = (t.StartTime ?? t.EndTime)?.DayOfWeek.ToString(),
                        ClockIn = t.StartTime,
                        ClockOut = t.EndTime,
                        Duration = t.Hours ?? "00:00"
                    }).ToList()
                }).ToList();

            return new TimeSheetResponseViewModel
            {
                TimeSheetSummary = new List<TimeSheetMonthViewModel>
                {
                    new TimeSheetMonthViewModel
                    {
                        Month = $"{new DateTime(model.Year ?? DateTime.Now.Year, model.Month ?? 1, 1).ToString("MMMM yyyy")}",
                        TotalMonthlyHours = CalculateTotalHours(timesheets),
                        Weeks = groupedWeeks // Filter applied here
                    }
                }
            };
        }

        private string CalculateTotalHours(IEnumerable<TimeSheetListSearchViewModel> timesheets)
        {
            var totalMinutes = timesheets.Where(t => !string.IsNullOrEmpty(t.Hours))
                .Sum(t => TryParseHoursToMinutes(t.Hours));

            return $"{totalMinutes / 60:D2}:{totalMinutes % 60:D2}";
        }

        private int TryParseHoursToMinutes(string hours)
        {
            if (string.IsNullOrEmpty(hours)) return 0;

            var parts = hours.Split(':').Select(p => int.TryParse(p.Trim(), out var result) ? result : 0).ToArray();
            return parts.Length == 2 ? (parts[0] * 60) + parts[1] : 0;
        }



    }
}
