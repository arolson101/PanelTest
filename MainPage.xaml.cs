using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PanelTest
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page, INotifyPropertyChanged
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private bool _CallInvalidateMeasure = false;
		public bool CallInvalidateMeasure
		{
			get => _CallInvalidateMeasure;
			set
			{
				_CallInvalidateMeasure = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallInvalidateMeasure)));
			}
		}

		private double _ChildHeight = 100;
		public double ChildHeight
		{
			get => _ChildHeight;
			set
			{
				_ChildHeight = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChildHeight)));
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ChildHeight += 10;

			if (CallInvalidateMeasure)
			{
				YogaPanel.InvalidateMeasure();
			}
		}
	}
}
