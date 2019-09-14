namespace DataSync
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.syncTabCbo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ckbFileds = new System.Windows.Forms.CheckedListBox();
            this.srcDataBaseCbo = new System.Windows.Forms.ComboBox();
            this.dstDataBaseCbo = new System.Windows.Forms.ComboBox();
            this.lblDelFail = new System.Windows.Forms.Label();
            this.txWhere = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.ckbInsert = new System.Windows.Forms.CheckBox();
            this.ckbUpdate = new System.Windows.Forms.CheckBox();
            this.ckbDelete = new System.Windows.Forms.CheckBox();
            this.cboAllField = new System.Windows.Forms.CheckBox();
            this.lblInsertFail = new System.Windows.Forms.Label();
            this.lblUpdateFail = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbMainOfOnToOne = new System.Windows.Forms.GroupBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.gbShowSync = new System.Windows.Forms.GroupBox();
            this.cboFourField = new System.Windows.Forms.CheckBox();
            this.cboLock = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbMainOfOnToOne.SuspendLayout();
            this.gbShowSync.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "源数据库：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "目标数据库：";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(678, 42);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(105, 44);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(502, 36);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(105, 44);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // syncTabCbo
            // 
            this.syncTabCbo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.syncTabCbo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.syncTabCbo.Enabled = false;
            this.syncTabCbo.FormattingEnabled = true;
            this.syncTabCbo.Location = new System.Drawing.Point(111, 88);
            this.syncTabCbo.Name = "syncTabCbo";
            this.syncTabCbo.Size = new System.Drawing.Size(299, 22);
            this.syncTabCbo.TabIndex = 4;
            this.syncTabCbo.SelectedIndexChanged += new System.EventHandler(this.SyncTabCbo_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 1;
            this.label5.Text = "同步的表：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 325);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 1;
            this.label6.Text = "同步的字段：";
            // 
            // ckbFileds
            // 
            this.ckbFileds.FormattingEnabled = true;
            this.ckbFileds.Location = new System.Drawing.Point(111, 341);
            this.ckbFileds.MultiColumn = true;
            this.ckbFileds.Name = "ckbFileds";
            this.ckbFileds.Size = new System.Drawing.Size(782, 130);
            this.ckbFileds.TabIndex = 5;
            // 
            // srcDataBaseCbo
            // 
            this.srcDataBaseCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.srcDataBaseCbo.FormattingEnabled = true;
            this.srcDataBaseCbo.Location = new System.Drawing.Point(111, 20);
            this.srcDataBaseCbo.Name = "srcDataBaseCbo";
            this.srcDataBaseCbo.Size = new System.Drawing.Size(299, 22);
            this.srcDataBaseCbo.TabIndex = 4;
            this.srcDataBaseCbo.SelectedIndexChanged += new System.EventHandler(this.SrcDataBaseCbo_SelectedIndexChanged);
            // 
            // dstDataBaseCbo
            // 
            this.dstDataBaseCbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dstDataBaseCbo.FormattingEnabled = true;
            this.dstDataBaseCbo.Location = new System.Drawing.Point(111, 54);
            this.dstDataBaseCbo.Name = "dstDataBaseCbo";
            this.dstDataBaseCbo.Size = new System.Drawing.Size(299, 22);
            this.dstDataBaseCbo.TabIndex = 4;
            this.dstDataBaseCbo.SelectedIndexChanged += new System.EventHandler(this.DstDataBaseCbo_SelectedIndexChanged);
            // 
            // lblDelFail
            // 
            this.lblDelFail.AutoSize = true;
            this.lblDelFail.ForeColor = System.Drawing.Color.Red;
            this.lblDelFail.Location = new System.Drawing.Point(240, 24);
            this.lblDelFail.Name = "lblDelFail";
            this.lblDelFail.Size = new System.Drawing.Size(0, 12);
            this.lblDelFail.TabIndex = 1;
            // 
            // txWhere
            // 
            this.txWhere.Location = new System.Drawing.Point(111, 141);
            this.txWhere.Multiline = true;
            this.txWhere.Name = "txWhere";
            this.txWhere.Size = new System.Drawing.Size(782, 160);
            this.txWhere.TabIndex = 7;
            this.txWhere.Text = "where";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(502, 42);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(105, 44);
            this.btnCompare.TabIndex = 3;
            this.btnCompare.Text = "对比";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // ckbInsert
            // 
            this.ckbInsert.AutoSize = true;
            this.ckbInsert.Checked = true;
            this.ckbInsert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbInsert.Location = new System.Drawing.Point(65, 81);
            this.ckbInsert.Name = "ckbInsert";
            this.ckbInsert.Size = new System.Drawing.Size(48, 16);
            this.ckbInsert.TabIndex = 8;
            this.ckbInsert.Text = "新增";
            this.ckbInsert.UseVisualStyleBackColor = true;
            // 
            // ckbUpdate
            // 
            this.ckbUpdate.AutoSize = true;
            this.ckbUpdate.Checked = true;
            this.ckbUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbUpdate.Location = new System.Drawing.Point(65, 51);
            this.ckbUpdate.Name = "ckbUpdate";
            this.ckbUpdate.Size = new System.Drawing.Size(48, 16);
            this.ckbUpdate.TabIndex = 8;
            this.ckbUpdate.Text = "更新";
            this.ckbUpdate.UseVisualStyleBackColor = true;
            // 
            // ckbDelete
            // 
            this.ckbDelete.AutoSize = true;
            this.ckbDelete.Checked = true;
            this.ckbDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbDelete.Location = new System.Drawing.Point(65, 20);
            this.ckbDelete.Name = "ckbDelete";
            this.ckbDelete.Size = new System.Drawing.Size(48, 16);
            this.ckbDelete.TabIndex = 8;
            this.ckbDelete.Text = "删除";
            this.ckbDelete.UseVisualStyleBackColor = true;
            // 
            // cboAllField
            // 
            this.cboAllField.AutoSize = true;
            this.cboAllField.Checked = true;
            this.cboAllField.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboAllField.Location = new System.Drawing.Point(111, 323);
            this.cboAllField.Name = "cboAllField";
            this.cboAllField.Size = new System.Drawing.Size(54, 18);
            this.cboAllField.TabIndex = 8;
            this.cboAllField.Text = "全选";
            this.cboAllField.UseVisualStyleBackColor = true;
            this.cboAllField.CheckedChanged += new System.EventHandler(this.CboAllField_CheckedChanged);
            // 
            // lblInsertFail
            // 
            this.lblInsertFail.AutoSize = true;
            this.lblInsertFail.ForeColor = System.Drawing.Color.Red;
            this.lblInsertFail.Location = new System.Drawing.Point(240, 85);
            this.lblInsertFail.Name = "lblInsertFail";
            this.lblInsertFail.Size = new System.Drawing.Size(0, 12);
            this.lblInsertFail.TabIndex = 1;
            // 
            // lblUpdateFail
            // 
            this.lblUpdateFail.AutoSize = true;
            this.lblUpdateFail.ForeColor = System.Drawing.Color.Red;
            this.lblUpdateFail.Location = new System.Drawing.Point(240, 55);
            this.lblUpdateFail.Name = "lblUpdateFail";
            this.lblUpdateFail.Size = new System.Drawing.Size(0, 12);
            this.lblUpdateFail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "条件：";
            // 
            // gbMainOfOnToOne
            // 
            this.gbMainOfOnToOne.Controls.Add(this.btnChange);
            this.gbMainOfOnToOne.Controls.Add(this.label2);
            this.gbMainOfOnToOne.Controls.Add(this.label5);
            this.gbMainOfOnToOne.Controls.Add(this.label6);
            this.gbMainOfOnToOne.Controls.Add(this.cboFourField);
            this.gbMainOfOnToOne.Controls.Add(this.cboLock);
            this.gbMainOfOnToOne.Controls.Add(this.cboAllField);
            this.gbMainOfOnToOne.Controls.Add(this.txWhere);
            this.gbMainOfOnToOne.Controls.Add(this.ckbFileds);
            this.gbMainOfOnToOne.Controls.Add(this.dstDataBaseCbo);
            this.gbMainOfOnToOne.Controls.Add(this.label3);
            this.gbMainOfOnToOne.Controls.Add(this.label1);
            this.gbMainOfOnToOne.Controls.Add(this.srcDataBaseCbo);
            this.gbMainOfOnToOne.Controls.Add(this.label4);
            this.gbMainOfOnToOne.Controls.Add(this.syncTabCbo);
            this.gbMainOfOnToOne.Controls.Add(this.btnStart);
            this.gbMainOfOnToOne.Controls.Add(this.btnCompare);
            this.gbMainOfOnToOne.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbMainOfOnToOne.Location = new System.Drawing.Point(12, 12);
            this.gbMainOfOnToOne.Name = "gbMainOfOnToOne";
            this.gbMainOfOnToOne.Size = new System.Drawing.Size(904, 491);
            this.gbMainOfOnToOne.TabIndex = 9;
            this.gbMainOfOnToOne.TabStop = false;
            this.gbMainOfOnToOne.Text = "控制台";
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(420, 20);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(23, 54);
            this.btnChange.TabIndex = 9;
            this.btnChange.Text = "交换";
            this.btnChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.BtnChange);
            // 
            // gbShowSync
            // 
            this.gbShowSync.Controls.Add(this.ckbDelete);
            this.gbShowSync.Controls.Add(this.lblDelFail);
            this.gbShowSync.Controls.Add(this.btnStop);
            this.gbShowSync.Controls.Add(this.ckbInsert);
            this.gbShowSync.Controls.Add(this.lblInsertFail);
            this.gbShowSync.Controls.Add(this.lblUpdateFail);
            this.gbShowSync.Controls.Add(this.ckbUpdate);
            this.gbShowSync.Location = new System.Drawing.Point(12, 509);
            this.gbShowSync.Name = "gbShowSync";
            this.gbShowSync.Size = new System.Drawing.Size(904, 111);
            this.gbShowSync.TabIndex = 10;
            this.gbShowSync.TabStop = false;
            this.gbShowSync.Text = "同步进度";
            // 
            // cboFourField
            // 
            this.cboFourField.AutoSize = true;
            this.cboFourField.Location = new System.Drawing.Point(192, 323);
            this.cboFourField.Name = "cboFourField";
            this.cboFourField.Size = new System.Drawing.Size(215, 18);
            this.cboFourField.TabIndex = 8;
            this.cboFourField.Text = "不选 USEC、CDAT、USEU、LSTU";
            this.cboFourField.UseVisualStyleBackColor = true;
            this.cboFourField.CheckedChanged += new System.EventHandler(this.CboFourField_CheckedChanged);
            // 
            // cboLock
            // 
            this.cboLock.AutoSize = true;
            this.cboLock.Location = new System.Drawing.Point(448, 40);
            this.cboLock.Name = "cboLock";
            this.cboLock.Size = new System.Drawing.Size(54, 18);
            this.cboLock.TabIndex = 8;
            this.cboLock.Text = "锁定";
            this.cboLock.UseVisualStyleBackColor = true;
            this.cboLock.CheckedChanged += new System.EventHandler(this.CboLock_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(109, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "例如： where 1=1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 628);
            this.Controls.Add(this.gbMainOfOnToOne);
            this.Controls.Add(this.gbShowSync);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据同步";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbMainOfOnToOne.ResumeLayout(false);
            this.gbMainOfOnToOne.PerformLayout();
            this.gbShowSync.ResumeLayout(false);
            this.gbShowSync.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox syncTabCbo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox ckbFileds;
        private System.Windows.Forms.ComboBox srcDataBaseCbo;
        private System.Windows.Forms.ComboBox dstDataBaseCbo;
        private System.Windows.Forms.Label lblDelFail;
        private System.Windows.Forms.TextBox txWhere;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.CheckBox ckbInsert;
        private System.Windows.Forms.CheckBox ckbUpdate;
        private System.Windows.Forms.CheckBox ckbDelete;
        private System.Windows.Forms.CheckBox cboAllField;
        private System.Windows.Forms.Label lblInsertFail;
        private System.Windows.Forms.Label lblUpdateFail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbMainOfOnToOne;
        private System.Windows.Forms.GroupBox gbShowSync;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cboFourField;
        private System.Windows.Forms.CheckBox cboLock;
    }
}

