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
        private bool isNguoiChoiA, isKetThuc;
        private Stack<Button> stack;
        private Timer timer;

        public GameCaro()
        {
            InitializeComponent();
            initMyComponent();
        }

        private void initMyComponent()
        {
            stack = new Stack<Button>();
            isNguoiChoiA = true;
            isKetThuc = false;
            soO = 10;
            banCo = new Button[soO, soO];

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
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

            SetThoiGianChoi();
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
                showNguoiChoiThang(false);
                while (stack.Count != 0)
                    stack.Pop().BackColor = Color.LightGray;

                //Set tat ca button ko click duoc
                setButtonNonClick();
                timer.Stop();
                return;
            }

            //khong thang
            isNguoiChoiA = !isNguoiChoiA;
            setNguoiChoi();
        }

        private void showNguoiChoiThang(bool hetGio)
        {
            if (hetGio)
            {
                if (isNguoiChoiA)
                    MessageBox.Show("Nguoi Choi B Thang!", "thong Bao");
                else
                    MessageBox.Show("Nguoi Choi A Thang!", "Thong Bao");
            }
            else
            {
                if (isNguoiChoiA)
                    MessageBox.Show("Nguoi Choi A Thang!", "thong Bao");
                else
                    MessageBox.Show("Nguoi Choi B Thang!", "Thong Bao");
            }
        }

        private void setButtonNonClick()
        {
            foreach (Button btn in banCo)
            {
                btn.Cursor = Cursors.No;
                btn.Click += new System.EventHandler(btnNoClick);
            }
        }

        private void btnNoClick(object sender, EventArgs ar)
        {
            MessageBox.Show("Hanh Dong Khong Chap Nhan", "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Point layToaDo(Button btn)
        {
            string[] str = btn.Tag.ToString().Split(',');
            return new Point(int.Parse(str[0]), int.Parse(str[1]));
        }

        private bool laThangGame(string[] ids)
        {
            int x = int.Parse(ids[0]), y = int.Parse(ids[1]);

            isKetThuc = laThangTheoChieuDoc(x, y) || laThangTheoChieuNgang(x, y) || laThangTheoCheoChinh(x, y) || laThangTheoCheoPhu(x, y);
            return isKetThuc;
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

            if (dem >= 5)
            {
                Button[] arr = stack.ToArray();
                Button btnCuoi = arr[0], btnDau = arr[arr.Length - 1];

                int count = 0;//Dem xem co bi chan 2 dau khong
                Point toaDo = layToaDo(btnDau);
                Button btnCheck;
                if (toaDo.X >= 1)
                {
                    btnCheck = banCo[toaDo.X - 1, toaDo.Y];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                toaDo = layToaDo(btnCuoi);
                if (toaDo.X < soO - 1)
                {
                    btnCheck = banCo[toaDo.X + 1, toaDo.Y];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                return count < 2;
            }

            return false;
        }

        //kiem tra thang theo chieu Ngang
        private bool laThangTheoChieuNgang(int x, int y)
        {
            stack.Clear();
            //find index;
            string nguoiChoiHienTai = banCo[x, y].Text;
            while (y >= 1 && banCo[x , y-1].Text.Equals(nguoiChoiHienTai))
                y -= 1;

            stack.Push(banCo[x, y]);

            //dem
            int dem = 1;
            while (y < soO - 1 && banCo[x , y+1].Text.Equals(nguoiChoiHienTai))
            {
                dem += 1;
                y += 1;
                stack.Push(banCo[x, y]);
            }

            if (dem >= 5)
            {
                Button[] arr = stack.ToArray();
                Button btnCuoi = arr[0], btnDau = arr[arr.Length - 1];

                int count = 0;//Dem xem co bi chan 2 dau khong
                Point toaDo = layToaDo(btnDau);
                Button btnCheck;
                if (toaDo.Y >= 1)
                {
                    btnCheck = banCo[toaDo.X, toaDo.Y - 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                toaDo = layToaDo(btnCuoi);
                if (toaDo.Y < soO - 1)
                {
                    btnCheck = banCo[toaDo.X, toaDo.Y + 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                return count < 2;
            }

            return false;
        }

        //kiem tra thang game theo cheo chinh
        private bool laThangTheoCheoChinh(int x, int y)
        {
            stack.Clear();
            //start index;
            string nguoiChoiHienTai = banCo[x, y].Text;
            while (x >= 1 && y >= 1 && banCo[x - 1, y - 1].Text.Equals(nguoiChoiHienTai))
            {
                x -= 1;
                y -= 1;
            }

            stack.Push(banCo[x, y]);
            //dem
            int dem = 1;
            while (x < soO - 1 && y < soO - 1 && banCo[x + 1, y + 1].Text.Equals(nguoiChoiHienTai))
            {
                dem += 1;
                x += 1;
                y += 1;
                stack.Push(banCo[x, y]);
            }
            if (dem >= 5)
            {
                Button[] arr = stack.ToArray();
                Button btnCuoi = arr[0], btnDau = arr[arr.Length - 1];

                int count = 0;//Dem xem co bi chan 2 dau khong
                Point toaDo = layToaDo(btnDau);
                Button btnCheck;
                if (toaDo.X >= 1 && toaDo.Y >= 1)
                {
                    btnCheck = banCo[toaDo.X - 1, toaDo.Y - 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                toaDo = layToaDo(btnCuoi);
                if (toaDo.X < soO - 1 && toaDo.Y < soO - 1)
                {
                    btnCheck = banCo[toaDo.X + 1, toaDo.Y + 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                return count < 2;
            }

            return false;
        }

        //kiem tra thang game theo cheo phu
        private bool laThangTheoCheoPhu(int x, int y)
        {
            stack.Clear();
            //start index;
            string nguoiChoiHienTai = banCo[x, y].Text;
            while (x >= 1 && y < soO -1 && banCo[x - 1, y + 1].Text.Equals(nguoiChoiHienTai))
            {
                x -= 1;
                y += 1;
            }

            stack.Push(banCo[x, y]);
            //dem
            int dem = 1;
            while (x < soO - 1 && y >= 1 && banCo[x + 1, y - 1].Text.Equals(nguoiChoiHienTai))
            {
                dem += 1;
                x += 1;
                y -= 1;
                stack.Push(banCo[x, y]);
            }
            if (dem >= 5)
            {
                Button[] arr = stack.ToArray();
                Button btnCuoi = arr[0], btnDau = arr[arr.Length - 1];

                int count = 0;//Dem xem co bi chan 2 dau khong
                Point toaDo = layToaDo(btnDau);
                Button btnCheck;
                if (toaDo.X >= 1 && toaDo.Y < soO - 1)
                {
                    btnCheck = banCo[toaDo.X - 1, toaDo.Y + 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                toaDo = layToaDo(btnCuoi);
                if (toaDo.X < soO - 1 && toaDo.Y >= 1)
                {
                    btnCheck = banCo[toaDo.X + 1, toaDo.Y - 1];
                    if (!btnCheck.Text.Equals(nguoiChoiHienTai) && !string.IsNullOrEmpty(btnCheck.Text))
                        count++;
                }

                return count < 2;
            }

            return false;
        }

        private void btnBatDau_Click(object sender, EventArgs e)
        {
            taiGiaoDien();
            setNguoiChoi();
            btnBatDau.Enabled = false;
            btnChoiLai.Enabled = true;
            timer.Start();
            SetThoiGianChoi();
        }

        private void btnChoiLai_Click(object sender, EventArgs e)
        {
            btnChoiLai.Enabled = false;
            btnBatDau.Enabled = true;
            timer.Stop();
            progress.Value = 0;
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

        private void SetThoiGianChoi()
        {
            progress.Value = 30;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (progress.Value == 0)
            {
                timer.Stop();
                setButtonNonClick();
                showNguoiChoiThang(true);
            }
            else
                progress.Value = progress.Value - progress.Step;
        }
    }
}