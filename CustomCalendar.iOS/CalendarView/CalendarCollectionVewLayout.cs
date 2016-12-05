using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar
{
	public class CalendarCollectionViewLayout : UICollectionViewFlowLayout
	{
		public CalendarCollectionViewLayout(CGSize itemSize) : base()
		{
			this.ItemSize = itemSize;
			this.MinimumLineSpacing = 0;
			this.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
		}
	}
}
