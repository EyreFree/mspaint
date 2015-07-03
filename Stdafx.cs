using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paint
{
    // 目标颜色类型
    public enum ColorTargetsType
    {
        前景,
        背景
    }

    //主窗体
    partial class FormMain
    {
        private FormChild m_FormChild = null;                             //子窗体
        public ToolsType m_ActiveTool = ToolsType.铅笔;                   //当前工具类型
    }

    //子窗体
    partial class FormChild
    {

        public FormMain m_FormMain = null;                                //父窗体
        public string m_FileFullName = "";                                //图片路径

        public bool m_bChanged = false;                                   //是否发生改变

        public Bitmap m_SourceImage = null;                               //源图片
        public Bitmap m_TempImage = null;                                 //临时图片

        //坐标List结构
        private List<Point> m_PointList = new List<Point>();

        //边界最大拖动距离
        const int SCOPE = 4;
    }
}
