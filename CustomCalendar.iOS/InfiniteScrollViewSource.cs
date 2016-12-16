using System;
using SkiaSharp;
using CoreGraphics;
using UIKit;
using System.Linq;
using System.Collections.Generic;
using Foundation;

namespace CustomCalendar.iOS
{
	public class InfiniteScrollViewSource : UICollectionViewSource
	{
		const int ItemCount = 3;

		CGPoint _currentOffset;

		IInfiniteScrollViewDelegate _del;

		void InitializeCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell cell, NSIndexPath indexPath)
		{
			var i = 0;
			if (indexPath.Row == 0)
			{
				i = -1;
			}
			else if (indexPath.Row == 2)
			{
				i = 1;
			}

			_del.InitializeCell(infiniteScrollView, cell, this.CurrentIndex + i);
		}

		void UpdateCell(InfiniteScrollView infiniteScrollView, InfiniteScrollViewCell cell, NSIndexPath indexPath)
		{
			var i = 0;
			if (indexPath.Row == 0)
			{
				i = -1;
			}
			else if (indexPath.Row == 2)
			{
				i = 1;
			}

			_del.UpdateCell(infiniteScrollView, cell, this.CurrentIndex + i);
		}

		void RefreshVisibleCells(InfiniteScrollView infiniteScrollView)
		{
			var visibleCells = infiniteScrollView.VisibleCells;

			foreach (var visibleCell in visibleCells)
			{
				var cell = visibleCell as InfiniteScrollViewCell;

				var indexPath = infiniteScrollView.IndexPathForCell(cell);

				UpdateCell(infiniteScrollView, cell, indexPath);
			}
		}

		public InfiniteScrollViewSource(IInfiniteScrollViewDelegate del)
		{
			_del = del;
		}

		public int CurrentIndex { get; private set; }

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(InfiniteScrollViewCell.Key, indexPath) as InfiniteScrollViewCell;
			var infiniteScrollView = collectionView as InfiniteScrollView;

			if (!cell.IsInitialized)
			{
				InitializeCell(infiniteScrollView, cell, indexPath);
				cell.IsInitialized = true;
			}

			UpdateCell(infiniteScrollView, cell, indexPath);

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
					this.CurrentIndex += 1;
					scrollView.ContentOffset = new CGPoint(scrollView.ContentOffset.X - (collectionView.Bounds.Width), 0);
					shouldRefreshVisibleCells = true;
				}

			}
			else if (_currentOffset.X > contentOffset.X)
			{
				// left

				if (visibleIndexPath.Row == 0)
				{
					this.CurrentIndex -= 1;
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
				RefreshVisibleCells(collectionView as InfiniteScrollView);
			}
		}

		public override void DecelerationEnded(UIScrollView scrollView)
		{
			_currentOffset = scrollView.ContentOffset;
		}
	}
}
