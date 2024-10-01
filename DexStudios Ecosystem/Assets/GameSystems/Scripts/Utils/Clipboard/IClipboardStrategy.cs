using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameSystems.Scripts.Utils.Clipboard
{
    internal interface IClipboardStrategy
    {
        void Set(string text);
        string Get(); 
    }
}
