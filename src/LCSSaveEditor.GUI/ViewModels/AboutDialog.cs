using System.Windows.Input;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class AboutDialog : DialogBase
    {
        public ICommand ShowEulaCommand => new RelayCommand
        (
            () =>
            {
                ShowInfo(
                    "GTA:LCS Save Editor License Agreement\n" +
                    "--------------------------------------------------\n" +
                    "\n" +
                    "By installing, copying, modifying, or otherwise using this software, you agree " +
                    "to be bound by the terms of this license agreement. If you do not agree with " +
                    "the terms of this license agreement, do not use the software.\n" +
                    "\n" +
                    "1) You may not rent, lease, lend, or sell the software in any way.\n" +
                    "2) You may not change the terms of this license agreement in any way.\n" +
                    "3) You may distribute the software or its derivatives over the internet or any " +
                    "other distribution medium only with this license agreement included.\n" +
                    "\n" +
                    "By using the software, you automatically agree to the above terms.\n",
                    "End User License Agreement");
            }
        );
    }
}
