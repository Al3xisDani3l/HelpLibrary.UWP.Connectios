using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpLibrary.UWP.Connections.Data.Static
{
    public static class Extencions
    {
        public static string ToStringFormat(this ITable dateable, string formato)
        {
            string Formato = "";

            int count = 0;



            foreach (PropertyInfo item in dateable.Propiedades)
            {
                var attrib = item.GetCustomAttribute<PropertyAttribute>();
                if (attrib != null)
                {
                    count++;
                }
                else
                {
                    if (attrib.AddToExcel)
                    {
                        count++;
                    }
                }
            }

            object[] buffer = new object[count];

            foreach (PropertyInfo item in dateable.Propiedades)
            {
                var attrib = item.GetCustomAttribute<PropertyAttribute>();
                if (attrib != null)
                {

                    buffer[attrib.StringPosition] = item.GetValue(dateable);

                }
                else
                {
                    if (attrib.AddToExcel)
                    {
                        buffer[attrib.StringPosition] = item.GetValue(dateable);
                    }
                }

            }

            Formato = string.Format(formato, buffer);

            return Formato;


        }
    }
}
