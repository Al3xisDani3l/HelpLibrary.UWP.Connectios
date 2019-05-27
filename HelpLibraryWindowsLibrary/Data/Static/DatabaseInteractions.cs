using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlTypes;
using Microsoft.Data.Sqlite;
using HelpLibrary.UWP.Elements;

namespace HelpLibrary.UWP.Connections.Data.Static
{
    public enum TypeDb
    {
        Odbc,
        sqlLite

    }


    public class DatabaseInteractions : IDisposable
    {
        #region Variables
        private OdbcConnection odbcConnection;

        private SqliteConnection sqliteConnection;

        private readonly TypeDb dbCurren;

        string cadenaDeConexion;
        #endregion

        #region Propiedades
        public string CadenaDeConexion
        {
            get
            {
                return cadenaDeConexion;
            }
            set
            {
                switch (dbCurren)
                {
                    case TypeDb.Odbc:
                        if (value.ToLowerInvariant().Contains("data source=") && value.ToLowerInvariant().Contains("Vercion="))
                        {
                            cadenaDeConexion = value;
                            odbcConnection = new OdbcConnection(value);
                        }
                        else
                        {
                            throw new Exception("La cadena de concexion no tiene un formato valido");
                        }
                        break;
                    case TypeDb.sqlLite:
                        if ((value.ToLowerInvariant().Contains("driver=") || value.ToLowerInvariant().Contains("dsn=")) && value.ToLowerInvariant().Contains("dbq="))
                        {
                            cadenaDeConexion = value;
                            sqliteConnection = new SqliteConnection(value);
                        }
                        else
                        {
                            throw new Exception("La cadena de concexion no tiene un formato valido");
                        }
                        break;
                }
            }
        }

        public object Conexion
        {
            get
            {
                switch (dbCurren)
                {
                    case TypeDb.Odbc:
                        return odbcConnection;
                    case TypeDb.sqlLite:
                        return sqliteConnection;
                    default:
                        throw new Exception("No se configuro correctamente la base de datos");
                }

            }
            set
            {
                switch (dbCurren)
                {
                    case TypeDb.Odbc:
                        if (value is OdbcConnection)
                        {
                            odbcConnection = value as OdbcConnection;
                        }
                        else
                        {
                            throw new Exception("No es un OdbcConnection valida verifique el objeto");
                        }
                        break;
                    case TypeDb.sqlLite:
                        if (value is SqliteConnection)
                        {
                            sqliteConnection = value as SqliteConnection;
                        }
                        else
                        {
                            throw new Exception("No es un SqliteConnection valida verifique el objeto");
                        }
                        break;
                    default:
                        throw new Exception("No se configuro correctamente la base de datos");

                }

            }
        }

        public bool ConfigureStatus
        {
            get
            {
                switch (dbCurren)
                {
                    case TypeDb.Odbc:
                        if (odbcConnection != null)
                        {
                            try
                            {
                                odbcConnection.Open();
                                return true;
                            }
                            catch (Exception Error)
                            {

                                new HelpLibraryLog(Error, odbcConnection);
                                return false;

                            }
                            finally
                            {
                                odbcConnection.Close();
                            }
                        }
                        else
                        {
                            return false;
                        }

                    case TypeDb.sqlLite:
                        if (sqliteConnection != null)
                        {
                            try
                            {
                                sqliteConnection.Open();
                                return true;

                            }
                            catch (Exception Error)
                            {
                                new HelpLibraryLog(Error, sqliteConnection);
                                return false;

                            }
                            finally
                            {
                                sqliteConnection.Close();
                            }
                        }
                        else
                        {
                            return false;
                        }

                    default:
                        return false;
                }
            }
        }

        public TypeDb DbCurrent { get => dbCurren; }
        #endregion

        #region Constructores 

        public DatabaseInteractions(TypeDb typeDb = TypeDb.Odbc)
        {
            this.dbCurren = typeDb;
        }

