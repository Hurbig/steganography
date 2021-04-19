/*--------------------------------------------------------------------//
 * Project: IDPA Steganographie Logic
 * FileName: ReaderThread.cs
 * 
 * Allows multi-threading of the decryption process
 * 
 * --------------------------------------------------------------------*/

using Steganographie.Logic.Helper;
using Steganographie.Logic.Image;

namespace Steganographie.Logic.Logic
{
    internal class ReaderThread
    {
        private readonly Picture _picture;
        private readonly int[] _usages;
        private readonly bool[] _targetArray;
        private readonly int _startPosition;
        private readonly int _length;

        internal ReaderThread(Picture picture, int[] channelUsages, bool[] targetArray, int startPosition, int length)
        {
            _picture = picture;
            _usages = channelUsages;
            _targetArray = targetArray;
            _startPosition = startPosition;
            _length = length;
        }

        internal void ReadBooleans()
        {
            var count = 0;
            for (var index = _startPosition; index < _startPosition + _length; index++)
            {
                if (_picture.Size < index)
                    return;

                for (var channelIndex = 0; channelIndex < 4; channelIndex++)
                {
                    var chnUsage = _usages[channelIndex];

                    for (var positionIndex = 0; positionIndex < chnUsage; positionIndex++)
                    {
                        var byteVal = _picture.GetPixelChannel(index, (Channel) channelIndex);
                        var bitVal = BitOperators.ReadOutByte(byteVal, positionIndex, 1, true);

                        _targetArray[_startPosition + count] = bitVal == 1;
                        count++;
                    }
                }
            }
        }
    }
}
