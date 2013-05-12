using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ProjectSunshine.Drawing
{
    using Logic.Aliens;
    using Logic;
    using Logic.Bonuses;
    
    /// <summary>
    /// Содержит спрайты объектов. Загрузка в память спрайтов происходит
    /// при инициализации класса.
    /// </summary>
    public class MyDrawing  
    {
        //DateTime second;

        //Сделать спрайты статическими полями
        Graphics m_graphic;

        Bitmap[] m_btmpShip;

        //Enemies
        Bitmap m_aircraft;
        Bitmap m_fighter;
        Bitmap m_heavyfighter;
        Bitmap m_gunboat;
        Bitmap m_bomber;
        Bitmap m_armoredbomber;
        Bitmap m_boss;

        Bitmap m_enemy;

        Bitmap[] m_backgrounds;
        //Bitmap m_back1;
        //Bitmap m_back2;
        //Bitmap m_back3;

        Bitmap m_explosion;

        Bitmap m_laser;
        Bitmap m_enemylaser;

        //Бонусы
        Bitmap m_battery;
        Bitmap m_spanner;
        Bitmap m_enhanceWeapon;
        Bitmap m_life;
        Bitmap m_invulnerability;

        SolidBrush m_br;
        SolidBrush m_brRed;
        SolidBrush m_brBlue;

        Bitmap m_shield;

        Bitmap[] m_lifes;

        Bitmap[] m_health;

        Bitmap m_kitty;

        Bitmap m_shipShield;
        Bitmap m_shipHealth;

        private int m_width;
        private int m_height;

        Font m_font;
        Font m_bigFont;

        //Для фона
        int m_x;
        int m_y;

        /// <summary>
        /// Создание класса и загрузка спрайтов в память
        /// </summary>
        /// <param name="g"></param>
        public MyDrawing(Graphics g, int width, int height)
        {
            m_width = width;
            m_height = height;

            m_graphic = g;
            m_br = new SolidBrush(Color.LightBlue);
            m_brBlue = new SolidBrush(Color.Cyan);
            m_brRed = new SolidBrush(Color.Red);

            m_btmpShip = new Bitmap[3];
            m_btmpShip[0] = new Bitmap(new Bitmap("Ship\\Ship.png"), new Size(40, 68));
            m_btmpShip[1] = new Bitmap(new Bitmap("Ship\\ShipLeft.png"), new Size(40, 68));
            m_btmpShip[2] = new Bitmap(new Bitmap("Ship\\ShipRight.png"), new Size(40, 68));

            m_lifes = new Bitmap[4];
            m_lifes[0] = new Bitmap(new Bitmap("Interface\\lifeBattery0.png"), new Size(40, 50));
            m_lifes[1] = new Bitmap(new Bitmap("Interface\\lifeBattery1.png"), new Size(40, 50));
            m_lifes[2] = new Bitmap(new Bitmap("Interface\\lifeBattery2.png"), new Size(40, 50));
            m_lifes[3] = new Bitmap(new Bitmap("Interface\\lifeBattery3.png"), new Size(40, 50));

            m_health = new Bitmap[4];
            m_health[0] = new Bitmap(new Bitmap("Interface\\low.png"), new Size(70, 70));
            m_health[1] = new Bitmap(new Bitmap("Interface\\medium.png"), new Size(70, 70));
            m_health[2] = new Bitmap(new Bitmap("Interface\\high.png"), new Size(70, 70));
            m_health[3] = new Bitmap(new Bitmap("Interface\\angry.png"), new Size(70, 70));

            m_kitty = new Bitmap(new Bitmap("Interface\\kitty.png"), new Size(70, 70));

            m_aircraft = new Bitmap(new Bitmap("Enemies\\aircraft.png"), new Size(40, 60));
            m_fighter = new Bitmap(new Bitmap("Enemies\\fighter.png"), new Size(60, 60));
            m_heavyfighter = new Bitmap(new Bitmap("Enemies\\heavyfighter.png"), new Size(40, 60));
            m_gunboat = new Bitmap(new Bitmap("Enemies\\gunboat.png"), new Size(40, 60));
            m_bomber = new Bitmap(new Bitmap("Enemies\\bomber.png"), new Size(60, 60));
            m_armoredbomber = new Bitmap(new Bitmap("Enemies\\armoredbomber.png"), new Size(60, 60));
            m_boss = new Bitmap(new Bitmap("Enemies\\boss1.png"), new Size(200, 200));

            m_enemy = new Bitmap(new Bitmap("Enemies\\boss1.png"), new Size(70, 70));
            
            m_explosion = new Bitmap(new Bitmap("Weapons\\explosion.png"), new Size(100, 100));

            m_laser = new Bitmap(new Bitmap("Weapons\\laser.png"), new Size(2, 8));
            m_enemylaser = new Bitmap(new Bitmap("Weapons\\enemylaser.png"), new Size(2, 8));

            m_battery = new Bitmap(new Bitmap("Bonuses\\battery.png"), new Size(30, 30));
            m_spanner = new Bitmap(new Bitmap("Bonuses\\spanner.png"), new Size(30, 30));
            m_enhanceWeapon = new Bitmap(new Bitmap("Bonuses\\enhance.png"), new Size(30, 30));
            m_life = new Bitmap(new Bitmap("Bonuses\\life.png"), new Size(30, 30));
            m_invulnerability =
                new Bitmap(new Bitmap("Bonuses\\invulnerability.png"), new Size(30, 30));

            m_shield = new Bitmap(new Bitmap("Ship\\shield.png"), new Size(68, 68));

            m_shipShield = new Bitmap(new Bitmap("Interface\\shipShield.png"), new Size(35, 35));
            m_shipHealth = new Bitmap(new Bitmap("Interface\\shipHealth.png"), new Size(35, 35));
            m_font = new Font("Comic Sans MS", 10);
            m_bigFont = new Font("Arial", 20);

            m_backgrounds = new Bitmap[3];
            m_backgrounds[0] = new Bitmap(new Bitmap("Backgrounds\\back1.jpg"), new Size(width, height * 2));
            m_backgrounds[1] = new Bitmap(new Bitmap("Backgrounds\\back2.jpg"), new Size(width, height * 2));
            m_backgrounds[2] = new Bitmap(new Bitmap("Backgrounds\\back3.jpg"), new Size(width, height * 2));

            m_x = 0;
            m_y = -height;
        }

        #region Private methods
        private void Bullet(Bullet b)
        {
            //m_graphic.FillRectangle(br, b.Xpoint - b.GetWidth / 2, 
            //    b.Ypoint - b.GetHeight / 2, b.GetWidth, b.GetHeight);
            if (b is EnemyBullet)
                m_graphic.DrawImage(m_enemylaser, b.Xpoint - b.GetWidth / 2, b.Ypoint - b.GetHeight / 2);
            else
                m_graphic.DrawImage(m_laser, b.Xpoint - b.GetWidth / 2, b.Ypoint - b.GetHeight / 2);
        }

        private void Alien(Alien a)
        {
            if (a.GetHealth == 0)
            {
                int size = a.GetHeight / 2;
                m_graphic.DrawImage(m_explosion, a.Xpoint - a.GetWidth / 4,
                    a.Ypoint - a.GetHeight / 4, size, size);
            }
            else
            {
                if (a is Boss)
                {
                    m_graphic.DrawImage(m_boss, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
                if (a is Aircraft)
                {
                    m_graphic.DrawImage(m_aircraft, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
                if (a is Fighter)
                {
                    m_graphic.DrawImage(m_fighter, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2 //a.GetWidth, a.GetHeight);
                        );
                    return;
                }
                if (a is HeavyFighter)
                {
                    m_graphic.DrawImage(m_heavyfighter, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
                if (a is GunBoat)
                {
                    m_graphic.DrawImage(m_gunboat, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
                if (a is Bomber)
                {
                    m_graphic.DrawImage(m_bomber, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
                if (a is ArmoredBomber)
                {
                    m_graphic.DrawImage(m_armoredbomber, a.Xpoint - a.GetWidth / 2,
                        a.Ypoint - a.GetHeight / 2);
                    return;
                }
            }
          
            //Point[] points = new Point[4];
            //points[0].X = a.GetPoints[0].X;
            //points[0].Y = a.GetPoints[0].Y;
            //points[1].X = a.GetPoints[1].X;
            //points[1].Y = a.GetPoints[1].Y;
            //points[2].X = a.GetPoints[2].X;
            //points[2].Y = a.GetPoints[2].Y;
            //points[3].X = a.GetPoints[3].X;
            //points[3].Y = a.GetPoints[3].Y;

            //m_graphic.FillPolygon(m_br, points);
        }

        #endregion

        public void Menu(int width, int height)
        {
            m_graphic.DrawImage(new Bitmap(
                new Bitmap("Interface\\menu.jpg"), new Size(width, height)), 0, 0);
        }

        public void About(int width, int height)
        {
            m_graphic.DrawImage(new Bitmap(
                new Bitmap("Interface\\about.jpg"), new Size(width, height)), 0, 0);
        }

        public void GameOver(int width, int height)
        {
            m_graphic.DrawString("GAME OVER\r\nнажмите Esc",
                    m_bigFont, m_br, width / 2 - 80, height / 2 - 50);
        }

        public void Ship(Ship ship)
        {
            m_graphic.DrawImage(m_btmpShip[ship.GetDirection],
                ship.Xpoint - ship.GetWidth / 2, ship.Ypoint - ship.GetHeight / 2);

            if (ship.IsInvulnerable)
                m_graphic.DrawImage(m_shield,
                ship.Xpoint - m_shield.Width / 2, ship.Ypoint - m_shield.Height / 2);

            int width = 2;
            int interval = 2;

            //int height = (int)(ship.GetHeight / 100.0 * ship.GetShield) / 3;
            //int x = ship.Xpoint - ship.GetWidth / 2 - interval - width;
            //int y = ship.Ypoint + ship.GetHeight / 2 - height;
            //m_graphic.FillRectangle(m_brBlue, x, y, width, height);

            //height = (int)(ship.GetHeight / 100.0 * ship.GetArmor) / 3;
            //x = ship.Xpoint + ship.GetWidth / 2 + interval;
            //y = ship.Ypoint + ship.GetHeight / 2 - height;
            //m_graphic.FillRectangle(m_brRed, x, y, width, height);



            m_graphic.FillRectangle(m_brRed, ship.Xpoint - ship.GetWidth / 2,
                ship.Ypoint + ship.GetHeight / 2,
                (int)(ship.GetWidth / 100.0 * ship.GetHeat)
                , width);
        }

        private void Status(Ship s)
        {
            int w = m_width / 5;
            int h = m_height - 50;
            m_graphic.DrawImage(m_shipShield, w - m_shipShield.Width, h);
            m_graphic.DrawString(s.GetShield.ToString() + "%", m_font, m_br,
                w, h + 20);
            m_graphic.DrawImage(m_shipHealth, m_width - w, h);
            m_graphic.DrawString(s.GetArmor.ToString() + "%", m_font, m_br, 
                m_width - w - m_shipShield.Width, h + 20);
        }

        private void Kitty(int x, int y, int level)
        {
            m_graphic.DrawImage(m_kitty, x, y);
            m_graphic.DrawString("Level " + level.ToString(), m_font, m_br, x, m_kitty.Height);
        }

        private void Health(Ship s, int x, int y)
        {
            if (s.IsInvulnerable)
            {
                m_graphic.DrawImage(m_health[3], x, y);
            }
            else
            {
                if (s.GetArmor + s.GetShield >= 140)
                {
                    m_graphic.DrawImage(m_health[2], x, y);
                    return;
                }
                if (s.GetArmor + s.GetShield >= 80)
                {
                    m_graphic.DrawImage(m_health[1], x, y);
                    return;
                }
                if (s.GetArmor + s.GetShield >= 0)
                {
                    m_graphic.DrawImage(m_health[0], x, y);
                    return;
                }
            }
        }

        private void Life(Ship s, int x, int y)
        {
            m_graphic.DrawImage(m_lifes[s.GetLifes], x, y);
        }

        public void Bullets(List<Bullet> bullets)
        {
            foreach (Bullet b in bullets)
            {
                if (b != null && !b.IsCharged)
                {
                    this.Bullet(b);
                }
            }
        }

        public void Bonus(Bonus b)
        {
            if (b is Battery)
            {
                m_graphic.DrawImage(m_battery, b.Xpoint - b.GetWidth / 2,
                    b.Ypoint - b.GetHeight / 2);
                return;
            }
            if (b is Spanner)
            {
                m_graphic.DrawImage(m_spanner, b.Xpoint - b.GetWidth / 2,
                    b.Ypoint - b.GetHeight / 2);
                return;
            }
            if (b is EnhanceWeapon)
            {
                m_graphic.DrawImage(m_enhanceWeapon, b.Xpoint - b.GetWidth / 2,
                    b.Ypoint - b.GetHeight / 2);
                return;
            }
            if (b is Life)
            {
                m_graphic.DrawImage(m_life, b.Xpoint - b.GetWidth / 2,
                    b.Ypoint - b.GetHeight / 2);
                return;
            }
            if (b is Invulnerability)
            {
                m_graphic.DrawImage(m_invulnerability, b.Xpoint - b.GetWidth / 2,
                    b.Ypoint - b.GetHeight / 2);
                return;
            }
        }

        public void Bonuses(List<Bonus> bonuses)
        {
            foreach (Bonus b in bonuses)
                if (b.IsActivated)
                    this.Bonus(b);
        }

        public void Cash(int cash, int x, int y)
        {
            m_graphic.DrawString(cash.ToString(), m_font,
               m_brBlue, x, y);
        }

        public void Background(int height, int type)
        {
            m_graphic.DrawImage(m_backgrounds[type], new Point(0, m_y++));
            if (m_y >= 0)
                m_y = -height;
        }

        public void Space(Space space)
        {
            Background(space.GetHeight, space.Type);
            Aliens(space.GetAliens);
            Ship(space.GetShip);
            Bullets(space.GetShip.GetAmmunition);
            Bullets(space.GetEnemyBullets);
            Bonus(space.GetBonus);
            Life(space.GetShip, space.GetWidth - 40, space.GetHeight - 60);
            Cash(space.GetShip.GetCash, 10, 10);
            Health(space.GetShip, 5, space.GetHeight - 70);
            Kitty(space.GetWidth - 65, 0, space.GetLevel);
            Status(space.GetShip);

            if (space.Loading)
                m_graphic.DrawString("Level " + space.GetLevel.ToString(),
                    m_bigFont, m_br, space.GetWidth / 2 - 50, space.GetHeight / 2);
        }

        public void Aliens(List<Alien> aliens)
        {
            foreach (Alien a in aliens)
            {
                Alien(a);
            }
        }
    }
}
