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
    public partial class Form1 : Form
    {
        static int PRMS_SIZE = 7;
        static int size = 5;
        int cX = 400;
        int cY = 400;

        Point[] figure_1;
        Point[] figure_1_for_paint;
        Point[] transformedFigure_1;
        double[] prms_1;
        Point[] figure_2;
        Point[] figure_2_for_paint;
        Point[] transformedFigure_2;
        double[] prms_2;

        public Form1()
        {
            InitializeComponent();

            figure_1 = new Point[size];
            figure_1_for_paint = new Point[size];
            figure_2 = new Point[size];
            figure_2_for_paint = new Point[size];
            prms_1 = new double[PRMS_SIZE];
            prms_2 = new double[PRMS_SIZE];
            transformedFigure_1 = new Point[size];
            transformedFigure_2 = new Point[size];

            /**
            triangle[0] = new Point(-50, 50);
            triangle[1] = new Point(50, 50);
            triangle[2] = new Point(50, -50);
            triangle[3] = new Point(-50, -50);
            */
            /*
            triangle[0] = new Point(50, 150);
            triangle[1] = new Point(150, 150);
            triangle[2] = new Point(150, 50);
            triangle[3] = new Point(50, 50);
             * */
            /*
            figure_1[0] = new Point(-50, -50);
            figure_1[1] = new Point(50, -50);
            figure_1[2] = new Point(50, 50);
            figure_1[3] = new Point(0, 100);
            figure_1[4] = new Point(-50, 50);
             * */
        }

        private double[,] Dot(double[,] m1, double[,] m2)
        {
            int m = m1.GetLength(1);
            int n =  m2.GetLength(1);
            double[,] res = new double[m, n];

            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < m; k++) // OR k<b.GetLength(0)
                        res[i, j] = res[i, j] + m1[i, k] * m2[k, j];
                }
            }
            return res;
        }

        private double[,] ComposeAffineMatrix(double[] prms)
        {
            double tmp_x = 0;
            double tmp_y = 0;

            double[,] s_sk = new double[2, 2];
            double s_x = prms[1];
            double sk_y = prms[4];
            double sk_x = prms[3];
            double s_y = prms[2];
            s_sk[0, 0] = prms[1];
            s_sk[0, 1] = prms[4];
            s_sk[1, 0] = prms[3];
            s_sk[1, 1] = prms[2];

            double alpha = prms[0];
            double[,] rot = new double[2, 2];
            rot[0, 0] = Math.Cos(alpha);
            rot[0, 1] = (-1) * Math.Sin(alpha);
            rot[1, 0] = Math.Sin(alpha);
            rot[1, 1] = Math.Cos(alpha);

            double[] transl = new double[2];
            transl[0] = prms[5];
            transl[1] = prms[6];

            double[,] matrix = Dot(s_sk, rot);
            double[,] res = new double[3, 3];
            res[0, 0] = matrix[0, 0];
            res[0, 1] = matrix[0, 1];
            res[1, 0] = matrix[1, 0];
            res[1, 1] = matrix[1, 1];

            //res[0, 0] = s_x * Math.Cos(alpha) + sk_y * Math.Sin(alpha);
            //res[0, 1] = -s_x * Math.Sin(alpha) + sk_y * Math.Cos(alpha);
            //res[1, 0] = sk_x * Math.Cos(alpha) + s_y * Math.Sin(alpha);
            //res[1, 1] = -sk_x * Math.Sin(alpha) + s_y * Math.Cos(alpha);

            res[0, 2] = -transl[0];
            res[1, 2] = -transl[1];
            res[2, 2] = 1;

            return res;
        }

        private double[,] AddOne(Point p)
        {
            double[,] res = new double[3, 1];
            res[0, 0] = p.X;
            res[1, 0] = p.Y;
            res[2, 0] = 1;

            return res;
        }

        private Point transformPoint(double[] prms, Point p)
        {
            double x = p.X;
            double y = p.Y;

            double tmp_x = 0;
            double tmp_y = 0;
            
            double[,] s_sk = new double[2, 2];
            s_sk[0, 0] = prms[1];
            s_sk[0, 1] = prms[4];
            s_sk[1, 0] = prms[3];
            s_sk[1, 1] = prms[2];

            double alpha = prms[0];
            double[,] rot = new double[2, 2];
            rot[0, 0] = Math.Cos(alpha);
            rot[0, 1] = (-1)*Math.Sin(alpha);
            rot[1, 0] = Math.Sin(alpha);
            rot[1, 1] = Math.Cos(alpha);

            double[] transl = new double[2];
            transl[0] = prms[5];
            transl[1] = prms[6];

            //Scaling && Skewing
            tmp_x = (int)(s_sk[0, 0] * x + s_sk[0, 1] * y);
            tmp_y = (int)(s_sk[1, 0] * x + s_sk[1, 1] * y);
            x = tmp_x;
            y = tmp_y;

            //Rotation
            tmp_x = (int)(rot[0, 0] * x + rot[0, 1] * y);
            tmp_y = (int)(rot[1, 0] * x + rot[1, 1] * y);
            x = tmp_x;
            y = tmp_y;

            //Translation
            x -= transl[0];
            y -= transl[1];

            return new Point((int)x, (int)y);
        }

        private Point NewTransformPoint(double[,] matrix, Point p)
        {
            double[,] po = AddOne(p);
            double[,] res = Dot(matrix, po);
            return new Point((int)res[0, 0], (int)res[1, 0]);
        }
        
        private List<double> ParsePointsFromString(String s)
        {
            char[] sub = new char[32];
            List<double> points = new List<double>();

            int c = 0;
            double parsedValue = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ',')
                {
                    for (int j = 0; j < sub.Length; j++)
                    {
                        if (sub[j] == '.')
                            sub[j] = ',';
                    }
                    String test = new String(sub);
                    if (!double.TryParse(test, out parsedValue))
                        MessageBox.Show("Parse fail! Use dots, Luke!");

                    points.Add(parsedValue);
                    sub = new char[32];
                    c = 0;
                }
                else
                {
                    sub[c++] += s[i];
                }
            }

            return points;
        }
        
        private void calc_Click(object sender, EventArgs e)
        {
            ReadAllParameters();
            //Transform points
            //for (int i = 0; i < size; i++)
            //{
            //    transformedFigure_1[i] = transformPoint(prms_1, figure_1[i]);
            //    transformedFigure_2[i] = transformPoint(prms_2, figure_2[i]);

            //    transformedFigure_1[i].X += cX;
            //    transformedFigure_1[i].Y += cY;
            //    transformedFigure_2[i].X += cX;
            //    transformedFigure_2[i].Y += cY;
            //}

            double[,] m1 = ComposeAffineMatrix(prms_1);
            double[,] m2 = ComposeAffineMatrix(prms_2);
            for (int i = 0; i < size; i++)
            {
                transformedFigure_1[i] = NewTransformPoint(m1, figure_1[i]);
                transformedFigure_2[i] = NewTransformPoint(m2, figure_2[i]);

                transformedFigure_1[i].X += cX;
                transformedFigure_1[i].Y += cY;
                transformedFigure_2[i].X += cX;
                transformedFigure_2[i].Y += cY;
            }

            double[,] a = new double[3, 3];
            a[0, 0] = 1;
            a[0, 1] = 2;
            a[0, 2] = 3;
            a[1, 0] = 4;
            a[1, 1] = 5;
            a[1, 2] = 6;
            a[2, 0] = 0;
            a[2, 1] = 0;
            a[2, 2] = 1;
            double[,] b = new double[3, 1];
            b[0, 0] = -3;
            b[1, 0] = 2;
            b[2, 0] = 1;
            double[,] r = Dot(a, b);

            this.Invalidate();
            /*
            string s = "";
            foreach (Point p in figure_1)
            {
                s += p.X - cX + " : " + (p.Y - cY) + "\r\n";
            }
            MessageBox.Show(s);
             * */
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawFillPolygon(new Pen(SystemBrushes.GrayText), triangle);

            Point[] tmp_1 = new Point[1];
            Point[] tmp_2 = new Point[1];

            if (radioButton1.Checked)
            {
                tmp_1 = figure_1_for_paint;
                tmp_2 = figure_2_for_paint;
            }
            if (radioButton2.Checked)
            {
                tmp_1 = transformedFigure_1;
                tmp_2 = transformedFigure_2;
            }
            if (radioButton3.Checked)
            {
                tmp_1 = transformedFigure_1;
            }
            if (radioButton4.Checked)
            {
                tmp_2 = transformedFigure_2;
            }

            e.Graphics.FillPolygon(SystemBrushes.GrayText, tmp_1);
            e.Graphics.FillPolygon(SystemBrushes.Highlight, tmp_2);

            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                new Point(cX, 0), new Point(cX, 800));
            e.Graphics.DrawLine(new Pen(SystemBrushes.ControlDark),
                new Point(0, cY), new Point(1000, cY));
        }

        private void ReadAllParameters()
        {
            String s = txtBoxMain.Text;
            List<String> arrays = new List<String>();
            int beginIndex = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\r')
                {
                    arrays.Add(new String(s.ToArray(), beginIndex, i - beginIndex));
                    i += 2;
                    beginIndex = i;
                }

                if (i == s.Length - 1)
                {
                    arrays.Add(new String(s.ToArray(), beginIndex, i - beginIndex + 1));
                    //i+=2;
                    //beginIndex = i;
                }
            }

            List<double> p_1 = ParsePointsFromString(arrays[0]);
            for (int i = 0, j = 0; i < p_1.Count; i+=2)
            {
                figure_1[j++] = new Point((int)p_1[i], (int)p_1[i + 1]);
            }
            List<double> p_2 = ParsePointsFromString(arrays[1]);
            for (int i = 0, j = 0; i < p_1.Count; i+=2)
            {
                figure_2[j++] = new Point((int)p_2[i], (int)p_2[i + 1]);
            }

            prms_1 = ParsePointsFromString(arrays[2]).ToArray();
            prms_2 = ParsePointsFromString(arrays[3]).ToArray();

            for (int i = 0; i < figure_1.Length; i++)
            {
                figure_1_for_paint[i].X = figure_1[i].X + cX;
                figure_1_for_paint[i].Y = figure_1[i].Y + cY;

                figure_2_for_paint[i].X = figure_2[i].X + cX;
                figure_2_for_paint[i].Y = figure_2[i].Y + cY;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = e.X.ToString() + " : " + e.Y.ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Perspective p = new Perspective();
            p.Show();
        }

    }


}
