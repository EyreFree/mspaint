using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace Paint
{
    partial class FormMain
    {
        //初始化FormChild
        public void InitChildForm()
        {
            Menu新建(true);             //新建画布
        }

        //初始化位图
        public bool InitBitmap(string path)
        {
            if (null == path || "" == path)
            {
                m_FormChild.m_FileFullName = "";
                m_FormChild.m_SourceImage = new Bitmap(m_FormChild.ClientRectangle.Width, m_FormChild.ClientRectangle.Height);
            }
            else
            {
                m_FormChild.m_FileFullName = path;
                try
                {
                    m_FormChild.m_SourceImage = new Bitmap(m_FormChild.m_FileFullName);
                }
                catch
                {
                    MessageBox.Show("无效的位图文件或不支持文件的格式。", "画图", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            if (CheckChildForm())
            {
                m_FormChild.ReDraw();
            }
            return true;
        }

        //检测子窗体状态
        private bool CheckChildForm()
        {
            return (null != m_FormChild);
        }

        //绘制颜色状态3D背景
        public void InitActiveColor()
        {
            Create3DBackGroundImage(foregroundColorButton, false); //生成前景色按钮
            Create3DBackGroundImage(backgroundColorButton, false); //生成背景色按钮
        }

        //绘制仿3D背景图片
        public void Create3DBackGroundImage(PictureBox targetPictureBox, bool bIsSag = true)
        {
            Bitmap new3DBitmap = new Bitmap(targetPictureBox.Size.Width, targetPictureBox.Size.Height);
            Graphics tempGraphics = Graphics.FromImage(new3DBitmap);
            tempGraphics.FillRectangle(new SolidBrush(targetPictureBox.BackColor), new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
            if (bIsSag) //凹陷
            {
                tempGraphics.DrawLine(Pens.Gray, new Point(0, 0), new Point(new3DBitmap.Width, 0));                                                 //Top1
                tempGraphics.DrawLine(Pens.Gray, new Point(0, 0), new Point(0, new3DBitmap.Height));                                                //Left1
                tempGraphics.DrawLine(Pens.Black, new Point(1, 1), new Point(new3DBitmap.Width - 1, 1));                                            //Top2
                tempGraphics.DrawLine(Pens.Black, new Point(1, 1), new Point(1, new3DBitmap.Height - 1));                                           //Left2
                tempGraphics.DrawLine(Pens.White, new Point(new3DBitmap.Width - 1, new3DBitmap.Height - 1), new Point(new3DBitmap.Width - 1, 1));   //Right1
                tempGraphics.DrawLine(Pens.White, new Point(new3DBitmap.Width - 1, new3DBitmap.Height - 1), new Point(1, new3DBitmap.Height - 1));  //Bottom1
            }
            else        //凸起
            {
                tempGraphics.DrawLine(Pens.White, new Point(0, 0), new Point(new3DBitmap.Width, 0));                                                //Top1
                tempGraphics.DrawLine(Pens.White, new Point(0, 0), new Point(0, new3DBitmap.Height));                                               //Left1
                tempGraphics.DrawLine(Pens.White, new Point(new3DBitmap.Width - 2, new3DBitmap.Height - 2), new Point(new3DBitmap.Width - 2, 1));   //Right2
                tempGraphics.DrawLine(Pens.White, new Point(new3DBitmap.Width - 2, new3DBitmap.Height - 2), new Point(1, new3DBitmap.Height - 2));  //Bottom2
                tempGraphics.DrawLine(Pens.Black, new Point(new3DBitmap.Width - 1, new3DBitmap.Height - 1), new Point(new3DBitmap.Width - 1, 1));   //Right1
                tempGraphics.DrawLine(Pens.Black, new Point(new3DBitmap.Width - 1, new3DBitmap.Height - 1), new Point(1, new3DBitmap.Height - 1));  //Bottom1
            }
            targetPictureBox.BackgroundImage = new3DBitmap;
        }

        // 将图片进行反色处理
        public Bitmap ReverseColor(Bitmap sourceBmp)
        {
            Bitmap newBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height);
            for (int x = 0; x < sourceBmp.Width; ++x)
            {
                for (int y = 0; y < sourceBmp.Height; ++y)
                {
                    Color pixel = sourceBmp.GetPixel(x, y);
                    newBmp.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
            return newBmp;
        }

        // 将图片指定颜色进行透明处理
        public Bitmap TransparentColor(Bitmap sourceBmp, Color toTtransparentColor, Color transparentColor)
        {
            Bitmap newBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height);
            for (int x = 0; x < sourceBmp.Width; ++x)
            {
                for (int y = 0; y < sourceBmp.Height; ++y)
                {
                    Color pixel = sourceBmp.GetPixel(x, y);
                    if (toTtransparentColor == pixel)
                    {
                        newBmp.SetPixel(x, y, transparentColor);
                    }
                    else
                    {
                        newBmp.SetPixel(x, y, pixel);
                    }
                }
            }
            return newBmp;
        }

        // 将图片除指定颜色以外进行透明处理
        public Bitmap GetTargetColor(Bitmap sourceBmp, Color targetColor)
        {
            Bitmap newBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height);
            for (int x = 0; x < sourceBmp.Width; ++ x)
            {
                for (int y = 0; y < sourceBmp.Height; ++ y)
                {
                    if (sourceBmp.GetPixel(x, y) == targetColor)
                    {
                        newBmp.SetPixel(x, y, targetColor);
                    }
                }
            }
            return newBmp;
        }

        //创建工具Size选项背景图
        public void CreateToolSelectionBackGroundImage(ToolsType theType, PictureBox targetPictureBox,int iCount)
        {
            Bitmap new3DBitmap = new Bitmap(targetPictureBox.Size.Width, targetPictureBox.Size.Height);
            Graphics tempGraphics = Graphics.FromImage(new3DBitmap);
            switch (theType)
            {
                case ToolsType.橡皮:
                    if (iCount == (int)Tools.m_ToolSize)
                    {
                        tempGraphics.FillRectangle(Brushes.White, new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
                        tempGraphics.DrawRectangle(new Pen(Color.FromArgb(51, 153, 255)), new Rectangle(0, 0, new3DBitmap.Width - 1, new3DBitmap.Height - 1));
                        tempGraphics.DrawRectangle(new Pen(Color.FromArgb(51, 153, 255)), new Rectangle(1, 1, new3DBitmap.Width - 3, new3DBitmap.Height - 3));
                    }
                    break;
                case ToolsType.喷枪:
                    new3DBitmap.Dispose();
                    if (iCount == (int)Tools.m_ToolSize)
                    {
                        switch (iCount)
                        {
                            case 0:
                                new3DBitmap = ReverseColor(new Bitmap("min.bmp"));
                                break;
                            case 1:
                                new3DBitmap = ReverseColor(new Bitmap("mid.bmp"));
                                break;
                            case 2:
                                new3DBitmap = ReverseColor(new Bitmap("max.bmp"));
                                break;
                        }
                    }
                    else
                    {
                        switch (iCount)
                        {
                            case 0:
                                new3DBitmap = new Bitmap("min.bmp");
                                break;
                            case 1:
                                new3DBitmap = new Bitmap("mid.bmp");
                                break;
                            case 2:
                                new3DBitmap = new Bitmap("max.bmp");
                                break;
                        }
                    }
                    break;
                case ToolsType.直线:
                case ToolsType.曲线:
                    if (iCount == (int)Tools.m_ToolSize)
                    {
                        tempGraphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 153, 255)), new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
                        tempGraphics.DrawLine(new Pen(Color.White, iCount + 1), new Point(2, targetPictureBox.Height / 2), new Point(targetPictureBox.Width - 2, targetPictureBox.Height / 2));
                    }
                    else
                    {
                        tempGraphics.FillRectangle(new SolidBrush(panelToolSelection.BackColor), new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
                        tempGraphics.DrawLine(new Pen(Color.Black, iCount + 1), new Point(2, targetPictureBox.Height / 2), new Point(targetPictureBox.Width - 2, targetPictureBox.Height / 2));
                    }
                    break;
                
                case ToolsType.放大镜:
                    break;
                case ToolsType.刷子:
                    break;
                case ToolsType.文字:
                case ToolsType.选定:
                case ToolsType.裁剪:
                    if (iCount != (int)Tools.m_ToolSize)
                    {
                        tempGraphics.FillRectangle(new SolidBrush(panelToolSelection.BackColor), 0, 0, new3DBitmap.Width, new3DBitmap.Height);
                        switch (iCount)
                        {
                            case 0:
                                tempGraphics.DrawImageUnscaled(TransparentColor(new Bitmap("Mode-a.bmp"), Color.FromArgb(128, 0, 0), panelToolSelection.BackColor),0,3);
                                break;
                            case 1:
                                tempGraphics.DrawImageUnscaled(TransparentColor(new Bitmap("Mode-b.bmp"), Color.FromArgb(128, 0, 0), panelToolSelection.BackColor),0,3);
                                break;
                        }
                    }
                    else
                    {
                        tempGraphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 153, 255)), 0, 0, new3DBitmap.Width, new3DBitmap.Height);
                        switch (iCount)
                        {
                            case 0:
                                tempGraphics.DrawImageUnscaled(TransparentColor(new Bitmap("Mode-a.bmp"), Color.FromArgb(128, 0, 0), Color.FromArgb(51, 153, 255)), 0, 3);
                                break;
                            case 1:
                                tempGraphics.DrawImageUnscaled(TransparentColor(new Bitmap("Mode-b.bmp"), Color.FromArgb(128, 0, 0), Color.FromArgb(51, 153, 255)), 0, 3);
                                break;
                        }
                    }
                    break;
                case ToolsType.矩形:
                case ToolsType.椭圆:
                case ToolsType.多边形:
                case ToolsType.圆角矩形:
                    const int iWageDistance = 3;
                    int RuleX = targetPictureBox.Width - iWageDistance * 2 - 1;
                    int RuleY = targetPictureBox.Height - iWageDistance * 2 - 1;
                    if (iCount == (int)Tools.m_ToolSize)
                    {
                        tempGraphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 153, 255)), new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
                        tempGraphics.DrawRectangle(Pens.White, iWageDistance, iWageDistance, RuleX, RuleY);
                    }
                    else
                    {
                        tempGraphics.FillRectangle(new SolidBrush(panelToolSelection.BackColor), new Rectangle(0, 0, new3DBitmap.Width, new3DBitmap.Height));
                        tempGraphics.DrawRectangle(Pens.Black, iWageDistance, iWageDistance, RuleX, RuleY);
                    }
                    switch (iCount)
                    {
                        case (int)ToolSize.Size_1:
                            break;
                        case (int)ToolSize.Size_2:
                            tempGraphics.FillRectangle (Brushes.Gray, new Rectangle(iWageDistance + 1, iWageDistance + 1, RuleX - 1, RuleY - 1));
                            break;
                        case (int)ToolSize.Size_3:
                            tempGraphics.FillRectangle(Brushes.Gray, new Rectangle(iWageDistance, iWageDistance, RuleX + 1, RuleY + 1));
                            break;
                    }
                    break;
            }
            targetPictureBox.BackgroundImage = new3DBitmap;
        }

        //获取当前前景色
        public Color GetForegroundColor()
        {
            return foregroundColorButton.BackColor;
        }

        //获取当前背景色
        public Color GetBackgroundColor()
        {
            return backgroundColorButton.BackColor;
        }

        //设置当前颜色
        public void SetColor(ColorTargetsType targetColorType, int R, int G, int B)
        {
            switch (targetColorType)
            {
                case ColorTargetsType.前景:
                    foregroundColorButton.BackColor = Color.FromArgb(R, G, B);
                    break;
                case ColorTargetsType.背景:
                    backgroundColorButton.BackColor = Color.FromArgb(R, G, B);
                    break;
            }
        }

        //获取工具箱按钮
        private ToolStripButton GetToolButton(ToolsType targetToolType)
        {
            switch (targetToolType)
            {
                case ToolsType.裁剪:
                    return toolStripButton_裁剪;
                case ToolsType.橡皮:
                    return toolStripButton_橡皮;
                case ToolsType.取色:
                    return toolStripButton_取色;
                case ToolsType.铅笔:
                    return toolStripButton_铅笔;
                case ToolsType.喷枪:
                    return toolStripButton_喷枪;
                case ToolsType.直线:
                    return toolStripButton_直线;
                case ToolsType.矩形:
                    return toolStripButton_矩形;
                case ToolsType.椭圆:
                    return toolStripButton_椭圆;
                case ToolsType.选定:
                    return toolStripButton_选定;
                case ToolsType.填充:
                    return toolStripButton_填充;
                case ToolsType.放大镜:
                    return toolStripButton_放大镜;
                case ToolsType.刷子:
                    return toolStripButton_刷子;
                case ToolsType.文字:
                    return toolStripButton_文字;
                case ToolsType.曲线:
                    return toolStripButton_曲线;
                case ToolsType.多边形:
                    return toolStripButton_多边形;
                case ToolsType.圆角矩形:
                    return toolStripButton_圆角矩形;
            }
            return null;
        }

        //初始化当前工具
        public void InitActiveTool()
        {
            SetActiveTool(ToolsType.铅笔);    //设置当前活动工具为铅笔
        }

        //设置当前活动工具
        public bool SetActiveTool(ToolsType theType)
        {
            if (ToolsType.多边形 == m_ActiveTool)
            {
                switch (Tools.m_ToolSize)
                {
                    case ToolSize.Size_1:
                        Tools.DrawPolygon(Graphics.FromImage(m_FormChild.m_SourceImage), GetForegroundColor());
                        break;
                    case ToolSize.Size_2:
                        Tools.DrawPolygon(Graphics.FromImage(m_FormChild.m_SourceImage), GetForegroundColor(), true, true);
                        break;
                    case ToolSize.Size_3:
                        Tools.DrawPolygon(Graphics.FromImage(m_FormChild.m_SourceImage), GetForegroundColor(), true, false);
                        break;
                }
                Tools.Init();
                m_FormChild.Draw();
            }
            else if (ToolsType.曲线 == m_ActiveTool)
            {
                Tools.曲线(MouseStateType.MouseUp, new Point(0, 0), GetForegroundColor(), m_FormChild.m_SourceImage, m_FormChild.m_TempImage, true);
                m_FormChild.Draw();
            }
            ToolStripButton oldButton = GetToolButton(m_ActiveTool);
            ToolStripButton newButton = GetToolButton(theType);
            if (null != newButton)
            {
                if (theType != m_ActiveTool)
                {
                    CreateToolSelections(theType);
                    if (null != oldButton)
                    {
                        oldButton.Checked = false;
                    }
                    else
                    {
                        toolStripButton_裁剪.Checked = false;
                        toolStripButton_橡皮.Checked = false;
                        toolStripButton_取色.Checked = false;
                        toolStripButton_铅笔.Checked = false;
                        toolStripButton_喷枪.Checked = false;
                        toolStripButton_直线.Checked = false;
                        toolStripButton_矩形.Checked = false;
                        toolStripButton_椭圆.Checked = false;
                        toolStripButton_选定.Checked = false;
                        toolStripButton_填充.Checked = false;
                        toolStripButton_放大镜.Checked = false;
                        toolStripButton_刷子.Checked = false;
                        toolStripButton_文字.Checked = false;
                        toolStripButton_曲线.Checked = false;
                        toolStripButton_多边形.Checked = false;
                        toolStripButton_圆角矩形.Checked = false;
                    }
                    newButton.Checked = true;
                    m_ActiveTool = theType;
                }
                else if (null != oldButton && !oldButton.Checked)
                {
                    oldButton.Checked = true;
                }
                if (CheckChildForm())
                {
                    m_FormChild.ChangeTool();
                }
                return true;
            }
            return false;
        }

        public void CreateToolSelections(ToolsType theType, bool bIsInit = true)
        {
            panelToolSelection.Controls.Clear();
            int iSelectCount = GetToolSelectionCount(theType);
            if (0 == iSelectCount)
            {
                return;
            }
            List<PictureBox> m_PointList = new List<PictureBox>();
            for (int i = 0; i < iSelectCount; ++i)
            {
                m_PointList.Add(new PictureBox());
            }
            switch (theType)
            {
                case ToolsType.橡皮:
                    {
                        if (bIsInit)
                        {
                            Tools.m_ToolSize = ToolSize.Size_3;
                        }
                        int iRuleX = panelToolSelection.Width / 2;
                        int iRuleY = panelToolSelection.Height / (iSelectCount + 1);
                        double size = 3;
                        for (int i = 0; i < iSelectCount; ++ i, ++ size)
                        {
                            m_PointList[i].Tag = i;
                            m_PointList[i].BackColor = Color.Black;
                            m_PointList[i].Size = new Size((int)(2 * size), (int)(2 * size));
                            if (0 == i)
                            {
                                m_PointList[i].Location = new Point((int)(iRuleX - size - 1), (int)(iRuleY / 2 - size));
                            }
                            else
                            {
                                m_PointList[i].Location = new Point((int)(iRuleX - size - 1), (int)(m_PointList[i - 1].Location.Y + m_PointList[i - 1].Height + iRuleY / 2));
                            }
                            CreateToolSelectionBackGroundImage(ToolsType.橡皮,m_PointList[i],i);
                            m_PointList[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.ToolSelection_Mouse_Down);
                            panelToolSelection.Controls.Add(m_PointList[i]);
                        }
                        break;
                    }
                case ToolsType.喷枪:
                    {
                        if (bIsInit)
                        {
                            Tools.m_ToolSize = ToolSize.Size_1;
                        }
                        int iRuleX = panelToolSelection.Width / 2;
                        int iRuleY = panelToolSelection.Height / (iSelectCount - 1);
                        for (int i = 0; i < iSelectCount; ++i)
                        {
                            m_PointList[i].Tag = i;
                            m_PointList[i].BackColor = Color.Black;
                            switch (i)
                            {
                                case 0:
                                    m_PointList[i].Size = new Size(iRuleX - 2, iRuleY - 10);
                                    m_PointList[i].Location = new Point(iRuleX * i + 2, 3);
                                    break;
                                case 1:
                                    m_PointList[i].Size = new Size(iRuleX - 2, iRuleY - 10);
                                    m_PointList[i].Location = new Point(iRuleX * i - 2, 3);
                                    break;
                                case 2:
                                    m_PointList[i].Size = new Size(iRuleX - 2, iRuleY - 10);
                                    m_PointList[i].Location = new Point(iRuleX / 2, iRuleY);
                                    break;
                            }
                            CreateToolSelectionBackGroundImage(ToolsType.喷枪, m_PointList[i], i);
                            m_PointList[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.ToolSelection_Mouse_Down);
                            panelToolSelection.Controls.Add(m_PointList[i]);
                        }
                        break;
                    }
                case ToolsType.直线:
                case ToolsType.曲线:
                    {
                        if (bIsInit)
                        {
                            Tools.m_ToolSize = ToolSize.Size_1;
                        }
                        for (int i = 0; i < iSelectCount; ++i)
                        {
                            int iRuleY = (int)((panelToolSelection.Height - 8) / iSelectCount);
                            m_PointList[i].Tag = i;
                            m_PointList[i].BackColor = Color.Black;
                            m_PointList[i].Size = new Size(panelToolSelection.Width - 8, iRuleY - 1);
                            m_PointList[i].Location = new Point(2, 3 + iRuleY * i);
                            CreateToolSelectionBackGroundImage(ToolsType.曲线, m_PointList[i], i);
                            m_PointList[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.ToolSelection_Mouse_Down);
                            panelToolSelection.Controls.Add(m_PointList[i]);
                        }
                        break;
                    }
                case ToolsType.选定:
                case ToolsType.文字:
                case ToolsType.裁剪:
                    break;
                case ToolsType.放大镜:
                    break;
                case ToolsType.刷子:
                    break;
                case ToolsType.矩形:
                case ToolsType.椭圆:
                case ToolsType.多边形:
                case ToolsType.圆角矩形:
                    {
                        if (bIsInit)
                        {
                            Tools.m_ToolSize = ToolSize.Size_1;
                        }
                        for (int i = 0; i < iSelectCount; ++i)
                        {
                            int iRuleY = (int)((panelToolSelection.Height - 8) / iSelectCount);
                            m_PointList[i].Tag = i;
                            m_PointList[i].BackColor = Color.Black;
                            m_PointList[i].Size = new Size(panelToolSelection.Width - 8, iRuleY - 2);
                            m_PointList[i].Location = new Point(2, 2 + (iRuleY + 1) * i);
                            CreateToolSelectionBackGroundImage(ToolsType.圆角矩形,m_PointList[i],i);
                            m_PointList[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.ToolSelection_Mouse_Down);
                            panelToolSelection.Controls.Add(m_PointList[i]);
                        }
                        break;
                    }
            }
            m_PointList.Clear();
        }

        public int GetToolSelectionCount(ToolsType theType)
        {
            switch (theType)
            {
                case ToolsType.取色:
                case ToolsType.铅笔:
                case ToolsType.填充:
                    return 0;
                case ToolsType.裁剪:
                case ToolsType.选定:
                case ToolsType.文字:
                    return 2;
                case ToolsType.喷枪:
                case ToolsType.矩形:
                case ToolsType.椭圆:
                case ToolsType.多边形:
                case ToolsType.圆角矩形:
                    return 3;
                case ToolsType.橡皮:
                case ToolsType.放大镜:
                    return 4;
                case ToolsType.直线:
                case ToolsType.曲线:
                    return 5;
                case ToolsType.刷子:
                    return 12;
            }
            return 0;
        }

        //获取时间字符串[纯数字]
        private String GetTimeString()
        {
            return ("" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
        }

        //新建
        private void Menu新建(bool bIsInit = false)
        {
            if (!CheckChildForm())
            {
                m_FormChild = new FormChild();
                m_FormChild.MdiParent = this;
                m_FormChild.Show();
                this.Text = "未命名 - 画图";
            }
            if (m_FormChild.m_bChanged)
            {
                switch ((int)MessageBox.Show(("" == m_FormChild.m_FileFullName) ? "将更改保存到 未命名 吗？" : "将更改保存到 " + m_FormChild.m_FileFullName.Substring(m_FormChild.m_FileFullName.LastIndexOf("\\") + 1).ToString() + " 吗？", "画图", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case 6:
                        Menu保存();
                        break;
                    case 7:
                        break;
                    default:
                        return;
                }
            }
            if (!bIsInit)
            {
                m_FormChild.Clear();
            }
            m_FormChild.m_FileFullName = "";
        }

        //打开
        private void Menu打开()
        {
            if (CheckChildForm())
            {
                if (m_FormChild.m_bChanged)
                {
                    switch ((int)MessageBox.Show(("" == m_FormChild.m_FileFullName) ? "将更改保存到 未命名 吗？" : "将更改保存到 " + m_FormChild.m_FileFullName.Substring(m_FormChild.m_FileFullName.LastIndexOf("\\") + 1).ToString() + " 吗？", "画图", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                    {
                        case 6:
                            Menu保存();
                            break;
                        case 7:
                            break;
                        default:
                            return;
                    }
                }
                OpenFileDialog openImageDialog = new OpenFileDialog();
                openImageDialog.Title = "打开";
                openImageDialog.Filter = @"位图|*.bmp|压缩图片|*.jpg|动态图片|*.gif";
                openImageDialog.FileName = "";
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    if (InitBitmap(openImageDialog.FileName))
                    {
                        m_FormChild.m_bChanged = false;
                    }

                }
            }
        }

        //保存
        private void Menu保存()
        {
            if (CheckChildForm())
            {
                if ("" == m_FormChild.m_FileFullName)
                {
                    SaveFileDialog saveImageDialog = new SaveFileDialog();
                    saveImageDialog.Title = "保存为";
                    saveImageDialog.Filter = @"位图|*.bmp|压缩图片|*.jpg|动态图片|*.gif";
                    saveImageDialog.FileName = "未命名.bmp";
                    if (saveImageDialog.ShowDialog() == DialogResult.OK)
                    {
                        string strFileName = saveImageDialog.FileName.ToString();
                        if (strFileName != "" && strFileName != null)
                        {
                            string fileExtName = strFileName.Substring(strFileName.LastIndexOf(".") + 1).ToString();
                            System.Drawing.Imaging.ImageFormat imgformat = System.Drawing.Imaging.ImageFormat.Bmp;//默认保存为JPG格式
                            if ("" != fileExtName)
                            {
                                switch (fileExtName)
                                {
                                    case "jpg":
                                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                        break;
                                    case "bmp":
                                        imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                        break;
                                    case "gif":
                                        imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                                        break;
                                    default:
                                        MessageBox.Show(this, "不支持文件的格式。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                }
                            }
                            m_FormChild.m_FileFullName = strFileName;
                            try
                            {
                                m_FormChild.m_SourceImage.Save(m_FormChild.m_FileFullName, imgformat);
                            }
                            catch
                            {
                                MessageBox.Show("画图程序不能存储该文件。");
                            }
                        }
                    }
                }
                else
                {
                    int iDotPos = m_FormChild.m_FileFullName.LastIndexOf(".");
                    String strPath = m_FormChild.m_FileFullName;
                    try
                    {
                        m_FormChild.m_SourceImage.Save(m_FormChild.m_FileFullName.Substring(0, iDotPos) + GetTimeString() + "." + strPath.Substring(iDotPos + 1), System.Drawing.Imaging.ImageFormat.Bmp);
                        m_FormChild.m_bChanged = false;
                    }
                    catch
                    {
                        MessageBox.Show("画图程序不能存储该文件。");
                    }
                }
            }
        }

        //另存为
        private void Menu另存为()
        {
            if (CheckChildForm())
            {
                string tempString = m_FormChild.m_FileFullName;
                m_FormChild.m_FileFullName = "";
                Menu保存();
                m_FormChild.m_FileFullName = tempString;
            }
        }
    }
}
