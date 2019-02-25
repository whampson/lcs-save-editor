#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.Models.DataTypes;
using System.Collections.Generic;

namespace LcsSaveEditor.Models
{
    public static class WeatherCycle
    {
        public static readonly IReadOnlyList<Weather> WeatherList = new List<Weather>()
        {
            Weather.Sunny,      Weather.ExtraSunny, Weather.Sunny,      Weather.Cloudy,
            Weather.Cloudy,     Weather.Sunny,      Weather.Sunny,      Weather.Sunny,
            Weather.ExtraSunny, Weather.ExtraSunny, Weather.Sunny,      Weather.Rainy,
            Weather.Sunny,      Weather.Sunny,      Weather.Cloudy,     Weather.Cloudy,
            Weather.Foggy,      Weather.Cloudy,     Weather.Sunny,      Weather.Sunny,
            Weather.Sunny,      Weather.Cloudy,     Weather.Rainy,      Weather.Foggy,
            Weather.Cloudy,     Weather.Cloudy,     Weather.Sunny,      Weather.ExtraSunny,
            Weather.Sunny,      Weather.Sunny,      Weather.Cloudy,     Weather.Foggy,
            Weather.Rainy,      Weather.Foggy,      Weather.Cloudy,     Weather.Sunny,
            Weather.Sunny,      Weather.Cloudy,     Weather.Hurricane,  Weather.Cloudy,
            Weather.Sunny,      Weather.ExtraSunny, Weather.ExtraSunny, Weather.Sunny,
            Weather.Sunny,      Weather.Sunny,      Weather.Sunny,      Weather.ExtraSunny,
            Weather.Sunny,      Weather.Sunny,      Weather.Cloudy,     Weather.Foggy,
            Weather.Cloudy,     Weather.Sunny,      Weather.Cloudy,     Weather.Cloudy,
            Weather.Sunny,      Weather.ExtraSunny, Weather.Cloudy,     Weather.Hurricane,
            Weather.Cloudy,     Weather.Sunny,      Weather.Sunny,      Weather.ExtraSunny
        };
    }
}
