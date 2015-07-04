using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Paint
{
    //public class Paint
    public partial class FormChild : Form
    {
        public FormChild()
        {
            InitializeComponent();

            this.Location = Point.Empty;
            ReSize();
        }

        //重绘
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (Tools.m_TempImageUsed)
            {
                Bitmap tempBitmap = new Bitmap(m_SourceImage, m_SourceImage.Size);
                Graphics.FromImage(tempBitmap).DrawImage(m_TempImage, 0, 0);
                e.Graphics.DrawImage(tempBitmap, 0, 0);
                return;
            }
            e.Graphics.DrawImage(m_SourceImage, 0, 0);
        }

        //计时器
        private void OnTimer(object sender, EventArgs e)
        {
            Tools.喷枪(MouseStateType.MouseMove, Point.Empty, m_FormMain.GetForegroundColor(), ref m_SourceImage);
            this.Draw();
        }

        //画布改变尺寸
        public void OnSizeChanged(object sender, EventArgs e)
        {
            ReSize();
        }

        //绘图区边界拉伸
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTRIGHT = 11;
            const int HTBOTTOM = 15;
            const int HTBOTTOMRIGHT = 17;
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    Point pPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                    pPoint = PointToClient(pPoint);
                    if (pPoint.X >= ClientSize.Width - SCOPE)
                    {
                        if (pPoint.Y >= ClientSize.Height - SCOPE)
                        {
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                        else
                        {
                            m.Result = (IntPtr)HTRIGHT;
                        }
                    }
                    else if (pPoint.Y >= ClientSize.Height - SCOPE)
                    {
                        m.Result = (IntPtr)HTBOTTOM;
                    }
                    break;
            }
        }

        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            if (CheckMainForm())
            {
                string strInf = e.X + "," + e.Y;
                if (MouseStateType.MouseDown == Tools.m_MouseState)
                {
                    switch (m_FormMain.m_ActiveTool)
                    {
                        case ToolsType.裁剪:
                            break;
                        case ToolsType.橡皮:
                            m_FormMain.StatusLabel_Middle.Text = strInf;
                            Tools.橡皮(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetBackgroundColor(), ref m_SourceImage);
                            this.Draw();
                            break;
                        case ToolsType.取色:
                            Color newColor = Tools.取色(MouseStateType.MouseMove, new Point(e.X, e.Y), ref m_SourceImage);
                            m_FormMain.panelToolSelection.BackColor = newColor;
                            break;
                        case ToolsType.铅笔:
                            m_FormMain.StatusLabel_Middle.Text = strInf;
                            Tools.铅笔(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage);
                            this.Draw();
                            break;
                        case ToolsType.喷枪:
                            m_FormMain.StatusLabel_Middle.Text = strInf;
                            Tools.m_PreviousPoint = new Point(e.X, e.Y);
                            break;
                        case ToolsType.直线:
                            m_FormMain.StatusLabel_Right.Text = strInf;
                            Tools.直线(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.矩形:
                            m_FormMain.StatusLabel_Right.Text = (e.X - Tools.m_PreviousPoint.X) + "," + (e.Y - Tools.m_PreviousPoint.Y);
                            Tools.矩形(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.椭圆:
                            m_FormMain.StatusLabel_Right.Text = (e.X - Tools.m_PreviousPoint.X) + "," + (e.Y - Tools.m_PreviousPoint.Y);
                            Tools.椭圆(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.选定:
                            break;
                        case ToolsType.填充:
                            break;
                        case ToolsType.放大镜:
                            break;
                        case ToolsType.刷子:
                            m_FormMain.StatusLabel_Middle.Text = strInf;
                            Tools.刷子(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.文字:
                            m_FormMain.StatusLabel_Right.Text = (e.X - Tools.m_PreviousPoint.X) + "," + (e.Y - Tools.m_PreviousPoint.Y);
                            Tools.文字(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref alphaTextBox);
                            break;
                        case ToolsType.曲线:
                            m_FormMain.StatusLabel_Right.Text = strInf;
                            Tools.曲线(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.多边形:
                            m_FormMain.StatusLabel_Right.Text = strInf;
                            Tools.多边形(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        case ToolsType.圆角矩形:
                            m_FormMain.StatusLabel_Right.Text = (e.X - Tools.m_PreviousPoint.X) + "," + (e.Y - Tools.m_PreviousPoint.Y);
                            Tools.圆角矩形(MouseStateType.MouseMove, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                            this.Draw();
                            break;
                        default:
                            return;
                    }
                    if (!m_bChanged)
                    {
                        m_bChanged = true;
                    }
                }
                else
                {
                    m_FormMain.StatusLabel_Middle.Text = strInf;
                }
            }
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CheckMainForm())
            {
                switch (m_FormMain.m_ActiveTool)
                {
                    case ToolsType.裁剪:
                        break;
                    case ToolsType.橡皮:
                        Tools.橡皮(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetBackgroundColor(), ref m_SourceImage);
                        this.Draw();
                        break;
                    case ToolsType.取色:
                        Color newColor = Tools.取色(MouseStateType.MouseDown, new Point(e.X, e.Y), ref m_SourceImage);
                        m_FormMain.panelToolSelection.BackColor = newColor;
                        break;
                    case ToolsType.铅笔:
                        Tools.铅笔(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage);
                        this.Draw();
                        break;
                    case ToolsType.喷枪:
                        Tools.喷枪(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage);
                        sprayGunTimer.Interval = 5;
                        sprayGunTimer.Enabled = true;
                        break;
                    case ToolsType.直线:
                        Tools.直线(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.矩形:
                        Tools.矩形(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.椭圆:
                        Tools.椭圆(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.选定:
                        break;
                    case ToolsType.填充:
                        break;
                    case ToolsType.放大镜:
                        break;
                    case ToolsType.刷子:
                        Tools.刷子(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.文字:
                        Tools.文字(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref alphaTextBox);
                        m_FormMain.文字工具栏ToolStripMenuItem.Enabled = alphaTextBox.Visible;
                        break;
                    case ToolsType.曲线:
                        Tools.曲线(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.多边形:
                        Tools.多边形(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.圆角矩形:
                        Tools.圆角矩形(MouseStateType.MouseDown, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    default:
                        return;
                }
            }
        }

        private void Mouse_Up(object sender, MouseEventArgs e)
        {
            if (MouseStateType.MouseDown == Tools.m_MouseState && e.Button == MouseButtons.Left && CheckMainForm())
            {
                switch (m_FormMain.m_ActiveTool)
                {
                    case ToolsType.裁剪:
                        break;
                    case ToolsType.橡皮:
                        Tools.橡皮(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetBackgroundColor(), ref m_SourceImage);
                        break;
                    case ToolsType.取色:
                        Color newColor = Tools.取色(MouseStateType.MouseUp, new Point(e.X, e.Y), ref m_SourceImage);
                        m_FormMain.SetColor(ColorTargetsType.前景, newColor.R, newColor.G, newColor.B);
                        //m_FormMain.SetActiveTool(ToolsType.铅笔);
                        m_FormMain.panelToolSelection.BackColor = Color.FromArgb(255, 240, 240, 240);
                        break;
                    case ToolsType.铅笔:
                        Tools.铅笔(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage);
                        break;
                    case ToolsType.喷枪:
                        sprayGunTimer.Enabled = false;
                        break;
                    case ToolsType.直线:
                        Tools.直线(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.矩形:
                        Tools.矩形(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.椭圆:
                        Tools.椭圆(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.选定:
                        break;
                    case ToolsType.填充:
                        break;
                    case ToolsType.放大镜:
                        break;
                    case ToolsType.刷子:
                        Tools.刷子(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.文字:
                        Tools.文字(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref alphaTextBox);
                        break;
                    case ToolsType.曲线:
                        Tools.曲线(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    case ToolsType.多边形:
                        Tools.多边形(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        break;
                    case ToolsType.圆角矩形:
                        Tools.圆角矩形(MouseStateType.MouseUp, new Point(e.X, e.Y), m_FormMain.GetForegroundColor(), ref m_SourceImage, ref m_TempImage);
                        this.Draw();
                        break;
                    default:
                        return;
                }
                m_FormMain.StatusLabel_Right.Text = "";
            }
        }

        private void Mouse_Leave(object sender, EventArgs e)
        {
            if (CheckMainForm())
            {
                m_FormMain.StatusLabel_Middle.Text = "";
                m_FormMain.StatusLabel_Right.Text = "";
            }
        }

        // 拖动编辑框:利用Windows的API函数：SendMessage 和 ReleaseCapture
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, uint wParam, uint lParam);
        [DllImport("user32.dll")]
        private static extern int ReleaseCapture();

        private void TextBox_Mouse_Down(object sender, MouseEventArgs e)
        {
            if (SCOPE > e.X || SCOPE > e.Y || alphaTextBox.Size.Width - SCOPE < e.X || alphaTextBox.Size.Height - SCOPE < e.Y)
            {
                ReleaseCapture();
                SendMessage((sender as Control).Handle, 0x0112, 0xF010 + 0x0002, 0);
                //                              WM_SYSCOMMAND   SC_MOVE     HTCAPTION
            }
        }

        //自动换行
        private void TextBox_Pre_Key_Down(object sender, PreviewKeyDownEventArgs e)
        {
            /*SizeF newSizeF = Graphics.FromImage(m_TempImage).MeasureString(alphaTextBox.Text + (char)e.KeyValue, alphaTextBox.Font);
            if (alphaTextBox.Height < newSizeF.Height)
            {
                alphaTextBox.Height = (int)newSizeF.Height;
            }
            
            if ( '\n' == (int)e.KeyCode)
            {
                alphaTextBox.Text += Environment.NewLine;
            }*/
            if (0 != alphaTextBox.Lines.Length)
            {
                SizeF preSizeF = Graphics.FromImage(m_TempImage).MeasureString(alphaTextBox.Lines[alphaTextBox.Lines.Length - 1], alphaTextBox.Font);
                if (preSizeF.Width >= (float)alphaTextBox.Width - 5)
                {
                    alphaTextBox.Text += Environment.NewLine;
                    alphaTextBox.SelectionStart = alphaTextBox.TextLength;
                }
            }
        }

        private void TextBox_Key_Down(object sender, KeyEventArgs e)
        {
            /*SizeF newSizeF = Graphics.FromImage(m_TempImage).MeasureString(alphaTextBox.Text + (char)e.KeyValue, alphaTextBox.Font);
            if()
            {
            
            }

            if (alphaTextBox.Height < newSizeF.Height)
            {
                alphaTextBox.Height = (int)newSizeF.Height;
            }
            */
            SizeF newSizeF = Graphics.FromImage(m_TempImage).MeasureString(alphaTextBox.Text, alphaTextBox.Font);
            if (alphaTextBox.Lines.Length * (alphaTextBox.Font.Height + 1) > (float)alphaTextBox.Height)
            {
                alphaTextBox.Height = alphaTextBox.Lines.Length * alphaTextBox.Font.Height;
            }
            if (alphaTextBox.Height < newSizeF.Height)
            {
                alphaTextBox.Height = (int)newSizeF.Height;
            }
        }
    }
}