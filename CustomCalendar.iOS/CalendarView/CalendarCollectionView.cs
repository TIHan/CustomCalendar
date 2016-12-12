using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS
{
	public class CalendarCollectionView : UICollectionView
	{
		public CalendarCollectionView(ICalendarViewDelegate del, CGRect frame) : base(frame, new CalendarCollectionViewLayout(frame.Size))
		{
			this.RegisterClassForCell(typeof(CalendarCollectionViewCell), CalendarCollectionViewCell.Key);
			this.ShowsVerticalScrollIndicator = false;
			this.ShowsHorizontalScrollIndicator = false;
			this.PagingEnabled = true;

			var source = new CalendarCollectionViewSource(del);

			this.Source = source;

			this.ScrollToItem(Foundation.NSIndexPath.FromItemSection(1, 0),
							  UICollectionViewScrollPosition.None, false);
			source.DecelerationEnded(this);
		}

		public Foundation.NSIndexPath TryGetVisibleIndexPath()
		{
			var visibleRect = new CGRect(this.ContentOffset, this.Bounds.Size);

			var visiblePoint = new CGPoint(visibleRect.GetMidX(), visibleRect.GetMidY());
			var visibleIndexPath = this.IndexPathForItemAtPoint(visiblePoint);

			return visibleIndexPath;
		}
	}
}
