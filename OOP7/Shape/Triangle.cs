using System;
using System.Drawing;
using System.IO;

namespace OOP7
{
    public class Triangle : Base
    {
        private int a;
        private double h;
        private PointF[] points=new PointF[4];

        public override char getCode()
        {
            return 'T';
        }

        public void initPoint()
        {
            h = a * 0.866;
            points[0].X = x + a / 2;
            points[0].Y = y + (float)h - (float)2 / 3 * (float)h;
            points[3] = points[0];

            points[1].X = x - a / 2;
            points[1].Y = y + (float)h -(float)2 / 3 * (float)h;

            points[2].X = x;
            points[2].Y = y-(float)2/3*(float)h;
        }
        public override void initPaintComps()
        {
            base.initPaintComps();
            a = 40;
            sizecollision = a;
            initPoint();
        }

        public Triangle()
        {
            initPaintComps();
        }
        public Triangle(int x, int y, Mylist mylist, int width, int height)
        {
            initPaintComps();
            if ((x-a/2-4>0) && (x+a/2+4<width) && (y-(float)2/3*0.866*a-4>0)&&(y+ (float)1 / 3 * 0.866 * a + 4<height))//если есть место для объекта создаём
            {
                this.x = x;
                this.y = y;
                initPoint();
                mylist.refreshSelected(mylist);
                Selected = true;
                mylist.add(this);
            }
        }
        public Triangle(Triangle copy)
        {
            initPaintComps();
            init(copy);
            a = copy.a;
            initPoint();
        }

        public override bool isClick(int x, int y, bool isCtrl, Mylist mylist)
        {
            initPoint();
            if ((points[2].Y < y && points[0].Y>y)&&(x-points[2].X>(points[2].Y-(float)y)/ 1.732) && (x - points[2].X < -(points[2].Y-(float)y) / 1.732))
            {
                toSelect(isCtrl, mylist);
                return true;
            }
            return false;
        }

        public void drawTriangle(Graphics gr)
        {
            gr.FillPolygon(br, points);
            gr.DrawLines(mainpen, points);
            
        }

        public void drawSelectedTriangle(Graphics gr) 
        {
            gr.DrawLines(redpen, points);
        }

        public override void print(Graphics gr)
        {
            drawTriangle(gr);
            if (Selected)
            {
                drawSelectedTriangle(gr);
            }
        }
        public override bool canMove(int x_, int y_, int width, int height, Mylist mylist)
        {
            return ((base.canMove(x_, y_, width, height, mylist)) && (points[0].X + x_ < width - 5) && (points[0].Y + y_ < height - 5) && (points[1].X + x_ > 5) && (points[2].Y + y_ > 5));
        }
        public override void move(int x_, int y_, int width, int height, Mylist mylist)
        {
            base.move(x_, y_,width,height,mylist);
            initPoint();
        }

        public override bool canScaled(int size, int width, int height, Mylist mylist)
        {
            return (base.canScaled(size, width, height, mylist) && (a + size > 15) && (points[0].X + size < width-5) && (points[0].Y + size  < height-5) && (points[1].X - size >5) && (points[2].Y - size > 5));
        }
        public override void changeSize(int size, int width, int height, Mylist mylist)
        {
            a += size;
            sizecollision = a;
            initPoint();
        }
        public override void save(string path)
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("{0} {1} {2} {3} {4} {5}", getCode(), x, y, color, a, h);
            writer.Close();
        }
        public override void load(string path, string[] tmp)
        {
            x = Int32.Parse(tmp[1]);
            y = Int32.Parse(tmp[2]);
            setBrush(tmp[3]);
            a = Int32.Parse(tmp[4]);
            h = Int32.Parse(tmp[5]);
        }
        public override string info()
        {
            string tmp = ("Triagle", x, y, color, a, h).ToString();
            return tmp;
        }
    }
}
