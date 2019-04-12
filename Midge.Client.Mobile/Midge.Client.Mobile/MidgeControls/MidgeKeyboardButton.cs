using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Midge.Client.Mobile.MidgeControls
{
	public class MidgeKeyboardButton : Button
	{
		public static readonly BindableProperty VirtualCodeProperty =
			BindableProperty.Create(nameof(VirtualCode), typeof(int), typeof(Button), default(int));

		public static readonly BindableProperty DoubleClickIntervalProperty =
			BindableProperty.Create(nameof(DoubleClickInterval), typeof(int), typeof(MidgeKeyboardButton), default(int));

		public static readonly BindableProperty TapTimeProperty =
			BindableProperty.Create(nameof(TapTime), typeof(int), typeof(MidgeKeyboardButton), 500);

		public int TapTime
		{
			get => (int) GetValue(TapTimeProperty);
			set => SetValue(TapTimeProperty, value);
		}

		public int DoubleClickInterval
		{
			get => (int)GetValue(DoubleClickIntervalProperty);
			set => SetValue(DoubleClickIntervalProperty, value);
		}

		public int VirtualCode
		{
			get => (int)GetValue(VirtualCodeProperty);
			set => SetValue(VirtualCodeProperty, value);
		}

		public event EventHandler Tapped;
		public event EventHandler KeyPressed;
		public event EventHandler KeyReleased;

		public MidgeKeyboardButton()
		{
			Released += MidgeKeyboardButtonReleased;
			Pressed += MidgeKeyboardButtonPressed;
		}

		private void MidgeKeyboardButtonReleased(object sender, EventArgs e)
		{
			lock (_lock)
			{
				var item = _pressedQueue.Dequeue();

				if (DateTime.Now - item.Time <= TimeSpan.FromMilliseconds(TapTime))
				{
					item.IsPressing = false;
					Tapped?.Invoke(this, EventArgs.Empty);
				}
				else
					KeyReleased?.Invoke(this, EventArgs.Empty);
			}
		}

		private readonly object _lock = new object();
		private readonly Queue<TapItem> _pressedQueue = new Queue<TapItem>();

		private void MidgeKeyboardButtonPressed(object sender, EventArgs e)
		{
			var item = new TapItem() { Time = DateTime.Now, IsPressing = true };

			lock (_lock)
				_pressedQueue.Enqueue(item);

			Device.StartTimer(TimeSpan.FromMilliseconds(TapTime), () =>
			{
				lock (_lock)
				{
					if (!item.IsPressing)
						return false;

					KeyPressed?.Invoke(this, EventArgs.Empty);
					return false;
				}
			});
		}

		private class TapItem
		{
			public DateTime Time { get; set; }
			public bool IsPressing { get; set; }
		}
	}
}
