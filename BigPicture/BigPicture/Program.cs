using System;
using System.Windows.Forms;
using System.Drawing;

public class MainForm : Form
{
    public MainForm()
    {
        // �t�H�[�����őO�ʂɐݒ�
        this.TopMost = true;
        this.Text = "���s�f�U�C�����e�N�m���W�[���w�Z(ad)";
        this.WindowState = FormWindowState.Maximized;  // �ő剻
        this.FormBorderStyle = FormBorderStyle.None;   // �g���\����
                                                       // �t�H�[��������Ȃ��悤�ɂ���
        //this.FormClosing += (sender, e) => { e.Cancel = true;  }; // �t�H�[���̕��铮����L�����Z��

        // �ŏ����{�^���������ɂ���
        this.MinimizeBox = false;
        this.MaximizeBox = false;

        // �^�X�N�o�[�ɕ\������Ȃ��悤�ɂ���i�I�v�V�����j
        // this.ShowInTaskbar = false;

        // PictureBox ���쐬���ĉ摜��ݒ�
        PictureBox pictureBox = new PictureBox();
        using (var client = new System.Net.WebClient())
        {
            byte[] data = client.DownloadData("https://kyoto-tech.ac.jp/assets/images/school/facility/exterior.jpg");
            using (var ms = new System.IO.MemoryStream(data))
            {
                pictureBox.Image = Image.FromStream(ms);
            }
        }
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;  // �T�C�Y����
        pictureBox.Dock = DockStyle.Fill;  // �t�H�[���S�̂ɉ摜��\��
        this.Controls.Add(pictureBox);

        // PictureBox ���őO�ʂɕ\��
        pictureBox.BringToFront();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
