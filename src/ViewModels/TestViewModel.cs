using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHampson.LcsSaveEditor.Helpers;
using WHampson.LcsSaveEditor.Models;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class TestViewModel : PageViewModel
    {
        public TestViewModel(SaveDataFile gameData)
            : base("Test", gameData)
        { }

        public void Update()
        {
            GameState.Scripts.GlobalVariables[141] = 69;
        }
    }
}
