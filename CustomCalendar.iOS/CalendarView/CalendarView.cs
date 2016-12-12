using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS
{
	public class CalendarView : UIView, ICalendarViewDelegate
	{
		public event Action<DateTime> DateSelected;

		public CalendarView(CGRect frame) : base(frame)
		{
			var collectionView = new CalendarCollectionView(this, frame);

			this.AddSubview(collectionView);
		}

		public void OnDateSelected(DateTime dt)
		{
			DateSelected?.Invoke(dt);
		}
	}
}
