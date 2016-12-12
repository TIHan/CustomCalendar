using Android.App;
using Android.Widget;
using Android.OS;
using SkiaSharp.Views.Android;
using Android.Views;
using SkiaSharp;
using System;
using System.Linq;
using Java.Lang;

namespace CustomCalendar.Droid
{

	public class CustomViewPager : Android.Support.V4.View.ViewPager
	{
		public CustomViewPager(Android.Content.Context context) : base(context)
		{
			this.AddOnPageChangeListener(new OnPageChangeListener(this));

			Item0 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item1 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item2 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			SetMonth(DateTime.Now);
		}

		public DrawableControlView<CalendarMonthControl> Item0 { get; private set; }

		public DrawableControlView<CalendarMonthControl> Item1 { get; private set; }

		public DrawableControlView<CalendarMonthControl> Item2 { get; private set; }

		public DateTime NextMonth { get; private set; }

		public DateTime CurrentMonth { get; private set; }

		public DateTime PreviousMonth { get; private set; }

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
			if (e.Action == MotionEventActions.Down)
			{
				var pager = this;
				if (pager.CurrentItem == 0)
				{
					//pager.ScrollX = pager.Width;
					pager.SetCurrentItem(1, false);
					pager.SetMonth(pager.PreviousMonth);
					this.Invalidate();
				}
				else if (pager.CurrentItem == 2)
				{
					//pager.ScrollX = pager.Width * 2;
					pager.SetCurrentItem(1, false);
					pager.SetMonth(pager.NextMonth);
					this.Invalidate();
				}
			}
			return base.DispatchTouchEvent(e);
		}
	}
	
}
