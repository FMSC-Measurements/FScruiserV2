using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class Form3PNumPad : Form
    {
        #region Designer Code

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
            this._outputView = new System.Windows.Forms.TextBox();
            this._mainContentPanel = new System.Windows.Forms.Panel();
            this._row4 = new System.Windows.Forms.Panel();
            this._STMBtn = new System.Windows.Forms.Button();
            this._clear = new System.Windows.Forms.Button();
            this._accept = new System.Windows.Forms.Button();
            this._num0 = new System.Windows.Forms.Button();
            this._row3 = new System.Windows.Forms.Panel();
            this._num8 = new System.Windows.Forms.Button();
            this._num9 = new System.Windows.Forms.Button();
            this._num7 = new System.Windows.Forms.Button();
            this._row2 = new System.Windows.Forms.Panel();
            this._num5 = new System.Windows.Forms.Button();
            this._num6 = new System.Windows.Forms.Button();
            this._num4 = new System.Windows.Forms.Button();
            this._row1 = new System.Windows.Forms.Panel();
            this._num2 = new System.Windows.Forms.Button();
            this._num3 = new System.Windows.Forms.Button();
            this._num1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._mainContentPanel.SuspendLayout();
            this._row4.SuspendLayout();
            this._row3.SuspendLayout();
            this._row2.SuspendLayout();
            this._row1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _outputView
            // 
            this._outputView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outputView.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
            this._outputView.Location = new System.Drawing.Point(0, 0);
            this._outputView.Name = "_outputView";
            this._outputView.ReadOnly = true;
            this._outputView.Size = new System.Drawing.Size(186, 35);
            this._outputView.TabIndex = 0;
            this._outputView.TabStop = false;
            // 
            // _mainContentPanel
            // 
            this._mainContentPanel.Controls.Add(this._row4);
            this._mainContentPanel.Controls.Add(this._row3);
            this._mainContentPanel.Controls.Add(this._row2);
            this._mainContentPanel.Controls.Add(this._row1);
            this._mainContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContentPanel.Location = new System.Drawing.Point(0, 35);
            this._mainContentPanel.Name = "_mainContentPanel";
            this._mainContentPanel.Size = new System.Drawing.Size(240, 256);
            // 
            // _row4
            // 
            this._row4.Controls.Add(this._STMBtn);
            this._row4.Controls.Add(this._accept);
            this._row4.Controls.Add(this._cancelBtn);
            this._row4.Controls.Add(this._num0);
            this._row4.Dock = System.Windows.Forms.DockStyle.Fill;
            this._row4.Location = new System.Drawing.Point(0, 192);
            this._row4.Name = "_row4";
            this._row4.Size = new System.Drawing.Size(240, 64);
            // 
            // _STMBtn
            // 
            this._STMBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._STMBtn.Location = new System.Drawing.Point(60, 0);
            this._STMBtn.Name = "_STMBtn";
            this._STMBtn.Size = new System.Drawing.Size(54, 64);
            this._STMBtn.TabIndex = 12;
            this._STMBtn.Text = "STM";
            this._STMBtn.Click += new System.EventHandler(this._STMBtn_Click);
            // 
            // _clear
            // 
            this._clear.Dock = System.Windows.Forms.DockStyle.Right;
            this._clear.Location = new System.Drawing.Point(186, 0);
            this._clear.Name = "_clear";
            this._clear.Size = new System.Drawing.Size(54, 35);
            this._clear.TabIndex = 10;
            this._clear.Text = "&Clear";
            this._clear.Click += new System.EventHandler(this._clear_Click);
            // 
            // _accept
            // 
            this._accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._accept.Dock = System.Windows.Forms.DockStyle.Right;
            this._accept.Location = new System.Drawing.Point(114, 0);
            this._accept.Name = "_accept";
            this._accept.Size = new System.Drawing.Size(60, 64);
            this._accept.TabIndex = 11;
            this._accept.Text = "&Accept";
            this._accept.Click += new System.EventHandler(this._accept_Click);
            // 
            // _num0
            // 
            this._num0.Dock = System.Windows.Forms.DockStyle.Left;
            this._num0.Location = new System.Drawing.Point(0, 0);
            this._num0.Name = "_num0";
            this._num0.Size = new System.Drawing.Size(60, 64);
            this._num0.TabIndex = 9;
            this._num0.Text = "0";
            this._num0.Click += new System.EventHandler(this._num0_Click);
            // 
            // _row3
            // 
            this._row3.Controls.Add(this._num8);
            this._row3.Controls.Add(this._num9);
            this._row3.Controls.Add(this._num7);
            this._row3.Dock = System.Windows.Forms.DockStyle.Top;
            this._row3.Location = new System.Drawing.Point(0, 128);
            this._row3.Name = "_row3";
            this._row3.Size = new System.Drawing.Size(240, 64);
            // 
            // _num8
            // 
            this._num8.Dock = System.Windows.Forms.DockStyle.Fill;
            this._num8.Location = new System.Drawing.Point(80, 0);
            this._num8.Name = "_num8";
            this._num8.Size = new System.Drawing.Size(80, 64);
            this._num8.TabIndex = 7;
            this._num8.Text = "8";
            this._num8.Click += new System.EventHandler(this._num8_Click);
            // 
            // _num9
            // 
            this._num9.Dock = System.Windows.Forms.DockStyle.Right;
            this._num9.Location = new System.Drawing.Point(160, 0);
            this._num9.Name = "_num9";
            this._num9.Size = new System.Drawing.Size(80, 64);
            this._num9.TabIndex = 8;
            this._num9.Text = "9";
            this._num9.Click += new System.EventHandler(this._num9_Click);
            // 
            // _num7
            // 
            this._num7.Dock = System.Windows.Forms.DockStyle.Left;
            this._num7.Location = new System.Drawing.Point(0, 0);
            this._num7.Name = "_num7";
            this._num7.Size = new System.Drawing.Size(80, 64);
            this._num7.TabIndex = 6;
            this._num7.Text = "7";
            this._num7.Click += new System.EventHandler(this._num7_Click);
            // 
            // _row2
            // 
            this._row2.Controls.Add(this._num5);
            this._row2.Controls.Add(this._num6);
            this._row2.Controls.Add(this._num4);
            this._row2.Dock = System.Windows.Forms.DockStyle.Top;
            this._row2.Location = new System.Drawing.Point(0, 64);
            this._row2.Name = "_row2";
            this._row2.Size = new System.Drawing.Size(240, 64);
            // 
            // _num5
            // 
            this._num5.Dock = System.Windows.Forms.DockStyle.Fill;
            this._num5.Location = new System.Drawing.Point(80, 0);
            this._num5.Name = "_num5";
            this._num5.Size = new System.Drawing.Size(80, 64);
            this._num5.TabIndex = 4;
            this._num5.Text = "5";
            this._num5.Click += new System.EventHandler(this._num5_Click);
            // 
            // _num6
            // 
            this._num6.Dock = System.Windows.Forms.DockStyle.Right;
            this._num6.Location = new System.Drawing.Point(160, 0);
            this._num6.Name = "_num6";
            this._num6.Size = new System.Drawing.Size(80, 64);
            this._num6.TabIndex = 5;
            this._num6.Text = "6";
            this._num6.Click += new System.EventHandler(this._num6_Click);
            // 
            // _num4
            // 
            this._num4.Dock = System.Windows.Forms.DockStyle.Left;
            this._num4.Location = new System.Drawing.Point(0, 0);
            this._num4.Name = "_num4";
            this._num4.Size = new System.Drawing.Size(80, 64);
            this._num4.TabIndex = 3;
            this._num4.Text = "4";
            this._num4.Click += new System.EventHandler(this._num4_Click);
            // 
            // _row1
            // 
            this._row1.Controls.Add(this._num2);
            this._row1.Controls.Add(this._num3);
            this._row1.Controls.Add(this._num1);
            this._row1.Dock = System.Windows.Forms.DockStyle.Top;
            this._row1.Location = new System.Drawing.Point(0, 0);
            this._row1.Name = "_row1";
            this._row1.Size = new System.Drawing.Size(240, 64);
            // 
            // _num2
            // 
            this._num2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._num2.Location = new System.Drawing.Point(80, 0);
            this._num2.Name = "_num2";
            this._num2.Size = new System.Drawing.Size(80, 64);
            this._num2.TabIndex = 1;
            this._num2.Text = "2";
            this._num2.Click += new System.EventHandler(this._num2_Click);
            // 
            // _num3
            // 
            this._num3.Dock = System.Windows.Forms.DockStyle.Right;
            this._num3.Location = new System.Drawing.Point(160, 0);
            this._num3.Name = "_num3";
            this._num3.Size = new System.Drawing.Size(80, 64);
            this._num3.TabIndex = 2;
            this._num3.Text = "3";
            this._num3.Click += new System.EventHandler(this._num3_Click);
            // 
            // _num1
            // 
            this._num1.Dock = System.Windows.Forms.DockStyle.Left;
            this._num1.Location = new System.Drawing.Point(0, 0);
            this._num1.Name = "_num1";
            this._num1.Size = new System.Drawing.Size(80, 64);
            this._num1.TabIndex = 0;
            this._num1.Text = "1";
            this._num1.Click += new System.EventHandler(this._num1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._outputView);
            this.panel1.Controls.Add(this._clear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 35);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this._cancelBtn.Location = new System.Drawing.Point(174, 0);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(66, 64);
            this._cancelBtn.TabIndex = 13;
            this._cancelBtn.Text = "Cancel";
            // 
            // Form3PNumPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 291);
            this.Controls.Add(this._mainContentPanel);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "Form3PNumPad";
            this.Text = "3P Number Pad";
            this._mainContentPanel.ResumeLayout(false);
            this._row4.ResumeLayout(false);
            this._row3.ResumeLayout(false);
            this._row2.ResumeLayout(false);
            this._row1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.TextBox _outputView;
        private System.Windows.Forms.Panel _mainContentPanel;
        private System.Windows.Forms.Panel _row4;
        private System.Windows.Forms.Panel _row3;
        private System.Windows.Forms.Panel _row2;
        private System.Windows.Forms.Button _num4;
        private System.Windows.Forms.Panel _row1;
        private System.Windows.Forms.Button _num2;
        private System.Windows.Forms.Button _num3;
        private System.Windows.Forms.Button _num1;
        private System.Windows.Forms.Button _clear;
        private System.Windows.Forms.Button _accept;
        private System.Windows.Forms.Button _num0;
        private System.Windows.Forms.Button _num8;
        private System.Windows.Forms.Button _num9;
        private System.Windows.Forms.Button _num7;
        private System.Windows.Forms.Button _num5;
        private System.Windows.Forms.Button _num6;

        #endregion Designer Code

        private int? _minValue;
        private int? _maxValue;
        private Button _STMBtn;
        private Panel panel1;
        private Button _cancelBtn;
        private bool _canReturnNull = false;

        public Form3PNumPad()
        {
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            _mainContentPanel.Resize += new EventHandler(_mainContentPanel_Resize);
            this.KeyPreview = true;
            //int rowHeight = _mainContentPanel.Height / 4;
            //int remainder = _mainContentPanel.Height % 4;
            //_row1.Height = rowHeight;
            //_row2.Height = rowHeight;
            //_row3.Height = rowHeight;
            //_row4.Height = rowHeight + remainder;
        }

        void _mainContentPanel_Resize(object sender, EventArgs e)
        {
            _row1.SuspendLayout();
            _row2.SuspendLayout();
            _row3.SuspendLayout();
            _row4.SuspendLayout();
            int rowHeight = _mainContentPanel.Height / 4;
            int remainder = _mainContentPanel.Height % 4;
            _row1.Height = rowHeight;
            _row2.Height = rowHeight;
            _row3.Height = rowHeight;
            _row4.Height = rowHeight + remainder;

            //_mainContentPanel.ResumeLayout(true);
            _row1.ResumeLayout();
            _row2.ResumeLayout();
            _row3.ResumeLayout();
            _row4.ResumeLayout();
        }

        public DialogResult ShowDialog(int? initialValue, bool canReturnNull)
        {
            return this.ShowDialog(null, null, initialValue, canReturnNull);
        }

        public DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull)
        {
            _minValue = (min != null && min > 0) ? min : null;

            _maxValue = (max != null && max > 0 && max > min) ? max : null;

            this.UserEnteredValue = initialValue;
            this._canReturnNull = canReturnNull;
            return this.ShowDialog();
        }

        //public int MaxValue { get; set; }
        /// <summary>
        /// returns -1 if value is STM
        /// </summary>
        public int? UserEnteredValue
        {
            get
            {
                if (String.IsNullOrEmpty(_outputView.Text))
                {
                    return null;
                }
                if (_outputView.Text == "STM")
                {
                    return -1;
                }
                return Convert.ToInt32(_outputView.Text);
            }
            set
            {
                _outputView.Text = (value != null) ? value.Value.ToString() : String.Empty;
            }
        }

        private void _num1_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "1";
        }

        private void _num2_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "2";
        }

        private void _num3_Click(object sender, EventArgs e)
        {
            _outputView.Text += "3";
        }

        private void _num4_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "4";
        }

        private void _num5_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "5";
        }

        private void _num6_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "6";
        }

        private void _num7_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "7";
        }

        private void _num8_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "8";
        }

        private void _num9_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "9";
        }

        private void _num0_Click(object sender, EventArgs e)
        {
            if (_outputView.Text == "STM")
            {
                _outputView.Text = String.Empty;
            }
            _outputView.Text += "0";
        }

        private void _clear_Click(object sender, EventArgs e)
        {
            _outputView.Text = String.Empty;
        }

        private void _accept_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.DialogResult == DialogResult.Cancel)
            {
                this.UserEnteredValue = null;
                return;
            }

            if (!_canReturnNull && this.UserEnteredValue == null)
            {
                MessageBox.Show("No Value Entered");
                e.Cancel = true;
            }

            if (this._outputView.Text == "STM")
            {
                return;
            }

            if (this._minValue != null && this.UserEnteredValue < this._minValue.Value)
            {
                MessageBox.Show("Must be Greater or Equal to " + this._minValue.ToString());
                e.Cancel = true;
            }
            else if (this._maxValue != null && this.UserEnteredValue > this._maxValue.Value)
            {
                MessageBox.Show("Must be Less Than or Equal to " + this._maxValue.ToString());
                e.Cancel = true;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':
                    {
                        _outputView.Text += e.KeyChar.ToString();
                        e.Handled = true;
                        break;
                    }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                this.Close();
                //this._accept_Click(null, null);
                e.Handled = true;
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Escape))
            {
                this.DialogResult = DialogResult.Cancel;
                this.UserEnteredValue = null;
                this.Close();
                e.Handled = true;
            }
            if ((e.KeyData == Keys.C))
            {
                this._clear_Click(null, null);
                e.Handled = true;
            }
            if ((e.KeyData == Keys.A))
            {
                this.Close();
                //this._accept_Click(null, null);
                e.Handled = true;
            }
        }

        private void _STMBtn_Click(object sender, EventArgs e)
        {
            this._outputView.Text = "STM";
        }
    }
}