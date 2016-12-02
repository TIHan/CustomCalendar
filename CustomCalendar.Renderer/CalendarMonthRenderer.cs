using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace CustomCalendar.Renderer
{
	public static class CalendarMonthRenderer
	{
		static void DrawText(SKCanvas canvas, SKPaint paint, string text, float x, float y, int fontSize)
		{
			paint.TextSize = fontSize;
			paint.IsAntialias = true;
			paint.Color = SKColors.Black;
			paint.IsStroke = false;
			paint.TextAlign = SKTextAlign.Center;

			canvas.DrawText(text, x, y, paint);
		}

		static void DrawRectangleOutline(SKCanvas canvas, SKPaint paint, SKPath path, float x, float y, float width, float height)
		{
			var lineWidth = 2;

			paint.IsAntialias = false;
			paint.StrokeCap = SKStrokeCap.Square;
			paint.Style = SKPaintStyle.Stroke;
			paint.StrokeWidth = lineWidth;
			paint.Color = SKColors.LightGray;

			path.MoveTo(x, y);
			path.LineTo(x, y + height);
			path.LineTo(x + width, y + height);
			path.LineTo(x + width, y);
			path.LineTo(x, y);

			canvas.DrawPath(path, paint);
			path.Reset();
		}

		static void DrawRectangle(SKCanvas canvas, SKPaint paint, float x, float y, float width, float height)
		{
			var rect = new SKRect(x, y, x + width, y + height);

			paint.IsAntialias = true;
			paint.StrokeCap = SKStrokeCap.Square;
			paint.Style = SKPaintStyle.Fill;
			paint.StrokeWidth = 0;
			paint.Color = SKColors.Red;

			canvas.DrawRect(rect, paint);
		}

		static void DrawCalendarDay(SKCanvas canvas, SKPaint textPaint, SKPaint paint, SKPath path, CalendarDayModel calendarDay)
		{
			var rect = calendarDay.Rectangle;
			var date = calendarDay.DateTime;

			var x = rect.Left;
			var y = rect.Top;
			var width = rect.Width;
			var height = rect.Height;

			DrawRectangleOutline(canvas, paint, path, x, y, width, height);

			if (calendarDay.IsHighlighted)
				DrawRectangle(canvas, paint, x, y, width, height);

			DrawText(canvas, textPaint,
			         text: date.Day.ToString(), 
			         x: x + (width / 2), y: y + (height / 2) + (height / 8), 
			         fontSize: (int)(width / 3));
		}

		public static void Draw(SKSurface surface, SKImageInfo info, CalendarMonthModel calendarMonth)
		{
			var canvas = surface.Canvas;

			canvas.Clear(SKColors.White);

			var gridSize = calendarMonth.GridSize;
			var dateTime = new DateTime(calendarMonth.Year, calendarMonth.Month, 1);
			var date = dateTime.AddDays(-dateTime.Day + 1);

			using (var textPaint = new SKPaint())
			{
				using (var paint = new SKPaint())
				{
					using (var path = new SKPath())
					{
						DrawText(canvas, textPaint, text: date.ToString("Y"), x: info.Width / 2, y: gridSize, fontSize: gridSize / 4);

						foreach (var calendarDay in calendarMonth.Days)
						{
							foreach (var highlightDay in calendarMonth.HighlightedDays)
							{
								if (highlightDay == calendarDay.DateTime.Day)
								{
									calendarDay.IsHighlighted = true;
								}
							}
	
							DrawCalendarDay(canvas, textPaint, paint, path, calendarDay);
						}
					}
				}
			}

			canvas.Flush();
		}
	}
}
