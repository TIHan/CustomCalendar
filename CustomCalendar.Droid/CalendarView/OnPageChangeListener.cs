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

	public class OnPageChangeListener : Java.Lang.Object, Android.Support.V4.View.ViewPager.IOnPageChangeListener
	{
		WeakReference<CustomViewPager> _weakPager;
		bool _isDragging;
		bool _isInitialized;
		float _previousScrollX;

		public OnPageChangeListener(CustomViewPager pager)
		{
			_weakPager = new WeakReference<CustomViewPager>(pager);
		}

		public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
			CustomViewPager pager = null;
			if (_weakPager.TryGetTarget(out pager))
			{
				var shouldUpdate = false;

				if (pager.ScrollX == 0 && !_isInitialized)
				{
					_isInitialized = true;
					pager.SetCurrentItem(1, false);
					pager.ScrollX = pager.Width;
					shouldUpdate = true;
				}

				if (_isDragging)
				{
					if (pager.ScrollX > pager.Width && pager.ScrollX > _previousScrollX)
					{
						pager.SetMonth(pager.NextMonth);
						pager.ScrollX = pager.ScrollX - pager.Width; // right

						shouldUpdate = true;
					}
					else if (pager.ScrollX < pager.Width && pager.ScrollX < _previousScrollX)
					{
						pager.SetMonth(pager.PreviousMonth);
						pager.ScrollX = pager.ScrollX + pager.Width; // left

						shouldUpdate = true;
					}
				}

				_previousScrollX = pager.ScrollX;

				if (shouldUpdate)
				{
					pager.Item0.Invalidate();
					pager.Item1.Invalidate();
					pager.Item2.Invalidate();
				}
			}
		}

		public void OnPageScrollStateChanged(int state)
		{
			_isDragging = state == 1;
		}

		public void OnPageSelected(int position)
		{
			Console.WriteLine(position);
		}
	}
	
}
