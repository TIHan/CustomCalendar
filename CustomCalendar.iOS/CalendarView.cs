using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CustomCalendar.iOS
{
	public class CalendarView : UIView
	{
		class CalendarViewDelegate : IInfiniteScrollViewDelegate<CalendarViewCell>
		{
			WeakReference<CalendarView> _weakView;

			void SelectDate(CalendarViewCell cell, DateTime? date)
			{
				if (date != null)
				{
					cell.ControlDelegate.HighlightedDates = new DateTime[] { date.Value };
				}
				else
				{
					cell.ControlDelegate.HighlightedDates = new DateTime[] { };
				}
			}

			public CalendarViewDelegate(WeakReference<CalendarView> weakView)
			{
				_weakView = weakView;
			}

			public void InitializeCell(InfiniteScrollView<CalendarViewCell> infiniteScrollView, CalendarViewCell cell, int index)
			{
				cell.ControlDelegate.DatesInteracted += dateTimes =>
				{
					CalendarView v;
					if (_weakView.TryGetTarget(out v))
					{
						var date = dateTimes.ElementAt(0);
						cell.ControlDelegate.HighlightedDates = new DateTime[] { date };
						v.DateSelected?.Invoke(date);
						v.SelectedDate = date;
						cell.SetNeedsDisplay();
					}
				};

				var weakCell = new WeakReference<CalendarViewCell>(cell);

				CalendarView view;
				if (_weakView.TryGetTarget(out view))
				{
					view.SelectedDateChanged += dateTime =>
					{
						CalendarViewCell c;
						if (weakCell.TryGetTarget(out c))
						{
							SelectDate(c, view.SelectedDate);
						}
					};
				}

			}

			public void UpdateCell(InfiniteScrollView<CalendarViewCell> infiniteScrollView, CalendarViewCell cell, int index)
			{
				CalendarView view;
				if (_weakView.TryGetTarget(out view))
				{
					if (infiniteScrollView.CurrentIndex < index) // right
					{
						cell.ControlDelegate.Date = view.Month.AddMonths(1);
					}
					else if (infiniteScrollView.CurrentIndex == index) // middle
					{
						cell.ControlDelegate.Date = view.Month;
					}
					else // left
					{
						cell.ControlDelegate.Date = view.Month.AddMonths(-1);
					}

					SelectDate(cell, view.SelectedDate);
					cell.SetNeedsDisplay();
				}
			}

			public void OnCurrentIndexChanged(InfiniteScrollView<CalendarViewCell> infiniteScrollView, int currentIndex)
			{
				CalendarView view;
				if (_weakView.TryGetTarget(out view))
				{
					if (view.CurrentIndex > infiniteScrollView.CurrentIndex) // left
					{
						view.Month = view.Month.AddMonths(-1);
					}
					else if (view.CurrentIndex < infiniteScrollView.CurrentIndex) // right
					{
						view.Month = view.Month.AddMonths(1);
					}

					view.CurrentIndex = infiniteScrollView.CurrentIndex;
				}
			}
		}

		DateTime Month { get; set; }

		public CalendarView(CGRect frame) : base(frame)
		{
			var del = new CalendarViewDelegate(new WeakReference<CalendarView>(this));
			var infiniteScrollView = new InfiniteScrollView<CalendarViewCell>(del, frame);

			this.AddSubview(infiniteScrollView);

			Month = DateTime.Now.Date; // default starting month is the current month
		}

		public event Action<DateTime> DateSelected;

		internal event Action<DateTime?> SelectedDateChanged;

		internal int CurrentIndex { get; set; }

		DateTime? _selectedDate;
		public DateTime? SelectedDate
		{
			get
			{
				return _selectedDate;
			}
			set
			{
				_selectedDate = value;
				SelectedDateChanged?.Invoke(value);
			}
		}
	}
}
