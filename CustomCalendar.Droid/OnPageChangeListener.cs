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

		public OnPageChangeListener(CustomViewPager pager)
		{
			_weakPager = new WeakReference<CustomViewPager>(pager);
		}

		public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
			CustomViewPager pager = null;
			if (_weakPager.TryGetTarget(out pager))
			{
				if (pager.ScrollX == 0)
					pager.SetCurrentItem(1, false);
				
				if (pager.ScrollX > pager.Width + (pager.Width / 2))
				{
					pager.ScrollX = pager.ScrollX - pager.Width;
				}
				else if (pager.ScrollX < (pager.Width / 2))
				{
					pager.ScrollX = pager.ScrollX + pager.Width;
				}
			}
		}

		public void OnPageScrollStateChanged(int state)
		{
		}

		public void OnPageSelected(int position)
		{
			CustomViewPager pager = null;
			if (_weakPager.TryGetTarget(out pager))
			{
				pager.SetCurrentItem(1, true);
			}
		}
	}
	
}
