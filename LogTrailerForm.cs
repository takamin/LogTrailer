using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LibTakamin;

namespace LogTrailer {
    public partial class LogTrailerForm : Form {
        public LogTrailerForm() {
            InitializeComponent();
            timer.Interval = 10;
            timer.Enabled = false;
            this.Load += new EventHandler(LogTrailerForm_Load);
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
                if (logTrailer.Filename != filenames[0]) {
                    timer.Enabled = false;
                    logTrailer.Filename = filenames[0];
                    Text = "LogTrailer - " + filenames[0];
                    timer.Enabled = true;
                }
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
                        listBoxLog.Items.AddRange(lines.ToArray<object>());
                    }
                    if (cbAlwaysShowLast.Checked) {
                        int count = listBoxLog.Items.Count;
                        if (count > 0 && listBoxLog.SelectedIndex != count - 1) {
                            int shownItemsCount = listBoxLog.ClientRectangle.Height / listBoxLog.ItemHeight;
                            int topIndex = count - shownItemsCount;
                            if (topIndex < 0) {
                                topIndex = 0;
                            }
                            if (listBoxLog.TopIndex != topIndex) {
                                listBoxLog.TopIndex = topIndex;
                            }
                        }
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
