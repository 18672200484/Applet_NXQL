using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMCS.CarTransport.QueueScreen.UserControls
{
    public partial class UCtrlMineInfo : UserControl
    {
        private int spacing = 0;
        [Browsable(true)]
        [Description("控件上下间距")]
        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        private string title;
        [Browsable(true)]
        [Description("标题")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                lblTitle.Text = value;
                base.Invalidate();
            }
        }

        private List<string> content;
        [Browsable(true)]
        [Description("内容")]
        public List<string> Content
        {
            get { return content; }
            set
            {
                content = value;

                lblCarNumbers.ResetText();

                int i = 1;

                foreach (var item in value)
                {
                    lblCarNumbers.Text += item + "  ";

                    if (i % 7 == 0)
                        lblCarNumbers.Text += "\n";

                    i++;
                }
                this.Height = lblTitle.Height + lblCarNumbers.Height + spacing;
                base.Invalidate();
            }
        }

        private Color ctlFontColor = Color.Red;
        [Browsable(true)]
        [Description("控件字体颜色")]
        public Color CtlFontColor
        {
            get { return ctlFontColor; }
            set
            {
                ctlFontColor = value;
                lblTitle.ForeColor = this.CtlFontColor;
                lblCarNumbers.ForeColor = this.CtlFontColor;
                base.Invalidate();
            }
        }

        public UCtrlMineInfo()
        {
            InitializeComponent();
        }

    }
}
