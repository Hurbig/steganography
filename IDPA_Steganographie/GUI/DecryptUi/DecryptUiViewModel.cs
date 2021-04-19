using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IDPA_Steganographie.BaseClasses;
using IDPA_Steganographie.HelperClasses;
using IDPA_Steganographie.HelperClasses.ErrorHelper;
using IDPA_Steganographie.HelperClasses.WaitDialogHelper;
using Steganographie.Logic;
using Steganographie.Logic.Image;
using Steganographie_Errors;

namespace IDPA_Steganographie.GUI.DecryptUi
{
    public class DecryptUiViewModel : ViewModelBase
    {
        private string _SaveFilePath;
        private string _OpenFilePath;
        private string _DecryptText;
        private string _Seed = "0";

        private bool _IsSeedValid = true;
        private int _SelectedItem;

        public DecryptUiViewModel()
        {
            SelectedIndex = 1;
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
        }

        public ObservableCollection<int> BitsItems { get; set; }

        public int SelectedIndex
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
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
        /// Path were the picture gets saved
        /// </summary>
        public string SaveFilePath
        {
            get { return _SaveFilePath; }
            set
            {
                _SaveFilePath = value;
                _OnPropertyChanged();
            }
        }

        /// <summary>
        /// Path from were the picture gets loaded
        /// </summary>
        public string OpenFilePath
        {
            get { return _OpenFilePath; }
            set
            {
                _OpenFilePath = value;
                _OnPropertyChanged();
                _OnPropertyChanged("ImageSource");
            }
        }

        /// <summary>
        /// Returns an image for a tooltip
        /// </summary>
        public ImageSource ImageSource
        {
            get { return !string.IsNullOrEmpty(OpenFilePath) ? new BitmapImage(new Uri(OpenFilePath)) : new BitmapImage(); }
        }

        public ICommand DecryptCommand
        {
            get
            {
                return new DelegateCommand(async () => await Decrypt(), () => !string.IsNullOrEmpty(OpenFilePath) && IsSeedValid);
            }
        }

        public ICommand ExportToTxtCommand
        {
            get
            {
                return new DelegateCommand(() => ExportTxt(), () => !string.IsNullOrEmpty(DecryptText));
            }
        }

        public ICommand Clear
        {
            get
            {
                return new DelegateCommand(() => DecryptText = string.Empty, () => !string.IsNullOrEmpty(DecryptText));
            }
        }

        /// <summary>
        /// The text which gets hidden in the picture
        /// </summary>
        public string DecryptText
        {
            get { return _DecryptText; }
            set
            {
                _DecryptText = value;
                _OnPropertyChanged();
            }
        }

        public async Task Decrypt()
        {
            await Decrypt(OpenFilePath, int.Parse(Seed), BitsItems[SelectedIndex]);
        }

        public async Task Decrypt(string filename, int seed, int bits)
        {
            await WaitDialogHelper.ExecuteThreadAsync("Decrypt Image", 
                () => DecryptText = Interface.Decrypt((BitmapImage)ImageSource, ChannelUsage.Default, seed), exception =>
                {
                    if (exception != null)
                        ErrorHelper.Add(3, exception.Message, exception.StackTrace);
                });
        }

        public void ExportTxt()
        {
            string path = RuntimeGlobals.ShowSaveFileDialog("Text-File (*.txt)|*.txt", RuntimeGlobals.Settings.SavePath);
            if(!string.IsNullOrEmpty(path))
            {
                WaitDialogHelper.ExecuteThread("Save Text", () =>
                                                            {
                                                                using(StreamWriter writer = new StreamWriter(path))
                                                                {
                                                                    writer.Write(DecryptText);
                                                                }
                                                            }, exception =>
                                                               {
                                                                   if(exception != null)
                                                                       ErrorHelper.Add(3, exception.Message, exception.StackTrace);
                                                               });

            }
        }
    }
}
