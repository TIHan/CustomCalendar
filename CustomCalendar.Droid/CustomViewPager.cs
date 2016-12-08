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

			//Item0.SetBackgroundColor(Android.Graphics.Color.Red);
			//Item1.SetBackgroundColor(Android.Graphics.Color.Blue);
			//Item2.SetBackgroundColor(Android.Graphics.Color.Yellow);
		}

		public View Item0 { get; private set; }

		public View Item1 { get; private set; }

		public View Item2 { get; private set; }

	}
	
}
