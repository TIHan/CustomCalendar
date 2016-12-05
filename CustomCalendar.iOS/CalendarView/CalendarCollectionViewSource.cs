using System;
using SkiaSharp;
using CoreGraphics;
using UIKit;
using System.Linq;
using System.Collections.Generic;

namespace CustomCalendar
{
	public class CalendarCollectionViewSource : UICollectionViewSource
	{
		const int ItemCount = 3;

		DateTime _nextMonth;
		DateTime _currentMonth;
		DateTime _previousMonth;

		CGPoint _currentOffset;

		public CalendarCollectionViewSource() : base()
		{
			SetMonth(DateTime.Now);
		}

		DateTime _selectedDate;
		public DateTime SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			set
			{
				_selectedDate = value.Date;
			}
		}

		void SetMonth(DateTime dateTime)
		{
			var monthDate = dateTime.Date;
			_nextMonth = monthDate.AddMonths(1);
			_currentMonth = monthDate;
			_previousMonth = monthDate.AddMonths(-1);
		}

		void UpdateSelectedDates(CalendarCollectionViewCell cell)
		{
			cell.ControlDelegate.HighlightedDates = new DateTime[] { SelectedDate };
			cell.SetNeedsDisplay();
		}

		void RefreshVisibleCells(UICollectionView collectionView)
		{
			if (collectionView == null)
				return;

			var visibleCells = collectionView.VisibleCells;

			foreach (var visibleCell in visibleCells)
			{
				var cell = visibleCell as CalendarCollectionViewCell;
				var indexPath = collectionView.IndexPathForCell(cell);

				switch (indexPath.Row)
				{
					case 0:
						{
							cell.ControlDelegate.Date = _previousMonth;
							break;
						}
					case 1:
						{
							cell.ControlDelegate.Date = _currentMonth;
							break;
						}
					case 2:
						{
							cell.ControlDelegate.Date = _nextMonth;
							break;
						}
					default:
						{
							break;
						}
				}

				UpdateSelectedDates(cell);
				cell.SetNeedsDisplay();
			}
		}

		static void HandleDatesIteracted(IEnumerable<DateTime> dates, WeakReference<CalendarCollectionViewSource> weakSource, WeakReference<CalendarCollectionViewCell> weakCell)
		{
			CalendarCollectionViewSource source = null;
			CalendarCollectionViewCell cell = null;

			if (weakSource.TryGetTarget(out source) && weakCell.TryGetTarget(out cell))
			{
				source.SelectedDate = dates.ElementAt(0);
				source.UpdateSelectedDates(cell);
				cell.SetNeedsDisplay();
			}
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(CalendarCollectionViewCell.Key, indexPath) as CalendarCollectionViewCell;

			if (!cell.IsInitialized)
			{
				var weakSource = new WeakReference<CalendarCollectionViewSource>(this);
				var weakCell = new WeakReference<CalendarCollectionViewCell>(cell);
				cell.ControlDelegate.DatesInteracted += dates =>
				{
					HandleDatesIteracted(dates, weakSource, weakCell);
				};
				cell.IsInitialized = true;
			}

			if (indexPath.Row == 0)
			{
				cell.ControlDelegate.Date = _previousMonth;
			}
			else if (indexPath.Row == 2)
			{
				cell.ControlDelegate.Date = _nextMonth;
			}
			else
			{
				cell.ControlDelegate.Date = _currentMonth;
			}

			UpdateSelectedDates(cell);

			return cell;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return ItemCount;
		}

		public override void Scrolled(UIScrollView scrollView)
		{
			var collectionView = scrollView as UICollectionView;
			if (collectionView == null)
				return;

			var contentOffset = scrollView.ContentOffset;

			var visibleRect = new CGRect(collectionView.ContentOffset, collectionView.Bounds.Size);

			var visiblePoint = new CGPoint(visibleRect.GetMidX(), visibleRect.GetMidY());
			var visibleIndexPath = collectionView.IndexPathForItemAtPoint(visiblePoint);

			if (visibleIndexPath == null)
				return;

			var shouldRefreshVisibleCells = false;

			if (_currentOffset.X < contentOffset.X)
			{
				// went down

				if (visibleIndexPath.Row == 2)
				{
					SetMonth(_currentMonth.AddMonths(1));
					scrollView.ContentOffset = new CGPoint(scrollView.ContentOffset.X - (collectionView.Bounds.Width), 0);
					shouldRefreshVisibleCells = true;

				}

			}
			else if (_currentOffset.X > contentOffset.X)
			{
				// went up

				if (visibleIndexPath.Row == 0)
				{
					
					SetMonth(_currentMonth.AddMonths(-1));
					scrollView.ContentOffset = new CGPoint(scrollView.ContentOffset.X + (collectionView.Bounds.Width), 0);
					shouldRefreshVisibleCells = true;
				}
			}

			if (collectionView.VisibleCells.Length > 1)
			{
				shouldRefreshVisibleCells = true;
			}

			if (shouldRefreshVisibleCells)
			{
				RefreshVisibleCells(collectionView);
			}
		}

		public override void DecelerationEnded(UIScrollView scrollView)
		{
			_currentOffset = scrollView.ContentOffset;
		}
	}
}
