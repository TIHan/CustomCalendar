using Android.App;
using Android.Widget;
using Android.OS;
using SkiaSharp.Views.Android;
using Android.Views;
using SkiaSharp;
using CustomCalendar.Renderer;
using System;
using System.Linq;

namespace CustomCalendar.Droid
{
	public class DrawableControlView<T> : SKCanvasView where T : IDrawableControlDelegate
	{
		readonly T _controlDelegate;

		public T ControlDelegate
		{
			get
			{
				return _controlDelegate;
			}
		}

		public DrawableControlView(Android.Content.Context context, T controlDelegate) : base(context)
		{
			_controlDelegate = controlDelegate;
		}

		protected override void OnDraw(SKSurface surface, SKImageInfo info)
		{
			base.OnDraw(surface, info);
			_controlDelegate.Draw(surface, info);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			var x = e.GetX();
			var y = e.GetY();
			var points = new SKPoint[] { new SKPoint(x, y) };

			if (e.Action == MotionEventActions.Up)
			{
				_controlDelegate.EndInteractions(points);
			}

			this.Invalidate();
			return true;
		}
	}

	[Activity(Label = "CustomCalendar.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var controlDelegate = new CalendarMonthControl();

			controlDelegate.DatesInteracted += dates =>
			{
				var date = dates.ElementAt(0);

				controlDelegate.HighlightedDates = new DateTime[] { date };
			};

			var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			this.AddContentView(new DrawableControlView<CalendarMonthControl>(this.BaseContext, controlDelegate),layout);
		}
	}
}

