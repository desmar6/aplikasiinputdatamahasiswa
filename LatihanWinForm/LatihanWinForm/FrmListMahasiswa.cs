using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatihanWinForm
{
    public partial class FrmListMahasiswa : Form
    {
        private bool isNewData = true;
        private Mahasiswa mhs = null;
        private IList<Mahasiswa> listOfMahasiswa = new List<Mahasiswa>();

        public FrmListMahasiswa()
        {
            InitializeComponent();
            InisialisasiListView();
        }

        private void InisialisasiListView()
        {
            lvmMahasiswa.View = System.Windows.Forms.View.Details;
            lvmMahasiswa.FullRowSelect = true;
            lvmMahasiswa.GridLines = true;

            lvmMahasiswa.Columns.Add("No.", 30, HorizontalAlignment.Center);
            lvmMahasiswa.Columns.Add("Npm", 70, HorizontalAlignment.Left);
            lvmMahasiswa.Columns.Add("Nama", 180, HorizontalAlignment.Left);
            lvmMahasiswa.Columns.Add("Jenis Kelamin", 80, HorizontalAlignment.Left);
            lvmMahasiswa.Columns.Add("Tempat Lahir", 75, HorizontalAlignment.Left);
            lvmMahasiswa.Columns.Add("Tgl Lahir", 75, HorizontalAlignment.Left);
        }

        private void FillToListView(bool isNewData, Mahasiswa mhs)
        {
            if (isNewData)
            {
                int noUrut = lvmMahasiswa.Items.Count + 1;

                ListViewItem item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(mhs.nim);
                item.SubItems.Add(mhs.nama);
                item.SubItems.Add(mhs.jnsKelamin);
                item.SubItems.Add(mhs.tmptlahir);
                item.SubItems.Add(mhs.tglLahir);

                lvmMahasiswa.Items.Add(item);
            }
            else
            {
                int row = lvmMahasiswa.SelectedIndices[0];

                ListViewItem itemRow = lvmMahasiswa.Items[row];
                itemRow.SubItems[1].Text = mhs.nim;
                itemRow.SubItems[2].Text = mhs.nama;
                itemRow.SubItems[3].Text = mhs.jnsKelamin;
                itemRow.SubItems[4].Text = mhs.tmptlahir;
                itemRow.SubItems[5].Text = mhs.tglLahir;
            }
        }

        public void mskinput (Mahasiswa obj)
        {
            this.isNewData = false;
            this.mhs = obj;

            mskNpm.Text = this.mhs.nim;
            txtNama.Text = this.mhs.nama;

            if(this.mhs.jnsKelamin == "Laki-laki")
            {
                rdoLakilaki.Checked = true;
            }else if(this.mhs.jnsKelamin == "Perempuan")
            {
                rdoPerempuan.Checked = true;
            }

            txtTempatLahir.Text = this.mhs.tmptlahir;
            dtpTanggalLahir.Value = DateTime.Parse(this.mhs.tglLahir);
        }

        private void ResetForm()
        {
            mskNpm.Clear();
            txtNama.Clear();
            rdoLakilaki.Checked = true;
            txtTempatLahir.Clear();
            dtpTanggalLahir.Value = DateTime.Today;

            mskNpm.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (!mskNpm.MaskFull)
            {
                MessageBox.Show("NPM harus diisi!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskNpm.Focus();
                return;
            }

            if (!(txtNama.Text.Length > 0))
            {
                MessageBox.Show("Nama harus diisi!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNama.Focus();
                return;
            }

            var jenisKelamin = rdoLakilaki.Checked ? "Laki-laki" : "Perempuan";

            var result = MessageBox.Show("Apakah data ingin disimpan?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes && isNewData)
            {
                mhs = new Mahasiswa();

                mhs.nim = mskNpm.Text;
                mhs.nama = txtNama.Text;
                mhs.jnsKelamin = rdoLakilaki.Checked ? "Laki-laki" : "Perempuan";
                mhs.tmptlahir = txtTempatLahir.Text;
                mhs.tglLahir = dtpTanggalLahir.Value.ToString("dd/MM/yyyy");

                FrmListMahasiswa_OnSave(mhs);
                ResetForm();
                           
            }
        }

        private void FrmListMahasiswa_OnSave(Mahasiswa obj)
        {
            listOfMahasiswa.Add(obj);
            FillToListView(true, obj);
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if(lvmMahasiswa.SelectedItems.Count > 0)
            {
                var index = listOfMahasiswa[lvmMahasiswa.SelectedIndices[0]];

                var msg = string.Format("Apakah data mahasiswa '{0}' ingin dihapus ?", index.nama);

                var result = MessageBox.Show(msg, "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    listOfMahasiswa.Remove(index);

                    lvmMahasiswa.Items.Clear();
                    foreach (var obj in listOfMahasiswa)
                    {
                        FillToListView(true, obj);
                    }
                }
            }
            else
            {
                MessageBox.Show("Data belum dipilih", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            var msg = "Apakah anda yakin?";

            var result = MessageBox.Show(msg, "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void lvmMahasiswa_DoubleClick(object sender, EventArgs e)
        {
            if (lvmMahasiswa.SelectedItems.Count > 0)
            {
                var index = listOfMahasiswa[lvmMahasiswa.SelectedIndices[0]];

                mskinput(index);
            }
            else
            {
                MessageBox.Show("Data belum anda pilih", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lvmMahasiswa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}