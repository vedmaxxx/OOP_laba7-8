using System;
using System.Drawing;


namespace OOP7
{
    public class Base
    {
        protected Pen mainpen;
        protected Pen redpen;
        protected Pen goldpen;
        public int x, y;
        public int sizecollision;
        protected bool Selected = false;
        protected Brush br = Brushes.White;
        protected string color = "White";

        protected bool slime=false;
        protected bool isSticked = false;

        public bool getIsSticked()
        {
            return isSticked;
        }
        public void setIsSticked(bool value)
        {
            isSticked = value;
        }

        public virtual void initPaintComps()
        {
            mainpen = new Pen(Color.Black);
            mainpen.Width = 1;
            redpen = new Pen(Color.Red);
            redpen.Width = 1;
        }

        public virtual void init(Base copy)
        {
            x = copy.x;
            y = copy.y;
            br = copy.br;
            Selected = copy.Selected;
            color = copy.color;
            sizecollision = copy.sizecollision;
        }

        public virtual void setMainPen(string pen)
        {
            mainpen = new Pen(Color.FromName(pen));
        }
		public virtual char getCode()
        {
            return 'B';
        }
        public virtual bool isClick(int x, int y, bool isCtrl, Mylist mylist) 
        {
            return true;
        }
        public virtual void setBrush(string color)///Blue/Brown/Yellow/Green/Purple/Red/White
        {
            br = new SolidBrush(Color.FromName(color));
            this.color = color;
        }
        public virtual void setSelect(bool value)
        {
            Selected = value;
        }
        public virtual bool getSelect()
        {
            return Selected;
        }
        public virtual void print(Graphics gr)
        {

        }

        public virtual bool collision(Base p2)
        {
            Point leftup = new Point(x - sizecollision / 2, y - sizecollision / 2);
            Point rightup = new Point(x + sizecollision / 2, y - sizecollision / 2);
            Point leftdn = new Point(x - sizecollision / 2, y + sizecollision / 2);
            Point rightdn = new Point(x + sizecollision / 2, y + sizecollision / 2);

            if (p2 != this)
            {
                float size = p2.sizecollision / 2;
                if (((p2.x + size) > leftup.X) && ((p2.y + size) > leftup.Y) &&
                    ((p2.x - size) < rightup.X) && ((p2.y + size) > rightup.Y) &&
                    ((p2.x + size) > leftdn.X) && ((p2.y - size) < leftdn.Y) &&
                    ((p2.x - size) < rightdn.X) && ((p2.y - size) < rightdn.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool canMove(int x_, int y_, int width, int height, Mylist mylist)
        {
            if (getSlime())
            {
                bool flag=true;
                Observer observer = new Observer();
                
                flag= observer.canMoveIsSlime(this,x_,y_,width,height,mylist);
                if (!flag)
                {
                    x += x_;
                    y += y_;
                }
                
                return flag;
            }
            else
            {
                bool flag=true;
                for (int i = 0; i < mylist.getSize(); i++)
                {
                    Base p = mylist.getObj(i);
                    if (p.getCode() == 'L')
                    {
                        flag= canMove(x_,y_,width,height,(Mylist)p);
                        if (!flag)
                        {
                            break;
                        }
                    }
                    else 
                    {
                        x += x_;
                        y += y_;
                        flag = !collision(p);
                        x -= x_;
                        y -= y_;
                        if (!flag)
                        {
                            break;
                        }
                    }
                }
                return (flag);
            }
        }

        public virtual void move(int x_, int y_, int width, int height,Mylist mylist)
        {
            if (isSticked && slime)
            {
                return;
            }
            if (slime&&Selected&&!isSticked)
            {
                Observer observer = new Observer();
                observer.moveIsSlime(this, x_, y_, width, height, mylist);
            }
            x += x_;
            y += y_;
        }

        public virtual bool canScaled(int size, int width, int height, Mylist mylist)
        {
            
            bool flag = true;
            for (int i = 0; i < mylist.getSize(); i++)
            {
                if (mylist.getObj(i).getCode() == 'L')
                {
                    flag = mylist.getObj(i).canScaled(size, width, height, (Mylist)mylist.getObj(i));
                    if (!flag)
                    {
                        break;
                    }
                }
                else if (mylist.getObj(i)!=this)
                {
                    mylist.getObj(i).changeSize(size, width, height, mylist);

                    flag = !collision(mylist.getObj(i));

                    mylist.getObj(i).changeSize(-size, width, height, mylist);

                    if (!flag)
                    {
                        break;
                    }
                }
            }
            return (flag);
            
        }
        public virtual void changeSize(int size, int width, int height, Mylist mylist)
        {
        }
        public virtual void refreshSelected(Mylist mylist)
        {
            for (int j = 0; j < mylist.getSize(); j++)
            {
                mylist.getObj(j).Selected = false;
            }
        }
        public virtual void toSelect(bool isCTRL, Mylist mylist)
        {
            if (!isCTRL)
            {
                mylist.refreshSelected(mylist);
            }
            setSelect(true);
        }
        public virtual void deleteSelected(Mylist list)
        {
            if (Selected) list.deleteObj(this);
        }

        public virtual bool getSlime()
        {
            return slime;
        }

        public virtual void setSlime(bool sl)
        {
            slime = sl;
            if (slime)
            {
                mainpen = Pens.Cyan;
            }
            else
            {
                mainpen = Pens.Black;
            }
        }

        public virtual void save(string path)
        {

        }
        public virtual void load(string path, string[] tmp)
        {

        }
        public virtual string info()
        {
            return string.Empty;
        }
    }
    class MyBaseFactory
	{
        public MyBaseFactory() { }

		public Base createBase(Base p)
		{
			Base _base = null;
			switch (p.getCode())
			{
                case 'C':
					_base = new Circle((Circle)p);
					break;
                case 'R':
                    _base = new Rectangle((Rectangle)p);
                    break;
                case 'S':
                    _base = new Square((Square)p);
                    break;
                case 'T':
                    _base = new Triangle((Triangle)p);
                    break;
                case 'L':
                    _base = new Mylist((Mylist)p);
                    break;
                default:
                    break;
			}
			return _base;
		}
        public Base createBase(char code)
        {
            Base _base = null;
            switch (code)
            {
                case 'C':
                    _base = new Circle();
                    break;
                case 'R':
                    _base = new Rectangle();
                    break;
                case 'S':
                    _base = new Square();
                    break;
                case 'T':
                    _base = new Triangle();
                    break;
                case 'L':
                    _base = new Mylist();
                    break;
                default:
                    break;
            }
            return _base;
        }
    }

};
