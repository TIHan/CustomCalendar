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
	[Activity(Label = "CustomCalendar.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		public void AddPager(LinearLayout linearLayout)
		{
			var pager = new CalendarViewPager(this);

			pager.DateSelected += (obj) => Console.WriteLine(obj);

			linearLayout.AddView(pager);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			layoutParams.SetMargins(16, 16, 16, 16);

			var linearLayout = new LinearLayout(this);
			linearLayout.Orientation = Orientation.Horizontal;
			linearLayout.LayoutParameters = layoutParams;

			this.AddPager(linearLayout);

			SetContentView(linearLayout);

		}
	}
}

