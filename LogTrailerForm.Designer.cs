namespace LogTrailer {
    partial class LogTrailerForm {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogTrailerForm));
            this.toolStripGeneral = new System.Windows.Forms.ToolStrip();
            this.cbAlwaysShowLast = new System.Windows.Forms.ToolStripButton();
            this.cmbEncoding = new System.Windows.Forms.ToolStripComboBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripGeneral
            // 
            this.toolStripGeneral.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbAlwaysShowLast,
            this.cmbEncoding,
            this.toolStripButtonOpen});
            this.toolStripGeneral.Location = new System.Drawing.Point(0, 0);
            this.toolStripGeneral.Name = "toolStripGeneral";
            this.toolStripGeneral.Size = new System.Drawing.Size(496, 26);
            this.toolStripGeneral.TabIndex = 2;
            this.toolStripGeneral.Text = "toolStrip1";
            // 
            // cbAlwaysShowLast
            // 
            this.cbAlwaysShowLast.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.cbAlwaysShowLast.Checked = true;
            this.cbAlwaysShowLast.CheckOnClick = true;
            this.cbAlwaysShowLast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAlwaysShowLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cbAlwaysShowLast.Image = ((System.Drawing.Image)(resources.GetObject("cbAlwaysShowLast.Image")));
            this.cbAlwaysShowLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cbAlwaysShowLast.Name = "cbAlwaysShowLast";
            this.cbAlwaysShowLast.Size = new System.Drawing.Size(101, 23);
            this.cbAlwaysShowLast.Text = "最終行を表示(&L)";
            // 
            // cmbEncoding
            // 
            this.cmbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.Size = new System.Drawing.Size(161, 26);
            this.cmbEncoding.SelectedIndexChanged += new System.EventHandler(this.cbEncoding_SelectedIndexChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 324);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(496, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // listBoxLog
            // 
            this.listBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 15;
            this.listBoxLog.Location = new System.Drawing.Point(0, 26);
            this.listBoxLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxLog.Size = new System.Drawing.Size(496, 298);
            this.listBoxLog.TabIndex = 4;
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(55, 23);
            this.toolStripButtonOpen.Text = "開く(&O)";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // LogTrailerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 346);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStripGeneral);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LogTrailerForm";
            this.Text = "LogTrailer";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.LogTrailerForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.LogTrailerForm_DragEnter);
            this.toolStripGeneral.ResumeLayout(false);
            this.toolStripGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripGeneral;
        private System.Windows.Forms.ToolStripButton cbAlwaysShowLast;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.ToolStripComboBox cmbEncoding;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;

    }
}

