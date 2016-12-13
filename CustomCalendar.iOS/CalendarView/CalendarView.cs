using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS
{
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
