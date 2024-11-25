using System;
using System.Windows.Forms;
using System.Drawing;

public class MainForm : Form
{
    public MainForm()
    {
        // フォームを最前面に設定
        this.TopMost = true;
        this.Text = "京都デザイン＆テクノロジー専門学校(ad)";
        this.WindowState = FormWindowState.Maximized;  // 最大化
        this.FormBorderStyle = FormBorderStyle.None;   // 枠を非表示に
                                                       // フォームを閉じられないようにする
        //this.FormClosing += (sender, e) => { e.Cancel = true;  }; // フォームの閉じる動作をキャンセル

        // 最小化ボタンも無効にする
        this.MinimizeBox = false;
        this.MaximizeBox = false;

        // タスクバーに表示されないようにする（オプション）
        // this.ShowInTaskbar = false;

        // PictureBox を作成して画像を設定
        PictureBox pictureBox = new PictureBox();
        using (var client = new System.Net.WebClient())
        {
            byte[] data = client.DownloadData("https://kyoto-tech.ac.jp/assets/images/school/facility/exterior.jpg");
            using (var ms = new System.IO.MemoryStream(data))
            {
                pictureBox.Image = Image.FromStream(ms);
            }
        }
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;  // サイズ調整
        pictureBox.Dock = DockStyle.Fill;  // フォーム全体に画像を表示
        this.Controls.Add(pictureBox);

        // PictureBox を最前面に表示
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
