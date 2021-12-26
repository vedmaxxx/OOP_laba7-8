using System;
using System.Drawing;
using System.Windows.Forms;


namespace OOP7
{
    public class Observer
    {
        public EventHandler observ;

        public void Update()
        {
            observ.Invoke(this, null);
        }

        public void toSlime(bool value, Mylist mylist)
        {
            for (int i = 0; i < mylist.getSize(); i++)
            {
                Base p = mylist.getObj(i);
                if (p.getCode() == 'L')
                {
                    toSlime(value, (Mylist)p);
                }
                else if (p.getSelect())
                {
                    p.setSlime(value);
                }
            }
        }

        public bool canMoveIsSlime(Base p, int x_, int y_, int width, int height, Mylist mylist)
        {
            bool flag = true;
            for (int i = 0; i < mylist.getSize(); i++)
            {
                Base p2 = mylist.getObj(i);
                if (p2.getCode() == 'L')
                {
                    flag = canMoveIsSlime(p, x_, y_, width, height, (Mylist)p2);
                    if (!flag) { break; }
                }
                else
                {
                    if (collision(p, p2))
                    {
                        p2.setIsSticked(true);
                        if (!canMoveNearSlime(p, p2, x_, y_, width, height, mylist))
                        {
                            p.x -= x_;
                            p.y -= y_;
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        p2.setIsSticked(false);
                    }
                }
            }
            return flag;
        }

        public void moveIsSlime(Base p, int x_, int y_, int width, int height, Mylist mylist)
        {
            if (!canMoveIsSlime(p,x_,y_,width,height, mylist)) { return; }
            for (int i = 0; i < mylist.getSize(); i++)
            {
                Base p2 = mylist.getObj(i);
                if (p2.getCode() != 'L')
                {
                    if (p2.getIsSticked())
                    {
                        p2.move(x_, y_, width, height, mylist);
                    }
                }
            }
        }

        public bool canMoveNearSlime(Base Slimeobj,Base p1, int x_, int y_, int width, int height, Mylist mylist)
        {
            //p - слайм
            bool flag = true;
            if (p1.getSlime())
            {
                return false;
            }
            p1.x += x_;
            p1.y += y_;
            if (!((p1.x + p1.sizecollision / 2 < width) && (p1.y + p1.sizecollision < height) && (p1.x - p1.sizecollision / 2 > 0) && (p1.y - p1.sizecollision / 2 > 0)))
            {
                p1.x -= x_;
                p1.y -= y_;
                return false;
            }
            p1.x -= x_;
            p1.y -= y_;
            for (int i = 0; i < mylist.getSize(); i++)
            {
                Base p2 = mylist.getObj(i);
                if (p2.getCode() == 'L')
                {
                    flag = canMoveNearSlime(Slimeobj, p1, x_, y_, width, height, ((Mylist)p2));
                    if (!flag) break;
                }
                else if (Slimeobj != p1 && Slimeobj!=p2)
                {
                    p1.x += x_;
                    p1.y += y_;

                    flag = !p1.collision(p2);
                    p1.x -= x_;
                    p1.y -= y_;
                    if (!flag) break;
                    
                }
            }
            return flag;
        }


        public bool collision(Base p1, Base p2)// Проверяет на коллизию объект 
        {
            float size1 = p1.sizecollision / 2;
            PointF leftup = new PointF(p1.x - size1, p1.y - size1);
            PointF rightup = new PointF(p1.x + size1, p1.y - size1);
            PointF leftdn = new PointF(p1.x - size1, p1.y + size1);
            PointF rightdn = new PointF(p1.x + size1, p1.y + size1);

            //p2 это объект из листа по индексу
            if (p2 != p1)
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
    }
}