using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BacteriaNetworks.Infrastructure
{
	public class AutoCompleteComboBox : ComboBox
	{
		private bool CanUpdate { get; set; } = true;
		private bool NeedUpdate { get; set; } = false;
		private Timer Timer { get; set; } = new Timer();

		public List<string> Data { get; set; }

		public AutoCompleteComboBox()
		{
			TextChanged += OnTextChanged;
			SelectedIndexChanged += OnSelectedIndexChanged;
			TextUpdate += AutoCompleteComboBox_TextUpdate;

			Timer.Tick += TimerOnTick;
		}

		private void TimerOnTick(object sender, EventArgs e)
		{
			CanUpdate = true;
			Timer.Stop();
			UpdateData();
		}

		private void AutoCompleteComboBox_TextUpdate(object sender, EventArgs e)
		{
			NeedUpdate = true;
		}

		private void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			NeedUpdate = false;
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			if (!NeedUpdate) return;

			if (CanUpdate)
			{
				CanUpdate = false;
				UpdateData();
			}
			else
			{
				RestartTimer();
			}
		}

		private void UpdateData()
		{
			var searchData = Text.Length > 0 && Data != null ? Data.Where(x => x.Contains(Text)).ToArray() : new string[]{};

			HandleTextChange(searchData);
		}

		private void HandleTextChange(string[] searchData)
		{
			if (searchData.Length > 0)
			{
				var txt = Text;

				Items.Clear();
				Items.Add(txt);
				Items.AddRange(searchData);

				DroppedDown = true;

				Items.Clear();
				Items.AddRange(searchData);

				SelectionStart = txt.Length;
			}
			else
			{
				DroppedDown = false;
				SelectionStart = Text.Length;
			}
		}

		private void RestartTimer()
		{
			Timer.Stop();
			CanUpdate = false;
			Timer.Start();
		}
	}
}
