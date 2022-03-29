using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace UI.Dates
{
    public static class DatePickerUtilities
    {
        public static string DateFormat = "yyyy-MM-dd";

        public static string ToDateString(this SerializableDate date)
        {
            return date.HasValue ? date.Date.ToDateString() : null;
        }
        /// <summary>
        /// 将日期转换为"yyyy-MM-dd"字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date)
        {
            return date.ToString(DateFormat);
        }

        public static string[] GetAbbreviatedDayNames()
        {            
            return Shift(DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames, (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
        }

        public static List<DateTime> GetDateRangeForDisplay(DateTime date)
        {
            var list = new List<DateTime>();

            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);            
            var firstDayOfWeekInCulture = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

            var day = firstDayOfMonth;
            while (day.DayOfWeek != firstDayOfWeekInCulture)
            {
                day = day.AddDays(-1);
            }            

            for (var x = 0; x < 42; x++)
            {
                list.Add(day);

                day = day.AddDays(1);
            }

            return list;
        }

        public static bool DateFallsWithinMonth(DateTime date, DateTime month)
        {
            // We cache the results of this check because it will be performed often
            var key = date.ToDateString() + "|" + month.ToDateString();
            if (!DatePickerCache._DateFallsWithinMonthResults.ContainsKey(key))
            {
                var firstDay = new DateTime(month.Year, month.Month, 1);
                var lastDay = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month)).AddDays(1).AddTicks(-1);

                DatePickerCache._DateFallsWithinMonthResults.Add(key, date.CompareTo(firstDay) >= 0 && date.CompareTo(lastDay) <= 0);
            }

            return DatePickerCache._DateFallsWithinMonthResults[key];                        
        }

        internal static T[] Shift<T>(T[] array, int positions)
        {
            T[] copy = new T[array.Length];
            Array.Copy(array, 0, copy, array.Length - positions, positions);
            Array.Copy(array, positions, copy, 0, array.Length - positions);
            return copy;
        }        
    }  
}
