using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisableInsertKey
{
    public partial class Form1 : Form
    {
        NotifyIcon notifyIcon;

        public bool isDisabledInsertKey = true;

//        DisableInsertKey

        public Form1()
        {
            InitializeComponent();

            // タスクバーに表示しない。
            //this.ShowInTaskbar = false;

            this.setComponents();

        }

        private void setComponents()
        {
            notifyIcon = new NotifyIcon();
            // アイコンの設定
            notifyIcon.Icon = Properties.Resources.red_favicon;
            // アイコンを表示する
            notifyIcon.Visible = true;
            //アイコンにマウスポインタを合わせたときのテキスト
            notifyIcon.Text = "Insert key is Disabled";
            // コンテキストメニュー
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem tsmi1 = new ToolStripMenuItem();
            ToolStripMenuItem tsmi2 = new ToolStripMenuItem();

            tsmi1.Text = " & Version";
            tsmi1.Click += ToolStripMenuItem_Version_Click;
            contextMenuStrip.Items.Add(tsmi1);

            tsmi2.Text = " & Exit";
            tsmi2.Click += ToolStripMenuItem_End_Click;
            contextMenuStrip.Items.Add(tsmi2);

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            // NotifyIconのクリックイベント
            notifyIcon.Click += NotifyIcon_Click;
        }

        private void ToolStripMenuItem_Version_Click(object sender, EventArgs e)
        {
            // バルーンヒントを表示する
            notifyIcon.BalloonTipTitle = "DisableInsertKey.exe 1.0";
            notifyIcon.BalloonTipText = "Disable the Insert key when running. Without changing the registry.    twitter.com/DisableInsert";
            notifyIcon.ShowBalloonTip(10000);
        }

        private void ToolStripMenuItem_End_Click(object sender, EventArgs e)
        {
            //通知アイコンを消去
            notifyIcon.Visible = false;

            // アプリケーションの終了
            Application.Exit();
        }


        private void NotifyIcon_Click(object sender, EventArgs e)
        {

            var eventArgs = e as MouseEventArgs;
            switch (eventArgs.Button)
            {
                // 左クリック時のみ処理
                case MouseButtons.Left:

                    // Formの表示/非表示を反転
                    isDisabledInsertKey = !isDisabledInsertKey;

                    DataProperty p = new DataProperty();
                    p.IsDisabled = isDisabledInsertKey;
                    // Console.WriteLine(p.IsDisabled);

                    if (isDisabledInsertKey)
                    {
                        notifyIcon.Icon = Properties.Resources.red_favicon;
                        notifyIcon.Text = "Insert key is Disabled";
                    }
                    else
                    {
                        notifyIcon.Icon = Properties.Resources.blue_favicon;
                        notifyIcon.Text = "Insert key is Enabled";
                    }

                    break;
            }
        }
    }
}
