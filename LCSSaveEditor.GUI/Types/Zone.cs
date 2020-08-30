using System.ComponentModel;

namespace LCSSaveEditor.GUI.Types
{
    public enum ZoneLevel
    {
        [Description("Portland")]
        Industrial = 1,

        [Description("Staunton Island")]
        Commercial,

        [Description("Shoreside Vale")]
        Suburban
    }
}
