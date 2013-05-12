using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Bonuses
{
    public abstract class Bonus : SpaceObject
    {
        protected int m_counter;
        public int GetCount
        {
            get
            {
                return m_counter;
            }
        }

        protected bool m_isActivated;
        public bool IsActivated
        {
            get
            {
                return m_isActivated;
            }
        }

        protected int m_needForDrop;
        public int GetNeedForDrop
        {
            get
            {
                return m_needForDrop;
            }
        }

        public Bonus()
        {
            m_width = 30;
            m_height = 30;
            m_counter = 0;
        }

        public abstract void Improve(Ship s);

        public void Count()
        {
            if (m_isActivated == false)
            {
                m_counter++;
                if (m_counter > 50)
                    m_counter = 0;
            }
        }

        public virtual void Fall()
        {
            m_y += 4;
        }

        public void Drop(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public void Activate()
        {
            m_counter = 0;
            m_isActivated = true;
        }

        public void Deactivate()
        {
            m_isActivated = false;
        }
    }
}
