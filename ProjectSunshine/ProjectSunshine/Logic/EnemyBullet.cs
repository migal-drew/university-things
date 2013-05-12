using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSunshine.Logic
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet() 
            : base()
        {
            m_damage = 30;
        }

        //public override void Fly()
        //{
        //    m_y += 3; 
        //}
    }
}
