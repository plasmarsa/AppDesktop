using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;

namespace AppCostos
{
    public partial class ctrolTornilloDetalle : UserControl
    {
        public List<Insumos> _insumos;
        private bool _modificable { get; set; }
        

        public ctrolTornilloDetalle(bool Modificable,ref List<Insumos> Insumos)
        {
            _insumos = Insumos;
            _modificable = Modificable;
            InitializeComponent();
            InicializarGrid();
            if (_modificable == false)
            {
                //sGrid.Enabled = false;
                txtParticipacion.Enabled = false;
                sGrid.PrimaryGrid.AllowRowInsert = false;
                sGrid.PrimaryGrid.AllowRowResize = false;
                sGrid.PrimaryGrid.AllowEdit = false;
                sGrid.PrimaryGrid.AllowRowDelete = false;
                groupPanel1.BackColor = System.Drawing.Color.LightGray;
            }
            sGrid.PrimaryGrid.SelectionGranularity = SelectionGranularity.Row;

        }

        public ctrolTornilloDetalle()
        {
            InitializeComponent();
            InicializarGrid();

        }

        public ctrolTornilloDetalle(bool Modificable)
        {
            _modificable = Modificable;
            InitializeComponent();
            InicializarGrid();
            if (_modificable == false)
            {
                //sGrid.Enabled = false;
                txtParticipacion.Enabled = false;
                sGrid.PrimaryGrid.AllowRowInsert = false;
                sGrid.PrimaryGrid.AllowRowResize = false;
                sGrid.PrimaryGrid.AllowEdit = false;
                sGrid.PrimaryGrid.AllowRowDelete = false;
                groupPanel1.BackColor = System.Drawing.Color.LightGray;
            }
            sGrid.PrimaryGrid.SelectionGranularity = SelectionGranularity.Row;

        }

        private void InicializarGrid()
        {
            GridPanel panel = sGrid.PrimaryGrid;
            GridColumn colCant = panel.Columns["cantidad"];
            colCant.EditorType = typeof(MyCampoNumerico);

            GridColumn colCodigo = panel.Columns["codigo"];
            colCodigo.EditorType = typeof(ModificarInsumo);
            colCodigo.EditorParams = new object[] { _insumos };
        }

        public class MyNumericEditControl : GridDoubleInputEditControl
        {
            public MyNumericEditControl()
            {
                //DisplayFormat = "N";
            }
        }

        private void txtParticipacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char a = '.';
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == a) && ((sender as TextBox).Text.IndexOf(a) > -1))
            {
                e.Handled = true;
            }


        }

        private void txtParticipacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void ctrolTornilloDetalle_Load(object sender, EventArgs e)
        {

           InicializarGrid();
        }

        private void sGrid_RowDeleted(object sender, GridRowDeletedEventArgs e)
        {
            e.GridPanel.PurgeDeletedRows();
        }
    }

    internal class MyCampoNumerico : GridTextBoxXEditControl
    {
        public MyCampoNumerico()
        {
            KeyPress += Mycamponumerico_KeyPress;
        }
        void Mycamponumerico_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            //char a = '.';
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == a) && ((sender as TextBox).Text.IndexOf(a) > -1))
            {
                e.Handled = true;
            }
        }
    }

    internal class ModificarInsumo: GridComboBoxExEditControl
    {
        List<Insumos> _lstInsumos;
        public ModificarInsumo(List<Insumos> Insumos)
        {
            _lstInsumos = Insumos;
            TextUpdate += CambioInsumo_TextUpdate;
        }
        void CambioInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataSource = _lstInsumos;
        }
        void CambioInsumo_TextUpdate(object sender, EventArgs e)
        {
                string filter_param = this.Text;
                List<Insumos> filteredItems = _lstInsumos.Where(s=>s.Codigo_Insumo.ToLower().Contains(filter_param.ToLower())).ToList();
                this.DataSource = null;
                this.DataSource = filteredItems;
                if (String.IsNullOrWhiteSpace(filter_param))
                {
                    this.DataSource = _lstInsumos;
                }
                Cursor.Current = Cursors.Default;
                this.DisplayMember = "Codigo_Insumo";
                this.ValueMember = "Codigo_Insumo";
                this.SelectedIndex = -1;
                this.DroppedDown = true;
                this.Text = filter_param;
               this.SelectionLength = filter_param.Length;
                this.Select(filter_param.Length, 0);
            //string p = "Inoooo";

        }



    }

    public class Insumos
    {
        public string Codigo_Insumo { get; set; }
        public string Descripcion { get; set; }
    }
}
