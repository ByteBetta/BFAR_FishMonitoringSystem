using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfPosApp
{
    public partial class frmSoldItemsChart : Form
    {
        frmRecords rc;
        public frmSoldItemsChart(frmRecords frm)
        {
            InitializeComponent();
            rc = frm;
        }
    }
}
