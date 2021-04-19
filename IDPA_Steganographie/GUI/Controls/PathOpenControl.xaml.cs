using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using IDPA_Steganographie.HelperClasses;
using IWin32Window = System.Windows.Forms.IWin32Window;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace IDPA_Steganographie.GUI.Controls
{
    public enum PathOpenStyle
    {
        SaveFileDialog = 0,
        OpenFileDialog = 1,
        FolderDialog = 2
    }
    /// <summary>
    /// Creates a graphical element that allows you to open or save files
    /// </summary>
    public partial class PathOpenControl : UserControl, INotifyPropertyChanged
    {
        public delegate void FileNameArg(string fileName, PathOpenStyle pathOpenStyle);

        public event FileNameArg FileNameChanged;
        public PathOpenStyle PathOpenStyle { get; set; }

        public ICommand DialogOpenerCommand
        {
            get
            {
                return new DelegateCommand(() => _OpenDialog());
            }
        }

        public ICommand ClearTextCommand
        {
            get
            {
                return new DelegateCommand(() =>
                                           {
                                               Text = "";
                                               _OnFileNameChanged("");
                                           }, () => !string.IsNullOrEmpty(Text));
            }
        }

        #region InitialDirectory Property
        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(
              "InitialDirectory", typeof(string), typeof(PathOpenControl),
              new PropertyMetadata(null, OnInitialDirectoryPropertyChanged));

        [Category("Common Properties"), Description("Some description goes here ...")]
        public string InitialDirectory
        {
            get { return (string)this.GetValue(InitialDirectoryProperty); }
            set
            {
                this.SetValue(InitialDirectoryProperty, value);
                _OnPropertyChanged();
            }
        }

        private static void OnInitialDirectoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateInitialDirectoryProperty(d as PathOpenControl, (string)e.NewValue);
        }

        private static void UpdateInitialDirectoryProperty(PathOpenControl control, string newvalue)
        {
            if (control == null) return;
            if (!control.IsInitialized) return;
        }
        #endregion


        #region Text Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
              "Text", typeof(string), typeof(PathOpenControl),
              new PropertyMetadata(null, OnTextPropertyChanged));

        [Category("Common Properties"), Description("Some description goes here ...")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                _OnPropertyChanged();
            }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextProperty(d as PathOpenControl, (string)e.NewValue);
        }

        private static void UpdateTextProperty(PathOpenControl control, string newvalue)
        {
            if (control == null) return;
            if (!control.IsInitialized) return;
        }
        #endregion

        /// <summary>
        /// By hovering the textbox with the mouse this tooltip appears
        /// </summary>
        public object TextBoxToolTip
        {
            get { return PathBox.ToolTip; }
            set { PathBox.ToolTip = value; }
        }

        public bool EnableDeleteCross
        {
            get
            {
                return DeleteButton.Visibility == Visibility.Visible;
            }
            set
            {
                DeleteButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public PathOpenControl()
        {
            InitializeComponent();
            PathOpenStyle = 0;
            EnableDeleteCross = true;
        }

        public PathOpenControl(PathOpenStyle pathOpenStyle)
            : this()
        {
            PathOpenStyle = pathOpenStyle;
            InitialDirectory = string.Empty;
        }

        private void _OpenDialog()
        {
            if (PathOpenStyle == PathOpenStyle.SaveFileDialog)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PNG Image (*.png)|*.png|BMP Image (*.bmp)|*.bmp|JPG Image (*.jpg)|*.jpg",
                    InitialDirectory = InitialDirectory
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    Text = saveFileDialog.FileName;
                    DeleteButton.IsEnabled = true;
                    _OnFileNameChanged(saveFileDialog.FileName);
                }
            }
            else if (PathOpenStyle == PathOpenStyle.OpenFileDialog)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg|All files (*.*)|*.*",
                    InitialDirectory = InitialDirectory
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    Text = openFileDialog.FileName;
                    DeleteButton.IsEnabled = true;
                    _OnFileNameChanged(openFileDialog.FileName);
                }
            }
            else if (PathOpenStyle == PathOpenStyle.FolderDialog)
            {
                var dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog(this.GetIWin32Window()) == DialogResult.OK)
                {
                    Text = dlg.SelectedPath;
                    DeleteButton.IsEnabled = true;
                    _OnFileNameChanged(Text);
                }
            }
        }

        protected virtual void _OnFileNameChanged(string filename)
        {
            if (FileNameChanged != null)
                FileNameChanged(filename, PathOpenStyle);
        }

        private void _PathBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(PathBox.Text))
            {
                _OpenExplorer(PathBox.Text);
            }
        }

        private static void _OpenExplorer(string path)
        {
            var directoryInfo = Directory.GetParent(path);
            if (directoryInfo != null)
                Process.Start("explorer.exe", directoryInfo.FullName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void _OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class WpfExtensions
    {
        public static IWin32Window GetIWin32Window(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as HwndSource;
            IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : IWin32Window
        {
            private readonly IntPtr _handle;
            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            IntPtr IWin32Window.Handle
            {
                get { return _handle; }
            }
        }
    }
}
