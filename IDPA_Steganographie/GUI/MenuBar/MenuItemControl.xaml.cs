using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using IDPA_Steganographie.Interfaces;

namespace IDPA_Steganographie.GUI.MenuBar
{
	/// <summary>
	/// Interaction logic for MenuItemControl.xaml
	/// </summary>
	public partial class MenuItemControl : IMenuItem
	{
	    private readonly Action _ClickAction;
	    private bool _IsSelected;

        public event EventHandler ItemClicked;
	    public void MouseEnterAction(object sender, MouseEventArgs mouseEventArgs)
	    {
            if (!_IsSelected)
                _itemBorderTemp.Visibility = Visibility.Visible;
	    }

	    public void MouseLeaveAction(object sender, MouseEventArgs mouseEventArgs)
	    {
            _itemBorderTemp.Visibility = Visibility.Collapsed;
	    }

	    public void LeftMouseButtonUpAction(object sender, MouseEventArgs mouseEventArgs)
	    {
            if (ItemClicked != null)
                ItemClicked(this, null);
            _ClickAction();
	    }

	    public MenuItemControl()
	    {
	        InitializeComponent();
	        
            Loaded += MenuItemControl_Loaded;
            Unloaded += MenuItemControl_Unloaded;
	    }

        public MenuItemControl(string title, Action clickAction)
            : this()
        {
            _itemText.Text = title;
            _ClickAction = clickAction;
        }

	    public MenuItemControl(string title, Action clickAction, ImageSource path)
	        : this(title, clickAction)
	    {
	        Icon.Source = path;
	    }

	    private void MenuItemControl_Unloaded(object sender, RoutedEventArgs e)
	    {
            MouseEnter -= MouseEnterAction;
            MouseLeave -= MouseLeaveAction;
            MouseLeftButtonUp -= LeftMouseButtonUpAction;
	    }

	    private void MenuItemControl_Loaded(object sender, RoutedEventArgs e)
	    {
	        MouseEnter += MouseEnterAction;
	        MouseLeave += MouseLeaveAction;
	        MouseLeftButtonUp += LeftMouseButtonUpAction;
	    }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                _itemBorderTemp.Visibility = Visibility.Collapsed;
                _itemBorderActive.Visibility = _IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }
	}
}