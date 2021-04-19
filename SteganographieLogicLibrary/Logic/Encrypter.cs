/*--------------------------------------------------------------------//
 * Project: IDPA Steganographie Logic
 * FileName: Encrypter.cs
 * 
 * Use this class to write text into a picture
 * 
 * --------------------------------------------------------------------*/

using System;
using System.Text;
using Steganographie.Logic.Helper;
using Steganographie.Logic.Image;

namespace Steganographie.Logic.Logic
{

    internal class Encrypter : IDisposable
    {
        private Picture _workingData;

        internal Encrypter(Picture inputData)
        {
            //Initialize new Encrypter object
            _workingData = inputData;
        }

        internal void InsertText(string text, ChannelUsage usage, bool useTags = true)
        {
            if(useTags)
                text = "<start>" + text + "<end>";

            //counters
            var pixelIndex = 0;
            var channelIndex = 0;
            var positionIndex = 0;

            //other info
            var currentChannelUsage = usage.GetChannelUsage(0);

            var bytes = Encoding.UTF8.GetBytes(text.ToCharArray());
            for(var bitIndex = 0; bitIndex < bytes.Length*8;bitIndex++, positionIndex++)
            {
                //get correct position
                while (positionIndex >= currentChannelUsage)
                {
                    channelIndex++;
                    positionIndex = 0;
                    if (channelIndex >= 4)
                    {
                        pixelIndex++;
                        channelIndex = 0;
                    }
                    currentChannelUsage = usage.GetChannelUsage((Channel) channelIndex);
                }

                //insert
                int data = _workingData.GetPixelChannel(pixelIndex, (Channel) channelIndex);

                var valByte = bytes[bitIndex >> 3];
                var val = BitOperators.ReadOutByte(valByte, 7 ^ (bitIndex & 7), 1, true) == 1;

                var result = BitOperators.InsertBit((byte)data, positionIndex, val);

                _workingData.SetPixelChannel(pixelIndex,(Channel)channelIndex, result);
            }
        }

        internal Picture GetPictureData()
        {
            return _workingData;
        }

        public void Dispose()
        {
            _workingData = null;
        }
    }
}
