using System;
namespace CustomCalendar.iOS
{
	public interface ICalendarViewDelegate
	{
		void OnDateSelected(DateTime dt);
	}
}
