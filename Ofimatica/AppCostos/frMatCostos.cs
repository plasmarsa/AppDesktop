using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using Dataccess;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using DevComponents.DotNetBar;
using System.Collections;  

using NsExcel = Microsoft.Office.Interop.Excel;

namespace AppCostos
{
    
    public partial class frMatCostos : Office2007Form
    {

        //this.ctrolModT1 = new AppCostos.ctrolTornilloDetalle(;


        OfimaticaDBContext dbcontext;
        private Background _Background1 = new Background(Color.White, Color.FromArgb(238, 244, 251), 45);
        char s = char.Parse(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        string strPedido,strMaquina,strCtrabajo;
        DateTime dFecha;
        decimal decCantidad;
        List<Insumos> insumos;
        int idLineaGrid;
        List<Resumen> Resumen;









        public frMatCostos(ref OfimaticaDBContext DbContext)
        {
            CultureInfo _culture = new CultureInfo("es-CO");
            //Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = _culture;
            dbcontext = DbContext;
            insumos = dbcontext.MTMERCIA.Where(x => x.TIPOINV == "MP" && x.HABILITADO == true && new String[] { "0104", "0109","0101","0110" }.Contains(x.CODLINEA)).Select(s => new Insumos() { Codigo_Insumo = s.CODIGO.Trim(), Descripcion = s.DESCRIPCIO.TrimEnd() }).ToList();
            InitializeComponent();
            sGridAuditoria.SelectionChanged += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridEventArgs>(this.sGridAuditoria_SelectionChanged);


        }
        public frMatCostos()
        {
            InitializeComponent();
        }

        private void frMatCostos_Load(object sender, EventArgs e)
        {
            InicializarGrids();
            CargarAuditoriasPendientes();
        }

        private void sGridAuditoria_SelectionChanged(object sender, DevComponents.DotNetBar.SuperGrid.GridEventArgs e)
        {
            InicializarControles();
            GridRow linea = sGridAuditoria.PrimaryGrid.Rows[e.GridPanel.ActiveRow.FullIndex] as GridRow;
            CargarDetallesAuditoriaPendiente(linea);
        }

        private void InicializarGrids()
        {
            sGridAuditoria.PrimaryGrid.Rows.Clear();
            sGridAuditoria.PrimaryGrid.Columns["pedido"].FilterAutoScan = true;
            ctrolModT1.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT1.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT1.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT2.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT2.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT2.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT3.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT3.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT3.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT4.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT4.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT4.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT5.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT5.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT5.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT6.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT6.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT6.sGrid.PrimaryGrid.AllowEdit = true;
            ctrolModT7.sGrid.PrimaryGrid.AllowRowDelete = true;
            ctrolModT7.sGrid.PrimaryGrid.AllowRowInsert = true;
            ctrolModT7.sGrid.PrimaryGrid.AllowEdit = true;

            GridPanel panelResumeninsumos = sGridResumenInsumos.PrimaryGrid;
            GridColumn colUltCompra = panelResumeninsumos.Columns["ultCompra"];
            GridColumn colTotal= panelResumeninsumos.Columns["total"];
            GridColumn colPesoResumen= panelResumeninsumos.Columns["cantidad"];
            
            colUltCompra.EditorType = typeof(MyCurrencyEditControl);
            colTotal.EditorType = typeof(MyCurrencyEditControl);
            colPesoResumen.EditorType = typeof(MyNumericEditControl);
            

            GridPanel panelAuditoriaPendiente = sGridAuditoria.PrimaryGrid;
            GridColumn colPeso = panelAuditoriaPendiente.Columns["peso"];
            GridColumn colCtrabajo = panelAuditoriaPendiente.Columns["ctrabajo"];
            colPeso.EditorType = typeof(MyNumericEditControl);
            colCtrabajo.CellStyles.Default.Alignment = Alignment.MiddleCenter;

            GridPanel panelRollosAuditoria = sGridRollos.PrimaryGrid;
            GridColumn colPesoRollo = panelRollosAuditoria.Columns["cantrollo"];
            GridColumn colTurno = panelRollosAuditoria.Columns["turno"];
            colPesoRollo.EditorType = typeof(MyNumericEditControl);
            colTurno.CellStyles.Default.Alignment = Alignment.MiddleCenter;



        }

        public class MyCurrencyEditControl : GridDoubleInputEditControl
        {
            public MyCurrencyEditControl()
            {
                DisplayFormat = "C";
            }
        }
        public class MyNumericEditControl : GridDoubleInputEditControl
        {
            public MyNumericEditControl()
            {
                DisplayFormat = "N";
            }
        }


        private void CalcularConsumo()
        {
            
            decimal parTornilloTotal = 0;
            decimal partTornillo1 = 0;
            decimal partTornillo2 = 0;
            decimal partTornillo3 = 0;
            decimal partTornillo4 = 0;
            decimal partTornillo5 = 0;
            decimal partTornillo6 = 0;
            decimal partTornillo7 = 0;
            List<InsumoCalculado> InsumosCalculados = new List<InsumoCalculado>();
            sGridResumenInsumos.PrimaryGrid.Rows.Clear();

            if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out partTornillo1))
            {
                parTornilloTotal += partTornillo1;
            }
            if (decimal.TryParse(ctrolModT2.txtParticipacion.Text, out partTornillo2))
            {
                parTornilloTotal += partTornillo2;
            }
            if (decimal.TryParse(ctrolModT3.txtParticipacion.Text, out partTornillo3))
            {
                parTornilloTotal += partTornillo3;
            }
            if (decimal.TryParse(ctrolModT4.txtParticipacion.Text, out partTornillo4))
            {
                parTornilloTotal += partTornillo4;
            }
            if (decimal.TryParse(ctrolModT5.txtParticipacion.Text, out partTornillo5))
            {
                parTornilloTotal += partTornillo5;
            }
            if (decimal.TryParse(ctrolModT6.txtParticipacion.Text, out partTornillo6))
            {
                parTornilloTotal += partTornillo6;
            }
            if (decimal.TryParse(ctrolModT7.txtParticipacion.Text, out partTornillo7))
            {
                parTornilloTotal += partTornillo7;
            }

            if (parTornilloTotal == 0)
                return;

            partTornillo1 = partTornillo1 / parTornilloTotal;
            partTornillo2 = partTornillo2 / parTornilloTotal;
            partTornillo3 = partTornillo3 / parTornilloTotal;
            partTornillo4 = partTornillo4 / parTornilloTotal;
            partTornillo5 = partTornillo5 / parTornilloTotal;
            partTornillo6 = partTornillo6 / parTornilloTotal;
            partTornillo7 = partTornillo7 / parTornilloTotal;


            decimal parInsumo = 0;
            decimal totalparticipacion;
            string Insumo;


