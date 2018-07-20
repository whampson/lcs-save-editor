#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using System;
using System.Windows.Forms;

namespace WHampson.LcsSaveEditor.UI
{
// Keep this so the Visual Studio Designer works
#if DEBUG
    public class Page : UserControl
#else
        public abstract class Page : UserControl
#endif
    {
        // Keep this so the Visual Studio Designer works
        private Page()
        { }

        public Page(string title)
            : this(title, PageVisibility.VisibleAlways)
        { }

        public Page(string title, PageVisibility visibility)
        {
            Text = title;
            Visibility = visibility;
            Load += Page_Load;
        }

        public PageVisibility Visibility
        {
            get;
        }

        public void Reload()
        {
            Page_Load(this, new EventArgs());
        }

// Keep this so the Visual Studio Designer works
#if DEBUG
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException("Must implement this!");
        }
#else
        protected abstract void Page_Load(object sender, EventArgs e);
#endif
    }

    public enum PageVisibility
    {
        VisibleWhenFileLoadedOnly,
        VisibleWhenFileNotLoadedOnly,
        VisibleAlways
    }
}
