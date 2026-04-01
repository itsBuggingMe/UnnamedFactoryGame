using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmi.Extensions;

internal static class RandomExtensions
{
    extension(Random random)
    {
        public float NextSingle(float min, float max)
        {
            return float.Lerp(min, max, random.NextSingle());
        }
    }
}
