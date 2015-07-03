using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint
{
    partial class FormChild
    {
        //检测父窗体状态
        private bool CheckMainForm()
        {
            if (null == m_FormMain)
            {
                if (null != this.MdiParent)
                {
                    m_FormMain = (FormMain)this.MdiParent;
                    return true;
                }
                return false;
            }
            return true;
        }

        //清除主位图
        public void Clear()
        {
            m_bChanged = false;
            Graphics.FromImage(m_SourceImage).FillRectangle(Brushes.White, new Rectangle(0, 0, m_SourceImage.Width, m_SourceImage.Height));
            Draw();
        }

        //绘制主位图
        public void Draw(bool bIsFull = false)
        {
            if (bIsFull)
            {
                this.CreateGraphics().FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, this.Width, this.Height));
            }
            if (Tools.m_TempImageUsed)
            {
                Bitmap tempBitmap = new Bitmap(m_SourceImage, m_SourceImage.Size);
                Graphics.FromImage(tempBitmap).DrawImage(m_TempImage, 0, 0);
                this.CreateGraphics().DrawImage(tempBitmap, 0, 0);
                return;
            }
            this.CreateGraphics().DrawImage(m_SourceImage, 0, 0);
        }

        //重绘背景[根据图片尺寸改变对话框尺寸]
        public void ReDraw()
        {
            if (null == m_SourceImage)
            {
                m_SourceImage = new Bitmap(this.Width, this.Height);
                Graphics.FromImage(m_SourceImage).FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, m_SourceImage.Width, m_SourceImage.Height));
                m_TempImage = new Bitmap(m_SourceImage.Width, m_SourceImage.Height);
            }
            else
            {
                this.Size = m_SourceImage.Size;
            }
            Draw(true);
        }

        //重绘背景[根据对话框尺寸改变图片尺寸]
        public void ReSize()
        {
            if (null == m_SourceImage)
            {
                m_SourceImage = new Bitmap(this.Width, this.Height);
                Graphics.FromImage(m_SourceImage).FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, m_SourceImage.Width, m_SourceImage.Height));
                m_TempImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
            else if (m_SourceImage.Size != this.Size)
            {
                Bitmap oldImage = m_SourceImage;
                m_SourceImage = new Bitmap(this.Width, this.Height);
                Graphics.FromImage(m_SourceImage).FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, m_SourceImage.Width, m_SourceImage.Height));
                Graphics.FromImage(m_SourceImage).DrawImage(oldImage, Point.Empty);
                m_TempImage = new Bitmap(m_SourceImage.Width, m_SourceImage.Height);
            }
            Draw(true);
        }

        //更换工具
        public void ChangeTool()
        {
            if (alphaTextBox.Visible)
            {
                Brush newBrush = new SolidBrush(Color.FromArgb(255, alphaTextBox.ForeColor));
                for (int i = 0; i < alphaTextBox.Lines.Length; ++i)
                {
                    Point newLocation = new Point(alphaTextBox.Location.X - 3, alphaTextBox.Location.Y + i * (alphaTextBox.Font.Height - 5) + 2);
                    Graphics.FromImage(m_SourceImage).DrawString(alphaTextBox.Lines[i], alphaTextBox.Font, newBrush, newLocation);
                }
                alphaTextBox.Text = "";
                alphaTextBox.Visible = false;
            }
            if (0 < m_PointList.Count)
            {
                Pen newPen = new Pen(m_FormMain.GetForegroundColor(), 1);
                Point[] newPoints = m_PointList.ToArray();
                Graphics.FromImage(m_SourceImage).DrawLines(newPen, newPoints);
                Graphics.FromImage(m_SourceImage).DrawLine(newPen, m_PointList[0], m_PointList[m_PointList.Count - 1]);
                Graphics newGraphics = this.CreateGraphics();
                newGraphics.DrawLine(newPen, m_PointList[0], m_PointList[m_PointList.Count - 1]);
                m_PointList.Clear();
            }
            if (sprayGunTimer.Enabled)
            {
                sprayGunTimer.Enabled = false;
            }
        }
    }
}
