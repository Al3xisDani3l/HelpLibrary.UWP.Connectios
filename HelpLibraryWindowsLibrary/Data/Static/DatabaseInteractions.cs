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





        //#region Variables



        //private TypeDb DatabaseCurrent;

        //private string cadenaDeConexion;

        //private string pathDatabase;

        //private string dsn = "MS Odbc Database";

        //private bool isConected = false;

        // connection;

        //#endregion

        //#region Propiedades

        //public bool IsConected => isConected;

        //public OdbcConnection Connection { get => connection; set => connection = value; }

        //public string Dsn
        //{
        //    get
        //    {
        //        return dsn;
        //    }
        //    set
        //    {

        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            if (!string.IsNullOrEmpty(cadenaDeConexion) && string.IsNullOrEmpty(dsn))
        //            {
        //                CadenaDeConexion = cadenaDeConexion.Replace(dsn, value);

        //            }
        //            else if (!string.IsNullOrEmpty(pathDatabase))
        //            {
        //                CadenaDeConexion = string.Format("Dsn={0};dbq={1}", value, pathDatabase);

        //            }
        //            else
        //            {
        //                dsn = value;
        //            }

        //        }
        //    }
        //}

        //public string CadenaDeConexion
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(cadenaDeConexion))
        //        {
        //            return cadenaDeConexion;
        //        }
        //        else
        //        {
        //            return string.Format("Dsn={{0}};dbq={1}", Dsn, pathDatabase);
        //        }

        //    }
        //    set
        //    {

        //        try
        //        {
        //            Connection = new OdbcConnection(value);
        //            Connection.Open();
        //            cadenaDeConexion = value;
        //            int startProvider = value.IndexOf('=');
        //            int endProvider = value.IndexOf(';');
        //            int startPath = value.IndexOf('=', endProvider);
        //            int cadenaLength = value.Length;
        //            dsn = value.Substring(startProvider + 1, (endProvider - startProvider) - 1);
        //            pathDatabase = value.Substring(startPath + 1, (cadenaLength - startPath) - 1);




        //        }
        //        catch (Exception Error)
        //        {
        //            Connection = null;

        //            new HelpLibraryLog(Error, this);
        //            throw Error;
        //        }
        //        finally
        //        {
        //            if (connection != null)
        //            {
        //                Connection.Close();
        //            }

        //        }



        //    }
        //}

        //public string PathDataBase
        //{
        //    get
        //    {
        //        return pathDatabase;
        //    }
        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            if (!string.IsNullOrEmpty(cadenaDeConexion) && string.IsNullOrEmpty(pathDatabase))
        //            {
        //                CadenaDeConexion = cadenaDeConexion.Replace(pathDatabase, value);

        //            }
        //            else if (!string.IsNullOrEmpty(dsn))
        //            {
        //                CadenaDeConexion = string.Format("Dsn={0};dbq={1}", dsn, value);

        //            }

        //            pathDatabase = value;

        //        }

        //    }
        //}
        //#endregion

        //#region Constructores

        //public DatabaseInteractions(OdbcConnection Connection, TypeDb database = TypeDb.Odbc)
        //{
        //    this.connection = Connection;
        //    DatabaseCurrent = database;
        //    cadenaDeConexion = connection.ConnectionString;
        //    try
        //    {
        //        connection.Open();
        //        isConected = true;
        //    }
        //    catch (Exception C1)
        //    {

        //        new HelpLibraryLog(C1, this);
        //        throw C1;

        //    }
        //    finally
        //    {
        //        if (connection != null)
        //        {
        //            connection.Close();
        //        }
        //    }

        //}

        //public DatabaseInteractions(TypeDb database = TypeDb.Odbc)
        //{
        //    DatabaseCurrent = database;
        //    isConected = false;

        //}

        //public DatabaseInteractions(string CadenaDeConexion, TypeDb database = TypeDb.Odbc)
        //{
        //    this.CadenaDeConexion = CadenaDeConexion;

        //}
        //#endregion


        //#region Funciones

        //public void Close()
        //{
        //    if (Connection != null)
        //    {
        //        Connection.Close();
        //        isConected = false;
        //    }
        //}


        //public bool Conectar(bool isAutomatico = true)
        //{


        //    if (!isAutomatico)
        //    {
        //        if (!string.IsNullOrWhiteSpace(this.dsn) || !string.IsNullOrWhiteSpace(this.pathDatabase))
        //        {
        //            try
        //            {
        //                Connection = new OdbcConnection(string.Format("Dsn={0};dbq={1}", Dsn, pathDatabase));
        //                Connection.Open();
        //                isConected = true;

        //                this.cadenaDeConexion = string.Format("Dsn={0};dbq={1}", Dsn, pathDatabase);
        //                return true;
        //            }
        //            catch (Exception Error)
        //            {
        //                new HelpLibraryLog(Error, this);

        //                return false;
        //                throw Error;

        //            }
        //            finally
        //            {
        //                Connection.Close();
        //                isConected = false;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //            throw new Exception("No se ha especificado una cadena de connection o una ruta valida, public void Conectar();");
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrWhiteSpace(this.cadenaDeConexion))
        //        {
        //            try
        //            {
        //                Connection = new OdbcConnection(CadenaDeConexion);
        //                Connection.Open();

        //                return true;
        //            }
        //            catch (Exception Error)
        //            {

        //                new HelpLibraryLog(Error, this);
        //                return false;

        //            }
        //            finally
        //            {
        //                Connection.Close();
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //            throw new Exception("No se ha especificado una cadena de connection o una ruta valida, public void Conectar();");
        //        }

        //    }

        //}

        //public async void InsertarAsync(ITable Objeto)
        //{

        //    try
        //    {
        //        using (OdbcCommand Comando = InsertToCommand(Objeto, Connection))
        //        {



        //            Connection.Open();
        //            isConected = true;
        //            await Comando.ExecuteNonQueryAsync();

        //        }
        //    }
        //    catch (OdbcException Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }

        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;

        //    }






        //}

        //public async void InsertarAsync(ITable[] Objetos)
        //{

        //    try
        //    {

        //        Connection.Open();
        //        isConected = true;
        //        for (int i = 0; i < Objetos.Length; i++)
        //        {
        //            using (OdbcCommand Comando = InsertToCommand(Objetos[i], connection))
        //            {


        //                await Comando.ExecuteNonQueryAsync();
        //            }
        //        }
        //        Connection.Close();
        //        isConected = false;
        //    }
        //    catch (OdbcException Error)
        //    {

        //        new HelpLibraryLog(Error, this);
        //        throw Error;

        //    }

        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;
        //    }

        //}

        //public async void DeleteAsync(ITable Objeto)
        //{

        //    string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objeto.Tabla);
        //    try
        //    {
        //        using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //        {
        //            OdbcParameter parameter = new OdbcParameter("Id", Objeto.Id);
        //            comando.Parameters.Add(parameter);

        //            Connection.Open();
        //            isConected = true;
        //            await comando.ExecuteNonQueryAsync();
        //        }
        //    }
        //    catch (OdbcException Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }
        //    finally
        //    {

        //        Connection.Close();
        //        isConected = false;
        //    }

        //}

        //public async void DeleteAsync(ITable[] Objetos)
        //{

        //    try
        //    {
        //        for (int i = 0; i < Objetos.Length; i++)
        //        {
        //            string cmd = string.Format("DELETE FROM {0} WHERE Id = ?", Objetos[i].Tabla);

        //            using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //            {


        //                OdbcParameter parameter = new OdbcParameter("Id", Objetos[i].Id);
        //                comando.Parameters.Add(parameter);
        //                Connection.Open();
        //                isConected = true;
        //                await comando.ExecuteNonQueryAsync();

        //            }
        //        }

        //    }
        //    catch (OdbcException Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;

        //    }

        //}

        //public async Task<List<T>> ReadAsync<T>(string filtro) where T : ITable, new()
        //{





        //    List<T> retorno = new List<T>();
        //    T buf = new T();

        //    string cmd = string.Format("SELECT * FROM {0} WHERE {1}", buf.Tabla, filtro);

        //    try
        //    {
        //        using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //        {
        //            Connection.Open();
        //            isConected = true;
        //            DbDataReader lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

        //            while (lectura.Read())
        //            {

        //                T buffer = new T();
        //                for (int i = 0; i < buffer.Propiedades.Count; i++)
        //                {

        //                    PropertyAttribute attrib = System.Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //                    if (attrib != null)
        //                    {
        //                        if (attrib.AddToDataBase)
        //                        {
        //                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
        //                    }



        //                }

        //                retorno.Add(buffer);

        //            }

        //            return retorno;

        //        }
        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        return new List<T>();
        //        throw Error;
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;

        //    }

        //}

        //public async Task<List<T>> ReadAsync<T>() where T : ITable, new()
        //{





        //    List<T> retorno = new List<T>();
        //    T temp = new T();

        //    string cmd = string.Format("SELECT * FROM {0}", temp.Tabla);

        //    try
        //    {
        //        using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //        {
        //            Connection.Open();
        //            isConected = true;
        //            var lectura = await comando.ExecuteReaderAsync().ConfigureAwait(false);

        //            while (lectura.Read())
        //            {

        //                T buffer = new T();
        //                for (int i = 0; i < buffer.Propiedades.Count; i++)
        //                {

        //                    PropertyAttribute attrib = System.Attribute.GetCustomAttribute(buffer.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //                    if (attrib != null)
        //                    {
        //                        if (attrib.AddToDataBase)
        //                        {
        //                            buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        buffer.Propiedades[i].SetValue(buffer, Convert.ChangeType(lectura[buffer.Propiedades[i].Name], buffer.Propiedades[i].PropertyType));
        //                    }





        //                }

        //                retorno.Add(buffer);

        //            }

        //            return retorno;

        //        }
        //    }
        //    catch (Exception Error)
        //    {

        //        new HelpLibraryLog(Error, this);
        //        return new List<T>();
        //        throw Error;

        //    }
        //    finally
        //    {

        //        Connection.Close();
        //        isConected = false;

        //    }

        //}

        //public async void UpdateAsyn(ITable Viejo, ITable Nuevo)
        //{




        //    try
        //    {
        //        using (OdbcCommand command = UpdateToCommand(Viejo, Nuevo, Connection))
        //        {
        //            Connection.Open();
        //            isConected = true;
        //            await command.ExecuteNonQueryAsync();

        //        }
        //    }
        //    catch (Exception Error)
        //    {

        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }
        //    finally
        //    {

        //        Connection.Close();
        //        isConected = false;
        //    }

        //}

        //public async void CreateTable<T>() where T : ITable, new()
        //{





        //    try
        //    {
        //        T buffer = new T();
        //        string cmd = string.Format("CREATE TABLE {0} (", buffer.Tabla);

        //        for (int i = 0; i < buffer.Propiedades.Count; i++)
        //        {
        //            if (buffer.Propiedades[i].Name != "Tabla" && buffer.Propiedades[i].Name != "Propiedades")
        //            {
        //                cmd += string.Format("[{0}] {1},", buffer.Propiedades[i].Name, CreateToCommand(buffer.Propiedades[i]));
        //            }

        //        }
        //        cmd.Remove(cmd.Length - 1);
        //        cmd += ");";

        //        using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //        {
        //            Connection.Open();
        //            isConected = true;
        //            await comando.ExecuteNonQueryAsync();


        //        }

        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }

        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;
        //    }



        //}

        //public async void CreateTable(string cadenaDeConexion, ITable tabla)
        //{

        //    try
        //    {
        //        string cmd = string.Format("CREATE TABLE {0} (", tabla.Tabla);

        //        for (int i = 0; i < tabla.Propiedades.Count; i++)
        //        {
        //            if (tabla.Propiedades[i].Name != "Tabla" && tabla.Propiedades[i].Name != "Propiedades")
        //            {
        //                if (i == tabla.Propiedades.Count - 1)
        //                {
        //                    cmd += string.Format("[{0}] {1}", tabla.Propiedades[i].Name, CreateToCommand(tabla.Propiedades[i]));
        //                }
        //                else
        //                {
        //                    cmd += string.Format("[{0}] {1},", tabla.Propiedades[i].Name, CreateToCommand(tabla.Propiedades[i]));
        //                }
        //            }

        //        }

        //        cmd += ");";

        //        using (OdbcConnection conexionTemp = new OdbcConnection(cadenaDeConexion))
        //        {
        //            using (OdbcCommand comandon = new OdbcCommand(cmd, conexionTemp))
        //            {
        //                try
        //                {
        //                    conexionTemp.Open();
        //                    isConected = true;
        //                    await comandon.ExecuteNonQueryAsync();

        //                }
        //                catch (Exception Error)
        //                {

        //                    new HelpLibraryLog(Error, this);
        //                }
        //                finally
        //                {
        //                    conexionTemp.Close();
        //                    isConected = false;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        new HelpLibraryLog(error, this);
        //        throw error;
        //    }

        //}

        //public async void CreateTable(OdbcConnection cadenaDeConexion, ITable tabla)
        //{

        //    try
        //    {
        //        string cmd = string.Format("CREATE TABLE {0} (", tabla.Tabla);

        //        for (int i = 0; i < tabla.Propiedades.Count; i++)
        //        {
        //            if (tabla.Propiedades[i].Name != "Tabla" && tabla.Propiedades[i].Name != "Propiedades")
        //            {
        //                if (i == tabla.Propiedades.Count - 1)
        //                {
        //                    cmd += string.Format("[{0}] {1}", tabla.Propiedades[i].Name, CreateToCommand(tabla.Propiedades[i]));
        //                }
        //                else
        //                {
        //                    cmd += string.Format("[{0}] {1},", tabla.Propiedades[i].Name, CreateToCommand(tabla.Propiedades[i]));
        //                }
        //            }

        //        }

        //        cmd += ");";

        //        try
        //        {
        //            using (OdbcCommand comandon = new OdbcCommand(cmd, cadenaDeConexion))
        //            {

        //                cadenaDeConexion.Open();
        //                isConected = true;
        //                await comandon.ExecuteNonQueryAsync();


        //            }
        //        }
        //        catch (Exception Error)
        //        {
        //            new HelpLibraryLog(Error, this);
        //            throw Error;
        //        }
        //        finally
        //        {
        //            cadenaDeConexion.Close();
        //            isConected = false;
        //        }



        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;

        //    }
        //    finally
        //    {
        //        cadenaDeConexion.Close();
        //    }

        //}

        //public async void CreateCampo(string tabla, PropertyInfo campo)
        //{
        //    string cmd = string.Format("ALTER TABLE {0} ADD {1} {2};", tabla, campo.Name, CreateToCommand(campo));
        //    try
        //    {
        //        using (OdbcCommand comando = new OdbcCommand(cmd, Connection))
        //        {

        //            Connection.Open();
        //            isConected = true;
        //            await comando.ExecuteNonQueryAsync();
        //        }
        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //        isConected = false;
        //    }



        //}

        //public void NormalizeDatabase(params ITable[] Tablas)
        //{
        //    try
        //    {



        //        for (int i = 0; i < Tablas.Length; i++)
        //        {
        //            NormalizeTable(Tablas[i]);
        //        }





        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }
        //    finally
        //    {
        //        this.Connection.Close();
        //        isConected = false;
        //    }
        //}

        //public void NormalizeTable(ITable modelo)
        //{

        //    try
        //    {
        //        if (TableExists(modelo))
        //        {
        //            foreach (PropertyInfo item in modelo.Propiedades)
        //            {

        //                if (!CampoExist(modelo, item))
        //                {
        //                    CreateCampo(modelo.Tabla, item);
        //                }


        //            }

        //        }
        //        else
        //        {
        //            CreateTable(this.Connection, modelo);
        //        }


        //    }
        //    catch (Exception Error)
        //    {
        //        new HelpLibraryLog(Error, this);
        //        throw Error;
        //    }






        //}




        //#endregion
        //#region Funciones internas

        //private string InsertToCommand(ITable dateable)
        //{

        //    string buffer = string.Format("INSERT INTO {0} (", dateable.Tabla);
        //    for (int i = 0; i < dateable.Propiedades.Count; i++)
        //    {

        //        PropertyAttribute attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase == true)
        //            {
        //                if (dateable.Propiedades[i].Name != "Id")
        //                {
        //                    buffer += string.Format("{0},", dateable.Propiedades[i].Name);
        //                }

        //            }
        //        }
        //        else
        //        {
        //            buffer += string.Format("{0},", dateable.Propiedades[i].Name);
        //        }





        //    }

        //    buffer = buffer.Remove(buffer.Length - 1);
        //    buffer += ")";
        //    buffer += " VALUES(";

        //    for (int i = 0; i < dateable.Propiedades.Count; i++)
        //    {
        //        var attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase)
        //            {
        //                if (dateable.Propiedades[i].Name != "Id")
        //                {
        //                    if (dateable.Propiedades[i].PropertyType == typeof(DateTime))
        //                    {

        //                        DateTime date = Convert.ToDateTime(dateable.Propiedades[i].GetValue(dateable));
        //                        string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
        //                        buffer += string.Format("\"{0}\",", bf);

        //                    }
        //                    else
        //                    {
        //                        if (dateable.Propiedades[i].PropertyType == typeof(string))
        //                        {
        //                            buffer += string.Format("\"{0}\",", dateable.Propiedades[i].GetValue(dateable).ToString());
        //                        }
        //                        else
        //                        {
        //                            buffer += string.Format("{0},", dateable.Propiedades[i].GetValue(dateable).ToString());
        //                        }

        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {

        //            if (dateable.Propiedades[i].PropertyType == typeof(DateTime))
        //            {

        //                DateTime date = Convert.ToDateTime(dateable.Propiedades[i].GetValue(dateable));
        //                string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
        //                buffer += string.Format("\"{0}\",", bf);

        //            }
        //            else
        //            {
        //                if (dateable.Propiedades[i].PropertyType == typeof(string))
        //                {
        //                    buffer += string.Format("\"{0}\",", dateable.Propiedades[i].GetValue(dateable).ToString());
        //                }
        //                else
        //                {
        //                    buffer += string.Format("{0},", dateable.Propiedades[i].GetValue(dateable).ToString());
        //                }

        //            }
        //        }




        //    }

        //    buffer = buffer.Remove(buffer.Length - 1);
        //    buffer += ");";

        //    return buffer;

        //}

        //private OdbcCommand InsertToCommand(ITable dateable, OdbcConnection connection)
        //{

        //    var commandBuffer = new OdbcCommand();

        //    commandBuffer.Connection = connection;

        //    string buffer = string.Format("INSERT INTO {0} (", dateable.Tabla);
        //    for (int i = 0; i < dateable.Propiedades.Count; i++)
        //    {

        //        PropertyAttribute attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase == true)
        //            {
        //                if (dateable.Propiedades[i].Name != "Id")
        //                {
        //                    buffer += string.Format("{0},", dateable.Propiedades[i].Name);
        //                }

        //            }
        //        }
        //        else
        //        {
        //            buffer += string.Format("{0},", dateable.Propiedades[i].Name);
        //        }





        //    }

        //    buffer = buffer.Remove(buffer.Length - 1);
        //    buffer += ")";
        //    buffer += " VALUES(";

        //    for (int i = 0; i < dateable.Propiedades.Count; i++)
        //    {
        //        var attrib = System.Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase)
        //            {
        //                if (dateable.Propiedades[i].Name != "Id")
        //                {

        //                    buffer += string.Format("{0},", "?");


        //                }

        //            }
        //        }
        //        else
        //        {
        //            buffer += string.Format("{0},", "?");
        //        }




        //    }

        //    buffer = buffer.Remove(buffer.Length - 1);
        //    buffer += ");";

        //    commandBuffer.CommandText = buffer;

        //    for (int i = 0; i < dateable.Propiedades.Count; i++)
        //    {
        //        var attrib = Attribute.GetCustomAttribute(dateable.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;
        //        if (attrib != null)
        //        {
        //            if (attrib.AddToDataBase)
        //            {
        //                if (dateable.Propiedades[i].Name != "Id")
        //                {
        //                    var parameter = new OdbcParameter(dateable.Propiedades[i].Name, dateable.Propiedades[i].GetValue(dateable));
        //                    commandBuffer.Parameters.Add(parameter);
        //                }

        //            }

        //        }
        //        else
        //        {
        //            var parameter = new OdbcParameter(dateable.Propiedades[i].Name, dateable.Propiedades[i].GetValue(dateable));
        //            commandBuffer.Parameters.Add(parameter);
        //        }
        //    }

        //    return commandBuffer;

        //}

        //private string UpdateToCommand(ITable Viejo, ITable Nuevo)
        //{
        //    string buffer = string.Format("UPDATE {0} SET ", Nuevo.Tabla);
        //    if (Viejo.Tabla == Nuevo.Tabla)
        //    {
        //        for (int i = 0; i < Nuevo.Propiedades.Count; i++)
        //        {
        //            var attrib = System.Attribute.GetCustomAttribute(Nuevo.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;

        //            if (attrib != null)
        //            {
        //                if (attrib.AddToExcel)
        //                {
        //                    if (Nuevo.Propiedades[i].PropertyType == typeof(DateTime))
        //                    {

        //                        DateTime date = Convert.ToDateTime(Nuevo.Propiedades[i].GetValue(Nuevo));
        //                        string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
        //                        buffer += string.Format("{0}=\'{1}\', ", Nuevo.Propiedades[i].Name, bf);

        //                    }
        //                    else
        //                    {
        //                        if (Nuevo.Propiedades[i].PropertyType == typeof(string))
        //                        {
        //                            buffer += string.Format("{0}=\"{1}\", ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
        //                        }
        //                        else
        //                        {
        //                            buffer += string.Format("{0}={1}, ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
        //                        }

        //                    }
        //                }

        //            }
        //            else
        //            {

        //                if (Nuevo.Propiedades[i].PropertyType == typeof(DateTime))
        //                {

        //                    DateTime date = Convert.ToDateTime(Nuevo.Propiedades[i].GetValue(Nuevo));
        //                    string bf = date.ToString("yyyy-MM-dd HH:mm:ss");
        //                    buffer += string.Format("{0}=\'{1}\', ", Nuevo.Propiedades[i].Name, bf);

        //                }
        //                else
        //                {
        //                    if (Nuevo.Propiedades[i].PropertyType == typeof(string))
        //                    {
        //                        buffer += string.Format("{0}=\"{1}\", ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
        //                    }
        //                    else
        //                    {
        //                        buffer += string.Format("{0}={1}, ", Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo).ToString());
        //                    }

        //                }
        //            }


        //        }

        //        buffer = buffer.Remove(buffer.Length - 2);



        //        buffer += string.Format(" WHERE Id={0};", Viejo.Id);
        //        return buffer;
        //    }
        //    else
        //    {
        //        return "";
        //        throw new Exception("Los objetos no pertenecen a la misma tabla.");
        //    }

        //}

        //private OdbcCommand UpdateToCommand(ITable Viejo, ITable Nuevo, OdbcConnection connection)
        //{
        //    var commandbuffer = new OdbcCommand();


        //    string buffer = string.Format("UPDATE {0} SET ", Nuevo.Tabla);
        //    if (Viejo.Tabla == Nuevo.Tabla)
        //    {
        //        for (int i = 0; i < Nuevo.Propiedades.Count; i++)
        //        {
        //            var attrib = System.Attribute.GetCustomAttribute(Nuevo.Propiedades[i], typeof(PropertyAttribute)) as PropertyAttribute;

        //            if (attrib != null)
        //            {
        //                if (attrib.AddToDataBase)
        //                {
        //                    if (Nuevo.Propiedades[i].Name != "Id")
        //                    {
        //                        OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo));
        //                        buffer += string.Format("{0}=?,", Nuevo.Propiedades[i].Name);
        //                        commandbuffer.Parameters.Add(parameter);

        //                    }
        //                    else
        //                    {
        //                        OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Viejo));
        //                        commandbuffer.Parameters.Add(parameter);
        //                    }
        //                }

        //            }
        //            else
        //            {

        //                OdbcParameter parameter = new OdbcParameter(Nuevo.Propiedades[i].Name, Nuevo.Propiedades[i].GetValue(Nuevo));
        //                buffer += string.Format("{0} = ?,", Nuevo.Propiedades[i].Name);
        //                commandbuffer.Parameters.Add(parameter);
        //            }


        //        }

        //        buffer = buffer.Remove(buffer.Length - 1);



        //        buffer += string.Format(" WHERE Id = {0};", "?");

        //        commandbuffer.CommandText = buffer;

        //        commandbuffer.Connection = connection;
        //        return commandbuffer;
        //    }
        //    else
        //    {
        //        return new OdbcCommand();
        //        throw new Exception("Los objetos no pertenecen a la misma tabla.");
        //    }

        //}

        //private string CreateToCommand(PropertyInfo propiedad)
        //{

        //    if (propiedad.Name == "Id")
        //    {

        //        return "COUNTER PRIMARY KEY";

        //    }
        //    else
        //    {
        //        if (propiedad.PropertyType == typeof(string))
        //        {
        //            return "TEXT";
        //        }
        //        else if (propiedad.PropertyType == typeof(int) || propiedad.PropertyType == typeof(Int16) || propiedad.PropertyType == typeof(Int64))
        //        {
        //            return "INTEGER";
        //        }
        //        else if (propiedad.PropertyType == typeof(double))
        //        {
        //            return "DOUBLE";
        //        }
        //        else if (propiedad.PropertyType == typeof(bool))
        //        {
        //            return "BIT";
        //        }
        //        else if (propiedad.PropertyType == typeof(DateTime))
        //        {
        //            return "DATETIME";
        //        }
        //        else if (propiedad.PropertyType == typeof(byte))
        //        {
        //            return "BYTE";
        //        }
        //        else if (propiedad.PropertyType == typeof(float))
        //        {
        //            return "FLOAT";
        //        }
        //        else if (propiedad.PropertyType == typeof(decimal))
        //        {
        //            return "DECIMAL";
        //        }
        //        else
        //        {
        //            return "BINARY";
        //        }
        //    }


        //}

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

        //private Type TypeParameter(OdbcType campo)
        //{
        //    switch (campo)
        //    {
        //        case OdbcType.BigInt:
        //            return typeof(long);
        //        case OdbcType.Binary:
        //            return typeof(byte[]);
        //        case OdbcType.Bit:
        //            return typeof(bool);
        //        case OdbcType.Char:
        //            return typeof(string);
        //        case OdbcType.Date:
        //            return typeof(DateTime);
        //        case OdbcType.DateTime:
        //            return typeof(DateTime);
        //        case OdbcType.Decimal:
        //            return typeof(decimal);
        //        case OdbcType.Double:
        //            return typeof(double);
        //        case OdbcType.Image:
        //            return typeof(byte[]);
        //        case OdbcType.Int:
        //            return typeof(int);
        //        case OdbcType.NChar:
        //            return typeof(string);
        //        case OdbcType.NText:
        //            return typeof(string);
        //        case OdbcType.Numeric:
        //            return typeof(decimal);
        //        case OdbcType.NVarChar:
        //            return typeof(string);
        //        case OdbcType.Real:
        //            return typeof(float);
        //        case OdbcType.SmallDateTime:
        //            return typeof(DateTime);
        //        case OdbcType.SmallInt:
        //            return typeof(short);
        //        case OdbcType.Text:
        //            return typeof(string);
        //        case OdbcType.Time:
        //            return typeof(DateTime);
        //        case OdbcType.Timestamp:
        //            return typeof(byte[]);
        //        case OdbcType.TinyInt:
        //            return typeof(byte);
        //        case OdbcType.UniqueIdentifier:
        //            return typeof(Guid);
        //        case OdbcType.VarBinary:
        //            return typeof(byte[]);
        //        case OdbcType.VarChar:
        //            return typeof(string);
        //        default:
        //            return typeof(object);

        //    }
        //}

        //private OdbcType TypeParameter(Type parametre)
        //{

        //    if (parametre == typeof(long))
        //    {
        //        return OdbcType.BigInt;
        //    }
        //    else if (parametre == typeof(byte))
        //    {
        //        return OdbcType.Binary;
        //    }
        //    else if (parametre == typeof(bool))
        //    {
        //        return OdbcType.Bit;
        //    }
        //    else if (parametre == typeof(string))
        //    {
        //        return OdbcType.Text;
        //    }
        //    else if (parametre == typeof(char))
        //    {
        //        return OdbcType.Char;
        //    }
        //    else if (parametre == typeof(decimal))
        //    {
        //        return OdbcType.Decimal;
        //    }
        //    else if (parametre == typeof(short))
        //    {
        //        return OdbcType.SmallInt;
        //    }
        //    else if (parametre == typeof(float))
        //    {
        //        return OdbcType.Real;
        //    }
        //    else if (parametre == typeof(DateTime))
        //    {
        //        return OdbcType.DateTime;
        //    }
        //    else if (parametre == typeof(double))
        //    {
        //        return OdbcType.Double;
        //    }
        //    else if (parametre == typeof(Guid))
        //    {
        //        return OdbcType.UniqueIdentifier;
        //    }
        //    else if (parametre == typeof(bool))
        //    {
        //        return OdbcType.Bit;
        //    }
        //    else if (parametre == typeof(byte[]))
        //    {
        //        return OdbcType.VarBinary;
        //    }
        //    else
        //    {
        //        return OdbcType.VarBinary;
        //    }

        //}

        //#endregion





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
