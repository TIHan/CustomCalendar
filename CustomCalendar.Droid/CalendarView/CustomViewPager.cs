using Android.App;
using Android.Widget;
using Android.OS;
using SkiaSharp.Views.Android;
using Android.Views;
using SkiaSharp;
using System;
using System.Linq;
using Java.Lang;
using System.Collections.Generic;

namespace CustomCalendar.Droid
{

	public class CustomViewPager : Android.Support.V4.View.ViewPager
	{
		void HandleDatesInteracted(IEnumerable<DateTime> dateTimes)
		{
			Item0.ControlDelegate.HighlightedDates = dateTimes;
			Item1.ControlDelegate.HighlightedDates = dateTimes;
			Item2.ControlDelegate.HighlightedDates = dateTimes;
			Item0.Invalidate();
			Item1.Invalidate();
			Item2.Invalidate();
		}

		public CustomViewPager(Android.Content.Context context) : base(context)
		{
			this.AddOnPageChangeListener(new OnPageChangeListener(this));

			Item0 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item1 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item2 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());

			Item0.ControlDelegate.DatesInteracted += HandleDatesInteracted;

			Item1.ControlDelegate.DatesInteracted += HandleDatesInteracted;

			Item2.ControlDelegate.DatesInteracted += HandleDatesInteracted;

			SetMonth(DateTime.Now);
		}

		public DrawableControlView<CalendarMonthControl> Item0 { get; private set; }

		public DrawableControlView<CalendarMonthControl> Item1 { get; private set; }

		public DrawableControlView<CalendarMonthControl> Item2 { get; private set; }

		public DateTime NextMonth { get; private set; }

		public DateTime CurrentMonth { get; private set; }

		public DateTime PreviousMonth { get; private set; }

		DateTime? _selectedDate;
		public DateTime? SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			private set
			{
				if (value.HasValue)
				{
					var dt = value.Value;
					_selectedDate = value.Value.Date;
				}
				else
				{
					_selectedDate = null;
				}
			}
		}

		public void UpdateSelectedDate(DateTime? dateTime)
		{
			SelectedDate = dateTime;
		}

		public void SetMonth(DateTime dateTime)
		{
			var monthDate = dateTime.Date;
			NextMonth = monthDate.AddMonths(1);
			CurrentMonth = monthDate;
			PreviousMonth = monthDate.AddMonths(-1);

			Item0.ControlDelegate.Date = PreviousMonth;
			Item1.ControlDelegate.Date = CurrentMonth;
			Item2.ControlDelegate.Date = NextMonth;
		}

		public override bool DispatchTouchEvent(MotionEvent e)
		{
			// We need to reset the current item when the user is about to start dragging.
			if (e.Action == MotionEventActions.Down)
			{
				var pager = this;
				if (pager.CurrentItem == 0)
				{
					pager.SetCurrentItem(1, false);
					pager.SetMonth(pager.PreviousMonth);
					this.Invalidate();
				}
				else if (pager.CurrentItem == 2)
				{
					pager.SetCurrentItem(1, false);
					pager.SetMonth(pager.NextMonth);
					this.Invalidate();
				}
			}

			if (e.Action == MotionEventActions.Up)
			{
				var x = e.GetX();
				var y = e.GetY();
				var points = new SKPoint[] { new SKPoint(x, y) };
				Item0.ControlDelegate.EndInteractions(points);
				Item1.ControlDelegate.EndInteractions(points);
				Item2.ControlDelegate.EndInteractions(points);
			}

			return base.DispatchTouchEvent(e);
		}
	}
	
}
