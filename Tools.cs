using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint
{
    // 工具类型
    public enum ToolsType
    {
        裁剪,
        橡皮,
        取色,
        铅笔,
        喷枪,
        直线,
        矩形,
        椭圆,
        选定,
        填充,
        放大镜,
        刷子,
        文字,
        曲线,
        多边形,
        圆角矩形
    }

    // 鼠标状态类型
    public enum MouseStateType
    {
        MouseDown,
        MouseMove,
        MouseUp
    }

    //工具型号
    public enum ToolSize
    {
        Size_1,
        Size_2,
        Size_3,
        Size_4,
        Size_5,
        Size_6,
        Size_7,
        Size_8,
        Size_9,
        Size_10,
        Size_11,
        Size_12
    }

    class Tools
    {
        public static ToolSize m_ToolSize = ToolSize.Size_1;
        public static bool m_TempImageUsed = false;
        public static MouseStateType m_MouseState = MouseStateType.MouseUp;
        public static Point m_PreviousPoint = Point.Empty;
        public static List<Point> m_PointList = new List<Point>();
        private static Random m_Random = new Random();             //随机数发生器

        public static void Init()
        {
            m_ToolSize = ToolSize.Size_1;
            m_TempImageUsed = false;
            m_MouseState = MouseStateType.MouseUp;
            m_PreviousPoint = Point.Empty;
            m_PointList.Clear();
        }

        /*
        static void 裁剪()
        {
            switch()
            {
        
            }
        }
        */

        public static void 橡皮(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap)
        {
            int iSize = 2 * (int)m_ToolSize + 1;
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    {
                        Graphics.FromImage(sourceBitmap).FillRectangle(new SolidBrush(colorType), targetPoint.X - iSize, targetPoint.Y - iSize, iSize * 2, iSize * 2);
                        m_MouseState = MouseStateType.MouseDown;
                        break;
                    }
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Graphics.FromImage(sourceBitmap).FillRectangle(new SolidBrush(colorType), targetPoint.X - iSize, targetPoint.Y - iSize, iSize * 2, iSize * 2);
                    }
                    break;
                case MouseStateType.MouseUp:
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static Color 取色(MouseStateType mouseType, Point targetPoint, ref Bitmap sourceBitmap)
        {
            Color returnColor = Color.Empty;
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    if (sourceBitmap.Width > targetPoint.X && 0 < targetPoint.X && sourceBitmap.Height > targetPoint.Y && 0 < targetPoint.Y)
                    {
                        returnColor = sourceBitmap.GetPixel(targetPoint.X, targetPoint.Y);
                    }
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState && sourceBitmap.Width > targetPoint.X && 0 < targetPoint.X && sourceBitmap.Height > targetPoint.Y && 0 < targetPoint.Y)
                    {
                        returnColor = sourceBitmap.GetPixel(targetPoint.X, targetPoint.Y);
                    }
                    break;
                case MouseStateType.MouseUp:
                    if (sourceBitmap.Width > targetPoint.X && 0 < targetPoint.X && sourceBitmap.Height > targetPoint.Y && 0 < targetPoint.Y)
                    {
                        returnColor = sourceBitmap.GetPixel(targetPoint.X, targetPoint.Y);
                    }
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
            return returnColor;
        }

        public static void 铅笔(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Graphics.FromImage(sourceBitmap).DrawLine(new Pen(colorType, 1), m_PreviousPoint, targetPoint);
                        m_PreviousPoint = targetPoint;
                    }
                    break;
                case MouseStateType.MouseUp:
                    m_PreviousPoint = Point.Empty;
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static void 喷枪(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    {
                        m_PreviousPoint = targetPoint;
                        m_MouseState = MouseStateType.MouseDown;
                        break;
                    }
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        int iSize = 5 * ((int)m_ToolSize + 1);
                        for (int i = 0; i < 25; ++i)
                        {
                            int x = 0, y = 0;
                            do
                            {
                                x = m_Random.Next(-iSize, iSize);
                                y = m_Random.Next(-iSize, iSize);
                            }
                            while (x * x + y * y > iSize * iSize);
                            int oriX = Tools.m_PreviousPoint.X + x, oriY = Tools.m_PreviousPoint.Y + y;
                            if (0 < oriX && oriX < sourceBitmap.Width && 0 < oriY && oriY < sourceBitmap.Height)
                            {
                                sourceBitmap.SetPixel(Tools.m_PreviousPoint.X + x, Tools.m_PreviousPoint.Y + y, colorType);
                            }
                        }
                    }
                    break;
                case MouseStateType.MouseUp:
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static void 直线(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        Graphics.FromImage(temporyImage).DrawLine(new Pen(colorType, (int)m_ToolSize), m_PreviousPoint, targetPoint);
                    }
                    break;
                case MouseStateType.MouseUp:
                    {
                        m_TempImageUsed = false;
                        Graphics.FromImage(sourceBitmap).DrawLine(new Pen(colorType, (int)m_ToolSize), m_PreviousPoint, targetPoint);
                        m_PreviousPoint = Point.Empty;
                        m_MouseState = MouseStateType.MouseUp;
                        break;
                    }
            }
        }

        public static void 矩形(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                Graphics.FromImage(temporyImage).DrawRectangle(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_2:
                                Graphics.FromImage(temporyImage).FillRectangle(Brushes.White, staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                Graphics.FromImage(temporyImage).DrawRectangle(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_3:
                                Graphics.FromImage(temporyImage).FillRectangle(new SolidBrush(colorType), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                        }
                    }
                    break;
                case MouseStateType.MouseUp:
                    {
                        m_TempImageUsed = false;
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                Graphics.FromImage(sourceBitmap).DrawRectangle(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_2:
                                Graphics.FromImage(sourceBitmap).FillRectangle(Brushes.White, staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                Graphics.FromImage(sourceBitmap).DrawRectangle(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_3:
                                Graphics.FromImage(sourceBitmap).FillRectangle(new SolidBrush(colorType), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                        }
                        m_PreviousPoint = Point.Empty;
                        m_MouseState = MouseStateType.MouseUp;
                        break;
                    }
            }
        }

        public static void 椭圆(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                Graphics.FromImage(temporyImage).DrawEllipse(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_2:
                                Graphics.FromImage(temporyImage).FillEllipse(Brushes.White, staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                Graphics.FromImage(temporyImage).DrawEllipse(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_3:
                                Graphics.FromImage(temporyImage).FillEllipse(new SolidBrush(colorType), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                        }
                    }
                    break;
                case MouseStateType.MouseUp:
                    {
                        m_TempImageUsed = false;
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                Graphics.FromImage(sourceBitmap).DrawEllipse(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_2:
                                Graphics.FromImage(sourceBitmap).FillEllipse(Brushes.White, staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                Graphics.FromImage(sourceBitmap).DrawEllipse(new Pen(colorType, 1), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                            case ToolSize.Size_3:
                                Graphics.FromImage(sourceBitmap).FillEllipse(new SolidBrush(colorType), staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y);
                                break;
                        }
                        m_PreviousPoint = Point.Empty;
                        m_MouseState = MouseStateType.MouseUp;
                        break;
                    }
            }
        }

        /*
                class 选定
                {
        
                }

                class 填充
                {
        
                }

                class 放大镜
                {
        
                }
        */

        public static void 刷子(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    m_PreviousPoint = targetPoint;
                    m_PointList.Add(m_PreviousPoint);
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        m_PreviousPoint = targetPoint;
                        m_PointList.Add(m_PreviousPoint);
                        DrawPolygon(Graphics.FromImage(temporyImage), colorType, false, false, false, 10);
                        m_TempImageUsed = true;
                    }
                    break;
                case MouseStateType.MouseUp:
                    m_TempImageUsed = false;
                    if (1 <= m_PointList.Count)
                    {
                        m_PointList.Add(targetPoint);
                        DrawPolygon(Graphics.FromImage(sourceBitmap), colorType, false, false, false,10);
                    }
                    m_PointList.Clear();
                    Graphics.FromImage(temporyImage).Clear(Color.Empty);
                    m_PreviousPoint = Point.Empty;
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static void 文字(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref System.Windows.Forms.TextBox alphaTextBox)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_PreviousPoint = targetPoint;
                    if (alphaTextBox.Visible)
                    {
                        for (int i = 0; i < alphaTextBox.Lines.Length; ++ i)
                        {
                            Point newLocation = new Point(alphaTextBox.Location.X, alphaTextBox.Location.Y + i * (alphaTextBox.Font.Height - 5));
                            Graphics.FromImage(sourceBitmap).DrawString(alphaTextBox.Lines[i], alphaTextBox.Font, new SolidBrush(Color.FromArgb(255, alphaTextBox.ForeColor)), newLocation);
                        }
                        //Graphics.FromImage(sourceBitmap).DrawString(alphaTextBox.Text, alphaTextBox.Font, new SolidBrush(Color.FromArgb(255, alphaTextBox.ForeColor)), new Point(alphaTextBox.Location.X, alphaTextBox.Location.Y));
                        alphaTextBox.Text = "";
                        alphaTextBox.Visible = false;
                    }
                    else
                    {
                        alphaTextBox.Location = Tools.m_PreviousPoint;
                        alphaTextBox.ForeColor = colorType;
                        alphaTextBox.Font = new Font(alphaTextBox.Font.FontFamily,22, alphaTextBox.Font.Style);
                        alphaTextBox.Visible = true;
                    }
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState && alphaTextBox.Visible)
                    {
                        alphaTextBox.Location = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        alphaTextBox.Size = new Size(Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X), Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y));
                    }
                    break;
                case MouseStateType.MouseUp:
                    m_PreviousPoint = Point.Empty;
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static void 曲线(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    if (0 == m_PointList.Count)
                    {
                        m_PointList.Add(targetPoint);
                    }
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        switch (m_PointList.Count)
                        {
                            case 1:
                                Graphics.FromImage(temporyImage).DrawLine(new Pen(colorType, (int)m_ToolSize), m_PointList[0], targetPoint);
                                break;
                            case 2:
                                Graphics.FromImage(temporyImage).DrawBezier(new Pen(colorType, (int)m_ToolSize), m_PointList[1], targetPoint, m_PointList[0], m_PointList[0]);
                                break;
                            case 3:
                                Graphics.FromImage(temporyImage).DrawBezier(new Pen(colorType, (int)m_ToolSize), m_PointList[1], targetPoint, m_PointList[2], m_PointList[0]);
                                break;
                        }
                    }
                    break;
                case MouseStateType.MouseUp:
                    {
                        if (m_PointList.Count < 3)
                        {
                            m_PointList.Add(targetPoint);
                        }
                        else
                        {
                            m_TempImageUsed = false;
                            Graphics.FromImage(sourceBitmap).DrawBezier(new Pen(colorType, (int)m_ToolSize), m_PointList[1], targetPoint, m_PointList[2], m_PointList[0]);
                            m_PointList.Clear();
                        }
                        m_MouseState = MouseStateType.MouseUp;
                        break;
                    }
            }
        }

        public static void 多边形(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    if (1 <= m_PointList.Count)
                    {
                        Graphics.FromImage(temporyImage).DrawLine(new Pen(colorType, 1), targetPoint, m_PointList[m_PointList.Count - 1]);
                    }
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        if (!m_TempImageUsed)
                        {
                            m_TempImageUsed = true;
                            if (0 == Tools.m_PointList.Count)
                            {
                                Tools.m_PointList.Add(m_PreviousPoint);
                            }
                        }
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        if (1 < m_PointList.Count)
                        {
                            Graphics.FromImage(temporyImage).DrawLines(new Pen(colorType, 1), m_PointList.ToArray());
                        }
                        Graphics.FromImage(temporyImage).DrawLine(new Pen(colorType, 1), targetPoint, m_PointList[m_PointList.Count - 1]);
                    }
                    break;
                case MouseStateType.MouseUp:
                    if (1 <= m_PointList.Count)
                    {
                        Tools.m_PointList.Add(targetPoint);
                        Graphics.FromImage(temporyImage).DrawLines(new Pen(colorType, 1), m_PointList.ToArray());
                    }
                    m_PreviousPoint = Point.Empty;
                    m_MouseState = MouseStateType.MouseUp;
                    break;
            }
        }

        public static void 圆角矩形(MouseStateType mouseType, Point targetPoint, Color colorType, ref Bitmap sourceBitmap, ref Bitmap temporyImage)
        {
            switch (mouseType)
            {
                case MouseStateType.MouseDown:
                    m_TempImageUsed = true;
                    m_PreviousPoint = targetPoint;
                    m_MouseState = MouseStateType.MouseDown;
                    break;
                case MouseStateType.MouseMove:
                    if (MouseStateType.MouseDown == m_MouseState)
                    {
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        Graphics.FromImage(temporyImage).Clear(Color.Empty);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                DrawRoundRectangle(Graphics.FromImage(temporyImage), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8);
                                break;
                            case ToolSize.Size_2:
                                DrawRoundRectangle(Graphics.FromImage(temporyImage), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8, true, true);
                                break;
                            case ToolSize.Size_3:
                                DrawRoundRectangle(Graphics.FromImage(temporyImage), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8, true, false);
                                break;
                        }
                    }
                    break;
                case MouseStateType.MouseUp:
                    {
                        m_TempImageUsed = false;
                        Point staticPoint = new Point((Tools.m_PreviousPoint.X < targetPoint.X) ? Tools.m_PreviousPoint.X : targetPoint.X, (Tools.m_PreviousPoint.Y < targetPoint.Y) ? Tools.m_PreviousPoint.Y : targetPoint.Y);
                        targetPoint.X = Math.Abs(targetPoint.X - Tools.m_PreviousPoint.X);
                        targetPoint.Y = Math.Abs(targetPoint.Y - Tools.m_PreviousPoint.Y);
                        switch (m_ToolSize)
                        {
                            case ToolSize.Size_1:
                                DrawRoundRectangle(Graphics.FromImage(sourceBitmap), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8);
                                break;
                            case ToolSize.Size_2:
                                DrawRoundRectangle(Graphics.FromImage(sourceBitmap), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8, true, true);
                                break;
                            case ToolSize.Size_3:
                                DrawRoundRectangle(Graphics.FromImage(sourceBitmap), colorType, new Rectangle(staticPoint.X, staticPoint.Y, targetPoint.X, targetPoint.Y), 8, true, false);
                                break;
                        }
                        m_PreviousPoint = Point.Empty;
                        m_MouseState = MouseStateType.MouseUp;
                        break;
                    }
            }
        }

        //绘制多边形
        public static void DrawPolygon(Graphics g, Color color, bool bISolid = false, bool bIsWhiteSolid = false, bool bIsCloseFigure = true,int iSize = 1)
        {
            m_TempImageUsed = false;
            GraphicsPath tempGraphicsPath = new GraphicsPath();
            if (1 < m_PointList.Count)
            {
                tempGraphicsPath.AddLines(Tools.m_PointList.ToArray());
                if (bIsCloseFigure)
                {
                    tempGraphicsPath.CloseFigure();
                }
                if (bISolid)
                {
                    if (!bIsWhiteSolid)
                    {
                        g.FillPath(new SolidBrush(color), tempGraphicsPath);
                        return;
                    }
                    g.FillPath(Brushes.White, tempGraphicsPath);
                }
                g.DrawPath(new Pen(color, iSize), tempGraphicsPath);
            }
        }

        //绘制圆角矩形
        public static void DrawRoundRectangle(Graphics g, Color color, Rectangle rect, int cornerRadius, bool bISolid = false, bool bIsWhiteSolid = false)
        {
            GraphicsPath tempGraphicsPath = CreateRoundedRectanglePath(rect, cornerRadius);
            if (bISolid)
            {
                if (!bIsWhiteSolid)
                {
                    g.FillPath(new SolidBrush(color), tempGraphicsPath);
                    return;
                }
                g.FillPath(Brushes.White, tempGraphicsPath);
            }
            g.DrawPath(new Pen(color), tempGraphicsPath);
        }

        //生成圆角矩形区域
        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, float cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            int iLen = rect.Height;
            if (rect.Height > rect.Width)
            {
                iLen = rect.Width;
            }
            if (iLen < 2 * cornerRadius)
            {
                roundedRect.AddEllipse(rect);
                return roundedRect;
            }
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.CloseFigure();
            return roundedRect;
        }
    }
}
