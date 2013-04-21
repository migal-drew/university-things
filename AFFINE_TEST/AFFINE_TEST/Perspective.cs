using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AFFINE_TEST
{
    public partial class Perspective : Form
    {
        //Figure
        List<Point> m_figure;

        //Parameters
        int m_1, m_2, m_3, m_4, m_5, m_6, m_7, m_8, m_9;

        public Perspective()
        {
            InitializeComponent();

            m_figure = new List<Point>();
            m_figure.Add(new Point(-200, -100));
            m_figure.Add(new Point(200, -100));
            m_figure.Add(new Point(200, 100));
            m_figure.Add(new Point(-200, 100));
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            m_figure = new List<Point>();
            m_figure.Add(new Point(-200, -100));
            m_figure.Add(new Point(200, -100));
            m_figure.Add(new Point(200, 100));
            m_figure.Add(new Point(-200, 100));

            double m_1, m_2, m_3, m_4, m_5, m_6, m_7, m_8, m_9;
            m_1 = double.Parse(this.textBox1.Text);
            m_2 = double.Parse(this.textBox2.Text);
            m_3 = double.Parse(this.textBox3.Text);
            m_4 = double.Parse(this.textBox4.Text);
            m_5 = double.Parse(this.textBox5.Text);
            m_6 = double.Parse(this.textBox6.Text);
            m_7 = double.Parse(this.textBox7.Text);
            m_8 = double.Parse(this.textBox8.Text);
            m_9 = double.Parse(this.textBox9.Text);

            for (int i = 0; i < m_figure.Count; i++)
            {
                int x = m_figure[i].X;
                int y = m_figure[i].Y;
                double z = (m_7 * x + m_8 * y + m_9);
                double x_n = (m_1 * x + m_2 * y ) / z;
                double y_n = (m_4 * x + m_5 * y ) / z;

                x_n += m_3;
                y_n += m_6;
                m_figure[i] = new Point((int)x_n, (int)y_n);
            }

            //m_figure = ShiftToCenter(m_figure, 400, 400);

            this.Invalidate();
        }

        private List<Point> ShiftToCenter(List<Point> points, int d_x, int d_y)
        {
            List<Point> res = new List<Point>();

            foreach (Point p in points)
                res.Add(new Point(p.X + d_x, p.Y + d_y));

            return res;
        }

        private void Perspective_Paint(object sender, PaintEventArgs e)
        {
            int cX = 400;
            int cY = 400;
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                new Point(cX, 0), new Point(cX, 800));
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                new Point(0, cY), new Point(1000, cY));

            e.Graphics.FillPolygon(SystemBrushes.ControlDarkDark, m_figure.ToArray());
        }


    }
}
