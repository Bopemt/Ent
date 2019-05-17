using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Project1
{
    public partial class MainForm : Form
    {
        SampleContext db = new SampleContext();

        private string path = Application.LocalUserAppDataPath;

        public void UpdatePD()
        {
            using (SampleContext db = new SampleContext())
            {
                //debug
                //db.Database.Log = (s=> System.Diagnostics.Debug.WriteLine(s));
                //db.PeripheralDevices.Load();
                var res = from dev in db.PeripheralDevices
                          join typ in db.Types on dev.TypeId equals typ.TypeId
                          join man in db.Manufacturers on dev.ManufacturerId equals man.ManufacturerId
                          join dep in db.Departaments on dev.DepartamentId equals dep.DepartamentId
                          select new
                          {
                              dev.PDID,
                              typ.TypeName,
                              man.ManufacturerName,
                              dev.name,
                              dev.status,
                              dep.DepartamentName,
                              dev.ComputerId
                          };
                dataGridView1.DataSource = res.ToList();
                //dataGridView1.DataSource = db.PeripheralDevices.Local.ToBindingList();
                //dataGridView1.Columns[4].Visible = false;
                //dataGridView1.Columns[6].Visible = false;
                //dataGridView1.Columns[8].Visible = false;
                //dataGridView1.Columns[10].Visible = false;
            }
        }

        public void UpdateComp()
        {
            using (SampleContext db = new SampleContext())
            {
                //db.Computers.Load();
                //dataGridView2.DataSource = db.Computers.Local.ToBindingList();
                //dataGridView2.Columns[3].Visible = false;
                //dataGridView2.Columns[5].Visible = false;
                //dataGridView2.Columns[7].Visible = false;
                var resn = from cmp in db.Computers
                          join mat in db.Motherboards on cmp.MotherboardId equals mat.MotherboardId
                          join hdd in db.HDDs on cmp.HddId equals hdd.HddId
                          join cp in db.CPUs on cmp.CpuId equals cp.CpuId
                          select new
                          {
                              cmp.ComputerId,
                              cmp.status,
                              mat.MotherboardName,
                              cp.CpuName,
                              hdd.HddName
                          };
                dataGridView2.DataSource = resn.ToList();
            }
        }

        public void UpdateEmployee()
        {
            using (SampleContext db = new SampleContext())
            {
                //db.Employees.Load();
                //dataGridView3.DataSource = db.Employees.Local.ToBindingList();
                //dataGridView3.Columns[4].Visible = false;
                //dataGridView3.Columns[6].Visible = false;
                var resn = from em in db.Employees
                           join dep in db.Departaments on em.DepartamentId equals dep.DepartamentId
                           select new
                           {
                               em.EmployeeID,
                               em.fullName,
                               em.position,
                               dep.DepartamentName,
                               em.ComputerId
                           };
                dataGridView3.DataSource = resn.ToList();
            }
        }

        public MainForm()
        {
            InitializeComponent();
            UpdatePD();
            UpdateComp();
            UpdateEmployee();
            pathBox.Text = path;
        }

        private void Delete(object sender, EventArgs e)
        {
            switch (Tabform.SelectedIndex)
            {
                case 0:
                    int index = dataGridView1.SelectedRows[0].Index;
                    int id = 0;
                    Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                    PeripheralDevice player = db.PeripheralDevices.Find(id);
                    db.PeripheralDevices.Remove(player);
                    db.SaveChanges();
                    UpdatePD();
                    break;
                case 1:
                    int index1 = dataGridView2.SelectedRows[0].Index;
                    int id1 = 0;
                    Int32.TryParse(dataGridView2[0, index1].Value.ToString(), out id1);
                    Computer player1 = db.Computers.Find(id1);
                    db.Computers.Remove(player1);
                    db.SaveChanges();
                    UpdateComp();
                    break;
                case 2:
                    int index2 = dataGridView3.SelectedRows[0].Index;
                    int id2 = 0;
                    Int32.TryParse(dataGridView3[0, index2].Value.ToString(), out id2);
                    Employee player2 = db.Employees.Find(id2);
                    db.Employees.Remove(player2);
                    db.SaveChanges();
                    UpdateEmployee();
                    break;
            }
        }

        private void Add(object sender, EventArgs e)
        {
            switch (Tabform.SelectedIndex)
            {
                case 0:
                    Add_Peripheral_Device pdForm = new Add_Peripheral_Device();
                    pdForm.ShowDialog();
                    UpdatePD();
                    break;
                case 1:
                    Add_Computer compForm = new Add_Computer();
                    compForm.ShowDialog();
                    UpdateComp();
                    break;
                case 2:
                    Add_Employee empForm = new Add_Employee();
                    empForm.ShowDialog();
                    UpdateEmployee();
                    break;
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            switch (Tabform.SelectedIndex)
            {
                case 0:
                    int index = dataGridView1.SelectedRows[0].Index;
                    int id = 0;
                    Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);

                    PeripheralDevice player = db.PeripheralDevices.Find(id);

                    Redit plForm = new Redit();

                    plForm.comboBox4.SelectedItem = player.status;
                    plForm.comboBox1.SelectedValue = player.ManufacturerId;
                    plForm.textBox2.Text = player.name;
                    plForm.comboBox2.SelectedValue = player.TypeId;
                    plForm.comboBox3.SelectedValue = player.DepartamentId;
                    plForm.comboBox5.SelectedValue = player.ComputerId;

                    DialogResult result = plForm.ShowDialog(this);

                    if (result == DialogResult.Cancel)
                        return;

                    player.status = plForm.comboBox4.SelectedItem.ToString();
                    player.name = plForm.textBox2.Text;

                    var val = int.Parse(plForm.comboBox1.SelectedValue.ToString());
                    player.manufacturer = db.Manufacturers.Where(s => s.ManufacturerId == val).FirstOrDefault<Manufacturer>();

                    var val1 = int.Parse(plForm.comboBox2.SelectedValue.ToString());
                    player.type = db.Types.Where(s => s.TypeId == val1).FirstOrDefault<Type>();

                    var val12 = int.Parse(plForm.comboBox3.SelectedValue.ToString());
                    player.departament = db.Departaments.Where(s => s.DepartamentId == val12).FirstOrDefault<Departament>();

                    var val3 = int.Parse(plForm.comboBox5.SelectedValue.ToString());
                    player.computer = db.Computers.Where(s => s.ComputerId == val3).FirstOrDefault<Computer>();

                    db.SaveChanges();
                    UpdatePD();
                    MessageBox.Show("Объект обновлен");
                    break;

                case 1:
                    int index1 = dataGridView2.SelectedRows[0].Index;
                    int id1 = 0;
                    Int32.TryParse(dataGridView2[0, index1].Value.ToString(), out id1);

                    Computer player1 = db.Computers.Find(id1);

                    CEdit plForm1 = new CEdit();

                    plForm1.comboBox4.SelectedItem = player1.status;
                    plForm1.comboBox1.SelectedValue = player1.MotherboardId;
                    plForm1.comboBox2.SelectedValue = player1.HddId;
                    plForm1.comboBox3.SelectedValue = player1.CpuId;

                    DialogResult result1 = plForm1.ShowDialog(this);

                    if (result1 == DialogResult.Cancel)
                        return;

                    player1.status = plForm1.comboBox4.SelectedItem.ToString();

                    var val5 = int.Parse(plForm1.comboBox1.SelectedValue.ToString());
                    player1.motherboard = db.Motherboards.Where(s => s.MotherboardId == val5).FirstOrDefault<Motherboard>();

                    var val6 = int.Parse(plForm1.comboBox2.SelectedValue.ToString());
                    player1.hdd = db.HDDs.Where(s => s.HddId == val6).FirstOrDefault<HDD>();

                    var val7 = int.Parse(plForm1.comboBox3.SelectedValue.ToString());
                    player1.cpu = db.CPUs.Where(s => s.CpuId == val7).FirstOrDefault<CPU>();

                    db.SaveChanges();
                    UpdateComp();
                    MessageBox.Show("Объект обновлен");
                    break;

                case 2:
                    int index2 = dataGridView3.SelectedRows[0].Index;
                    int id2 = 0;
                    Int32.TryParse(dataGridView3[0, index2].Value.ToString(), out id2);

                    Employee player2 = db.Employees.Find(id2);

                    Eedit plForm2 = new Eedit();

                    plForm2.textBox1.Text = player2.fullName;
                    plForm2.textBox2.Text = player2.position;
                    plForm2.comboBox1.SelectedValue = player2.DepartamentId;
                    plForm2.comboBox2.SelectedValue = player2.ComputerId;

                    DialogResult result2 = plForm2.ShowDialog(this);

                    if (result2 == DialogResult.Cancel)
                        return;

                    player2.fullName = plForm2.textBox1.Text;
                    player2.position = plForm2.textBox2.Text;
                    var val8 = int.Parse(plForm2.comboBox1.SelectedValue.ToString());
                    player2.departament = db.Departaments.Where(s => s.DepartamentId == val8).FirstOrDefault<Departament>();

                    var val9 = int.Parse(plForm2.comboBox2.SelectedValue.ToString());
                    player2.computer = db.Computers.Where(s => s.ComputerId == val9).FirstOrDefault<Computer>();

                    db.SaveChanges();
                    UpdateEmployee();
                    MessageBox.Show("Объект обновлен");
                    break;
            }
        }

        private void Tabform_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePD();
            UpdateComp();
            UpdateEmployee();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void writerVuttom_Click(object sender, EventArgs e)
        {
            //WirterForm wr = new WirterForm();
            //wr.ShowDialog();
            List<List<string>> s;
            Writer wr = new Writer();
            string ComputerId = "0";
            string status;
            string DepartamentName;
            string fullName;

            string PDID;
            string name;
            string type;
            string manufac;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    wr.ReadyToC();
            var res0 = from cmp in db.Computers
                       join vrk in db.Employees on cmp.ComputerId equals vrk.ComputerId
                       join nrk in db.Computers on cmp.ComputerId equals nrk.ComputerId
                       join dep in db.Departaments on vrk.DepartamentId equals dep.DepartamentId
                       where cmp.status == "On work"
                       select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           dep.DepartamentName,
                           vrk.fullName
                       };
            s = res0.ToList();
            for (int i = 0; i != s.Count; i++)
            {
                ComputerId = s[i][0];
                status = s[i][1];
                DepartamentName = s[i][2];
                fullName = s[i][3];
                wr.DoWriteC(ComputerId, status, DepartamentName, fullName);
            }
                    break;
                case 1:
                    wr.ReadyToC();
                    var res1 = from cmp in db.Computers
                               join vrk in db.Employees on cmp.ComputerId equals vrk.ComputerId
                               join nrk in db.Computers on cmp.ComputerId equals nrk.ComputerId
                               join dep in db.Departaments on vrk.DepartamentId equals dep.DepartamentId
                               where cmp.status == "On repair"
                               select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           dep.DepartamentName,
                           vrk.fullName
                       };
                    s = res1.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        ComputerId = s[i][0];
                        status = s[i][1];
                        DepartamentName = s[i][2];
                        fullName = s[i][3];
                        wr.DoWriteC(ComputerId, status, DepartamentName, fullName);
                    }
                    break;
                    case 2:
                    wr.ReadyToC();
                    var res2 = from cmp in db.Computers
                               join vrk in db.Employees on cmp.ComputerId equals vrk.ComputerId
                               join nrk in db.Computers on cmp.ComputerId equals nrk.ComputerId
                               join dep in db.Departaments on vrk.DepartamentId equals dep.DepartamentId
                               where cmp.status == "Broke"
                               select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           dep.DepartamentName,
                           vrk.fullName
                       };
                    s = res2.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        ComputerId = s[i][0];
                        status = s[i][1];
                        DepartamentName = s[i][2];
                        fullName = s[i][3];
                        wr.DoWriteC(ComputerId, status, DepartamentName, fullName);
                    }
                    break;
                    case 3:
                    wr.ReadyToD();
                    var pdr1 = from dev in db.PeripheralDevices
                              join typ in db.Types on dev.TypeId equals typ.TypeId
                              join man in db.Manufacturers on dev.ManufacturerId equals man.ManufacturerId
                              join dep in db.Departaments on dev.DepartamentId equals dep.DepartamentId
                               where dev.status == "On work"
                               select new List<string>()
                          {
                              dev.PDID.ToString(),
                              typ.TypeName,
                              man.ManufacturerName,
                              dev.name,
                              dev.status,
                              dep.DepartamentName,
                              dev.ComputerId.ToString()
                          };
                    s = pdr1.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        PDID = s[i][0];
                        type = s[i][1];
                        manufac = s[i][2];
                        name = s[i][3];
                        status = s[i][4];
                        DepartamentName = s[i][5];
                        ComputerId = s[i][6];
                        wr.DoWriteP(PDID, ComputerId, name, type, manufac, status);
                    }
                    break;
                    case 4:
                    wr.ReadyToD();
                    var pdr2 = from dev in db.PeripheralDevices
                               join typ in db.Types on dev.TypeId equals typ.TypeId
                               join man in db.Manufacturers on dev.ManufacturerId equals man.ManufacturerId
                               join dep in db.Departaments on dev.DepartamentId equals dep.DepartamentId
                               where dev.status == "On repair"
                               select new List<string>()
                          {
                              dev.PDID.ToString(),
                              typ.TypeName,
                              man.ManufacturerName,
                              dev.name,
                              dev.status,
                              dep.DepartamentName,
                              dev.ComputerId.ToString()
                          };
                    s = pdr2.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        PDID = s[i][0];
                        type = s[i][1];
                        manufac = s[i][2];
                        name = s[i][3];
                        status = s[i][4];
                        DepartamentName = s[i][5];
                        ComputerId = s[i][6];
                        wr.DoWriteP(PDID, ComputerId, name, type, manufac, status);
                    }
                    break;
                    case 5:
                    wr.ReadyToD();
                    var pdr3 = from dev in db.PeripheralDevices
                               join typ in db.Types on dev.TypeId equals typ.TypeId
                               join man in db.Manufacturers on dev.ManufacturerId equals man.ManufacturerId
                               join dep in db.Departaments on dev.DepartamentId equals dep.DepartamentId
                               where dev.status == "Broke"
                               select new List<string>()
                          {
                              dev.PDID.ToString(),
                              typ.TypeName,
                              man.ManufacturerName,
                              dev.name,
                              dev.status,
                              dep.DepartamentName,
                              dev.ComputerId.ToString()
                          };
                    s = pdr3.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        PDID = s[i][0];
                        type = s[i][1];
                        manufac = s[i][2];
                        name = s[i][3];
                        status = s[i][4];
                        DepartamentName = s[i][5];
                        ComputerId = s[i][6];
                        wr.DoWriteP(PDID, ComputerId, name, type, manufac, status);
                    }
                    break;
                    case 6:
                    wr.ReadyToAC();
                    var resn1 = from cmp in db.Computers
                               join mat in db.Motherboards on cmp.MotherboardId equals mat.MotherboardId
                               join cpu in db.CPUs on cmp.CpuId equals cpu.CpuId
                               join hdd in db.HDDs on cmp.HddId equals hdd.HddId
                               where cmp.status == "On work"
                               select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           mat.MotherboardName,
                           cpu.CpuName,
                           hdd.HddName
                       };
                    s = resn1.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        ComputerId = s[i][0];
                        status = s[i][1];
                        DepartamentName = s[i][2];
                        fullName = s[i][3];
                        manufac = s[i][4];
                        wr.DoWriteAC(ComputerId, status, DepartamentName, fullName, manufac);
                    }
                    break;
                    case 7:
                    wr.ReadyToAC();
                    var resn2 = from cmp in db.Computers
                                join mat in db.Motherboards on cmp.MotherboardId equals mat.MotherboardId
                                join cpu in db.CPUs on cmp.CpuId equals cpu.CpuId
                                join hdd in db.HDDs on cmp.HddId equals hdd.HddId
                                where cmp.status == "On repair"
                                select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           mat.MotherboardName,
                           cpu.CpuName,
                           hdd.HddName
                       };
                    s = resn2.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        ComputerId = s[i][0];
                        status = s[i][1];
                        DepartamentName = s[i][2];
                        fullName = s[i][3];
                        manufac = s[i][4];
                        wr.DoWriteAC(ComputerId, status, DepartamentName, fullName, manufac);
                    }
                    break;
                    case 8:
                    wr.ReadyToAC();
                    var resn3 = from cmp in db.Computers
                                join mat in db.Motherboards on cmp.MotherboardId equals mat.MotherboardId
                                join cpu in db.CPUs on cmp.CpuId equals cpu.CpuId
                                join hdd in db.HDDs on cmp.HddId equals hdd.HddId
                                where cmp.status == "Broke"
                                select new List<string>()
                       {
                           cmp.ComputerId.ToString(),
                           cmp.status,
                           mat.MotherboardName,
                           cpu.CpuName,
                           hdd.HddName
                       };
                    s = resn3.ToList();
                    for (int i = 0; i != s.Count; i++)
                    {
                        ComputerId = s[i][0];
                        status = s[i][1];
                        DepartamentName = s[i][2];
                        fullName = s[i][3];
                        manufac = s[i][4];
                        wr.DoWriteAC(ComputerId, status, DepartamentName, fullName, manufac);
                    }
                    break;
            }
            wr.SaveFile(path);
        }

        private void pathBattum_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    path = dialog.SelectedPath;
            pathBox.Text = path;
        }
    }
}

