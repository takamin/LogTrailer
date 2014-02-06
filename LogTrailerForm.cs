using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LibTakamin;
using LibTakamin.Configuration;

namespace LogTrailer {
    public partial class LogTrailerForm : Form {
        /// <summary>
        /// 
        /// </summary>
        static private AppSettingsWrapper conf = new AppSettingsWrapper(ConfigurationManager.AppSettings);
        private RegexStyle[] lineStyleList = null;
        public LogTrailerForm() {
            InitializeComponent();
            timer.Interval = 10;
            timer.Enabled = false;
            this.Load += new EventHandler(LogTrailerForm_Load);
            lineStyleList = RegexStyle.Parse(conf.GetValueAsString("pattern", ""));
            if (lineStyleList.Length > 0) {
                listBoxLog.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxLog.DrawItem += new DrawItemEventHandler(listBoxLog_DrawItem);
            }
        }
        /// <summary>
        /// draws item by using color that defined in application settings with regular expression.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listBoxLog_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0) {
                e.DrawBackground();
                return;
            }

            // tests the string by regular expression pattern to determine the drawing style. 
            string s = listBoxLog.Items[e.Index] as string;
            Color color = Color.Black;
            Color backgroundColor = Color.White;
            bool bold = false;
            foreach (RegexStyle style in lineStyleList) {
                if (style.IsMatch(s)) {
                    color = style.Color;
                    backgroundColor = style.BackgroundColor;
                    bold = style.IsBold;
                }
            }

            // if the item is selected, it is drawn by system colors.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                color = SystemColors.HighlightText;
                backgroundColor = SystemColors.Highlight;
            }

            // draws string by style above.
            Brush backgroundBrush = new SolidBrush(backgroundColor);
            Font font = e.Font;
            if (bold) {
                font = new Font(font, FontStyle.Bold);
            }
            Brush textBrush = new SolidBrush(color);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            e.Graphics.DrawString(s, font, textBrush, e.Bounds.Location);
            
            // draws focus rect, if it is focused.
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus) {
                e.DrawFocusRectangle();
            }
        }

        void LogTrailerForm_Load(object sender, EventArgs e) {
            logTrailer.EncodingChanged += new EventHandler(logTrailer_EncodingChanged);
            AddEncodingToCombo(Encoding.Default);
            AddEncodingToCombo(Encoding.ASCII);
            AddEncodingToCombo(Encoding.Unicode);
            AddEncodingToCombo(Encoding.UTF8);
            AddEncodingToCombo(Encoding.GetEncoding(932));
            AddEncodingToCombo(Encoding.GetEncoding(50220));
            AddEncodingToCombo(Encoding.GetEncoding(51932));
            cmbEncoding.SelectedIndex = GetEncodingIndex(Encoding.Default);

            

        }
        LogTrailReader logTrailer = new LogTrailReader();

        private void toolStripButtonOpen_Click(object sender, EventArgs e) {
            using (OpenFileDialog dlg = new OpenFileDialog()) {
                dlg.CheckPathExists = true;
                dlg.CheckFileExists = true;
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    OpenFile(dlg.FileName);
                }
            }
        }
        private void LogTrailerForm_DragEnter(object sender, DragEventArgs e) {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filenames.Length == 1 && GetFileSize(filenames[0]) >= 0) {
                e.Effect = DragDropEffects.All;
            }
        }
        private void LogTrailerForm_DragDrop(object sender, DragEventArgs e) {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (filenames.Length == 1 && GetFileSize(filenames[0]) >= 0) {
                e.Effect = DragDropEffects.All;
                OpenFile(filenames[0]);
            }
        }
        private void OpenFile(string filename) {
            if (logTrailer.Filename != filename) {
                timer.Enabled = false;
                logTrailer.Filename = filename;
                Text = "LogTrailer - " + filename;
                timer.Enabled = true;
            }
        }
        private long GetFileSize(string filename) {
            long filelen = -1;
            try {
                FileInfo fileInfo = new FileInfo(filename);
                filelen = fileInfo.Length;
            } catch (Exception) { }
            return filelen;
        }
        object lockobj = new Object();
        private void timer_Tick(object sender, EventArgs e) {
            if (logTrailer.Filename != "") {
                List<string> lines = new List<string>();
                lock (lockobj) {
                    logTrailer.Read(lines);
                }
                Invoke((MethodInvoker)delegate {
                    if (lines.Count > 0) {
                        listBoxLog.BeginUpdate();
                        listBoxLog.Items.AddRange(lines.ToArray<object>());
                        listBoxLog.EndUpdate();
                    }
                    if (cbAlwaysShowLast.Checked) {
                        int count = listBoxLog.Items.Count;
                        int shownItemsCount = listBoxLog.ClientRectangle.Height / listBoxLog.ItemHeight;
                        int topIndex = count - shownItemsCount;
                        if (topIndex < 0) {
                            topIndex = 0;
                        }
                        if (listBoxLog.TopIndex != topIndex) {
                            listBoxLog.TopIndex = topIndex;
                        }
                        //if (count > 0 && listBoxLog.SelectedIndex != count - 1) {
                        //    int shownItemsCount = listBoxLog.ClientRectangle.Height / listBoxLog.ItemHeight;
                        //    int topIndex = count - shownItemsCount;
                        //    if (topIndex < 0) {
                        //        topIndex = 0;
                        //    }
                        //    if (listBoxLog.TopIndex != topIndex) {
                        //        listBoxLog.TopIndex = topIndex;
                        //    }
                        //}
                    }
                });
            }
        }
        class EncodingComboItem {
            public string Name { get; set; }
            public Encoding Encoding { get; set; }
            public override string ToString() {
                return Name;
            }
        }
        void logTrailer_EncodingChanged(object sender, EventArgs e) {
            int index = GetEncodingIndex(logTrailer.Encoding);
            Invoke((MethodInvoker)delegate {
                EncodingComboItem item = null;
                if (index >= 0) {
                    item = GetEncodingItemByIndex(index);
                } else {
                    item = AddEncodingToCombo(logTrailer.Encoding);
                }
                cmbEncoding.SelectedItem = item;
            });
        }
        private int GetEncodingIndex(Encoding encoding) {
            int index = 0;
            foreach (object obj in cmbEncoding.Items) {
                if (obj is EncodingComboItem) {
                    EncodingComboItem item = (EncodingComboItem)obj;
                    if (item.Encoding.Equals(encoding)) {
                        return index;
                    }
                }
                index++;
            }
            return -1;
        }
        private EncodingComboItem GetEncodingItemByIndex(int index) {
            object obj = cmbEncoding.Items[index];
            if (obj is EncodingComboItem) {
                return (EncodingComboItem)obj;
            }
            return null;
        }
        private EncodingComboItem AddEncodingToCombo(Encoding encoding) {
            EncodingComboItem item = null;
            int index = GetEncodingIndex(encoding);
            if (index >= 0) {
                item = GetEncodingItemByIndex(index);
            } else {
                item = new EncodingComboItem();
                item.Name = encoding.EncodingName;
                item.Encoding = encoding;
                cmbEncoding.Items.Add(item);
            }
            return item;
        }

        private void cbEncoding_SelectedIndexChanged(object sender, EventArgs e) {
            Object obj = cmbEncoding.Items[cmbEncoding.SelectedIndex];
            if (obj != null && obj is EncodingComboItem) {
                EncodingComboItem item = (EncodingComboItem)obj;
                if (!item.Encoding.Equals(logTrailer.Encoding)) {
                    logTrailer.Encoding = item.Encoding;
                }
            }
        }
    }
}
