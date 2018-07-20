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
using System.Collections.Generic;
using System.Windows.Forms;

namespace WHampson.LcsSaveEditor.UI
{
    public partial class MainForm : Form
    {
        private List<Type> pageTypes;
        private Dictionary<string, Tuple<Page, TabPage>> activeTabs;

        public MainForm()
        {
            pageTypes = new List<Type>()
            {
                typeof(WelcomePage),
                typeof(WeaponsPage)
            };
            activeTabs = new Dictionary<string, Tuple<Page, TabPage>>();

            InitializeComponent();
            RefreshActiveTabs();
        }

        private bool IsFileOpen
        {
            get { return Game.ActiveGameState != null; }
        }

        protected override CreateParams CreateParams
        {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED to reduce flickering
                return cp;
            }
        }

        private void AddTab(Page page)
        {
            page.Dock = DockStyle.Fill;

            TabPage tp = new TabPage(page.Text);
            tp.UseVisualStyleBackColor = true;
            tp.Margin = new Padding(4);
            tp.Padding = new Padding(4);
            tp.AutoScroll = true;
            tp.AutoScrollMinSize = page.Size;
            tp.Controls.Add(page);

            tabControl.TabPages.Add(tp);
            activeTabs[page.Text] = new Tuple<Page, TabPage>(page, tp);
        }

        private void RemoveTab(string pageName)
        {
            bool pageAdded = activeTabs.TryGetValue(pageName, out var pair);
            if (pageAdded) {
                if (tabControl.SelectedTab == pair.Item2) {
                    tabControl.SelectedIndex = tabControl.SelectedIndex - 1;
                }
                tabControl.TabPages.Remove(pair.Item2);
                activeTabs.Remove(pageName);
            }
        }

        private void ReloadTab(string pageName)
        {
            bool pageAdded = activeTabs.TryGetValue(pageName, out var pair);
            if (pageAdded) {
                pair.Item1.Reload();
            }
        }

        private bool IsTabActive(Page page)
        {
            return activeTabs.ContainsKey(page.Text);
        }

        private void RefreshActiveTabs()
        {
            bool isFileLoaded = (Game.ActiveGameState != null);

            foreach (Type t in pageTypes) {
                if (!typeof(Page).IsAssignableFrom(t)) {
                    continue;
                }

                Page p = (Page) Activator.CreateInstance(t);
                bool isActive = IsTabActive(p);
                string pageName = p.Text;

                switch (p.Visibility){
                    case PageVisibility.VisibleAlways:
                        if (!isActive) {
                            AddTab(p);
                        }
                        else {
                            ReloadTab(pageName);
                        }
                        break;
                    case PageVisibility.VisibleWhenFileLoadedOnly:
                        if (isFileLoaded && !isActive) {
                            AddTab(p);
                        }
                        else if (!isFileLoaded && isActive) {
                            RemoveTab(pageName);
                        }
                        else if (isActive) {
                            ReloadTab(pageName);
                        }
                        break;
                    case PageVisibility.VisibleWhenFileNotLoadedOnly:
                        if (isFileLoaded && isActive) {
                            RemoveTab(pageName);
                        }
                        else if (!isFileLoaded && !isActive) {
                            AddTab(p);
                        }
                        else if (isActive) {
                            ReloadTab(pageName);
                        }
                        break;
                }

                if (tabControl.TabPages.Count == 1) {
                    tabControl.SelectedTab = tabControl.TabPages[0];
                }

                tabControl.Invalidate();
            }
        }

        private void OpenFile(string path)
        {
            if (IsFileOpen) {
                CloseFile();
            }

            SaveDataFile saveData = SaveDataFile.Load(path);
            Game.ActiveGameState = new Game(saveData);
            closeFileMenuItem.Enabled = true;
        }

        private void CloseFile()
        {
            Game.ActiveGameState = null;
            closeFileMenuItem.Enabled = false;
        }

        private void OpenMenuItem_OnClick(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            DialogResult result = diag.ShowDialog();

            if (result == DialogResult.OK) {
                OpenFile(diag.FileName);
            }

            RefreshActiveTabs();
        }

        private void CloseFileMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
            RefreshActiveTabs();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
