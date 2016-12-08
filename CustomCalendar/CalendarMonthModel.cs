using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace CustomCalendar
{
	public class CalendarMonthModel
	{
		public int Year { get; private set; }

		public int Month { get; private set; }

		public IEnumerable<CalendarDayModel> Days { get; private set; }

		public int GridSize { get; private set; }

		public IEnumerable<int> HighlightedDays { get; private set; }

		CalendarMonthModel(int year, int month, IEnumerable<CalendarDayModel> calendarDays, int gridSize, IEnumerable<int> highlightedDays)
		{
			Year = year;
			Month = month;
			Days = calendarDays;
			GridSize = gridSize;
			HighlightedDays = highlightedDays;
		}

		public CalendarDayModel TryFindCalendarDayByPoint(SKPoint point)
		{
			return this.Days.FirstOrDefault(day => day.Rectangle.Contains(point));
		}

		public static CalendarMonthModel Create(int year, int month, IEnumerable<int> highlightedDays, int width, int height)
		{
			var monthDate = new DateTime(year, month, 1);

			var calendarDays = new List<CalendarDayModel>();

			var rows = 6;
			var columns = 7;
			var date = monthDate;

			var gridSize = 0;
			if (width > height)
			{
				gridSize = height / columns;
			}
			else
			{
				gridSize = width / columns;
			}

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					var canCreateDate = (i > 0 || int.Parse(date.DayOfWeek.ToString("D")) <= j) && month == date.Month;
					if (!canCreateDate)
						continue;

					var x = (float)((gridSize * j));

					var y = gridSize + (float)((gridSize * i)) + gridSize;

					calendarDays.Add(new CalendarDayModel(date.Year, date.Month, date.Day, x, y, gridSize, gridSize));

					date = date.AddDays(1);
				}
			}

			return new CalendarMonthModel(year, month, calendarDays, gridSize, highlightedDays);
		}
	}
}
