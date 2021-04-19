using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using IDPA_Steganographie.Interfaces;

namespace IDPA_Steganographie.GUI.MenuBar
{
	/// <summary>
	/// Interaction logic for MenuControl.xaml
	/// </summary>
	public partial class MenuControl : INotifyPropertyChanged
	{
        //Sagt dem Menu ob es horizontal oder vertikal sein soll
        public Orientation MenuOrientation
	    {
	        get
	        {
	            return _MenuOrientation;
	        }
	        set
	        {
	            _MenuOrientation = value;
	            _OnPropertyChanged("MenuOrientation");
	        }
	    }

	    private Orientation _MenuOrientation;

        public MenuControl()
        {
            InitializeComponent();
            
            //Verbindet die Orientation-Property mit dem GUI
            Binding menuOrientationBinding = new Binding("MenuOrientation")
            {
                Source = this
            };
            BindingOperations.SetBinding(LayoutRoot, StackPanel.OrientationProperty, menuOrientationBinding);

            MenuOrientation = Orientation.Vertical;
            Background = (Brush)FindResource("BackgroundBrush");
        }

        //Fügt ein Item dem Menu hinzu
	    public void AddMenuItem(IMenuItem menuItem)
        {
            menuItem.ItemClicked += menuItem_ItemClicked;
            LayoutRoot.Children.Add((UIElement)menuItem);
        }

        //Markiert das angeklickte MenuItem
        void menuItem_ItemClicked(object sender, EventArgs e)
        {
            var mi = sender as IMenuItem;
            _SelectItem(mi);
        }

        //Markiert das angeklickte MenuItem
        private void _SelectItem(IMenuItem mi)
        {
            foreach (IMenuItem item in LayoutRoot.Children)
            {
                if (!item.Equals(mi))
                    item.IsSelected = false;
            }
            if (mi != null)
                mi.IsSelected = true;
        }

        //Markiert das erste MenuItem in der Liste
        public void SelectFirst()
        {
            if(LayoutRoot.Children.Count == 0)
                return;

            _SelectItem((IMenuItem) LayoutRoot.Children[0]);
        }

        //Löscht alle MenuItems
	    public void ClearMenuItems()
	    {
	        LayoutRoot.Children.Clear();
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

        //Teilt dem Gui mit wenn sich ein Property Wert geändert hat
	    protected virtual void _OnPropertyChanged(string propertyName)
	    {
	        var handler = PropertyChanged;
	        if(handler != null)
	            handler(this, new PropertyChangedEventArgs(propertyName));
	    }

        //Gibt die Anzahl MenuItems zurück
        public int Count { get { return LayoutRoot.Children.Count; } }
	}
}