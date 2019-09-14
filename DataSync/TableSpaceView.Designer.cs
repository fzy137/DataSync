namespace DataSync
{
    partial class TableSpaceView
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
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.database = new System.Windows.Forms.Label();
            this.cboDataBase = new System.Windows.Forms.ComboBox();
            this.tablespace_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableSpaceUseSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.free = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tablespace_name,
            this.total,
            this.tableSpaceUseSize,
            this.free,
            this.usePercent});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Name = "dgvList";
            this.dgvList.RowTemplate.Height = 23;
            this.dgvList.Size = new System.Drawing.Size(729, 372);
            this.dgvList.TabIndex = 1;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.database);
            this.splitContainer.Panel1.Controls.Add(this.cboDataBase);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dgvList);
            this.splitContainer.Size = new System.Drawing.Size(729, 420);
            this.splitContainer.SplitterDistance = 44;
            this.splitContainer.TabIndex = 2;
            // 
            // database
            // 
            this.database.AutoSize = true;
            this.database.Location = new System.Drawing.Point(9, 16);
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(41, 12);
            this.database.TabIndex = 2;
            this.database.Text = "数据库";
            // 
            // cboDataBase
            // 
            this.cboDataBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataBase.FormattingEnabled = true;
            this.cboDataBase.Location = new System.Drawing.Point(55, 12);
            this.cboDataBase.Name = "cboDataBase";
            this.cboDataBase.Size = new System.Drawing.Size(264, 20);
            this.cboDataBase.TabIndex = 1;
            this.cboDataBase.SelectedIndexChanged += new System.EventHandler(this.CboDataBase_SelectedIndexChanged);
            // 
            // tablespace_name
            // 
            this.tablespace_name.DataPropertyName = "tablespace_name";
            this.tablespace_name.HeaderText = "表空间名";
            this.tablespace_name.Name = "tablespace_name";
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            this.total.HeaderText = "表空间大小(G)";
            this.total.Name = "total";
            // 
            // tableSpaceUseSize
            // 
            this.tableSpaceUseSize.DataPropertyName = "tableSpaceUseSize";
            this.tableSpaceUseSize.HeaderText = "表空间使用大小(G)";
            this.tableSpaceUseSize.Name = "tableSpaceUseSize";
            // 
            // free
            // 
            this.free.DataPropertyName = "free";
            this.free.HeaderText = "表空间剩余大小(G)";
            this.free.Name = "free";
            // 
            // usePercent
            // 
            this.usePercent.DataPropertyName = "usePercent";
            this.usePercent.HeaderText = "使用率%";
            this.usePercent.Name = "usePercent";
            // 
            // TableSpaceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 420);
            this.Controls.Add(this.splitContainer);
            this.MaximizeBox = false;
            this.Name = "TableSpaceView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORACLE表空间查询";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label database;
        private System.Windows.Forms.ComboBox cboDataBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn tablespace_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableSpaceUseSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn free;
        private System.Windows.Forms.DataGridViewTextBoxColumn usePercent;
    }
}