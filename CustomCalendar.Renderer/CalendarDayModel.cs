using System;
using System.Collections.Generic;
using SkiaSharp;

namespace CustomCalendar.Renderer
{
	public class CalendarDayModel
	{
		public DateTime DateTime { get; private set; }

		public SKRect Rectangle { get; private set; }

		public bool IsHighlighted { get; internal set; }

		internal CalendarDayModel(int year, int month, int day, float x, float y, float width, float height)
		{
			DateTime = new DateTime(year, month, day);
			IsHighlighted = false;
			Rectangle = new SKRect(x, y, x + width, y + height);
		}
	}
}
