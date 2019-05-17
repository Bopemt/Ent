using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class WirterForm : Form
    {
        public WirterForm()
        {
            InitializeComponent();
            Writer wr = new Writer();
            SampleContext db = new SampleContext();
            //foreach (Computer cm in db.Computers)
            //    wr.DoWriteC(cm);
            //foreach (PeripheralDevice pd in db.PeripheralDevices)
            //    wr.DoWriteP(pd);
            string path = Application.LocalUserAppDataPath;
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    path = dialog.SelectedPath;
            wr.SaveFile(path);
        }
    }
}
