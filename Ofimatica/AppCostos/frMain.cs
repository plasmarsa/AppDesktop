using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Dataccess;
namespace AppCostos
{
    public partial class frMain : Form
    {
        Properties.Settings _configuracion = new Properties.Settings();
        OfimaticaDBContext dbContext;
        string strUsuarioOfimatica;
        string strEmpresaOfimatica;
        public frMain()
        {
            InitializeComponent();
        }


        #region Coneccion a sql server
        private void ConexionDB()
        {
            string[] Argumentos = Environment.GetCommandLineArgs();
            if (Argumentos.Length > 1)
            {
                dbContext = new OfimaticaDBContext();
                dbContext = new OfimaticaDBContext(ModificarDBConexion(_configuracion.ServidorSQL, Argumentos[2], _configuracion.UsuarioDB, _configuracion.PasswordUsuarioDB));
                strUsuarioOfimatica = Argumentos[3].ToUpper();
                strEmpresaOfimatica = Argumentos[2].ToUpper();
            }
            else
            {
                dbContext = new OfimaticaDBContext(ModificarDBConexion(_configuracion.ServidorSQL, _configuracion.BasedeDatosSQL, _configuracion.UsuarioDB, _configuracion.PasswordUsuarioDB));
                strUsuarioOfimatica = _configuracion.UsuarioDB;
                strEmpresaOfimatica = _configuracion.BasedeDatosSQL.ToUpper();
            }
        }

        
        private String ModificarDBConexion(String Servidor, String Database, string DBUser, string DBPassword)
        {
            //Data Source=hestia;Initial Catalog=PLASMARSA;Persist Security Info=True;User ID=ofimatica;Password=ofimatica
            String connString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", Servidor, Database, DBUser, DBPassword);
            return connString;
        }
        #endregion


        private void explosiónMaterialesExtruderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = new frMatCostos(ref dbContext) {Text="Rollos Extrusion - Materiales Mezcla" };
            obj.MdiParent = this;
            obj.StartPosition = FormStartPosition.CenterParent;
            obj.Show();

        }

        private void frMain_Load(object sender, EventArgs e)
        {
            try
            {
                ConexionDB();
                this.Text += string.Format("    ...[Usuario: {0} -- Empresa: {1}]", strUsuarioOfimatica, strEmpresaOfimatica);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Se recibio el siguiente mensaje al tratar de conectarse con el servidor de datos:{0}", ex.Message), "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }
    }
}
