using System.Collections.Generic;
using System.Linq;

namespace Steganographie.Logic.Image
{
    public class ChannelUsage
    {
        public Dictionary<Channel, int> UsedPositions;

        public ChannelUsage(int usage) : this(usage,usage,usage,usage)
        {
            
        }

        public ChannelUsage(int red = 0, int green = 0, int blue = 0, int alpha= 0)
        {
            UsedPositions = new Dictionary<Channel, int>
            {
                {Channel.Red, red},
                {Channel.Green, green},
                {Channel.Blue, blue},
                {Channel.Alpha, alpha}
            };
        }

        public int GetChannelUsage(Channel channel)
        {
            return UsedPositions[channel];
        }

        public int GetTotaleUsage()
        {
            return UsedPositions.Values.Sum();
        }

        public int[] GetUsages()
        {
            var array = new int[4];
            for (var i = 0; i < 4; i++)
            {
                array[i] = GetChannelUsage((Channel) i);
            }

            return array;
        }

        public static ChannelUsage Default => new ChannelUsage(2);
    }
}