        public DatabaseInteractions(object conexion,TypeDb typeDb = TypeDb.Odbc)
        {
            dbCurren = typeDb;

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    if (conexion!=null && conexion is OdbcConnection)
                    {
                        this.odbcConnection = conexion as OdbcConnection;
                    }
                    else
                    {
                        throw new Exception("Conexion no validad revise que tipo de conexion usa");
                    }
                    break;
                case TypeDb.sqlLite:
                    if (conexion != null && conexion is OdbcConnection)
                    {
                        this.odbcConnection = conexion as OdbcConnection;
                    }
                    else
                    {
                        throw new Exception("Conexion no validad revise que tipo de conexion usa");
                    }
                    break;
            }
        }

        public DatabaseInteractions(string cadenaDeConexion, TypeDb typeDb = TypeDb.Odbc)
        {
            dbCurren = typeDb;

            CadenaDeConexion = cadenaDeConexion;
        }



        #endregion


        #region Funciones Publicas

        public void close()
        {
            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    if (odbcConnection != null)
                    {
                        odbcConnection.Close();
                    }
                    break;
                case TypeDb.sqlLite:
                    if (sqliteConnection != null)
                    {
                        sqliteConnection.Close();
                    }
                    break;
               
            }
        }

        public void Insert(ITable Objeto)
        {

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand Comando = InsertToCommand(Objeto, odbcConnection))
                        {



                            odbcConnection.Open();

                            Comando.ExecuteNonQuery();

                        }
                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        throw Error;
                    }

                    finally
                    {
                        odbcConnection.Close();


                    }
                    break;
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand Comando = new SqliteCommand(InsertToCommand(Objeto), sqliteConnection))

                        {



                            odbcConnection.Open();

                            Comando.ExecuteNonQuery();

                        }
                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        throw Error;
                    }

                    finally
                    {
                        sqliteConnection.Close();


                    }

                    break;
            }


        }

        public async void InsertAsync(ITable Objeto)
        {

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand Comando = InsertToCommand(Objeto, odbcConnection))
                        {



                            odbcConnection.Open();
                           
                            await Comando.ExecuteNonQueryAsync();

                        }
                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        throw Error;
                    }

                    finally
                    {
                        odbcConnection.Close();
                       

                    }
                    break;
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand Comando = new SqliteCommand(InsertToCommand(Objeto),sqliteConnection))

                        {



                            odbcConnection.Open();

                            await Comando.ExecuteNonQueryAsync();

                        }
                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        throw Error;
                    }

                    finally
                    {
                        sqliteConnection.Close();


                    }

                    break;
            }

           
        }

        public void Insert(IList<ITable> Objetos)
        {
            for (int i = 0; i < Objetos.Count; i++)
            {
                Insert(Objetos[i]);
            }

        }

        public async void InsertAsync(IList<ITable> Objetos)
        {

            await Task.Run(() =>
            {
                for (int i = 0; i < Objetos.Count; i++)
                {
                    InsertAsync(Objetos[i]);
                }
            });
        }

        public IEnumerable<Table> Read<Table>() where Table : ITable, new()
        {





            List<Table> retorno = new List<Table>();
            Table temp = new Table();

            string cmd = string.Format("SELECT * FROM {0}", temp.Tabla);

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                        {
                            odbcConnection.Open();

                            var lectura = comando.ExecuteReader();

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {

                        odbcConnection.Close();


                    }
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                        {
                            odbcConnection.Open();

                            var lectura = comando.ExecuteReader();

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {
                        sqliteConnection.Close();
                    }
                default:
                    return new List<Table>();

            }


        }

        public async Task<IEnumerable<Table>> ReadAsync<Table>() where Table : ITable, new()
        {





            List<Table> retorno = new List<Table>();
            Table temp = new Table();

            string cmd = string.Format("SELECT * FROM {0}", temp.Tabla);

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                        {
                            odbcConnection.Open();
                          
                            var lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {

                        odbcConnection.Close();
                      

                    }
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                        {
                            odbcConnection.Open();

                            var lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;
                       
                    }
                    finally
                    {
                        sqliteConnection.Close();
                    }
                default:
                    return new List<Table>();
                   
            }
           

        }

        public IEnumerable<Table> Read<Table>(string where) where Table : ITable, new()
        {





            List<Table> retorno = new List<Table>();
            Table temp = new Table();

            string cmd = string.Format("SELECT * FROM {0} WHERE {1}", temp.Tabla, where);

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                        {
                            odbcConnection.Open();

                            var lectura = comando.ExecuteReader();

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {

                        odbcConnection.Close();


                    }
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                        {
                            odbcConnection.Open();

                            var lectura = comando.ExecuteReader();

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {
                        sqliteConnection.Close();
                    }
                default:
                    return new List<Table>();

            }


        }

        public async Task<IEnumerable<Table>> ReadAsync<Table>(string where) where Table : ITable, new()
        {





            List<Table> retorno = new List<Table>();
            Table temp = new Table();

            string cmd = string.Format("SELECT * FROM {0} WHERE {1}", temp.Tabla, where);

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                        {
                            odbcConnection.Open();

                            var lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {

                        odbcConnection.Close();


                    }
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                        {
                            odbcConnection.Open();

                            var lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

                            while (lectura.Read())
                            {

                                Table buffer = new Table();
                                for (int i = 0; i < buffer.Propiedades.Count; i++)
                                {

                                    PropertyAttribute attrib = Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                                    if (attrib != null)
                                    {
                                        if (attrib.AddToDataBase)
                                        {
                                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
                                    }





                                }

                                retorno.Add(buffer);

                            }

                            return retorno;

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return new List<Table>();
                        throw Error;

                    }
                    finally
                    {
                        sqliteConnection.Close();
                    }
                default:
                    return new List<Table>();

            }


        }

        public bool Delete(ITable Objeto)
        {
            int afectados = 0;

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                       
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objeto.Tabla);

                            using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                            {


                                OdbcParameter parameter = new OdbcParameter("Id", Objeto.Id);
                                comando.Parameters.Add(parameter);
                                odbcConnection.Open();

                                afectados = comando.ExecuteNonQuery();

                            }


                        return afectados > 0 ? true : false;


                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return false;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }

                case TypeDb.sqlLite:
                    try
                    {
                       
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = {0}", Objeto.Tabla, Objeto.Id);

                            using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                            {



                                sqliteConnection.Open();

                                afectados = comando.ExecuteNonQuery();

                            }


                        return afectados > 0 ? true : false;


                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return false;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }
                default:
                    return false;


            }



        }

        public async Task<bool> DeleteAsync(ITable Objeto)
        {
            int afectados = 0;

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {

                        string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objeto.Tabla);

                        using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                        {


                            OdbcParameter parameter = new OdbcParameter("Id", Objeto.Id);
                            comando.Parameters.Add(parameter);
                            await odbcConnection.OpenAsync();

                            afectados = await comando.ExecuteNonQueryAsync();

                        }


                        return afectados > 0 ? true : false;


                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return false;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }

                case TypeDb.sqlLite:
                    try
                    {

                        string cmd = string.Format("DELETE FROM {0} WHERE Id = {0}", Objeto.Tabla, Objeto.Id);

                        using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                        {



                            await sqliteConnection.OpenAsync();

                            afectados = await comando.ExecuteNonQueryAsync();

                        }


                        return afectados > 0 ? true : false;


                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return false;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }
                default:
                    return false;


            }



        }

        public int Delete(IList<ITable> Objetos)
        {
            int afectados = 0;

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        for (int i = 0; i < Objetos.Count; i++)
                        {
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objetos[i].Tabla);

                            using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                            {


                                OdbcParameter parameter = new OdbcParameter("Id", Objetos[i].Id);
                                comando.Parameters.Add(parameter);
                                odbcConnection.Open();

                                afectados = comando.ExecuteNonQuery();

                            }
                        }

                        return afectados;


                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }

                case TypeDb.sqlLite:
                    try
                    {
                        for (int i = 0; i < Objetos.Count; i++)
                        {
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = {0}", Objetos[i].Tabla, Objetos[i].Id);

                            using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                            {



                                sqliteConnection.Open();

                                afectados = comando.ExecuteNonQuery();

                            }
                        }

                        return afectados;


                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }
                default:
                    return 0;

            }



        }

        public async Task<int> DeleteAsync(IList<ITable> Objetos)
        {
            int afectados = 0;

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        for (int i = 0; i < Objetos.Count; i++)
                        {
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objetos[i].Tabla);

                            using (OdbcCommand comando = new OdbcCommand(cmd, odbcConnection))
                            {


                                OdbcParameter parameter = new OdbcParameter("Id", Objetos[i].Id);
                                comando.Parameters.Add(parameter);
                                await odbcConnection.OpenAsync();

                                afectados = await comando.ExecuteNonQueryAsync();

                            }
                        }

                        return afectados;


                    }
                    catch (OdbcException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }

                case TypeDb.sqlLite:
                    try
                    {
                        for (int i = 0; i < Objetos.Count; i++)
                        {
                            string cmd = string.Format("DELETE FROM {0} WHERE Id = {0}", Objetos[i].Tabla, Objetos[i].Id);

                            using (SqliteCommand comando = new SqliteCommand(cmd, sqliteConnection))
                            {



                                sqliteConnection.Open();

                                afectados = await comando.ExecuteNonQueryAsync();

                            }
                        }

                        return afectados;


                    }
                    catch (SqliteException Error)
                    {
                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {
                        odbcConnection.Close();


                    }
                default:
                    return 0;

            }



        }

        public int Update(ITable Viejo, ITable Nuevo)
        {


            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand command = UpdateToCommand(Viejo, Nuevo, odbcConnection))
                        {
                            odbcConnection.Open();

                            return command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {

                        odbcConnection.Close();

                    }

                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand command = new SqliteCommand(UpdateToCommand(Viejo, Nuevo), sqliteConnection))
                        {
                            odbcConnection.Open();

                            return command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {

                        sqliteConnection.Close();

                    }
                default:
                    return 0;


            }



        }

        public async Task<int> UpdateAsyn(ITable Viejo, ITable Nuevo)
        {
            

            switch (dbCurren)
            {
                case TypeDb.Odbc:
                    try
                    {
                        using (OdbcCommand command = UpdateToCommand(Viejo, Nuevo, odbcConnection))
                        {
                            odbcConnection.Open();
                         
                          return  await command.ExecuteNonQueryAsync();

                        }
                    }
                    catch (Exception Error)
                    {
                        
                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {

                        odbcConnection.Close();
                       
                    }
                  
                case TypeDb.sqlLite:
                    try
                    {
                        using (SqliteCommand command = new SqliteCommand(UpdateToCommand(Viejo, Nuevo),sqliteConnection))
                        {
                            odbcConnection.Open();

                            return await command.ExecuteNonQueryAsync();

                        }
                    }
                    catch (Exception Error)
                    {

                        new HelpLibraryLog(Error, this);
                        return 0;
                        throw Error;
                    }
                    finally
                    {

                        sqliteConnection.Close();

                    }
                default:
                    return 0;
                    
              
                    
            }

            
        }

        public int Update(IList<ITable> viejos,IList<ITable> nuevos)
        {
            int sumador = 0;
            if (viejos.Count != nuevos.Count)
            {
                return 0;
            }
            for (int i = 0; i < viejos.Count; i++)
            {
                sumador += Update(viejos[i], nuevos[i]);
            }
            return sumador;

        }

        public async Task<int> UpdateAsync(IList<ITable> viejos, IList<ITable> nuevos)
        {
         return await Task.Run( async () =>
            {
                int con = 0;
                if (viejos.Count != nuevos.Count)
                {
                    return 0;
                }

                for (int i = 0; i < viejos.Count; i++)
                {
                   con += await UpdateAsyn(viejos[i], nuevos[i]);
                }
                return con;
            });


        }




        #endregion






        #region Funciones internas

        private string InsertToCommand(ITable dateable)
        {

            string buffer = string.Format("INSERT INTO {0} (", dateable.Tabla);
            for (int i = 0; i < dateable.Propiedades.Count; i++)
            {

                PropertyAttribute attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                if (attrib != null)
                {
                    if (attrib.AddToDataBase == true)
                    {
                        if (dateable.Propiedades[i].Name != "Id")
                        {
                            buffer += string.Format("{0},", dateable.Propiedades[i].Name);
                        }

                    }
                }
                else
                {
                    buffer += string.Format("{0},", dateable.Propiedades[i].Name);
                }





            }

            buffer = buffer.Remove(buffer.Length - 1);
            buffer += ")";
            buffer += " VALUES(";

            for (int i = 0; i < dateable.Propiedades.Count; i++)
            {
                var attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                if (attrib != null)
                {
                    if (attrib.AddToDataBase)
                    {
                        if (dateable.Propiedades[i].Name != "Id")
                        {
                            if (dateable.Propiedades[i].PropertyType == typeof(DateTime))
                            {

                                DateTime date = Convert.ToDateTime(dateable.Propiedades[i].GetValue(dateable));
                                string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
                                buffer += string.Format("\"{0}\",", bf);

                            }
                            else
                            {
                                if (dateable.Propiedades[i].PropertyType == typeof(string))
                                {
                                    buffer += string.Format("\"{0}\",", dateable.Propiedades[i].GetValue(dateable).ToString());
                                }
                                else
                                {
                                    buffer += string.Format("{0},", dateable.Propiedades[i].GetValue(dateable).ToString());
                                }

                            }
                        }

                    }
                }
                else
                {

                    if (dateable.Propiedades[i].PropertyType == typeof(DateTime))
                    {

                        DateTime date = Convert.ToDateTime(dateable.Propiedades[i].GetValue(dateable));
                        string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
                        buffer += string.Format("\"{0}\",", bf);

                    }
                    else
                    {
                        if (dateable.Propiedades[i].PropertyType == typeof(string))
                        {
                            buffer += string.Format("\"{0}\",", dateable.Propiedades[i].GetValue(dateable).ToString());
                        }
                        else
                        {
                            buffer += string.Format("{0},", dateable.Propiedades[i].GetValue(dateable).ToString());
                        }

                    }
                }




            }

            buffer = buffer.Remove(buffer.Length - 1);
            buffer += ");";

            return buffer;

        }

        private OdbcCommand InsertToCommand(ITable dateable, OdbcConnection connection)
        {

            var commandBuffer = new OdbcCommand();

            commandBuffer.Connection = connection;

            string buffer = string.Format("INSERT INTO {0} (", dateable.Tabla);
            for (int i = 0; i < dateable.Propiedades.Count; i++)
            {

                PropertyAttribute attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                if (attrib != null)
                {
                    if (attrib.AddToDataBase == true)
                    {
                        if (dateable.Propiedades[i].Name != "Id")
                        {
                            buffer += string.Format("{0},", dateable.Propiedades[i].Name);
                        }

                    }
                }
                else
                {
                    buffer += string.Format("{0},", dateable.Propiedades[i].Name);
                }





            }

            buffer = buffer.Remove(buffer.Length - 1);
            buffer += ")";
            buffer += " VALUES(";

            for (int i = 0; i < dateable.Propiedades.Count; i++)
            {
                var attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                if (attrib != null)
                {
                    if (attrib.AddToDataBase)
                    {
                        if (dateable.Propiedades[i].Name != "Id")
                        {

                            buffer += string.Format("{0},", "?");


                        }

                    }
                }
                else
                {
                    buffer += string.Format("{0},", "?");
                }




            }

            buffer = buffer.Remove(buffer.Length - 1);
            buffer += ");";

            commandBuffer.CommandText = buffer;

            for (int i = 0; i < dateable.Propiedades.Count; i++)
            {
                var attrib = Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
                if (attrib != null)
                {
                    if (attrib.AddToDataBase)
                    {
                        if (dateable.Propiedades[i].Name != "Id")
                        {
                            var parameter = new OdbcParameter(dateable.Propiedades[i].Name, dateable.Propiedades[i].GetValue(dateable));
                            commandBuffer.Parameters.Add(parameter);
                        }

                    }

                }
                else
                {
                    var parameter = new OdbcParameter(dateable.Propiedades[i].Name, dateable.Propiedades[i].GetValue(dateable));
                    commandBuffer.Parameters.Add(parameter);
                }
            }

            return commandBuffer;

        }

        private string UpdateToCommand(ITable Viejo, ITable Nuevo)
        {
            string buffer = string.Format("UPDATE {0} SET ", Nuevo.Tabla);
            if (Viejo.Tabla == Nuevo.Tabla)
            {
                for (int i = 0; i < Nuevo.Propiedades.Count; i++)
                {
                    var attrib = System.Attribute.GetCustomAttribute(Nuevo.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;

                    if (attrib != null)
                    {
                        if (attrib.AddToExcel)
                        {
                            if (Nuevo.Propiedades[i].PropertyType == typeof(DateTime))
                            {

                                DateTime date = Convert.ToDateTime(Nuevo.Propiedades[i].GetValue(Nuevo));
                                string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
                                buffer += string.Format("{0}=\'{1}\', ", Nuevo.Propiedades[i].Name, bf);

                            }
                            else
                            {
                                if (Nuevo.Propiedades[i].PropertyType == typeof(string))
                                {
                                    buffer += string.Format("{0}=\"{1}\", ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
                                }
                                else
                                {
                                    buffer += string.Format("{0}={1}, ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
                                }

                            }
                        }

                    }
                    else
                    {

                        if (Nuevo.Propiedades[i].PropertyType == typeof(DateTime))
                        {

                            DateTime date = Convert.ToDateTime(Nuevo.Propiedades[i].GetValue(Nuevo));
                            string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
                            buffer += string.Format("{0}=\'{1}\', ", Nuevo.Propiedades[i].Name, bf);

                        }
                        else
                        {
                            if (Nuevo.Propiedades[i].PropertyType == typeof(string))
                            {
                                buffer += string.Format("{0}=\"{1}\", ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
                            }
                            else
                            {
                                buffer += string.Format("{0}={1}, ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
                            }

                        }
                    }


                }

                buffer = buffer.Remove(buffer.Length - 2);



                buffer += string.Format(" WHERE Id={0};", Viejo.Id);
                return buffer;
            }
            else
            {
                return "";
                throw new Exception("Los objetos no pertenecen a la misma tabla.");
            }

        }

        private OdbcCommand UpdateToCommand(ITable Viejo, ITable Nuevo, OdbcConnection connection)
        {
            var commandbuffer = new OdbcCommand();


            string buffer = string.Format("UPDATE {0} SET ", Nuevo.Tabla);
            if (Viejo.Tabla == Nuevo.Tabla)
            {
                for (int i = 0; i < Nuevo.Propiedades.Count; i++)
                {
                    var attrib = System.Attribute.GetCustomAttribute(Nuevo.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;

                    if (attrib != null)
                    {
                        if (attrib.AddToDataBase)
                        {
                            if (Nuevo.Propiedades[i].Name != "Id")
                            {
                                OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo));
                                buffer += string.Format("{0}=?,", Nuevo.Propiedades[i].Name);
                                commandbuffer.Parameters.Add(parameter);

                            }
                            else
                            {
                                OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Viejo));
                                commandbuffer.Parameters.Add(parameter);
                            }
                        }

                    }
                    else
                    {

                        OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo));
                        buffer += string.Format("{0} = ?,", Nuevo.Propiedades[i].Name);
                        commandbuffer.Parameters.Add(parameter);
                    }


                }

                buffer = buffer.Remove(buffer.Length - 1);



                buffer += string.Format(" WHERE Id = {0};", "?");

                commandbuffer.CommandText = buffer;

                commandbuffer.Connection = connection;
                return commandbuffer;
            }
            else
            {
                return new OdbcCommand();
                throw new Exception("Los objetos no pertenecen a la misma tabla.");
            }

        }

        private string CreateToCommand(PropertyInfo propiedad)
        {

            if (propiedad.Name == "Id")
            {

                return "COUNTER PRIMARY KEY";

            }
            else
            {
                if (propiedad.PropertyType == typeof(string))
                {
                    return "TEXT";
                }
                else if (propiedad.PropertyType == typeof(int) || propiedad.PropertyType == typeof(Int16) || propiedad.PropertyType == typeof(Int64))
                {
                    return "INTEGER";
                }
                else if (propiedad.PropertyType == typeof(double))
                {
                    return "DOUBLE";
                }
                else if (propiedad.PropertyType == typeof(bool))
                {
                    return "BIT";
                }
                else if (propiedad.PropertyType == typeof(DateTime))
                {
                    return "DATETIME";
                }
                else if (propiedad.PropertyType == typeof(byte))
                {
                    return "BYTE";
                }
                else if (propiedad.PropertyType == typeof(float))
                {
                    return "FLOAT";
                }
                else if (propiedad.PropertyType == typeof(decimal))
                {
                    return "DECIMAL";
                }
                else
                {
                    return "BINARY";
                }
            }


        }

        //private bool TableExists(ITable Tabla)
        //{
        //    try
        //    {
        //        connection.Open();
        //        isConected = true;
        //        var exists = connection.GetSchema("Tables", new string[4] { null, null, Tabla.Tabla, "TABLE" }).Rows.Count > 0;
        //        return exists;
        //    }
        //    catch (Exception Error)
        //    {

        //        new HelpLibraryLog(Error, this);
        //        return false;
        //        throw Error;

        //    }
        //    finally
        //    {
        //        connection.Close();
        //        isConected = false;

        //    }



        //}

        //private bool CampoExist(ITable tabla, PropertyInfo campo)
        //{
        //    if (campo.Name != "Id")
        //    {
        //        PropertyAttribute attrib = System.Attribute.GetCustomAttribute(campo, typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase)
        //            {
        //                try
        //                {
        //                    bool prueba = false;
        //                    connection.Open();

        //                    isConected = true;
        //                    var t = connection.GetSchema("Columns", new string[] { null, null, tabla.Tabla, null });

        //                    for (int i = 0; i < t.Rows.Count; i++)
        //                    {

        //                        prueba = prueba || t.Rows[i].ItemArray[3].ToString().Contains(campo.Name);


        //                    }
        //                    connection.Close();
        //                    isConected = false;
        //                    return prueba;
        //                }
        //                catch (Exception exception)
        //                {

        //                    new HelpLibraryLog(exception, this);
        //                    return false;

        //                }
        //                finally
        //                {
        //                    connection.Close();
        //                    isConected = false;
        //                }
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                bool prueba = false;
        //                connection.Open();
        //                isConected = true;
        //                var t = connection.GetSchema("Columns", new string[] { null, null, tabla.Tabla, null });

        //                for (int i = 0; i < t.Rows.Count; i++)
        //                {

        //                    prueba = prueba || t.Rows[i].ItemArray[3].ToString().Contains(campo.Name);


        //                }
        //                connection.Close();
        //                isConected = false;
        //                return prueba;
        //            }
        //            catch (Exception Error)
        //            {
        //                new HelpLibraryLog(Error, this);
        //                return false;
        //                throw Error;

        //            }
        //            finally
        //            {
        //                connection.Close();
        //                isConected = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            bool prueba = false;
        //            connection.Open();
        //            isConected = true;
        //            var t = connection.GetSchema("Columns", new string[] { null, null, tabla.Tabla, null });

        //            for (int i = 0; i < t.Rows.Count; i++)
        //            {

        //                prueba = prueba || t.Rows[i].ItemArray[3].ToString().Contains(campo.Name);


        //            }
        //            connection.Close();
        //            isConected = false;
        //            return prueba;
        //        }
        //        catch (Exception Error)
        //        {

        //            new HelpLibraryLog(Error, this);

        //            return false;
        //            throw Error;

        //        }
        //        finally
        //        {
        //            connection.Close();
        //            isConected = false;
        //        }
        //    }







        //}

        private Type TypeParameter(OdbcType campo)
        {
            switch (campo)
            {
                case OdbcType.BigInt:
                    return typeof(long);
                case OdbcType.Binary:
                    return typeof(byte[]);
                case OdbcType.Bit:
                    return typeof(bool);
                case OdbcType.Char:
                    return typeof(string);
                case OdbcType.Date:
                    return typeof(DateTime);
                case OdbcType.DateTime:
                    return typeof(DateTime);
                case OdbcType.Decimal:
                    return typeof(decimal);
                case OdbcType.Double:
                    return typeof(double);
                case OdbcType.Image:
                    return typeof(byte[]);
                case OdbcType.Int:
                    return typeof(int);
                case OdbcType.NChar:
                    return typeof(string);
                case OdbcType.NText:
                    return typeof(string);
                case OdbcType.Numeric:
                    return typeof(decimal);
                case OdbcType.NVarChar:
                    return typeof(string);
                case OdbcType.Real:
                    return typeof(float);
                case OdbcType.SmallDateTime:
                    return typeof(DateTime);
                case OdbcType.SmallInt:
                    return typeof(short);
                case OdbcType.Text:
                    return typeof(string);
                case OdbcType.Time:
                    return typeof(DateTime);
                case OdbcType.Timestamp:
                    return typeof(byte[]);
                case OdbcType.TinyInt:
                    return typeof(byte);
                case OdbcType.UniqueIdentifier:
                    return typeof(Guid);
                case OdbcType.VarBinary:
                    return typeof(byte[]);
                case OdbcType.VarChar:
                    return typeof(string);
                default:
                    return typeof(object);

            }
        }

        private OdbcType TypeParameter(Type parametre)
        {

            if (parametre == typeof(long))
            {
                return OdbcType.BigInt;
            }
            else if (parametre == typeof(byte))
            {
                return OdbcType.Binary;
            }
            else if (parametre == typeof(bool))
            {
                return OdbcType.Bit;
            }
            else if (parametre == typeof(string))
            {
                return OdbcType.Text;
            }
            else if (parametre == typeof(char))
            {
                return OdbcType.Char;
            }
            else if (parametre == typeof(decimal))
            {
                return OdbcType.Decimal;
            }
            else if (parametre == typeof(short))
            {
                return OdbcType.SmallInt;
            }
            else if (parametre == typeof(float))
            {
                return OdbcType.Real;
            }
            else if (parametre == typeof(DateTime))
            {
                return OdbcType.DateTime;
            }
            else if (parametre == typeof(double))
            {
                return OdbcType.Double;
            }
            else if (parametre == typeof(Guid))
            {
                return OdbcType.UniqueIdentifier;
            }
            else if (parametre == typeof(bool))
            {
                return OdbcType.Bit;
            }
            else if (parametre == typeof(byte[]))
            {
                return OdbcType.VarBinary;
            }
            else
            {
                return OdbcType.VarBinary;
            }

        }

       

        #endregion





        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).

                    cadenaDeConexion = null;
                  
                    if (odbcConnection != null)
                    {

                        odbcConnection.Dispose();
                    }
                    if (sqliteConnection != null)
                    {
                        sqliteConnection.Dispose();
                    }

                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~DatabaseInteractions() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
