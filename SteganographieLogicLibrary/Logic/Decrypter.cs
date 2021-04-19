/*--------------------------------------------------------------------//
 * Project: IDPA Steganographie Logic
 * FileName: Decrypter.cs
 * 
 * Use this class to read text from a picture
 * 
 * --------------------------------------------------------------------*/

using System;
using System.Text;
using System.Threading;
using Steganographie.Logic.Helper;
using Steganographie.Logic.Image;

namespace Steganographie.Logic.Logic
{
    internal class Decrypter : IDisposable
    {
        private static readonly int _threadSizeModifier = 17;
        private Picture _inputData;

        internal Decrypter(Picture inputData)
        {
            _inputData = inputData;
        }

        internal string ReadText(ChannelUsage usage, bool returnFilteredText = true, bool useThreads = true)
        {
            var values = new bool[usage.GetTotaleUsage() * _inputData.Size];
            var usages = usage.GetUsages();

            var numThreads = useThreads ? _inputData.Size >> _threadSizeModifier | 1 : 1;
            var threadLength = _inputData.Size / numThreads;

            var threads = new Thread[numThreads];
            for (var i = 0; i < numThreads; i++)
            {
                var reader = new ReaderThread(_inputData, usages, values, i * threadLength, threadLength);
                threads[i] = new Thread(reader.ReadBooleans);
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            var bytes = BitOperators.BitsToBytes(values);
            var text = Encoding.UTF8.GetString(bytes);
            return returnFilteredText ? FindOriginalText(text) : text;
        }

        private static string FindOriginalText(string fullText)
        {
            var startIndex = fullText.IndexOf("<start>", StringComparison.Ordinal);
            var endIndex = fullText.IndexOf("<end>", StringComparison.Ordinal);

            if (startIndex == -1 || endIndex == -1)
                return "";

            startIndex = (startIndex + 7) % fullText.Length;

            return fullText.Substring(startIndex, endIndex - startIndex);
        }

        public void Dispose()
        {
            _inputData = null;
        }
    }
}
