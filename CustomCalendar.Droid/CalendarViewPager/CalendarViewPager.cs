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

	public class CalendarViewPager : Android.Support.V4.View.ViewPager
	{
		class OnPageChangeListener : Java.Lang.Object, Android.Support.V4.View.ViewPager.IOnPageChangeListener
		{
			WeakReference<CalendarViewPager> _weakPager;

			bool _isInitialized;
			float _previousScrollX;

			public OnPageChangeListener(CalendarViewPager pager)
			{
				_weakPager = new WeakReference<CalendarViewPager>(pager);
			}

			public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
			{
				CalendarViewPager pager = null;
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

					if (pager.IsDragging)
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
				CalendarViewPager pager = null;
				if (_weakPager.TryGetTarget(out pager))
				{
					pager.IsDragging = state == 1;
				}
			}

			public void OnPageSelected(int position)
			{
			}
		}

		void UpdateSelectedDates(IEnumerable<DateTime> dateTimes)
		{
			if (dateTimes.Count() == 1)
			{
				this.DateSelected?.Invoke(dateTimes.ElementAt(0));
			}
			Item0.ControlDelegate.HighlightedDates = dateTimes;
			Item1.ControlDelegate.HighlightedDates = dateTimes;
			Item2.ControlDelegate.HighlightedDates = dateTimes;
			Item0.Invalidate();
			Item1.Invalidate();
			Item2.Invalidate();
		}

		void SetMonth(DateTime dateTime)
		{
			var monthDate = dateTime.Date;
			NextMonth = monthDate.AddMonths(1);
			CurrentMonth = monthDate;
			PreviousMonth = monthDate.AddMonths(-1);

			Item0.ControlDelegate.Date = PreviousMonth;
			Item1.ControlDelegate.Date = CurrentMonth;
			Item2.ControlDelegate.Date = NextMonth;
		}

		bool IsDragging { get; set; }

		DateTime NextMonth { get; set; }

		DateTime CurrentMonth { get; set; }

		DateTime PreviousMonth { get; set; }

		public CalendarViewPager(Android.Content.Context context) : base(context)
		{
			this.Adapter = new CalendarPageAdapter(this);

			this.AddOnPageChangeListener(new OnPageChangeListener(this));

			Item0 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item1 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());
			Item2 = new DrawableControlView<CalendarMonthControl>(context, new CalendarMonthControl());

			Item1.ControlDelegate.DatesInteracted += UpdateSelectedDates;

			SetMonth(DateTime.Now);
		}

		public event Action<DateTime> DateSelected;

		public DrawableControlView<CalendarMonthControl> Item0 { get; set; }

		public DrawableControlView<CalendarMonthControl> Item1 { get; set; }

		public DrawableControlView<CalendarMonthControl> Item2 { get; set; }

		DateTime? _selectedDate;
		public DateTime? SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			set
			{
				if (value.HasValue)
				{
					var dt = value.Value;
					_selectedDate = value.Value.Date;
					UpdateSelectedDates(new DateTime[] { dt });
				}
				else
				{
					_selectedDate = null;
					UpdateSelectedDates(new DateTime[] { });
				}
			}
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

			if (e.Action == MotionEventActions.Up && !IsDragging)
			{
				var x = e.GetX();
				var y = e.GetY();
				var points = new SKPoint[] { new SKPoint(x, y) };
				Item1.ControlDelegate.EndInteractions(points);
			}

			return base.DispatchTouchEvent(e);
		}
	}
	
}
