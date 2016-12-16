using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS
{
	public class InfiniteScrollView : UICollectionView
	{
		public InfiniteScrollView(IInfiniteScrollViewDelegate del, CGRect frame) : base(frame, new InfiniteScrollViewLayout(frame.Size))
		{
			this.RegisterClassForCell(typeof(InfiniteScrollViewCell), InfiniteScrollViewCell.Key);
			this.ShowsVerticalScrollIndicator = false;
			this.ShowsHorizontalScrollIndicator = false;
			this.PagingEnabled = true;

			this.Source = new InfiniteScrollViewSource(del);

			this.ScrollToItem(Foundation.NSIndexPath.FromItemSection(1, 0),
							  UICollectionViewScrollPosition.None, false);
			this.Source.DecelerationEnded(this);
		}

		public Foundation.NSIndexPath TryGetVisibleIndexPath()
		{
			var visibleRect = new CGRect(this.ContentOffset, this.Bounds.Size);

			var visiblePoint = new CGPoint(visibleRect.GetMidX(), visibleRect.GetMidY());
			var visibleIndexPath = this.IndexPathForItemAtPoint(visiblePoint);

			return visibleIndexPath;
		}

		public int CurrentIndex
		{
			get
			{
				return (this.Source as InfiniteScrollViewSource).CurrentIndex;
			}
		}
	}
}
