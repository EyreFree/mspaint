using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paint
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            InitActiveTool();       //设置当前活动工具
            InitActiveColor();      //初始化活动颜色

            InitChildForm();        //初始化子对话框
        }

        //工具栏鼠标悬停事件
        private void Mouse_Hover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripButton tempToolButton = (System.Windows.Forms.ToolStripButton)sender;
            StatusLabel_Left.Text = (string)tempToolButton.Tag;
        }

        //工具栏鼠标离开事件
        private void Mouse_Leave(object sender, EventArgs e)
        {
            StatusLabel_Left.Text = "要获得帮助，请在“帮助”菜单中，单击“帮助主题”。";
        }

        //新建菜单
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu新建();
        }

        //打开菜单
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu打开();
        }

        //保存菜单
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu保存();
        }

        //另存为菜单
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Menu另存为();
        }

        private void 从扫描仪或照相机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "从扫描仪或照相机", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 打印预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "打印预览", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "页面设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "打印", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 发送ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "发送", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 设置为墙纸平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "设置为墙纸平铺", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 设置为墙纸居中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "设置为墙纸居中", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "撤销", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 重复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "重复", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "剪切", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "复制", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "粘贴", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 清除选定内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "清除选定内容", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "全选", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 复制到ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "复制到", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 粘贴来源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "粘贴来源", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 工具箱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (工具箱ToolStripMenuItem.Checked)
            {
                toolBox_Left.Visible = false;
                toolBox_Right.Visible = false;
                panelToolSelection.Visible = false;
                panelTools.Visible = false;
                工具箱ToolStripMenuItem.Checked = false;
            }
            else
            {
                toolBox_Left.Visible = true;
                toolBox_Right.Visible = true;
                panelToolSelection.Visible = true;
                panelTools.Visible = true;
                工具箱ToolStripMenuItem.Checked = true;
            }
        }

        private void 颜料盒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (颜料盒ToolStripMenuItem.Checked)
            {
                panelColor.Visible = false;
                颜料盒ToolStripMenuItem.Checked = false;
            }
            else
            {
                panelColor.Visible = true;
                颜料盒ToolStripMenuItem.Checked = true;
            }
        }

        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (状态栏ToolStripMenuItem.Checked)
            {
                statusBox.Visible = false;
                状态栏ToolStripMenuItem.Checked = false;
            }
            else
            {
                statusBox.Visible = true;
                状态栏ToolStripMenuItem.Checked = true;
            }
        }

        private void 文字工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Color = GetForegroundColor();
            fontDialog.AllowScriptChange = true;
            fontDialog.ShowColor = true;
            if (CheckChildForm())
            {
                fontDialog.Font = m_FormChild.alphaTextBox.Font;
                if (fontDialog.ShowDialog() != DialogResult.Cancel)
                {
                    m_FormChild.alphaTextBox.Font = fontDialog.Font;    //将当前选定的文字改变字体
                    m_FormChild.alphaTextBox.ForeColor = fontDialog.Color;
                }
            }
        }

        private void 缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "缩放", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 查看位图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "查看位图", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 翻转旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "翻转旋转", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 拉伸扭曲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "拉伸扭曲", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 反色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_FormChild.m_SourceImage = ReverseColor(m_FormChild.m_SourceImage);
            m_FormChild.Draw();
        }

        private void 属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "属性", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 清除图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "清除图形", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 不透明处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "不透明处理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 编辑颜色EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();        //新建颜色对话框
            colorDialog.Color = GetForegroundColor();

            if (colorDialog.ShowDialog() == DialogResult.OK)    //打开颜色对话框，并接收对话框操作结果,如果用户点击OK
            {
                var color = colorDialog.Color;                  //获取用户选择的颜色，然后你就可以用这个颜色了
                SetColor(ColorTargetsType.前景, color.R, color.G, color.B);
            }
        }

        private void 帮助主题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "帮助主题", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void 关于画图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "关于画图", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toolStripButton_裁剪_Click(object sender, EventArgs e)
        {
            //SetActiveTool(ToolsType.裁剪);
            MessageBox.Show("Unfinished.", "裁剪", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toolStripButton_橡皮_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.橡皮);
        }

        private void toolStripButton_取色_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.取色);
        }

        private void toolStripButton_铅笔_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.铅笔);
        }

        private void toolStripButton_喷枪_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.喷枪);
        }

        private void toolStripButton_直线_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.直线);
        }

        private void toolStripButton_矩形_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.矩形);
        }

        private void toolStripButton_椭圆_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.椭圆);
        }

        private void toolStripButton_选定_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "选定", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //SetActiveTool(ToolsType.选定);
        }

        private void toolStripButton_填充_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "填充", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //SetActiveTool(ToolsType.填充);
        }

        private void toolStripButton_放大镜_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unfinished.", "放大镜", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //SetActiveTool(ToolsType.放大镜);
        }

        private void toolStripButton_刷子_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.刷子);
        }

        private void toolStripButton_文字_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.文字);
        }

        private void toolStripButton_曲线_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.曲线);
        }

        private void toolStripButton_多边形_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.多边形);
        }

        private void toolStripButton_圆角矩形_Click(object sender, EventArgs e)
        {
            SetActiveTool(ToolsType.圆角矩形);
        }

        private void ColorButton_Mouse_Down(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.PictureBox tempPictuenBox = (System.Windows.Forms.PictureBox)sender;
            SetColor((MouseButtons.Left == e.Button) ? ColorTargetsType.前景 : ColorTargetsType.背景, tempPictuenBox.BackColor.R, tempPictuenBox.BackColor.G, tempPictuenBox.BackColor.B);
        }

        private void ToolSelection_Mouse_Down(object sender, MouseEventArgs e)
        {
            Tools.m_ToolSize = (ToolSize)((PictureBox)sender).Tag;
            CreateToolSelections(m_ActiveTool, false);
        }

        private void PicturnBox_ColorState_BackGroundColor_Changed(object sender, EventArgs e)
        {
            Create3DBackGroundImage((System.Windows.Forms.PictureBox)sender, false);
        }

        //鼠标拖放完成
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            if (CheckChildForm())
            {
                var filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in filePath)         // 每个file都是被拖拽文件的完整路径
                {
                    InitBitmap(file);
                }
            }
        }

        //鼠标拖放进入窗体用户区
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
