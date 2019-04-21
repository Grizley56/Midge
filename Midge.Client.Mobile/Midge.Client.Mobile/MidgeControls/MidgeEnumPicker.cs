using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Midge.Client.Mobile.MidgeControls
{
	public class MidgeEnumPicker<T> : Picker where T : struct
	{
		public MidgeEnumPicker()
		{
			SelectedIndexChanged += OnSelectedIndexChanged;
			//Fill the Items from the enum
			foreach (var value in Enum.GetValues(typeof(T)))
			{
				Items.Add(value.ToString());
			}
		}

		public new static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(T), typeof(MidgeEnumPicker<T>), default(T), propertyChanged: OnSelectedItemChanged, defaultBindingMode: BindingMode.TwoWay);

		public new T SelectedItem
		{
			get => (T)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = default(T);
			}
			else
			{
				SelectedItem = (T)Enum.Parse(typeof(T), Items[SelectedIndex]);
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as MidgeEnumPicker<T>;
			if (newvalue != null)
			{
				picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
			}
		}
	}
}
