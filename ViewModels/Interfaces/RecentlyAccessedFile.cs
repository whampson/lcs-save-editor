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

using LcsSaveEditor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public class RecentlyAccessedFile : ObservableObject,
        IEquatable<RecentlyAccessedFile>
    {
        private string m_path;

        public string Path
        {
            get { return m_path; }
            set { m_path = value; OnPropertyChanged(); }
        }

        public ICommand Command
        {
            get;
            set;
        }

        public bool Equals(RecentlyAccessedFile other)
        {
            return Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RecentlyAccessedFile f)) {
                return false;
            }

            return Equals(f);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 31 * Path.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
