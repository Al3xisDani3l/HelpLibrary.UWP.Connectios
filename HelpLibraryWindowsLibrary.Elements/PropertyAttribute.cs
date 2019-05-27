using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpLibrary.UWP.Elements
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {



        public bool AddToExcel { get; set; } = false;

        public bool AddToDataBase { get; set; } = false;


    }
}
