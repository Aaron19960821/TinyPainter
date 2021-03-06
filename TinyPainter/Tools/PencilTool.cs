﻿//==============================================================
//  Create by Yuchen Wang at 5/23/2017 10:06:00 PM.
//  Version 1.0
//  Yuchen Wang [mail: wyc8094@gmail.com]
//==============================================================


using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyPainter.Algorithm;

namespace TinyPainter.Tools
{
    class PencilTool:ITools
    {
        protected bool isDrawing;
        protected Pen DrawingPen;
        protected Point curPoint;

        public PencilTool(PaintSettings settings, ImageFile newfile, PictureBox mainpic)
            :base(settings, newfile, mainpic)
        {
            this.isDrawing = false;
            this.swapgraphics = Graphics.FromImage(swapImage.Bitmap);
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                this.updateMaingraph();
            }
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.isDrawing = true;
                this.DrawingPen = new Pen(settings.PrimaryColor, settings.Width);
                this.curPoint = new Point(e.Location.X, e.Location.Y);
            }

        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                this.startPoint = this.curPoint;
                this.curPoint = new Point(e.Location.X, e.Location.Y);

                swapgraphics.DrawLine(DrawingPen, startPoint, curPoint);
                g.DrawLine(DrawingPen, startPoint, curPoint);
            }
        }
    }
}