            //Recorrer tornillo 1
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT1.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT1.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo1;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 2
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT2.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT2.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT2.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo2;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 3
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT3.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT3.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT3.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo3;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 4
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT4.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT4.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT4.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo4;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 5
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT5.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT5.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo5;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 6
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT6.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT6.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT6.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo6;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 7
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT7.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT7.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT7.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo7;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            MostrarResumenConsumo(InsumosCalculados);
        }
        private DateTime ValidaFechaTurno(DateTime FechaInicial,string Turno ,string Hora)
        {
            DateTime fecha = FechaInicial;
            //int hora,minuto;
            
            Hora=Hora.Replace('.', ',');
            var p = Hora.Split(',');
            int limite=int.Parse(string.Format("{0}{1}",p[0],p[1]));

            if (Turno.Trim()=="3" && (limite>=0 && limite<=545) )
            {
                fecha=fecha.AddDays(-1);
                fecha = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
                return fecha;
            }

            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0); 
        }

        private DateTime ValidaFechaTurno(DateTime FechaInicial, string Turno)
        {
            DateTime fecha = FechaInicial;
            int aux = int.Parse(string.Format("{0}{1}", FechaInicial.Hour.ToString(), FechaInicial.Minute.ToString()));
            
            if (Turno.Trim()=="3" && aux>=0 && aux<=545)
            {
                fecha=fecha.AddDays(-1);
                fecha = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
                return fecha;
            }

            return new DateTime(fecha.Year,fecha.Month,fecha.Day,0,0,0);
        }

        private string CalculaTurno(DateTime FechaHora)
        {
            int valor;
            if(int.TryParse(string.Format("{0}{1}", FechaHora.Hour, FechaHora.Minute),out valor))
            {
                if (valor >= 546 && valor <= 1345)
                    return "1";
                if (valor >= 1346 && valor <= 2045)
                    return "2";

            }
            return "3";
        }
        private void CargarAuditoriasPendientes()
        {
            Resumen = new List<Resumen>();
            var o = dbcontext.tblMezclaRolloCentroPesaje.Join(dbcontext.ETQPLASMAR, piso => new { IdCtrolPiso = piso.IdCtrolPiso }, t1 => new { IdCtrolPiso = t1.IDETIQUETA }, (t0, t1) => new { centro = t0, ctrol = t1 })
                    .Join(dbcontext.MTMERCIA, pesaje => pesaje.ctrol.CODIGO, m => m.CODIGO, (pesaje, m) => new { pesaje = pesaje, m = m })
                    .Where(temp1 => (temp1.pesaje.centro.Aprobado == false) && (temp1.pesaje.ctrol.COLA == false))
                    .Select(s => new
                    {
                        HORA = s.pesaje.ctrol.HORA,
                        IdCtrolPiso = s.pesaje.centro.IdCtrolPiso,
                        Fecha = s.pesaje.ctrol.FECHA,
                        FechaOriginal = s.pesaje.ctrol.FECHA,
                        Hora = s.pesaje.ctrol.HORA.ToString(),
                        Ctrabajo = s.pesaje.ctrol.CTRABAJO,
                        Pedido = s.pesaje.ctrol.PEDIDO,
                        Codigo = s.pesaje.ctrol.CODIGO,
                        Maquina = s.pesaje.ctrol.MAQUINA.TrimEnd(new Char[0]).TrimStart(new Char[0]),
                        Cantidad = s.pesaje.ctrol.PESON,
                        Turno = s.pesaje.ctrol.TURNO,
                        Descripcion = s.m.DESCRIPCIO,
                        Rollo = s.pesaje.ctrol.ROLLO.Trim(),
                        Tipo = "Produccion"

                    });





            if (o!=null)
            {
                foreach(var item in o)
                {
                    Resumen prodDetalle = new Resumen();
                    var hora = string.Format("{0},{1}",item.Fecha.Value.Hour,item.Fecha.Value.Minute);
                    prodDetalle.Id = item.IdCtrolPiso;
                    prodDetalle.Fecha = ValidaFechaTurno((DateTime)item.Fecha, item.Turno.ToString().Trim(), item.Hora);
                    prodDetalle.Hora = item.Hora;
                    prodDetalle.Ctrabajo = item.Ctrabajo.Trim();
                    prodDetalle.Pedido = item.Pedido.Trim();
                    prodDetalle.Codigo = item.Codigo.Trim();
                    prodDetalle.Descripcion = item.Descripcion.Trim();
                    prodDetalle.Maquina = item.Maquina.Trim();
                    prodDetalle.Rollo = item.Rollo;
                    prodDetalle.Tipo = item.Tipo;
                    prodDetalle.Turno = item.Turno.ToString().Trim();
                    prodDetalle.FechaOriginal = item.Fecha;
                    prodDetalle.Cantidad = Math.Round( (decimal)item.Cantidad,2);
                    Resumen.Add(prodDetalle);

                }
            }

            //Consultar Retal
            var r = dbcontext.tblRetalRegistro.Join(dbcontext.MVTRADE, t0 => new { Pedido = t0.Pedido }, t1 => new { Pedido = t1.NRODCTO }, (t0, t1) => new { t0 = t0, t1 = t1 })
                    .Where(temp0 => ((((!(temp0.t0.Auditado == (Boolean?)true) && !(temp0.t0.Consumido == (Boolean?)true)) && new String[] { "0201", "0211" }.Contains(temp0.t0.Codcc)
                  ) && (temp0.t1.ORIGEN == "fac")) && (temp0.t1.TIPODCTO == "pd")) && (temp0.t0.Auditado!=true) )
                  .Select(temp0 => new
                  {
                      Id = temp0.t0.IdReg,
                      Rollo = temp0.t0.IdPesaje,
                      Fecha = temp0.t0.FechaHora_Pesaje,
                      Pedido = temp0.t0.Pedido,
                      Codcc = temp0.t0.Codcc,
                      Codigo = temp0.t1.PRODUCTO.TrimEnd(new Char[0]).TrimStart(new Char[0]),
                      DESCRIPCIO = temp0.t1.MTMERCIA.DESCRIPCIO,
                      Maquina = temp0.t0.IdMaquina,
                      Cantidad = temp0.t0.PesoBruto
                  });

                                 
            if (r != null)
            {
                foreach (var item in r)
                {
                    Resumen retalDetalle = new Resumen();
                    retalDetalle.Id =item.Id;
                    retalDetalle.Fecha =    ValidaFechaTurno((DateTime)item.Fecha,CalculaTurno((DateTime)item.Fecha)) ;
                    retalDetalle.FechaOriginal = item.Fecha;
                    retalDetalle.Hora = string.Format("{0},{1}",item.Fecha.Hour,item.Fecha.Minute);
                    retalDetalle.Ctrabajo = item.Codcc == "0201" ? "EXT" : "EXL";
                    retalDetalle.Pedido = item.Pedido.Trim();
                    retalDetalle.Codigo = item.Codigo.Trim();
                    retalDetalle.Descripcion = item.DESCRIPCIO.ToString().Trim();
                    retalDetalle.Maquina = item.Maquina.Trim();
                    retalDetalle.Tipo = "Retal";
                    retalDetalle.Rollo = item.Rollo;
                    retalDetalle.Cantidad = Math.Round( (decimal)item.Cantidad,2);
                    retalDetalle.Turno = CalculaTurno(item.Fecha).Trim();
                    Resumen.Add(retalDetalle);
                }
               
            }

            //Agrupar
            var agru = Resumen.GroupBy(x => new { Fecha = x.Fecha,Ctrabajo=x.Ctrabajo, Pedido = x.Pedido, Codigo = x.Codigo,Descripcion=x.Descripcion, Maquina = x.Maquina }).
                    Select(s => new { Fecha = s.Key.Fecha,Ctrabajo=s.Key.Ctrabajo, Pedido = s.Key.Pedido, Codigo = s.Key.Codigo,Descripcion=s.Key.Descripcion,
                        Maquina = s.Key.Maquina, Cantidad = s.Sum(p => p.Cantidad) });

            foreach (var item in agru )
            {
                object[] objDetalle = new object[8];
                //objDetalle[0] = item.Fecha;
                objDetalle[0]=((DateTime)item.Fecha).ToString("dd/MM/yyyy");
                objDetalle[1] = item.Ctrabajo;
                objDetalle[2] = item.Pedido;
                objDetalle[3] = item.Codigo;
                objDetalle[4] = item.Descripcion;
                objDetalle[5] = item.Maquina;
                objDetalle[6] = item.Cantidad;
                objDetalle[7] = false;
                GridRow nuevaLinea = new GridRow(objDetalle);
                sGridAuditoria.PrimaryGrid.Rows.Add(nuevaLinea);

            }


        }

        private void CargarDetalleInsumosModTornillo(int idDetalleTornillo, int idTornillo,ctrolTornilloDetalle objDetalle)
        {
            var t = dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.Where(w => w.IdDetalleTornillo == idDetalleTornillo);
            foreach(var item in t)
            {
                object[] objDetInsumo = new object[3];
                objDetInsumo[0] = item.IdDetalleInsumo.ToString();
                objDetInsumo[1] = item.Insumo.ToString();
                objDetInsumo[2] = item.Factor.ToString();
                GridRow nuevaLinea = new GridRow(objDetInsumo);
                objDetalle.sGrid.PrimaryGrid.Rows.Add(nuevaLinea);
            }
        }



        private void MostrarDetalleInsumoOrden(object[] obj,GridPanel panel)
        {
            GridRow lineaDetalle;
            if(  obj[1].ToString().Trim()!=string.Empty && obj[1]!=null)
            {
                lineaDetalle = new GridRow(obj);
                panel.Rows.Add(lineaDetalle);
            }

        }


        

        private void CargarDetallesOrdenTornillos(int idtornillo,string Pedido,ctrolTornilloDetalle objDetalle )
        {
            MEZCLASMQ y = dbcontext.MEZCLASMQ.Where(w => w.ORDENNRO == Pedido).SingleOrDefault();
            object[] objDetalleTornilloInsumo = new object[3];

            if (y == null)
                return;

            txtMaquinaProgramada.Text = y.NMAQUINA.Trim();





            if (idtornillo == 1)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_A.ToString();

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "A";
                objDetalleTornilloInsumo[1] = y.MEZCLA_A6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_A6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

            }
            if (idtornillo == 2)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_B.ToString();

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "B";
                objDetalleTornilloInsumo[1] = y.MEZCLA_B6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_B6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);
            }
            if (idtornillo == 3)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_C.ToString();
                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "C";
                objDetalleTornilloInsumo[1] = y.MEZCLA_C6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_C6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

            }
            if (idtornillo == 4)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_D.ToString();
                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "D";
                objDetalleTornilloInsumo[1] = y.MEZCLA_D6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_D6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);
            }
            if (idtornillo == 5)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_E.ToString();
                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "E";
                objDetalleTornilloInsumo[1] = y.MEZCLA_E6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_E6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);
            }
            if (idtornillo == 6)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_F.ToString();
                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "F";
                objDetalleTornilloInsumo[1] = y.MEZCLA_F6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_F6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);
            }
            if (idtornillo == 7)
            {
                objDetalle.txtParticipacion.Text = y.VELTOR_G.ToString();
                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G1.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G1.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G2.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G2.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G3.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G3.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G4.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G4.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G5.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G5.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);

                objDetalleTornilloInsumo[0] = "G";
                objDetalleTornilloInsumo[1] = y.MEZCLA_G6.ToString();
                objDetalleTornilloInsumo[2] = y.KILOS_G6.ToString();
                MostrarDetalleInsumoOrden(objDetalleTornilloInsumo, objDetalle.sGrid.PrimaryGrid);
            }
        }

        private void GuardarDetalleTornillo(ctrolTornilloDetalle detalleTornillo, tblMezclaRolloCentroPesajeResumen resumen,int IdTornillo)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            decimal participacion;
            detalleTornillo.sGrid.PrimaryGrid.PurgeDeletedRows();

            int cantInsumosTornillo = 0;
            foreach(GridRow linea in detalleTornillo.sGrid.PrimaryGrid.Rows)
            {
                if(linea.Cells[1].Value!=null)
                {
                    string p;
                    p = linea.Cells[1].Value.ToString();
                    cantInsumosTornillo += 1;

                }
            }


            if(cantInsumosTornillo>0)
            {
                decimal.TryParse(detalleTornillo.txtParticipacion.Text, out participacion);
                tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                tornillo.IdDetalleResumen = resumen.IdDetalleResumen;
                tornillo.IdTornillo = IdTornillo;
                tornillo.Participacion = participacion;
                dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                dbcontext.SubmitChanges();
                //Recorrer insumos de tornillo

                foreach (GridRow linea in detalleTornillo.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                        insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                        insumo.Insumo = linea[1].Value.ToString();
                        insumo.Factor = decimal.Parse(linea[2].Value.ToString().Replace('.', a).Replace(',', a));
                        dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                        dbcontext.SubmitChanges();
                    }

                }
            }
            //if (decimal.TryParse(detalleTornillo.txtParticipacion.Text, out participacion))
            //{
                

            //}
        }

        private void GuardarResumenInsumen(tblMezclaRolloCentroPesajeResumen resumen,SuperGridControl gridResumen)
        {
            foreach (GridRow linea in sGridResumenInsumos.PrimaryGrid.Rows)
            {
                tblMezclaRolloCentroPesaje_ResumenInsumo resumenInsumo = new tblMezclaRolloCentroPesaje_ResumenInsumo();
                resumenInsumo.IdDetalleResumen = resumen.IdDetalleResumen;
                resumenInsumo.FechaConsumo = resumen.Fecha;
                resumenInsumo.CodInsumo = linea[0].Value.ToString();
                resumenInsumo.Cantidad = decimal.Parse(linea[1].Value.ToString());
                resumenInsumo.UltPrecioCompra = decimal.Parse(linea[2].Value.ToString());
                resumenInsumo.Consumido = false;
                dbcontext.tblMezclaRolloCentroPesaje_ResumenInsumo.InsertOnSubmit(resumenInsumo);
                dbcontext.SubmitChanges();
            }
        }


        private void btnGuardarTemporal_Click(object sender, EventArgs e)
        {
          

            //Buscar si ya ha sido guardado el registro
            tblMezclaRolloCentroPesajeResumen objRegistrado = new tblMezclaRolloCentroPesajeResumen();
            objRegistrado = dbcontext.tblMezclaRolloCentroPesajeResumen.
                Where(x => x.Fecha == dFecha && x.Pedido == strPedido && x.IdMaquina == strMaquina && x.Cantidad == decCantidad && x.Tipo == strCtrabajo).SingleOrDefault();

            if (objRegistrado != null)
            {
                CargarDetallesPreviosGuardados(objRegistrado);
                dbcontext.tblMezclaRolloCentroPesajeResumen.DeleteOnSubmit(objRegistrado);
                dbcontext.SubmitChanges();
            }

            tblMezclaRolloCentroPesajeResumen r = new tblMezclaRolloCentroPesajeResumen();
            r.Fecha = dFecha;
            r.Pedido = strPedido;
            r.IdMaquina = strMaquina;
            r.Cantidad = decCantidad;
            r.Tipo = strCtrabajo;
            dbcontext.tblMezclaRolloCentroPesajeResumen.InsertOnSubmit(r);
            dbcontext.SubmitChanges();


            GuardarDetalleTornillo(ctrolModT1, r,1);
            GuardarDetalleTornillo(ctrolModT2, r,2);
            GuardarDetalleTornillo(ctrolModT3, r,3);
            GuardarDetalleTornillo(ctrolModT4, r,4);
            GuardarDetalleTornillo(ctrolModT5, r,5);
            GuardarDetalleTornillo(ctrolModT6, r,6);
            GuardarDetalleTornillo(ctrolModT7, r,7);

            CalcularConsumo();
            GuardarResumenInsumen(r, sGridResumenInsumos);


            /*

                        //Recorrer Cambios en tornillos
                        //Tornillo1
                        if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 1;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT1.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo2
                        if (decimal.TryParse(ctrolModT2.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 2;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();

                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT2.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo3
                        if (decimal.TryParse(ctrolModT3.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 3;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT3.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo4
                        if (decimal.TryParse(ctrolModT4.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 4;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT4.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo5
                        if (decimal.TryParse(ctrolModT5.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 5;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT5.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo6
                        if (decimal.TryParse(ctrolModT6.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 6;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT6.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }

                        //Tornillo7
                        if (decimal.TryParse(ctrolModT7.txtParticipacion.Text, out participacion) && participacion > 0)
                        {
                            tblMezclaRolloCentroPesaje_DetallesTornillo tornillo = new tblMezclaRolloCentroPesaje_DetallesTornillo();
                            tornillo.IdDetalleResumen = r.IdDetalleResumen;
                            tornillo.IdTornillo = 7;
                            tornillo.Participacion = participacion;
                            dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.InsertOnSubmit(tornillo);
                            dbcontext.SubmitChanges();
                            //Recorrer insumos de tornillo
                            foreach (GridRow linea in ctrolModT7.sGrid.PrimaryGrid.Rows)
                            {
                                if (linea[1].Value != null)
                                {
                                    tblMezclaRolloCentroPesaje_DetallesInsumos insumo = new tblMezclaRolloCentroPesaje_DetallesInsumos();
                                    insumo.IdDetalleTornillo = tornillo.IdDetalleTornillo;
                                    insumo.Insumo = linea[1].Value.ToString();
                                    insumo.Factor = decimal.Parse(linea[2].Value.ToString());
                                    dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.InsertOnSubmit(insumo);
                                    dbcontext.SubmitChanges();
                                }

                            }

                        }


                        //Guarda resumen de insumos

                        foreach (GridRow linea in sGridResumenInsumos.PrimaryGrid.Rows)
                        {
                            //r.IdDetalleResumen
                            tblMezclaRolloCentroPesaje_ResumenInsumo resumenInsumo = new tblMezclaRolloCentroPesaje_ResumenInsumo();
                            resumenInsumo.IdDetalleResumen = r.IdDetalleResumen;
                            resumenInsumo.FechaConsumo = r.Fecha;
                            resumenInsumo.CodInsumo = linea[0].Value.ToString();
                            resumenInsumo.Cantidad = decimal.Parse(linea[1].Value.ToString());
                            resumenInsumo.Consumido = false;
                            dbcontext.tblMezclaRolloCentroPesaje_ResumenInsumo.InsertOnSubmit(resumenInsumo);
                            dbcontext.SubmitChanges();
                        }
                        */
        }



        private void MostrarResumenConsumo(List<InsumoCalculado> insumos_calculados)
        {
            sGridResumenInsumos.PrimaryGrid.Rows.Clear();
            decimal dblCostototalMP = 0;
            var l = insumos_calculados.GroupBy(x => new { Insumo = x.Insumo }).Select(f => new { Insumo = f.Key.Insumo, Cantidad = f.Sum(p => p.Cantidad) });

            foreach (var p in l)
            {
                decimal decUltValorCompra = 1;
                MVTRADE ultMovCompra = dbcontext.MVTRADE.Where(x => x.PRODUCTO == p.Insumo && x.ORIGEN == "COM" && x.TIPODCTO == "FA" && x.FECHA<=dFecha).OrderByDescending(x => x.FECHA).Take(1).SingleOrDefault();


                if (ultMovCompra != null)
                {
                    decUltValorCompra = (decimal)ultMovCompra.VALORUNIT;
                }
                else
                    decUltValorCompra = 0;

                   if(dbcontext.MVPRECIO.Any(x=>x.CODPRODUC==p.Insumo && x.CODPRECIO=="05") && decUltValorCompra==0)
                    {
                        decUltValorCompra = (decimal)dbcontext.MVPRECIO.Where(x => x.CODPRODUC == p.Insumo && x.CODPRECIO == "05").SingleOrDefault().PRECIO;
                    }



                object[] objDetalle = new object[4];
                objDetalle[0] = p.Insumo;
                objDetalle[1] =Math.Round( (decimal)p.Cantidad,2);
                objDetalle[2] = Math.Round(decUltValorCompra, 2);
                objDetalle[3] = Math.Round( (decimal)(decUltValorCompra * p.Cantidad),2);
                dblCostototalMP += Math.Round((decimal)(decUltValorCompra * p.Cantidad), 2);
                GridRow nuevaLinea = new GridRow(objDetalle);
               sGridResumenInsumos.PrimaryGrid.Rows.Add(nuevaLinea);
            }

            ActualizarTotalCostoMateriaPrima(dblCostototalMP);
        }

        private void ActualizarTotalCostoMateriaPrima(decimal costoTotal)
        {
            txtCostoKilo.Text = (costoTotal / decCantidad).ToString("N2");
            txtCantidadTotal.Text = decCantidad.ToString("N2");
            txtCostoTotal.Text = costoTotal.ToString("C2");
        }

        private void MostrarInfoDetalleTornillo(ctrolTornilloDetalle ctrol, tblMezclaRolloCentroPesaje_DetallesTornillo tornillo)
        {
            ctrol.sGrid.PrimaryGrid.Rows.Clear();
            ctrol.txtParticipacion.Text = string.Empty;

            if (tornillo!=null)
            {
                ctrol.txtParticipacion.Text = tornillo.Participacion.ToString();
                var p = dbcontext.tblMezclaRolloCentroPesaje_DetallesInsumos.Where(x => x.IdDetalleTornillo == tornillo.IdDetalleTornillo).ToList();
                foreach (var detalleInsumosTornillo in p)
                {
                    object[] objdetInsumoTornillo = new object[3];
                    objdetInsumoTornillo[0] = detalleInsumosTornillo.IdDetalleInsumo;
                    objdetInsumoTornillo[1] = detalleInsumosTornillo.Insumo;
                    objdetInsumoTornillo[2] = detalleInsumosTornillo.Factor;
                    GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                    ctrol.sGrid.PrimaryGrid.Rows.Add(lineaGrid);

                }
            }
        }



        private void CargarDetallesAuditoriaPendiente( GridRow detalleAuditoriaPendienteGrid)
        {
            GridCell cFecha = sGridAuditoria.GetCell(detalleAuditoriaPendienteGrid.FullIndex, 0) as GridCell;
            GridCell cPedido = sGridAuditoria.GetCell(detalleAuditoriaPendienteGrid.FullIndex, 2) as GridCell;
            GridCell cMaquina = sGridAuditoria.GetCell(detalleAuditoriaPendienteGrid.FullIndex, 5) as GridCell;
            GridCell cCantidad = sGridAuditoria.GetCell(detalleAuditoriaPendienteGrid.FullIndex, 6) as GridCell;
            GridCell cTrabajo = sGridAuditoria.GetCell(detalleAuditoriaPendienteGrid.FullIndex, 1) as GridCell;

            strPedido = cPedido.Value.ToString();
            strMaquina = cMaquina.Value.ToString();
            dFecha = new DateTime(int.Parse(cFecha.Value.ToString().Substring(6, 4)), int.Parse(cFecha.Value.ToString().Substring(3, 2)), int.Parse(cFecha.Value.ToString().Substring(0, 2)), 0, 0, 0);
            decCantidad = (decimal)cCantidad.Value;
            strCtrabajo = cTrabajo.Value.ToString();

            //Buscar si hay informacion previa de detalles de tornillos e insumos
            tblMezclaRolloCentroPesajeResumen objAuditoriaResumen = new tblMezclaRolloCentroPesajeResumen();
            objAuditoriaResumen = dbcontext.tblMezclaRolloCentroPesajeResumen.
                Where(x => x.Fecha == new DateTime(dFecha.Year,dFecha.Month,dFecha.Day,0,0,0) && x.Pedido == strPedido && x.IdMaquina == strMaquina && x.Cantidad == decCantidad && x.Tipo == strCtrabajo).SingleOrDefault();

            if (objAuditoriaResumen != null)
            {
                //CargarDetallesPreviosGuardados(objAuditoriaResumen);
                //Cargar detalles de tornillos ya guardados
                var p = dbcontext.tblMezclaRolloCentroPesaje_DetallesTornillo.Where(x => x.IdDetalleResumen == objAuditoriaResumen.IdDetalleResumen).ToList();
                foreach (var tornillo in p)
                {
                    
                    if (tornillo.IdTornillo == 1)
                        MostrarInfoDetalleTornillo(ctrolModT1, tornillo);
                    if (tornillo.IdTornillo == 2)
                        MostrarInfoDetalleTornillo(ctrolModT2, tornillo);
                    if (tornillo.IdTornillo == 3)
                        MostrarInfoDetalleTornillo(ctrolModT3, tornillo);
                    if (tornillo.IdTornillo == 4)
                        MostrarInfoDetalleTornillo(ctrolModT4, tornillo);
                    if (tornillo.IdTornillo == 5)
                        MostrarInfoDetalleTornillo(ctrolModT5, tornillo);
                    if (tornillo.IdTornillo == 6)
                        MostrarInfoDetalleTornillo(ctrolModT6, tornillo);
                    if (tornillo.IdTornillo == 7)
                        MostrarInfoDetalleTornillo(ctrolModT7, tornillo);

                    //CargarDetallesOrdenTornillos(1, strPedido, ctrolOrdenT1);
                    //CargarDetallesOrdenTornillos(2, strPedido, ctrolOrdenT2);
                    //CargarDetallesOrdenTornillos(3, strPedido, ctrolOrdenT3);
                    //CargarDetallesOrdenTornillos(4, strPedido, ctrolOrdenT4);
                    //CargarDetallesOrdenTornillos(5, strPedido, ctrolOrdenT5);
                    //CargarDetallesOrdenTornillos(6, strPedido, ctrolOrdenT6);
                    //CargarDetallesOrdenTornillos(7, strPedido, ctrolOrdenT7);



                }
                
            }else
            {
                //Cargar detalles de tornillos desde orden


                CargarDetallesOrdenTornillos(1, strPedido, ctrolModT1);
                CargarDetallesOrdenTornillos(2, strPedido, ctrolModT2);
                CargarDetallesOrdenTornillos(3, strPedido, ctrolModT3);
                CargarDetallesOrdenTornillos(4, strPedido, ctrolModT4);
                CargarDetallesOrdenTornillos(5, strPedido, ctrolModT5);
                CargarDetallesOrdenTornillos(6, strPedido, ctrolModT6);
                CargarDetallesOrdenTornillos(7, strPedido, ctrolModT7);
            }

            CargarDetallesOrdenTornillos(1, strPedido, ctrolOrdenT1);
            CargarDetallesOrdenTornillos(2, strPedido, ctrolOrdenT2);
            CargarDetallesOrdenTornillos(3, strPedido, ctrolOrdenT3);
            CargarDetallesOrdenTornillos(4, strPedido, ctrolOrdenT4);
            CargarDetallesOrdenTornillos(5, strPedido, ctrolOrdenT5);
            CargarDetallesOrdenTornillos(6, strPedido, ctrolOrdenT6);
            CargarDetallesOrdenTornillos(7, strPedido, ctrolOrdenT7);

            CalcularConsumo();
            CargarDetalleRollosAuditoriaPendiente(sGridRollos, strPedido,dFecha,strCtrabajo,strMaquina);
            


        }

        private List<Rollo> CargarDetalleRollosAuditoriaPendiente(string pedido, DateTime fecha, string ctrabajo, string maquina)
        {
            List<Rollo> lstRollos;
            lstRollos = new List<Rollo>();

            var j = Resumen.Where(x => x.Fecha == fecha && x.Ctrabajo == ctrabajo && x.Pedido == pedido && x.Maquina == maquina);
            foreach (var item in j)
            {
                Rollo rollo = new Rollo();
                rollo.FechaJornada = (DateTime)item.Fecha;
                rollo.Fecha = (DateTime)item.FechaOriginal;
                rollo.Pedido = pedido;
                rollo.IdMaquina = item.Maquina;
                rollo.Hora = item.Hora.Replace(',','.');
                rollo.Turno = item.Turno;
                rollo.IdRollo = item.Rollo;
                rollo.Cantidad = (decimal)item.Cantidad;
                rollo.Tipo = item.Tipo;
                lstRollos.Add(rollo);
            }
            return lstRollos;

        }



        private void CargarDetalleRollosAuditoriaPendiente(SuperGridControl gridRollos,string pedido,DateTime fecha,string ctrabajo,string maquina)
        {
            sGridRollos.PrimaryGrid.Rows.Clear();
            var j = Resumen.Where(x => x.Fecha == fecha && x.Ctrabajo == ctrabajo && x.Pedido == pedido && x.Maquina == maquina);
            decimal cantRetal=0,cantProduc=0;
            txtDetalleRollos.Text = string.Empty;
            foreach (var item in j)
            {
                object[] objDetalle = new object[6];
                objDetalle[0] = ((DateTime)item.FechaOriginal).ToString("dd/MM/yyyy");
                objDetalle[1] = item.Hora.Replace(',','.').Replace('.',':');
                objDetalle[2] = item.Turno;
                objDetalle[3] = item.Rollo;
                objDetalle[4] = item.Cantidad;
                objDetalle[5] = item.Tipo;
                GridRow nuevaLinea = new GridRow(objDetalle);
                sGridRollos.PrimaryGrid.Rows.Add(nuevaLinea);
                if (item.Tipo == "Produccion")
                    cantProduc += (decimal)item.Cantidad;
                if (item.Tipo == "Retal")
                    cantRetal += (decimal)item.Cantidad;
            }
            txtDetalleRollos.Text = string.Format("Total: {0} Kg     Total Producción: {1} Kg     Total Retal: {2} Kg",cantRetal+cantProduc,cantProduc,cantRetal);

        }


        private void sGridAuditoria_RowClick(object sender, GridRowClickEventArgs e)
        {
            //InicializarControles();
            //GridRow linea = sGridAuditoria.PrimaryGrid.Rows[e.GridRow.FullIndex] as GridRow;
            //CargarDetallesAuditoriaPendiente(linea);
        }

        private void InicializarControles()
        {
            ctrolModT1.txtParticipacion.Text = string.Empty;
            ctrolModT2.txtParticipacion.Text = string.Empty;
            ctrolModT3.txtParticipacion.Text = string.Empty;
            ctrolModT4.txtParticipacion.Text = string.Empty;
            ctrolModT5.txtParticipacion.Text = string.Empty;
            ctrolModT6.txtParticipacion.Text = string.Empty;
            ctrolModT7.txtParticipacion.Text = string.Empty;
            ctrolModT1.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT2.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT3.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT4.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT5.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT6.sGrid.PrimaryGrid.Rows.Clear();
            ctrolModT7.sGrid.PrimaryGrid.Rows.Clear();

            ctrolOrdenT1.txtParticipacion.Text = string.Empty;
            ctrolOrdenT2.txtParticipacion.Text = string.Empty;
            ctrolOrdenT3.txtParticipacion.Text = string.Empty;
            ctrolOrdenT4.txtParticipacion.Text = string.Empty;
            ctrolOrdenT5.txtParticipacion.Text = string.Empty;
            ctrolOrdenT6.txtParticipacion.Text = string.Empty;
            ctrolOrdenT7.txtParticipacion.Text = string.Empty;
            ctrolOrdenT1.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT2.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT3.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT4.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT5.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT6.sGrid.PrimaryGrid.Rows.Clear();
            ctrolOrdenT7.sGrid.PrimaryGrid.Rows.Clear();
        }
        
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            decimal parTornilloTotal = 0;
            decimal partTornillo1 = 0;
            decimal partTornillo2 = 0;
            decimal partTornillo3 = 0;
            decimal partTornillo4 = 0;
            decimal partTornillo5 = 0;
            decimal partTornillo6 = 0;
            decimal partTornillo7 = 0;
            List<InsumoCalculado> InsumosCalculados = new List<InsumoCalculado>();

            if(decimal.TryParse(ctrolModT1.txtParticipacion.Text,out partTornillo1))
            {
                parTornilloTotal += partTornillo1;
            }
            if (decimal.TryParse(ctrolModT2.txtParticipacion.Text, out partTornillo2))
            {
                parTornilloTotal += partTornillo2;
            }
            if (decimal.TryParse(ctrolModT3.txtParticipacion.Text, out partTornillo3))
            {
                parTornilloTotal += partTornillo3;
            }
            if (decimal.TryParse(ctrolModT4.txtParticipacion.Text, out partTornillo4))
            {
                parTornilloTotal += partTornillo4;
            }
            if (decimal.TryParse(ctrolModT5.txtParticipacion.Text, out partTornillo5))
            {
                parTornilloTotal += partTornillo5;
            }
            if (decimal.TryParse(ctrolModT6.txtParticipacion.Text, out partTornillo6))
            {
                parTornilloTotal += partTornillo6;
            }
            if (decimal.TryParse(ctrolModT7.txtParticipacion.Text, out partTornillo7))
            {
                parTornilloTotal += partTornillo7;
            }

            if (parTornilloTotal == 0)
                return;

            partTornillo1 = partTornillo1 / parTornilloTotal;
            partTornillo2 = partTornillo2 / parTornilloTotal;
            partTornillo3 = partTornillo3 / parTornilloTotal;
            partTornillo4 = partTornillo4 / parTornilloTotal;
            partTornillo5 = partTornillo5 / parTornilloTotal;
            partTornillo6 = partTornillo6 / parTornilloTotal;
            partTornillo7 = partTornillo7 / parTornilloTotal;


            decimal parInsumo=0;
            decimal totalparticipacion;
            string Insumo;
            
            //Recorrer tornillo 1
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT1.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT1.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString())/totalparticipacion;
                        parInsumo = parInsumo *decCantidad*partTornillo1;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo,2) });
                    }

                }
                

            }

            //Recorrer tornillo 2
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT2.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT2.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT2.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo2;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 3
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT3.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT3.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT3.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo3;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 4
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT4.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT4.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT4.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo4;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 5
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT1.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT5.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT5.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo5;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 6
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT6.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT6.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT6.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo6;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            //Recorrer tornillo 7
            totalparticipacion = 0;
            Insumo = "";
            if (decimal.TryParse(ctrolModT7.txtParticipacion.Text, out totalparticipacion) && totalparticipacion > 0)
            {
                totalparticipacion = 0;
                //Recorrer insumos de tornillo
                foreach (GridRow linea in ctrolModT7.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        totalparticipacion += decimal.Parse(linea[2].Value.ToString());
                    }

                }
                foreach (GridRow linea in ctrolModT7.sGrid.PrimaryGrid.Rows)
                {
                    if (linea[1].Value != null)
                    {
                        Insumo = linea[1].Value.ToString().TrimEnd();
                        parInsumo = decimal.Parse(linea[2].Value.ToString()) / totalparticipacion;
                        parInsumo = parInsumo * decCantidad * partTornillo7;
                        InsumosCalculados.Add(new InsumoCalculado { Insumo = Insumo, Cantidad = Math.Round(parInsumo, 2) });
                    }

                }


            }

            MostrarResumenConsumo(InsumosCalculados);

        }

        private void CargarDetallesPreviosGuardados(tblMezclaRolloCentroPesajeResumen objResumenGuardado)
        {
            object[] objdetInsumoTornillo;
            object[] objdetResumen;

            //Cargar detalle Tornillos
            foreach (var tornillo in objResumenGuardado.tblMezclaRolloCentroPesaje_DetallesTornillo)
            {
                if(tornillo.IdTornillo==1)
                {
                    foreach(tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT1.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT1.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 2)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT2.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT2.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 3)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT3.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT3.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 4)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT4.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT4.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 5)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT5.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT5.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 6)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT6.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT6.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
                if (tornillo.IdTornillo == 7)
                {
                    foreach (tblMezclaRolloCentroPesaje_DetallesInsumos detalleInsumo in tornillo.tblMezclaRolloCentroPesaje_DetallesInsumos)
                    {
                        ctrolModT7.sGrid.PrimaryGrid.Rows.Clear();
                        objdetInsumoTornillo = new object[3];
                        objdetInsumoTornillo[0] = detalleInsumo.IdDetalleInsumo;
                        objdetInsumoTornillo[1] = detalleInsumo.Insumo;
                        objdetInsumoTornillo[2] = detalleInsumo.Factor;
                        GridRow lineaGrid = new GridRow(objdetInsumoTornillo);
                        ctrolModT7.sGrid.PrimaryGrid.Rows.Add(lineaGrid);
                    }
                }
            }


            //Cargar detalle resumen insumos
            sGridResumenInsumos.PrimaryGrid.Rows.Clear();
            foreach(tblMezclaRolloCentroPesaje_ResumenInsumo resumenInsumo in objResumenGuardado.tblMezclaRolloCentroPesaje_ResumenInsumo)
            {
                objdetResumen = new object[2];
                objdetResumen[0] = resumenInsumo.CodInsumo;
                objdetResumen[1] = resumenInsumo.Cantidad;
                GridRow lineaGrid = new GridRow(objdetResumen);
                sGridResumenInsumos.PrimaryGrid.Rows.Add(lineaGrid);
            }
            

        }




       

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            GridRow lineaGrid = sGridAuditoria.ActiveRow as GridRow;
            btnGuardarTemporal_Click(null, null);
            tblMezclaRolloCentroPesajeResumen tblMRes = dbcontext.tblMezclaRolloCentroPesajeResumen.
                Where(x => x.Pedido == lineaGrid.Cells[2].Value.ToString()
                && x.Tipo == lineaGrid.Cells[1].Value.ToString()
                && x.Fecha == DateTime.Parse(lineaGrid.Cells[0].Value.ToString())
                && x.IdMaquina == lineaGrid.Cells[5].Value.ToString()
                && x.Cantidad==(decimal)lineaGrid.Cells[6].Value
                ).
                SingleOrDefault();

            if(tblMRes!=null)
            {
                
                tblMRes.Aprobado = true;
                sGridAuditoria.PrimaryGrid.Rows.RemoveAt(lineaGrid.FullIndex);

                foreach (GridRow rollo in sGridRollos.PrimaryGrid.Rows)
                {
                    tblMezclaRolloCentroPesaje_DetallesRollos DetalleRollo = new tblMezclaRolloCentroPesaje_DetallesRollos();
                    DetalleRollo.IdDetalleResumen = tblMRes.IdDetalleResumen;


                    if (rollo.Cells["Tipo"].Value.ToString() == "Produccion")
                    {
                        tblMezclaRolloCentroPesaje rolloProd = new tblMezclaRolloCentroPesaje();
                        var u = Resumen.Where(w => w.Rollo == rollo.Cells["rollo"].Value.ToString()).SingleOrDefault();
                        rolloProd = dbcontext.tblMezclaRolloCentroPesaje.Where(r => r.IdCtrolPiso == u.Id).SingleOrDefault();
                        DetalleRollo.Id_IdCtrolPiso_IdPesaje = rolloProd.IdCtrolPiso.ToString();
                        rolloProd.Aprobado = true;
                    }
                    else
                    {
                        tblRetalRegistro rolloRetal = new tblRetalRegistro();
                        rolloRetal = dbcontext.tblRetalRegistro.Where(r => r.IdPesaje == rollo.Cells["rollo"].Value.ToString()).SingleOrDefault();
                        DetalleRollo.Id_IdCtrolPiso_IdPesaje = rolloRetal.IdPesaje;
                        rolloRetal.Auditado = true;
                    }
                    dbcontext.tblMezclaRolloCentroPesaje_DetallesRollos.InsertOnSubmit(DetalleRollo);
                }
                dbcontext.SubmitChanges();
                sGridAuditoria.Refresh();

            }





            /*
            
            
            sGridAuditoria.PrimaryGrid.Rows.RemoveAt(indice[0].GridPanel.ActiveRow.FullIndex);
            

            tblMezclaRolloCentroPesajeResumen res = new tblMezclaRolloCentroPesajeResumen();
            //res=dbcontext.tblMezclaRolloCentroPesajeResumen.Where(x=>x.Fecha==indice)
            
            foreach (GridRow rollo in sGridRollos.PrimaryGrid.Rows)
            {
                if(rollo.Cells["Tipo"].Value.ToString()=="Produccion")
                {
                    tblMezclaRolloCentroPesaje prod = new tblMezclaRolloCentroPesaje();
                    var u = Resumen.Where(w => w.Rollo == rollo.Cells["rollo"].Value.ToString()).SingleOrDefault();
                    prod = dbcontext.tblMezclaRolloCentroPesaje.Where(r => r.IdCtrolPiso==u.Id).SingleOrDefault();
                    prod.Aprobado = true;
                }else
                {
                    tblRetalRegistro retal = new tblRetalRegistro();
                    retal = dbcontext.tblRetalRegistro.Where(r => r.IdPesaje == rollo.Cells["rollo"].Value.ToString()).SingleOrDefault();
                    retal.Auditado = true;
                }
            }
            dbcontext.SubmitChanges();*/
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            var p = dbcontext.tblMezclaRolloCentroPesajeResumen.Where(r => r.Fecha == dFecha && r.Tipo == strCtrabajo && r.Pedido == strPedido && r.Cantidad == decCantidad).SingleOrDefault();
            if (p != null)
            {
                dbcontext.tblMezclaRolloCentroPesajeResumen.DeleteOnSubmit(p);
                dbcontext.SubmitChanges();

                //Limpiar tornillos
                ctrolModT1.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT2.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT3.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT4.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT5.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT6.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT7.sGrid.PrimaryGrid.Rows.Clear();
                ctrolModT1.txtParticipacion.Text = string.Empty;
                ctrolModT2.txtParticipacion.Text = string.Empty;
                ctrolModT3.txtParticipacion.Text = string.Empty;
                ctrolModT4.txtParticipacion.Text = string.Empty;
                ctrolModT5.txtParticipacion.Text = string.Empty;
                ctrolModT6.txtParticipacion.Text = string.Empty;
                ctrolModT7.txtParticipacion.Text = string.Empty;


                //Cargar datos desde orden
                CargarDetallesOrdenTornillos(1, strPedido, ctrolModT1);
                CargarDetallesOrdenTornillos(2, strPedido, ctrolModT2);
                CargarDetallesOrdenTornillos(3, strPedido, ctrolModT3);
                CargarDetallesOrdenTornillos(4, strPedido, ctrolModT4);
                CargarDetallesOrdenTornillos(5, strPedido, ctrolModT5);
                CargarDetallesOrdenTornillos(6, strPedido, ctrolModT6);
                CargarDetallesOrdenTornillos(7, strPedido, ctrolModT7);
            }

        }

        private void sGridAuditoria_Click(object sender, EventArgs e)
        {

        }

        private void btnQuitarFiltros_Click(object sender, EventArgs e)
        {
            GridPanel panel = sGridAuditoria.PrimaryGrid;

            foreach (GridColumn column in panel.Columns)
                column.FilterExpr = null;
        }

        private void btnExportDetalleRollos_Click(object sender, EventArgs e)
        {
            GridPanel panel = sGridAuditoria.PrimaryGrid;
            List<Rollo> lstRollos = new List<Rollo>();
            foreach(GridRow item in panel.Rows)
            {
                if (bool.Parse(item["seleccion"].Value.ToString()) == true)
                {
                    lstRollos.AddRange(CargarDetalleRollosAuditoriaPendiente(item["Pedido"].Value.ToString(),DateTime.Parse(item["Fecha"].Value.ToString()), item["ctrabajo"].Value.ToString(), item["Maquina"].Value.ToString()));
                }
            }
            if(lstRollos.Count>0)
            {

                ExportarAExcel(lstRollos, "");

                /*SaveFileDialog ExportarArchivo = new SaveFileDialog();
                ExportarArchivo.InitialDirectory = @"C:\";
                ExportarArchivo.Title = "Exportar detalle rollos a excel.";
                ExportarArchivo.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                ExportarArchivo.RestoreDirectory = true;


                if (ExportarArchivo.ShowDialog()==DialogResult.OK)
                {
                    ExportarAExcel(lstRollos, ExportarArchivo.FileName);
                }*/
            }

            /*
                 SaveFileDialog saveFileDialog1 = newSaveFileDialog();  
    saveFileDialog1.InitialDirectory = @ "C:\";  
    saveFileDialog1.Title = "Save text Files";  
    saveFileDialog1.CheckFileExists = true;  
    saveFileDialog1.CheckPathExists = true;  
    saveFileDialog1.DefaultExt = "txt";  
    saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";  
    saveFileDialog1.FilterIndex = 2;  
    saveFileDialog1.RestoreDirectory = true;  
    if (saveFileDialog1.ShowDialog() == DialogResult.OK) {  
        textBox1.Text = saveFileDialog1.FileName;  
    }  
             */
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            GridPanel panel = sGridAuditoria.PrimaryGrid;
            foreach(GridRow item in panel.Rows)
            {
                if(bool.Parse(item["seleccion"].Value.ToString())==true)
                {
                    item["seleccion"].Value = false;
                }
            }

        }

        private void ExportarAExcel(List<Rollo> lstRollosExpExcel,string rutaArchivoExcel)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelWorkBook;
            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            excel.DisplayAlerts = true;
            excelWorkBook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorksheet;
            ExcelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkBook.ActiveSheet;
            ExcelWorksheet.Name = "Detalle Rollos";
            ExcelWorksheet.Cells[1, 1] = "Fecha";
            ExcelWorksheet.Cells[1, 2] = "Fecha Pesaje";
            ExcelWorksheet.Cells[1, 3] = "Pedido";
            ExcelWorksheet.Cells[1, 4] = "Maquina";
            ExcelWorksheet.Cells[1, 5] = "Hora";
            ExcelWorksheet.Cells[1, 6] = "Rollo";
            ExcelWorksheet.Cells[1, 7] = "Cantidad";
            ExcelWorksheet.Cells[1, 8] = "Tipo";

            int conlinea = 2;
            foreach(Rollo rollo in lstRollosExpExcel)
            {
                if(rollo.Tipo=="Produccion")
                {
                    rollo.Hora=rollo.Hora.Replace('.', ':');
                    string[] _hora = rollo.Hora.ToString().Split(':');
                    rollo.Fecha = new DateTime(rollo.Fecha.Year, rollo.Fecha.Month, rollo.Fecha.Day, int.Parse(_hora[0].ToString()), int.Parse(_hora[1].ToString()),0);
                }

                ExcelWorksheet.Cells[conlinea, 1] = rollo.FechaJornada;
                ExcelWorksheet.Cells[conlinea, 2] = rollo.Fecha;
                ExcelWorksheet.Cells[conlinea, 3] = rollo.Pedido;
                ExcelWorksheet.Cells[conlinea, 4] = rollo.IdMaquina;
                ExcelWorksheet.Cells[conlinea, 5] = rollo.Hora;
                ExcelWorksheet.Cells[conlinea, 6] = rollo.IdRollo;
                ExcelWorksheet.Cells[conlinea, 7] = rollo.Cantidad;
                ExcelWorksheet.Cells[conlinea, 8] = rollo.Tipo;
                conlinea += 1;
            }

            MessageBox.Show("Proceso terminado!","Exportar resultados a excel",MessageBoxButtons.OK,MessageBoxIcon.Information);
            

            /*ExcelFile file = new ExcelFile();
            ExcelWorksheet sheet = file.Worksheets.Add("Exported List");

            for (int i = 0; i < properties.Count; i++)
                sheet.Cells[0, i].Value = properties[i].Name;

            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < properties.Count; j++)
                    sheet.Cells[i + 1, j].Value = properties[j].GetValue(list[i]);

            file.Save(rutaArchivoExcel);
            */
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            GridPanel panel = sGridAuditoria.PrimaryGrid;
            foreach(GridRow item in panel.Rows)
            {
                if(panel.IsFiltered)
                {
                    if(!item.IsRowFilteredOut)
                    {
                        item["seleccion"].Value = true;
                    }
                }
                else
                {
                    item["seleccion"].Value = true;
                }

                
            }
        }

        private void comboBoxEx1_TextUpdate(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            

        }


    }
    public class Rollo
    {
        public DateTime Fecha { get; set; }
        public DateTime FechaJornada { get; set; }
        public string IdMaquina { get; set; }
        public string Hora { get; set; }
        public string Turno { get; set; }
        public string Pedido { get; set; }
        public string IdRollo { get; set; }
        public decimal Cantidad { get; set; }
        public string Tipo { get; set; }
    }

    public class  PesajeDetalle
    {
        public string Pedido { get; set; }
        public string Codigo { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? Hora { get; set; }
        public int IdCtrolPiso { get; set; }
        public string Nit { get; set; }
        public string Operario1 { get; set; }
        public string Operario2 { get; set; }
        public string Rollo { get; set; }
        public string Maquina { get; set; }
        public decimal? PesoNeto { get; set; }
        public decimal? PesoBruto { get; set; }
        public string Turno { get; set; }
        public string Tipo { get; set; }
    }
    public class PesajeExtruderResumido
    {
        public string Pedido { get; set; }
        public string Codigo { get; set; }
        public decimal? Cantidad { get; set; }
        public string Maquina { get; set; }
        public string Tipo { get; set; }
    }
    public class Resumen
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaOriginal { get; set; }
        public string Hora { get; set; }
        public string Ctrabajo { get; set; }
        public string Pedido { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Maquina { get; set; }
        public string Tipo { get; set; }
        public string Turno { get; set; }
        public decimal? Cantidad { get; set; }
        public string Rollo { get; set; }
    }
    public class InsumoCalculado
    {
        public string Insumo { get; set; }
        public decimal? Cantidad { get; set; }

    }
}

