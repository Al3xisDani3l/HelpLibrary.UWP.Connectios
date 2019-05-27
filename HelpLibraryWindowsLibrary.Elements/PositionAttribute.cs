using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpLibrary.UWP.Elements
{
   public class PositionAttribute:Attribute
    {
        public int StringPosition { get; set; } = int.MaxValue;

        public int TablePosition { get; set; } = int.MaxValue;

        public int ExcelPosition { get; set; } = int.MaxValue;

        public int Other { get; set; } = int.MaxValue;
    }
}
