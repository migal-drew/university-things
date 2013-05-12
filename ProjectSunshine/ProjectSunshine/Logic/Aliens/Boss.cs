using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class Boss : Alien
    {
        private bool m_attack;
        private DateTime m_time;

        public Boss(int x, int y, Direction position)
            : base(x, y, 200, 200, position)
        {
            m_cash = 100;
            m_speed = 2;
            m_health = 1000;
            m_attack = false;
            m_time = DateTime.Now;
        }

        public override void Move(int width, int height)
        {
            //m_points[0].X = m_x - m_width / 2;
            //m_points[0].Y = m_y - m_height / 2;
            //m_points[1].X = m_x + m_width / 2;
            //m_points[1].Y = m_y - m_height / 2;
            //m_points[2].X = m_x + m_width / 2;
            //m_points[2].Y = m_y + m_height / 2;
            //m_points[3].X = m_x - m_width / 2;
            //m_points[3].Y = m_y + m_height / 2;

            if (m_attack)
            {
                m_y += 8;
                if (m_y + m_height / 2 > height)
                    m_attack = false;

                return;
            }
            else
            {
                if (m_y > height / 4)
                {
                    m_y -= 3;
                }
                if (DateTime.Now.Subtract(m_time).TotalSeconds > 20)
                {
                    m_attack = true;
                    m_time = DateTime.Now;
                }


                if (m_y < height / 5)
                {
                    m_y++;
                    return;
                }

                if (m_x + GetWidth / 2 > width - 10)
                    m_direction = Direction.Left;
                if (m_x - GetWidth / 2 < 10)
                    m_direction = Direction.Right;

                if (m_direction == Direction.Right)
                {
                    m_x += 3;
                }
                else
                {
                    m_x -= 3;
                }
            }
            
        }

        public override bool Shoot(DateTime now, List<Bullet> bullets)
        {
            //return base.Shoot(now, bullets);
            if (now.Subtract(m_lastShot).Seconds > 2)
            {
                bullets.Add(new EnemyBullet());
                bullets[bullets.Count - 1].Discharge(m_x - m_width / 3, m_y + m_height / 2, -1, 5);
                bullets.Add(new EnemyBullet());
                bullets[bullets.Count - 1].Discharge(m_x + m_width / 3, m_y + m_height / 2, 1, 5);
                bullets.Add(new EnemyBullet());
                bullets[bullets.Count - 1].Discharge(m_x, m_y + m_height / 2, 0, 5);
                m_lastShot = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