//private void ConsultarResumen( ref OfimaticaDBContext DbContext, DateTime fecha_inicial, DateTime fecha_final)
//{
//    fecha_final=fecha_final.AddDays(1);
//    decimal? iniJornada = new decimal(5.45);
//    decimal? finJornada = new decimal(5.45);

//    List<PesajeDetalle> pesaje = new List<PesajeDetalle>();
//    List<PesajeDetalle> retal = new List<PesajeDetalle>();
//    pesaje = dbcontext.CTROLPISO.
//        Where(x => (x.FECHA.Value.Year == fecha_inicial.Year && x.FECHA.Value.Month == fecha_inicial.Month && x.FECHA.Value.Day == fecha_inicial.Day && x.HORA >= iniJornada)
//                || (x.FECHA.Value.Year == fecha_final.Year && x.FECHA.Value.Month == fecha_final.Month && x.FECHA.Value.Day == fecha_final.Day && x.HORA >= finJornada)
//                && (x.CCORIGEN == "0201" || x.CCORIGEN == "0211") && x.SEMANA == 0).
//                Select(s => new PesajeDetalle { Pedido = s.PEDIDO, Codigo = s.CODIGO, Fecha = s.FECHA, Hora = s.HORA, IdCtrolPiso = s.IDCTRLPISO, Nit = s.NIT,
//                Operario1=s.OPERARIO1,Operario2=s.OPERARIO2,Rollo=s.ROLLO,Maquina=s.MAQUINA,PesoNeto=s.PESON,PesoBruto=s.PESOB,Tipo="Produccion"}).
//                ToList();

