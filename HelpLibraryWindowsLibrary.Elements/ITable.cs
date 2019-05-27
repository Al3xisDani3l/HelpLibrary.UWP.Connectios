using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpLibrary.UWP.Elements
{
    /// <summary>
    /// Interfaz entre un objeto comun de .Net core y una Tabla de cualquier base de datos.
    /// Contiene implementaciones que hace posible la interaccion de manera fluida entre un objeto y una Tabla,
    /// Implementando el modelo Propiedad-Campo donde las propiedades representan en una Tabla sus campos.
    /// si se desea que una propiedad no sea incluida en una tabla debe implementar el atributo  <see cref="PropertyAttribute.AddToDataBase"/>
    /// </summary>
    public interface ITable
    {
        int Id { get; set; }
        string Tabla { get; set; }
        List<PropertyInfo> Propiedades { get; }

    }
    /// <summary>
    /// Clase base de todos los objetos con capacidad de interactuar con Tablas de bases de datos, esta clase se debe implementar.
    /// </summary>
    public abstract class TableObject : ITable
    {
        [Property(AddToDataBase = true, AddToExcel = true)]
        public virtual int Id { get; set; }
        [Property(AddToDataBase = false, AddToExcel = false)]
        public virtual string Tabla { get { return GetType().Name; } set { throw new Exception("Sobreescriba esta propiedad para poder asignar un valor!"); } }
        [Property(AddToDataBase = false, AddToExcel = false)]
        public virtual List<PropertyInfo> Propiedades { get { return GetType().GetRuntimeProperties().Where(SelectProperty).ToList(); } }

        internal bool SelectProperty(PropertyInfo o)
        {

            PropertyAttribute atr =  o.GetCustomAttribute<PropertyAttribute>();

            if (atr == null)
            {
                return true;
            }
            else
            {
                return (atr.AddToDataBase == true && atr.AddToExcel == true);
            }

        }


    }
}
