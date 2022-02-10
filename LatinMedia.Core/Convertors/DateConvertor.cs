using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using MD.PersianDateTime.Core;

namespace LatinMedia.Core.Convertors
{
   public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar PC = new PersianCalendar();
            return PC.GetYear(value).ToString() + "/" + PC.GetMonth(value).ToString("00") + "/" + PC.GetDayOfMonth(value).ToString("00");

        }
        public static string ToShamsiForEdit(this DateTime value)
        {
            PersianCalendar PC = new PersianCalendar();
            return PC.GetMonth(value).ToString() + "/" + PC.GetDayOfMonth(value).ToString("00") + "/" + PC.GetYear(value);

        }

        public static string ConvertMiladiToShamsi(this DateTime date, string Format)
        {
            PersianDateTime persianDateTime = new PersianDateTime(date);
            return persianDateTime.ToString(Format);
        }

    }
}
