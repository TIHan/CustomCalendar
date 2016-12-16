using System;
using CoreGraphics;
using UIKit;

namespace CustomCalendar.iOS.Sample
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			// TODO: Need to be able to select range.
			var calendarView = new CalendarView(this.View.Bounds);

			calendarView.SelectedDate = DateTime.Now;
			calendarView.DateSelected += dt => Console.WriteLine(dt);

			this.Add(calendarView);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
