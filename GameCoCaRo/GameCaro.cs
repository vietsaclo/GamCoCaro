using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCoCaRo
{
    public partial class GameCaro : Form
    {
        //Variables
        private Button[,] banCo;
        private int soO;
        private bool isNguoiChoiA;
        private Stack<Button> stack;

        public GameCaro()
        {
            InitializeComponent();
            initMyComponent();
        }

        private void initMyComponent()
        {
            stack = new Stack<Button>();
            isNguoiChoiA = true;
            soO = 10;
            banCo = new Button[soO, soO];
        }

        private void taiGiaoDien()
        {
            pnBanCo.Controls.Clear();
            Button btn;
            for (int i = 0; i < soO; i++)
            {
                for (int j = 0; j < soO; j++)
                {
                    btn = createButton(i, j);
                    banCo[i, j] = btn;
                    pnBanCo.Controls.Add(btn);
                }
            }
        }

        private Button createButton(int i, int j)
        {
            Button btn = new Button();
            btn.Tag = i + "," + j;
            btn.Width = btn.Height = 50;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font(btn.Font.FontFamily, 20f);
            btn.Location = new Point(j * (btn.Width + 3), i * (btn.Height + 3));
            btn.Click += btn_Click;
            return btn;
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //Kiem tra co cho click ko
            if (btn.Cursor == Cursors.No)
                return;

            if (isNguoiChoiA)
            {
                btn.Text = "X";
                btn.ForeColor = Color.Red;
            }
            else
            {
                btn.Text = "O";
                btn.ForeColor = Color.Blue;
            }
            btn.Cursor = Cursors.No;
            btn.Click+= new EventHandler(btnNoClick);

            //kiem tra co thang ko
            if (laThangGame(btn.Tag.ToString().Split(',')))
            {
                MessageBox.Show("thang");
                while (stack.Count != 0)
                    stack.Pop().BackColor = Color.Aqua;
                return;
            }

            //khong thang
            isNguoiChoiA = !isNguoiChoiA;
            setNguoiChoi();
        }

        private void btnNoClick(object sender, EventArgs ar)
        {

        }

        private bool laThangGame(string[] ids)
        {
            int x = int.Parse(ids[0]), y = int.Parse(ids[1]);

            return laThangTheoChieuDoc(x, y);
        }

        //kiem tra thang theo chieu doc
        private bool laThangTheoChieuDoc(int x, int y)
        {
            stack.Clear();
            //find index;
            string nguoiChoiHienTai = banCo[x, y].Text;
            while (x >= 1 && banCo[x - 1, y].Text.Equals(nguoiChoiHienTai))
                x -= 1;

            stack.Push(banCo[x, y]);

            //dem
            int dem = 1;
            while (x < soO - 1 && banCo[x + 1, y].Text.Equals(nguoiChoiHienTai))
            {
                dem += 1;
                x += 1;
                stack.Push(banCo[x, y]);
            }

            if (dem == 5)
                return true;

            return false;
        }

        private void btnBatDau_Click(object sender, EventArgs e)
        {
            taiGiaoDien();
            setNguoiChoi();
            btnBatDau.Enabled = false;
            btnChoiLai.Enabled = true;
        }

        private void btnChoiLai_Click(object sender, EventArgs e)
        {
            btnChoiLai.Enabled = false;
            btnBatDau.Enabled = true;
        }

        private void setNguoiChoiA()
        {
            tbNguoiChoiA.BackColor = Color.Brown;
            tbNguoiChoiA.Font = new Font(tbNguoiChoiA.Font, FontStyle.Bold);
            tbNguoiChoiA.ForeColor = Color.White;

            tbNguoiChoiB.BackColor = Color.White;
            tbNguoiChoiB.Font = new Font(tbNguoiChoiA.Font, FontStyle.Regular);
            tbNguoiChoiB.ForeColor = Color.Black;
        }

        private void setNguoiChoiB()
        {
            tbNguoiChoiB.BackColor = Color.DarkBlue;
            tbNguoiChoiB.Font = new Font(tbNguoiChoiA.Font, FontStyle.Bold);
            tbNguoiChoiB.ForeColor = Color.White;

            tbNguoiChoiA.BackColor = Color.White;
            tbNguoiChoiA.Font = new Font(tbNguoiChoiA.Font, FontStyle.Regular);
            tbNguoiChoiA.ForeColor = Color.Black;
        }

        private void setNguoiChoi()
        {
            if (isNguoiChoiA)
                setNguoiChoiA();
            else
                setNguoiChoiB();
        }
    }
}