﻿using System;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using TinyPainter.Tools;
using TinyPainter.Algorithm;

namespace TinyPainter
{
    public partial class mainPainter : Form
    {
        private const int initwidth = 600;
        private const int initheight = 450;

        protected ToolStripItem curbutton;
        protected bool isSaved;
        ITools curITools;
        PaintSettings cursettings;

        //operatormap is used to store the runtime image
        //while maingraph is used to store the whole image
        ImageFile operatormap;
        ImageFile maingraph;
        ImageFile clipboard;

        public mainPainter()
        {
            InitializeComponent();
            InitOthers();
        }

        /// <summary>
        /// initialize other elements in the current form
        /// </summary>
        public void InitOthers()
        {
            this.isSaved = true;
            this.WidthBox.Text = "5";
            cursettings = new PaintSettings();
            cursettings.g = operatebox.CreateGraphics();
            operatormap = new ImageFile(initwidth, initheight, Color.White);
            maingraph = operatormap.CloneImage();
            curITools = new ArrowTool(cursettings,operatormap,operatebox);
            curbutton = Arrow;
            curstate.Image = curbutton.Image;
            colors.BackColor = Color.Black;
            ShowImage();
        }

        protected void update()
        { 
            this.isSaved = false;
            curITools.updateMaingraph();
        }

        public void Arrow_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new ArrowTool(cursettings, operatormap, operatebox);
        }

        public void Pencil_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new PencilTool(cursettings, operatormap, operatebox);
        }

        public void Eraser_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new EraseTool(cursettings, operatormap, operatebox);
        }

        public void MagicPencil_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new MagicPencilTool(cursettings, operatormap, operatebox);
        }

        public void Fill_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new FillTool(cursettings, operatormap, operatebox);
        }

        public void Line_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new LineTool(cursettings, operatormap, operatebox);
        }

        public void Eclipse_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new EclipseTool(cursettings, operatormap, operatebox);
        }

        public void Rectangle_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new RectangleTool(cursettings, operatormap, operatebox);
        }

        public void Text_Clicked(object sender, EventArgs e)
        {
            update();
            curITools.UnloadTool();
            curITools = new TextTool(cursettings, operatormap, operatebox);
        }

        public void Color_Clicked(object sender, EventArgs e)
        {
            Changecolor();
        }

        /// <summary>
        /// change current primary color
        /// </summary>
        protected void Changecolor()
        {
            ColorDialog colornavigator = new ColorDialog();
            if(colornavigator.ShowDialog() == DialogResult.OK)
            {
                Color nextColor = colornavigator.Color;
                this.cursettings.PrimaryColor = nextColor;
                colors.BackColor = nextColor;
            }
            return;
        }

        ///<summary>
        /// used to show basic information of this application
        ///    </summary>
        protected void About_App(object sender, EventArgs e)
        {
            AboutTinyPainter painterInfo = new AboutTinyPainter();
            painterInfo.ShowDialog();
            return;
        }

        protected void new_pic(object sender, EventArgs e)
        {
            Size newtab = new Size();
            if(newtab.ShowDialog() == DialogResult.OK)
            {
                int newwidth = newtab.widthres;
                int newheight = newtab.heightres;
                maingraph.Bitmap.Dispose();
                operatormap.Bitmap.Dispose();
                operatormap = new ImageFile(newwidth, newheight, Color.White);
                maingraph = operatormap.CloneImage();
                operatebox.Height = newheight;
                operatebox.Width = newwidth;
                operatebox.Invalidate();
            }
        }

        /// <summary>
        /// used to open an image from a existing file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void open_pic(object sender, EventArgs e)
        {
            OpenFileDialog FileNavigator = new OpenFileDialog();
            FileNavigator.Filter = "*.bmp|*.jpg|*.png|*.jpeg";
            if (FileNavigator.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(FileNavigator.ToString());

                if (operatormap.Open(FileNavigator.FileName))
                {
                    ShowImage();
                }
                else
                {
                    MessageBox.Show("Load Image Error(pleace check your file)");
                }
            }
            return;
        }

        /// <summary>
        /// save current file to the memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void save_pic(object sender, EventArgs e)
        {
            if(isSaved == false)
            {
                maingraph = operatormap.CloneImage();
                isSaved = true;
            }
        }

        protected void saveas_pic(object sender, EventArgs e)
        {
            if (isSaved == false)
            {
                maingraph = operatormap.CloneImage();
                isSaved = true;
            }

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Bitmap (*.BMP)|*.BMP";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                if (!maingraph.Save(saveDlg.FileName))
                    MessageBox.Show("Error");
                else
                {
                    operatormap.FileName = saveDlg.FileName;
                    ShowImage();
                }
            }
        }

        /// <summary>
        /// copy image into clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void copy_pic(object sender, EventArgs e)
        {
            clipboard = operatormap.CloneImage();
            return;
        }

        protected void paste_pic(object sender, EventArgs e)
        {
            operatormap = clipboard.CloneImage();
        }

        protected void cut_pic(object sender, EventArgs e)
        {
            clipboard = operatormap.CloneImage();
            Graphics.FromImage(operatormap.Bitmap).Clear(Color.White);
            curITools.flushSwap();
            this.operatebox.Invalidate();
        }

        protected void clear_pic(object sender, EventArgs e)
        {
            this.isSaved = false;
            curITools.swapgraphics.Clear(Color.White);
            curITools.updateMaingraph();
            this.operatebox.Invalidate();
        }

        //exit this application when exit option is pressed
        protected void Exit_App(object sender, EventArgs e)
        {
            Application.Exit();
            return;
        }

        // the status bar on the bottom of this application


        protected void pos_display(object sender, MouseEventArgs e)
        {
            cursorPos.Text = e.Location.ToString();
            return;
        }

        /// <summary>
        /// show the image from a existing image file
        /// </summary>
        protected void ShowImage()
        {
            string newfile = operatormap.FileName;
            base.Text = string.Format("TinyPainter - [{0}]", newfile == null ? "Untitled" : new FileInfo(newfile).Name);
            operatebox.ClientSize = operatormap.Bitmap.Size;
        }

        /// <summary>
        /// handle paint event happened in the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PaintOnBox(object sender, PaintEventArgs e)
        {
            cursettings.g.DrawImage(curITools.swapImage.Bitmap, 0, 0);
            return;
        }

        /// <summary>
        /// set new width of drawing pen
        /// </summary>
        /// <param name="newwidth"></param>
        protected void setLineWidth(int newwidth)
        {
            if(this.cursettings != null)
            {
                this.cursettings.Width = newwidth;
            }
        }

        /// <summary>
        /// set slim size of line width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void slimWidth(object sender, EventArgs e)
        {
            this.setLineWidth(1);
        }

        /// <summary>
        /// set medium size of width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void mediumWidthe(object sender, EventArgs e)
        {
            this.setLineWidth(5);
        }

        /// <summary>
        /// set large line width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void largeWidth(object sender, EventArgs e)
        {
            this.setLineWidth(10);
        }

        /// <summary>
        /// flush the text when text box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void flushBox(object sender, EventArgs e)
        {
            this.WidthBox.Text = "";
        }

        /// <summary>
        /// set the width of line by users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selfmadeWidth(object sender, EventArgs e)
        {
            string res = WidthBox.Text;
            int newwidth = 0;
            if (int.TryParse(res, out newwidth) == true)
                this.setLineWidth(newwidth);
            else
            {
                MessageBox.Show("Invalid Value");
                WidthBox.Text = cursettings.Width.ToString();
            }
            return;
        }
    }
}
