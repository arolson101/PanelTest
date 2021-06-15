using System;
using System.Windows;
using Facebook.Yoga;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PanelTest
{
	public interface IYogaNode
	{
		YogaNode YogaNode { get; }
	}

	public class YogaPanel : Panel, IYogaNode
	{
		private readonly YogaNode Node = new YogaNode
		{
			JustifyContent = YogaJustify.FlexStart,
			AlignItems = YogaAlign.Baseline,
			FlexDirection = YogaFlexDirection.Column,
		};
#if DEBUG
		private string LayoutString = string.Empty;
#endif

		public static readonly DependencyProperty YogaFlexProperty = DependencyProperty.RegisterAttached(
			"YogaFlex",
			typeof(double),
			typeof(YogaPanel),
			new PropertyMetadata(double.NaN)
		);

		public static void SetYogaFlex(UIElement element, double value)
		{
			element.SetValue(YogaFlexProperty, value);
		}
		public static double GetYogaFlex(UIElement element)
		{
			return (double)element.GetValue(YogaFlexProperty);
		}

		public YogaNode YogaNode => Node;

		public YogaPanel()
			: base()
		{
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Node.Clear();

			Size infinite = new Size(double.PositiveInfinity, double.PositiveInfinity);

			foreach (FrameworkElement child in Children)
			{
				child.Measure(infinite);

				//if (child is IYogaNode childyoga)
				//{
				//	Node.AddChild(childyoga.YogaNode);
				//}
				//else
				{
					var childNode = new YogaNode
					{
						Width = (float)child.DesiredSize.Width,
						Height = (float)child.DesiredSize.Height
					};

					childNode.Flex = GetYogaFlex(child).ToYogaFloat();

					Node.AddChild(childNode);
				}
			}

			Node.Width = availableSize.Width.ToYogaFloat();
			Node.Height = availableSize.Height.ToYogaFloat();
			Node.CalculateLayout();
#if DEBUG
			LayoutString = Node.Print();
#endif

			// call measure with the correct size; this fixes child ListView having incorrect size
			for (var i = 0; i < Children.Count; i++)
			{
				var child = Children[i];
				var childNode = Node[i];

				child.Measure(childNode.GetSize());
			}

			//var str = Node.Print();
			return Node.GetSize();
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			Debug.Assert(Children.Count == Node.Count);

			Node.CalculateLayout();
#if DEBUG
			LayoutString = Node.Print();
#endif

			for (var i = 0; i < Children.Count; i++)
			{
				var child = Children[i];
				var childNode = Node[i];

				var location = new Point(childNode.LayoutX, childNode.LayoutY);
				var size = new Size(childNode.LayoutWidth, childNode.LayoutHeight);
				child.Arrange(new Rect(location, size));
			}

			return finalSize;
		}
	}

	public static partial class Extensions
	{
		public static Size GetSize(this YogaNode node)
		{
			var size = new Size(node.LayoutWidth, node.LayoutHeight);
			if (float.IsInfinity(node.Width.Value))
				size.Width = node.Count > 0 ? node.Max(child => child.LayoutX + child.LayoutWidth) : 0;
			if (float.IsInfinity(node.Height.Value))
				size.Height = node.Count > 0 ? node.Max(child => child.LayoutY + child.LayoutHeight) : 0;

			Debug.Assert(!double.IsNaN(size.Width) && !double.IsInfinity(size.Width));
			Debug.Assert(!double.IsNaN(size.Height) && !double.IsInfinity(size.Height));
			return size;
		}

		public static float ToYogaFloat(this double value)
		{
			if (double.IsInfinity(value))
				return YogaConstants.Undefined;
			else
				return (float)value;
		}
	}
}
