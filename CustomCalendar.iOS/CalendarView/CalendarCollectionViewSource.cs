using System;
using SkiaSharp;
using CoreGraphics;
using UIKit;
using System.Linq;
using System.Collections.Generic;

namespace CustomCalendar.iOS
{
	public class CalendarCollectionViewSource : UICollectionViewSource
	{
		const int ItemCount = 3;

		DateTime _nextMonth;
		DateTime _currentMonth;
		DateTime _previousMonth;

		CGPoint _currentOffset;

		WeakReference<ICalendarViewDelegate> _weakCalendarViewDelegate;

		void UpdateSelectedDates(CalendarCollectionViewCell cell)
		{
			if (this.SelectedDate.HasValue)
			{
				cell.ControlDelegate.HighlightedDates = new DateTime[] { this.SelectedDate.Value };
			}
			else
			{
				cell.ControlDelegate.HighlightedDates = new DateTime[] { };
			}
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

		void HandleDatesIteracted(IEnumerable<DateTime> dates, WeakReference<CalendarCollectionViewCell> weakCell)
		{
			CalendarCollectionViewCell cell = null;

			if (weakCell.TryGetTarget(out cell))
			{
				this.SelectedDate = dates.ElementAt(0);
				this.UpdateSelectedDates(cell);
				cell.SetNeedsDisplay();

				ICalendarViewDelegate del = null;
				if (_weakCalendarViewDelegate.TryGetTarget(out del))
				{
					if (this.SelectedDate.HasValue)
					{
						del.OnDateSelected(this.SelectedDate.Value);
					}
				}
			}
		}

		void SetMonth(DateTime dateTime)
		{
			var monthDate = dateTime.Date;
			_nextMonth = monthDate.AddMonths(1);
			_currentMonth = monthDate;
			_previousMonth = monthDate.AddMonths(-1);
		}

		public CalendarCollectionViewSource(ICalendarViewDelegate del) : base()
		{
			SetMonth(DateTime.Now);
			_weakCalendarViewDelegate = new WeakReference<ICalendarViewDelegate>(del);
		}

		DateTime? _selectedDate;
		public DateTime? SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			private set
			{
				if (value.HasValue)
				{
					var dt = value.Value;
					_selectedDate = value.Value.Date;
				}
				else
				{
					_selectedDate = null;
				}
			}
		}

		public void UpdateSelectedDate(UICollectionView collectionView, DateTime? dateTime)
		{
			SelectedDate = dateTime;
			RefreshVisibleCells(collectionView);
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(CalendarCollectionViewCell.Key, indexPath) as CalendarCollectionViewCell;

			if (!cell.IsInitialized)
			{
				var weakCell = new WeakReference<CalendarCollectionViewCell>(cell);
				cell.ControlDelegate.DatesInteracted += dates =>
				{
					HandleDatesIteracted(dates, weakCell);
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
				// right

				if (visibleIndexPath.Row == 2)
				{
					SetMonth(_currentMonth.AddMonths(1));
					scrollView.ContentOffset = new CGPoint(scrollView.ContentOffset.X - (collectionView.Bounds.Width), 0);
					shouldRefreshVisibleCells = true;

				}

			}
			else if (_currentOffset.X > contentOffset.X)
			{
				// left

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
