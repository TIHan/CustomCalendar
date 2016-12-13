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
	}
	
}
