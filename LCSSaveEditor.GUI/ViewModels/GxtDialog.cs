using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LCSSaveEditor.Core;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class GxtDialog : DialogBase
    {
        private Gxt m_gxt;
        private Dictionary<string, string> m_table;
        private string m_tableName;
        private bool m_allowTableSelection;
        private KeyValuePair<string, string> m_selectedItem;
        private int m_selectedIndex;

        public Dictionary<string, string> Table
        {
            get { return m_table; }
            set { m_table = value; OnPropertyChanged(); }
        }

        public string TableName
        {
            get { return m_tableName; }
            set { m_tableName = value; OnPropertyChanged(); }
        }

        public bool AllowTableSelection
        {
            get { return m_allowTableSelection; }
            set { m_allowTableSelection = value; OnPropertyChanged(); }
        }

        public KeyValuePair<string, string> SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; OnPropertyChanged(); }
        }

        public int SelectedIndex
        {
            get { return m_selectedIndex; }
            set { m_selectedIndex = value; OnPropertyChanged(); }
        }

        public IEnumerable<string> TableNames => Gxt.TheText.TableNames.Select(x => x);

        public GxtDialog()
        {
            Table = new Dictionary<string, string>();
            TableName = "";
            SelectedIndex = -1;
        }

        public ICommand SelectCommand => new RelayCommand
        (
            () => CloseDialog(true),
            () => SelectedIndex != -1
        );
    }
}
