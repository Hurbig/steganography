/*--------------------------------------------------------------------//
 * Project: IDPA Steganographie Logic
 * FileName: BitOperators.cs
 * 
 * This class is used to read the pixel information in bits
 * and to write bits into a pixel
 * --------------------------------------------------------------------*/

using System;

namespace Steganographie.Logic.Helper
{
    internal static class BitOperators
    {
        internal static byte GetByteMask(int bitsPerChannel)
        {
            return (byte) ~((1 << bitsPerChannel) - 1);
            //return (byte) (256 - (int) Math.Pow(2, bitsPerChannel));
        }

        internal static byte GetSingleByteMask(int position)
        {
            return (byte) ~(1 << position);
        }

        internal static int ReadOutByte(byte byteInfo, int position, int length, bool shiftToRight)
        {
            //1 for length 1, 3 for length 2, 7 for length 3...
            var bytePosition = (1 << length - 1) << position;

            var byteResult = byteInfo & bytePosition;
            if (shiftToRight)
                byteResult = byteResult >> position;

            return byteResult;
        }

        internal static byte InsertBit(byte data, int position, bool val)
        {
            var temp = (int) data;
            temp &= GetSingleByteMask(position);
            temp |= val ? 1 << position : 0;

            return (byte) temp;
        }

        internal static byte[] BitsToBytes(bool[] bools)
        {
            var length = bools.Length;
            var bytes = length >> 3;
            if ((length & 0x07) != 0) ++bytes;

            var resArray = new byte[bytes];

            for (var i = 0; i < bools.Length; i++)
            {
                if (bools[i])
                    resArray[i >> 3] |= (byte) (1 << (~i & 0x07));
            }

            return resArray;
        }
    }
}
