using System;
using Foundation;

namespace CustomCalendar.iOS
{
	public interface IInfiniteScrollViewDelegate
	{
		void InitializeCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell infiniteScrollViewCell, int index);
		void UpdateCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell infiniteScrollViewCell, int index);
	}
}
