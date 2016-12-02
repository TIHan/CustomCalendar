using Android.App;
using Android.Widget;
using Android.OS;
using SkiaSharp.Views.Android;
using Android.Views;
using SkiaSharp;
using CustomCalendar.Renderer;
using System;

namespace CustomCalendar.Droid
{
	public class SkiaView : SkiaSharp.Views.Android.SKCanvasView
	{
		public SkiaView(Android.Content.Context context) : base(context)
		{
		}

		protected override void OnDraw(SKSurface surface, SKImageInfo info)
		{
			var dateTime = DateTime.Now;
			var model = CalendarMonthRenderModel.Create(dateTime.Year, dateTime.Month, 0, info);
			CalendarMonthRenderer.Draw(surface, info, model);
		}
	}

	[Activity(Label = "CustomCalendar.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			this.AddContentView(new SkiaView(this.BaseContext),layout);
		}
	}
}