//    fecha_inicial = new DateTime(fecha_inicial.Year, fecha_inicial.Month, fecha_inicial.Day, 5, 46, 0);
//    fecha_final = new DateTime(fecha_final.Year, fecha_final.Month, fecha_final.Day, 5, 45, 0);

//    retal = dbcontext.tblRetalRegistros.
//               Where(w=>w.FechaHora_Pesaje>=fecha_inicial && w.FechaHora_Pesaje<=fecha_final && (w.Codcc=="0201" && w.Codcc=="0211") && w.Descartado==false ).
//                    Select(s => new PesajeDetalle
//                    {
//                        Pedido = s.Pedido,Codigo = "",Fecha = s.FechaHora_Pesaje,Hora = 0,IdCtrolPiso =s.IdReg,Nit = "",Operario1 = s.Idresponsable,
//                        Operario2 = "",Rollo = "",Maquina = s.IdMaquina,PesoNeto = s.PesoBruto,PesoBruto = s.PesoBruto,Tipo = "Retal"}).ToList();

//    pesaje.AddRange(retal);
//    retal = null;

//    //List<PesajeExtruderResumido> ResumenPesaje = new List<PesajeExtruderResumido>();
//    //ResumenPesaje = pesaje.GroupBy(g => new { g.Pedido, g.Codigo, g.Maquina, g.Tipo }).
//    //    Select(s => new PesajeExtruderResumido() { Pedido = s.Key.Pedido, Codigo = s.Key.Codigo, Maquina = s.Key.Maquina, Tipo = s.Key.Tipo, Cantidad = s.Sum(s => s.PesoNeto) }).ToList();
//    AdicionarResumen(
//        pesaje.GroupBy(g => new { g.Pedido, g.Codigo, g.Maquina, g.Tipo }).
//        Select(s => new PesajeExtruderResumido() { Pedido = s.Key.Pedido, Codigo = s.Key.Codigo, Maquina = s.Key.Maquina, Tipo = s.Key.Tipo, Cantidad = s.Sum(sum => sum.PesoNeto) }).ToList()
//        );



//    //List < PesajeExtruderResumido> ResumenPesaje = new List<PesajeExtruderResumido>();
//    //ResumenPesaje = dbcontext.CTROLPISO.
//    //    Where(x => (x.FECHA.Value.Year== fecha_inicial.Year && x.FECHA.Value.Month == fecha_inicial.Month && x.FECHA.Value.Day == fecha_inicial.Day && x.HORA>= iniJornada)  
//    //            || (x.FECHA.Value.Year == fecha_final.Year && x.FECHA.Value.Month == fecha_final.Month && x.FECHA.Value.Day == fecha_final.Day && x.HORA >= finJornada)) .
//    //    GroupBy(g => new { g.PEDIDO, g.CODIGO, g.MAQUINA }).
//    //    Select(s=> new PesajeExtruderResumido() { Pedido = s.Key.PEDIDO.Trim(), Codigo = s.Key.CODIGO.Trim(),Maquina=s.Key.MAQUINA.Trim(),Cantidad=s.Sum(g=>g.CANTIDAD) ,Tipo="produccion" })
//    //    .ToList();
//    //AdicionarResumen(ResumenPesaje);

//}
