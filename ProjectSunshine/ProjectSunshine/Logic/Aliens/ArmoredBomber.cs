using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class ArmoredBomber : Alien
    {
        public ArmoredBomber(int x, int y, Direction position)
            : base(x, y, 60, 60, position)
        {
            m_cash = 100;
            m_speed = 2;
            m_health = 150;
        }

        public override bool Shoot(DateTime now, List<Bullet> bullets)
        {
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
