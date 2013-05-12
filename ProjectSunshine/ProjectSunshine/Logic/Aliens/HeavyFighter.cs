using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic.Aliens
{
    public class HeavyFighter : Alien
    {
        public HeavyFighter(int x, int y, Direction position)
            : base(x, y, 40, 60, position)
        {
            m_cash = 100;
            m_speed = 2;
            m_health = 100;
        }

        public override bool Shoot(DateTime now, List<Bullet> bullets)
        {
            if (now.Subtract(m_lastShot).Seconds > 2)
            {
                EnemyBullet b = new EnemyBullet();
                b.Discharge(m_x - m_width / 3, m_y + m_height / 2, -1, 5);
                bullets.Add(b);
                bullets.Add(new EnemyBullet());
                bullets[bullets.Count - 1].Discharge(m_x + m_width / 3, m_y + m_height / 2, 1, 5);
                m_lastShot = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
