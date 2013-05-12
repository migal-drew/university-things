using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class Alien : SpaceObject
    {
        protected Random m_rnd;

        protected int m_speed;

        protected int m_cash;
        public int GetCash
        {
            get
            {
                return m_cash;
            }
        }

        protected int m_health;
        public int GetHealth
        {
            get
            {
                return m_health;
            }
        }

        protected DateTime m_lastShot;

        protected int m_reloadTime;

        protected Direction m_direction;

        protected SpacePoint[] m_points;
        public SpacePoint[] GetPoints
        {
            get
            {
                return m_points;
            }
        }

        protected DateTime m_explosion;
        public DateTime TimeOfExplosion
        {
            get
            {
                return m_explosion;
            }
        }

        public Alien(int x, int y, int width, int height, Direction position)
        {
            m_rnd = new Random();

            m_speed = 4;

            //Определение поведения корабля,
            //его прямой полет либо полет со стрейфом
            //после достижения опереденного расстояния
            if (position == Direction.Center)
            {
                m_direction = (Direction)m_rnd.Next(3);
            }
            else
            {
                if (position == Direction.Left)
                    m_direction = (Direction)m_rnd.Next(1,2);
                if (position == Direction.Right)
                    m_direction = (Direction)m_rnd.Next(2);
            }
            m_health = 100;
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_lastShot = DateTime.Now;

            m_points = new SpacePoint[4];
            m_points[0] = new SpacePoint(m_x - m_width / 2, m_y - m_height / 2);
            m_points[1] = new SpacePoint(m_x + m_width / 2, m_y - m_height / 2);
            m_points[2] = new SpacePoint(m_x + m_width / 2, m_y + m_height / 2);
            m_points[3] = new SpacePoint(m_x - m_width / 2, m_y + m_height / 2);
        }

        public virtual bool Shoot(DateTime now, List<Bullet> bullets)
        {
            if (now.Subtract(m_lastShot).Seconds > 2)
            {
                EnemyBullet b = new EnemyBullet();
                b.Discharge(m_x, m_y + m_height / 2, 0, 5);
                bullets.Add(b);
                m_lastShot = DateTime.Now;
                return true;
            }
            return false;
        }

        public virtual void Move(int width, int height)
        {
            //m_points[0].X = m_x - m_width / 2;
            //m_points[0].Y = m_y - m_height / 2;
            //m_points[1].X = m_x + m_width / 2;
            //m_points[1].Y = m_y - m_height / 2;
            //m_points[2].X = m_x + m_width / 2;
            //m_points[2].Y = m_y + m_height / 2;
            //m_points[3].X = m_x - m_width / 2;
            //m_points[3].Y = m_y + m_height / 2;

            m_y += m_speed;
            if (m_y >= height / 3)
            {
                if (m_direction == Direction.Left)
                {
                    m_x -= m_speed;
                    return;
                }
                if (m_direction == Direction.Right)
                {
                    m_x += m_speed;
                    return;
                }
            }
        }

        public void Damaged(Bullet b, DateTime now)
        {
            m_health -= b.GetDamage;
            if (m_health < 0)
            {
                m_explosion = now;
                m_health = 0;
            }
        }

        public void Stop()
        {
            m_speed = 0;
        }
    }
}
