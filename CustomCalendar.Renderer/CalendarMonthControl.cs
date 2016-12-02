using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace CustomCalendar.Renderer
{
	public class CalendarMonthControl : IDrawableControlDelegate
	{
		CalendarMonthModel Model { get; set; }

		IEnumerable<DateTime> _highlightedDates;
		public IEnumerable<DateTime> HighlightedDates
		{
			get
			{
				return _highlightedDates;
			}
			set
			{
				_highlightedDates = value.Select(x => x.Date);
			}
		}

		DateTime _date;
		public DateTime Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value.Date;
			}
		}

		bool TryFindDateByPoint(SKPoint point, out DateTime dateTime)
		{
			var calendarDay = Model.TryFindCalendarDayByPoint(point);
			if (calendarDay != null)
			{
				dateTime = calendarDay.DateTime;
				return true;
			}
			else
			{
				dateTime = new DateTime();
				return false;
			}
		}

		public event Action<IEnumerable<DateTime>> DatesInteracted;

		public void Draw(SKSurface surface, SKImageInfo info)
		{
			var highlightedDays = this.HighlightedDates.TakeWhile(date =>
			{
				if (date.Year == Date.Year && date.Month == Date.Month)
				{
					return true;
				}
				return false;
			}).Select(date => date.Day);

			Model = CalendarMonthModel.Create(this.Date.Year, this.Date.Month, highlightedDays, info.Width, info.Height);
			CalendarMonthRenderer.Draw(surface, info, Model);
		}

		public void EndInteractions(IEnumerable<SKPoint> points)
		{
			if (points.Count() == 1)
			{
				var dateTime = new DateTime();
				if (TryFindDateByPoint(points.ElementAt(0), out dateTime))
				{
					DatesInteracted?.Invoke(new DateTime[] { dateTime });
				}
			}
		}

		public CalendarMonthControl()
		{
			Date = DateTime.Now;
			HighlightedDates = new DateTime[] { };
			Model = CalendarMonthModel.Create(this.Date.Year, this.Date.Month, new int[] { }, 0, 0);
		}
	}
}
