namespace AppCostos
{
    partial class ctrolTornilloDetalle
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtParticipacion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.sGrid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.iddetalleinsumo = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.codigo = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.cantidad = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtParticipacion);
            this.splitContainer1.Panel1.Controls.Add(this.labelX1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(278, 279);
            this.splitContainer1.SplitterDistance = 31;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtParticipacion
            // 
            // 
            // 
            // 
            this.txtParticipacion.Border.Class = "TextBoxBorder";
            this.txtParticipacion.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtParticipacion.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParticipacion.Location = new System.Drawing.Point(116, 7);
            this.txtParticipacion.Name = "txtParticipacion";
            this.txtParticipacion.PreventEnterBeep = true;
            this.txtParticipacion.Size = new System.Drawing.Size(100, 22);
            this.txtParticipacion.TabIndex = 1;
            this.txtParticipacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParticipacion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtParticipacion_KeyDown);
            this.txtParticipacion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParticipacion_KeyPress);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.Location = new System.Drawing.Point(14, 6);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(96, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Participación:";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.groupPanel1.Controls.Add(this.sGrid);
            this.groupPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(278, 244);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "Insumos";
            // 
            // sGrid
            // 
            this.sGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sGrid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sGrid.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.sGrid.Location = new System.Drawing.Point(0, 0);
            this.sGrid.Name = "sGrid";
            // 
            // 
            // 
            this.sGrid.PrimaryGrid.AllowRowDelete = true;
            this.sGrid.PrimaryGrid.AllowRowInsert = true;
            this.sGrid.PrimaryGrid.Columns.Add(this.iddetalleinsumo);
            this.sGrid.PrimaryGrid.Columns.Add(this.codigo);
            this.sGrid.PrimaryGrid.Columns.Add(this.cantidad);
            this.sGrid.PrimaryGrid.MultiSelect = false;
            this.sGrid.PrimaryGrid.RowHeaderIndexOffset = 1;
            this.sGrid.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sGrid.PrimaryGrid.ShowInsertRow = true;
            this.sGrid.PrimaryGrid.ShowRowGridIndex = true;
            this.sGrid.Size = new System.Drawing.Size(272, 223);
            this.sGrid.TabIndex = 0;
            this.sGrid.Text = "sGridDetalleInsumos";
            this.sGrid.RowDeleted += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowDeletedEventArgs>(this.sGrid_RowDeleted);
            // 
            // iddetalleinsumo
            // 
            this.iddetalleinsumo.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
            this.iddetalleinsumo.CellStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.iddetalleinsumo.HeaderStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.iddetalleinsumo.HeaderText = "Id";
            this.iddetalleinsumo.Name = "iddetalleinsumo";
            this.iddetalleinsumo.Visible = false;
            // 
            // codigo
            // 
            this.codigo.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.AllCells;
            this.codigo.CellStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.codigo.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridComboBoxExEditControl);
            this.codigo.HeaderStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.codigo.HeaderText = "Insumo";
            this.codigo.Name = "codigo";
            // 
            // cantidad
            // 
            this.cantidad.CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleRight;
            this.cantidad.CellStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.cantidad.HeaderStyles.Default.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.cantidad.HeaderText = "Cantidad";
            this.cantidad.Name = "cantidad";
            // 
            // ctrolTornilloDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ctrolTornilloDetalle";
            this.Size = new System.Drawing.Size(278, 279);
            this.Load += new System.EventHandler(this.ctrolTornilloDetalle_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.SuperGrid.GridColumn codigo;
        private DevComponents.DotNetBar.LabelX labelX1;
        public DevComponents.DotNetBar.SuperGrid.SuperGridControl sGrid;
        public DevComponents.DotNetBar.Controls.TextBoxX txtParticipacion;
        private DevComponents.DotNetBar.SuperGrid.GridColumn iddetalleinsumo;
        private DevComponents.DotNetBar.SuperGrid.GridColumn cantidad;
    }
}
