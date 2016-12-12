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

	public class CustomPagerAdapter : Android.Support.V4.View.PagerAdapter
	{
		WeakReference<CustomViewPager> _weakPager;

		public CustomPagerAdapter(CustomViewPager pager)
		{
			_weakPager = new WeakReference<CustomViewPager>(pager);
		}

		public override int Count
		{
			get
			{
				return 3;
			}
		}

		public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
		{
			CustomViewPager pager = null;
			if (_weakPager.TryGetTarget(out pager))
			{
				var color = Android.Graphics.Color.Red;
				if (position == 1)
				{
					color = Android.Graphics.Color.Blue;
				}
				else if (position == 2)
				{
					color = Android.Graphics.Color.Gold;
				}

				View view;
				switch (position)
				{
					case 0:
						{
							view = pager.Item0;
							break;
						}
					case 1:
						{
							view = pager.Item1;
							break;
						}
					default:
						{
							view = pager.Item2;
							break;
						}
				}

				container.RemoveView(view);
				container.AddView(view);
				return view;	
			}

			return null;
		}

		public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
		{
		}

		public override bool IsViewFromObject(Android.Views.View view, Java.Lang.Object objectValue)
		{
			return view == objectValue;
		}
	}
	
}
