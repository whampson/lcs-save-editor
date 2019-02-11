using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Models;

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// Represents a named <see cref="ScriptVariable"/>.
    /// </summary>
    public class NamedScriptVariable : ObservableObject
    {
        private string m_name;
        private ScriptVariable m_value;

        public NamedScriptVariable()
            : this(string.Empty, new ScriptVariable())
        { }

        public NamedScriptVariable(ScriptVariable value)
            : this(string.Empty, value)
        { }

        public NamedScriptVariable(string name, ScriptVariable value)
        {
            m_name = name;
            m_value = value;
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public ScriptVariable Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}",
                nameof(Name), Name,
                nameof(Value), Value);
        }
    }
}
