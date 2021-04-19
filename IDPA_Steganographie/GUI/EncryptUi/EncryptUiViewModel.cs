using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using IDPA_Steganographie.BaseClasses;
using IDPA_Steganographie.GUI.Controls;
using IDPA_Steganographie.HelperClasses;
using IDPA_Steganographie.HelperClasses.ErrorHelper;
using IDPA_Steganographie.HelperClasses.WaitDialogHelper;
using Microsoft.Win32;
using Steganographie.Logic;
using Steganographie.Logic.Helper;
using Steganographie.Logic.Image;

namespace IDPA_Steganographie.GUI.EncryptUi
{
    /// <summary>
    /// Interface to communicate with the steganographie library
    /// </summary>
    public class EncryptUiViewModel : ViewModelBase
    {
        private string _OpenFilePath;
        private string _EncryptText;
        private string _Seed = "0";
        private bool _IsSeedValid = true;
        private int _SelectedItem;
        private int _CharsToGo;
        private int _TotalCharsToGo;
        private ImageSource _ImageSource;

        public int TotalCharsToGo
        {
            get
            {
                return _TotalCharsToGo;
            }
            set
            {
                _TotalCharsToGo = value;
                _OnPropertyChanged();
            }
        }

        public ObservableCollection<int> BitsItems { get; set; }

        public int CharsToGo
        {
            get
            {
                return _CharsToGo;
            }
            set
            {
                _CharsToGo = value;
                _OnPropertyChanged();
            }
        }

        public bool IsSeedValid
        {
            get { return _IsSeedValid; }
            set
            {
                _IsSeedValid = value;
                _OnPropertyChanged();
            }
        }

        public string Seed
        {
            get { return _Seed; }
            set
            {
                int result;
                IsSeedValid = int.TryParse(value, out result);
                _Seed = value;
                _OnPropertyChanged();
            }
        }

        /// <summary>
        /// Path from were the picture gets loaded
        /// </summary>
        public string OpenFilePath
        {
            get
            {
                return _OpenFilePath;
            }
            set
            {
                _OpenFilePath = value;
                _ImageSource = !string.IsNullOrEmpty(OpenFilePath) && InputOutput.CheckPathIsValidImage(value) ? new BitmapImage(new Uri(value)) : new BitmapImage();
                if (InputOutput.CheckPathIsValidImage(value))
                {
                    TotalCharsToGo = GetCharsToGo(ImageSource.Width, ImageSource.Height, SelectedItem, RuntimeGlobals.Settings.UseAlphaChannel);
                    CharsToGo = TotalCharsToGo;
                }
                else if (value.Equals(string.Empty))
                {
                    CharsToGo = 0;
                    TotalCharsToGo = 0;
                }
                else
                {
                    ErrorHelper.Add(4);
                }
                _OnPropertyChanged();
                _OnPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// Returns an image for a tooltip
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return _ImageSource;
            }
        }

        /// <summary>
        /// The text which gets hidden in the picture
        /// </summary>
        public string EncryptText
        {
            get { return _EncryptText; }
            set
            {
                _EncryptText = value;
                if (!string.IsNullOrEmpty(OpenFilePath) && InputOutput.CheckPathIsValidImage(OpenFilePath))
                {
                    CharsToGo = TotalCharsToGo - value.Count();
                    CharsToGo = CharsToGo < 0 ? 0 : CharsToGo;
                }
                _OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                SelectedItem = BitsItems[value];
                TotalCharsToGo = !string.IsNullOrEmpty(OpenFilePath) ? GetCharsToGo(ImageSource.Width, ImageSource.Height, SelectedItem, !OpenFilePath.ToLower().EndsWith(".jpg")) : 0;
                CharsToGo = TotalCharsToGo;
                _OnPropertyChanged();
            }
        }

        public int SelectedItem { get; set; }

        public ICommand EncryptCommand
        {
            get
            {
                return new DelegateCommand(async () => await Encrypt(), () => !string.IsNullOrEmpty(EncryptText) && !string.IsNullOrEmpty(OpenFilePath) && IsSeedValid);
            }
        }

        public EncryptUiViewModel()
        {
            BitsItems = new ObservableCollection<int>()
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8
            };
            SelectedIndex = 1;
        }

        /// <summary>
        /// Encrypts a text to a picture and saves it 
        /// </summary>
        public async Task Encrypt()
        {
            await Encrypt(OpenFilePath, EncryptText, int.Parse(Seed), SelectedItem);
        }

        public int GetCharsToGo(double width, double height, int bits, bool useAlpha)
        {
            return Interface.GetMaxCharCount((int)width, (int)height, ChannelUsage.Default);
        }

        /// <summary>
        /// Encrypts a text to a picture and saves it
        /// </summary>
        /// <param name="openFilePath">Path from were the picture gets loaded</param>
        /// <param name="encryptText">The text which gets hidden in the picture</param>
        /// <param name="seed"></param>
        /// <param name="bits"></param>
        public async Task Encrypt(string openFilePath, string encryptText, int seed, int bits)
        {
            await WaitDialogHelper.ExecuteThreadAsync("Encrypt Message", () =>
                                                                         {
                                                                             string saveFilePath = RuntimeGlobals.ShowSaveFileDialog("PNG Image (*.png)|*.png", RuntimeGlobals.Settings.SavePath);

                                                                             if (string.IsNullOrEmpty(saveFilePath))
                                                                                 return;
                                                                             if (!InputOutput.CheckPathIsValidImage(saveFilePath))
                                                                                 throw new FileFormatException();
                                                                             if (saveFilePath.Equals(openFilePath))
                                                                                 throw new FileLoadException();

                                                                             var image = Interface.Encrypt((BitmapImage)ImageSource, ChannelUsage.Default, seed, encryptText);
                                                                             InputOutput.SaveImageToFile(saveFilePath, image);
                                                                             GC.Collect();

                                                                         }, exception =>
                                                                            {
                                                                                if (exception != null)
                                                                                {
                                                                                    if (exception is OutOfMemoryException)
                                                                                        ErrorHelper.Add(6, exception.Message, exception.StackTrace);
                                                                                    else if (exception is FileFormatException)
                                                                                        ErrorHelper.Add(4);
                                                                                    else if (exception is FileLoadException)
                                                                                        ErrorHelper.Add(5);
                                                                                    else
                                                                                        ErrorHelper.Add(3, exception.GetType() + "\n" + exception.Message, exception.StackTrace);
                                                                                }
                                                                            });
        }
    }
}