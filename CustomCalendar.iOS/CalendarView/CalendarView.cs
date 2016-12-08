using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS
{
	public class CalendarView : UIView
	{
		public CalendarView(CGRect frame) : base(frame)
		{
			var collectionView = new CalendarCollectionView(frame);

			this.AddSubview(collectionView);
		}
	}
}
