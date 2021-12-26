using System;
using System.Drawing;
using System.IO;

namespace OOP7
{
    public class Square : Base
    {
        private int a;

        public override char getCode()
        {
            return 'S';
        }

        public override void initPaintComps()
        {
            base.initPaintComps();
            a = 40;
            sizecollision = a;
        }
        public Square()
        {
            initPaintComps();
        }
        public Square(int x, int y, Mylist mylist, int width, int height)
        {
            initPaintComps();
            if ((x + a / 2 < width) && (y + a / 2 < height) && (x - a / 2 > 0) && (y - a / 2 > 0))//если есть место для объекта создаём
            {
                this.x = x;
                this.y = y;
                mylist.refreshSelected(mylist);
                Selected = true;
                mylist.add(this);
            }
        }

        public Square(Square copy)
        {
            initPaintComps();
            init(copy);
            a = copy.a;
        }

        public override bool isClick(int x, int y, bool isCtrl, Mylist mylist)
        {
            if ((x > (this.x - (a / 2))) && (x < (this.x + (a / 2))) && (y > (this.y - (a / 2))) && (y < (this.y + (a / 2))))
            {
                toSelect(isCtrl, mylist);
                return true;
            }
            return false;
        }

        public void drawRectangle(Graphics gr)
        {
            gr.FillRectangle(br, x - a / 2, y - a / 2, a, a);
            gr.DrawRectangle(mainpen, x - a / 2, y - a / 2, a, a);
        }
        public void drawSelectedRectangle(Graphics gr)
        {
            gr.DrawRectangle(redpen, x - a / 2, y - a / 2, a, a);
        }
        public override void print(Graphics gr)
        {
            drawRectangle(gr);
            if (Selected)
            {
                drawSelectedRectangle(gr);
            }
        }
        public override bool canMove(int x_, int y_, int width, int height, Mylist mylist)
        {
            return (base.canMove(x_, y_, width, height, mylist)) && ((x + a / 2 + x_ < width - 5) && (y + a / 2 + y_ < height - 5) && (x - a / 2 + x_ > 5) && (y - a / 2 + y_  > 5));
        }

        public override bool canScaled(int size, int width, int height, Mylist mylist)
        {
            return (base.canScaled(size, width, height, mylist) && (a + size > 6) && (x + a / 2 + size  < width-5) && (y + a / 2 + size  < height-5) && (x - a / 2 - size  > 5) && (y - a / 2 - size > 5));
        }
        public override void changeSize(int size, int width, int height, Mylist mylist)
        {
            a += size * 2;
            sizecollision = a;
        }
        public override void save(string path)
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine("{0} {1} {2} {3} {4}", getCode(), x, y, color, a);
            writer.Close();
        }
        public override void load(string path, string[] tmp)
        {
            x = Int32.Parse(tmp[1]);
            y = Int32.Parse(tmp[2]);
            setBrush(tmp[3]);
            a = Int32.Parse(tmp[4]);
        }
        public override string info()
        {
            string tmp = ("Square", x, y, color, a).ToString();
            return tmp;
        }
    }
}
