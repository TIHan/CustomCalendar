using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CustomCalendar.iOS
{
	public class CalendarView2 : UIView, IInfiniteScrollViewDelegate
	{
		public CalendarView2(CGRect frame) : base(frame)
		{
			var infiniteScrollView = new InfiniteScrollView(this, frame);

			this.AddSubview(infiniteScrollView);
		}

		public event Action<DateTime> DateSelected;

		public void InitializeCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell infiniteScrollViewCell, int index)
		{
			var view = new DrawableControlView<CalendarMonthControl>(new CalendarMonthControl());
			infiniteScrollViewCell.Add(view);
			view.Frame = infiniteScrollViewCell.Bounds;
			infiniteScrollViewCell.BackgroundColor = UIColor.White;
		}

		public void UpdateCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell infiniteScrollViewCell, int index)
		{

		}
	}

	public class CalendarView : UIView, ICalendarViewDelegate
	{
		WeakReference<CalendarCollectionView> _weakCollectionView;

		public event Action<DateTime> DateSelected;

		public CalendarView(CGRect frame) : base(frame)
		{
			var collectionView = new CalendarCollectionView(this, frame);

			_weakCollectionView = new WeakReference<CalendarCollectionView>(collectionView);

			this.AddSubview(collectionView);
		}

		public DateTime? SelectedDate
		{
			get
			{
				CalendarCollectionView collectionView;
				if (_weakCollectionView.TryGetTarget(out collectionView))
				{
					var source = collectionView.Source as CalendarCollectionViewSource;
					return source.SelectedDate;
				}
				return null;
			}
			set
			{
				CalendarCollectionView collectionView;
				if (_weakCollectionView.TryGetTarget(out collectionView))
				{
					var source = collectionView.Source as CalendarCollectionViewSource;
					source.UpdateSelectedDate(collectionView, value);
				}
			}
		}

		public void OnDateSelected(DateTime dt)
		{
			DateSelected?.Invoke(dt);
		}
	}
}
