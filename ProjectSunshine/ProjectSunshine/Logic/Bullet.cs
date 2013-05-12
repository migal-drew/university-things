using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class Bullet : SpaceObject
    {
        protected int m_dx; //Смещение пули по осям за 1 такт.
        protected int m_dy; //Возможность стрельбы под различными углами

        protected int m_damage;
        public int GetDamage
        {
            get
            {
                return m_damage;
            }
        }

        protected bool m_charge;
        public bool IsCharged
        {
            get
            {
                return m_charge;
            }
            set
            {
                m_charge = value;
            }
        }

        public Bullet()
        {
            m_charge = true;
            m_damage = 15;
            m_height = 6;
            m_width = 3;
            m_dx = 0;
            m_dy = 0;
        }

        /// <summary>
        /// Начальные координаты и смещение по осям
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void Discharge(int x, int y, int dx, int dy)
        {
            m_charge = false;
            m_x = x;
            m_y = y;
            m_dx = dx;
            m_dy = dy;
        }

        public virtual void Fly()
        {
            m_y += m_dy;
            m_x += m_dx;
        }
    }
}
