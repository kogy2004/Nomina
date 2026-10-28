namespace Nomina.Formularios
{
    partial class frmCitasProgramadas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvCitasProgramadas = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.mntFecha = new System.Windows.Forms.MonthCalendar();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasProgramadas)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 43);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Citas Programas";
            // 
            // dgvCitasProgramadas
            // 
            this.dgvCitasProgramadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCitasProgramadas.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvCitasProgramadas.Location = new System.Drawing.Point(321, 43);
            this.dgvCitasProgramadas.Name = "dgvCitasProgramadas";
            this.dgvCitasProgramadas.Size = new System.Drawing.Size(479, 407);
            this.dgvCitasProgramadas.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMensaje);
            this.panel2.Location = new System.Drawing.Point(33, 311);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 102);
            this.panel2.TabIndex = 2;
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(14, 34);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 13);
            this.lblMensaje.TabIndex = 0;
            // 
            // mntFecha
            // 
            this.mntFecha.Location = new System.Drawing.Point(33, 118);
            this.mntFecha.Name = "mntFecha";
            this.mntFecha.TabIndex = 3;
            this.mntFecha.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.mntFecha_DateSelected);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Location = new System.Drawing.Point(30, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Seleccione la Fecha";
            // 
            // frmCitasProgramadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mntFecha);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvCitasProgramadas);
            this.Controls.Add(this.panel1);
            this.Name = "frmCitasProgramadas";
            this.Text = "frmCitasProgramadas";
            this.Load += new System.EventHandler(this.frmCitasProgramadas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCitasProgramadas)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvCitasProgramadas;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MonthCalendar mntFecha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMensaje;
    }
}