using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace KCKC_2S
{
    public partial class Form1 : Form
    {

        UdpClient Server = new UdpClient(15000);

        string data = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            try
            {
                Server.BeginReceive(new AsyncCallback(recv), null);

            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message.ToString();
            }
        }

        void recv(IAsyncResult res)
        {
            IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 54000);
            byte[] received = Server.EndReceive(res, ref RemoteIP);
            data = ToString(received);
            //data = Encoding.UTF8.GetString(received).TrimEnd('\0');
            string[] splitedArray = { };
            splitedArray = GetSplitedData(data);
            string commandResult = parserCommand(splitedArray);
            this.Invoke(new MethodInvoker(delegate
            {
                richTextBox1.Text += "\nReceived command: " + commandResult;
            }));

            Server.BeginReceive(new AsyncCallback(recv), null);
        }

        public static string ToString(byte[] bytes)
        {
            string response = "";
            foreach (byte b in bytes)
            {   if (b == '\0') break;
                response += (Char)b;
            }
            return response;
        }
        public static string[] GetSplitedData(string command)
        {
            return command.Trim().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        }

        string parserCommand(string[] command)
        {
            Graphics field = pictureBox1.CreateGraphics();
            SolidBrush d, b;
            Rectangle figure;
            Pen pen;

            Int16 x0 = 0;
            Int16 y0 = 0;
            Int16 x1 = 0;
            Int16 y1 = 0;
            Int16 w = 0;
            Int16 h = 0;
            Int16 size = 0;
            string drawText = "";
            string font = "";
            Int16 colorR = 0;
            Int16 colorG = 0;
            Int16 colorB = 0;
            Int16 colR = 0;
            Int16 colG = 0;
            Int16 colB = 0;
            string errorMsg = "Неверный номер функции";
            string errorCountParams = "Недостаточно входных параметров";


            try
            {
                int funcNum = Convert.ToInt32(command[0]);
                switch (funcNum)
                {
                    case 1:
                        if (command.Length >= 4)
                        {
                            colorR = Convert.ToInt16(command[1]);
                            colorG = Convert.ToInt16(command[2]);
                            colorB = Convert.ToInt16(command[3]);
                            if (colorR > 255
                                    || colorG > 255
                                    || colorB > 255
                                    || colorR < 0
                                    || colorG < 0
                                    || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                               
                                field.Clear(Color.FromArgb(colorR, colorG, colorB));
                                return "Clear";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 2:
                        if (command.Length >= 6)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            colorR = Convert.ToInt16(command[3]);
                            colorG = Convert.ToInt16(command[4]);
                            colorB = Convert.ToInt16(command[5]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, 2, 2);
                                d = new SolidBrush(Color.FromArgb(colorR, colorG, colorB));
                                field.FillEllipse(d, figure);
                                return "Draw pixel";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 3:
                        if (command.Length >= 8)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            x1 = Convert.ToInt16(command[3]);
                            y1 = Convert.ToInt16(command[4]);
                            colorR = Convert.ToInt16(command[5]);
                            colorG = Convert.ToInt16(command[6]);
                            colorB = Convert.ToInt16(command[7]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || x1 > pictureBox1.Width
                                || y1 > pictureBox1.Height
                                || x1 < 0
                                || y1 < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                pen = new Pen(Color.FromArgb(colorR, colorG, colorB));
                                field.DrawLine(pen, x0, y0, x1, y1);
                                return "Draw line";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 4:
                        if (command.Length >= 8)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            h = Convert.ToInt16(command[4]);
                            colorR = Convert.ToInt16(command[5]);
                            colorG = Convert.ToInt16(command[6]);
                            colorB = Convert.ToInt16(command[7]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + h) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + h) < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, w, h);
                                pen = new Pen(Color.FromArgb(colorR, colorG, colorB));
                                field.DrawRectangle(pen, figure);
                                //gl.DrawRectangle(x0, y0, w, h, colorR, colorG, colorB);
                                return "Rectangle";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 5:
                        if (command.Length >= 8)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            h = Convert.ToInt16(command[4]);
                            colorR = Convert.ToInt16(command[5]);
                            colorG = Convert.ToInt16(command[6]);
                            colorB = Convert.ToInt16(command[7]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + h) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + h) < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                d = new SolidBrush(Color.FromArgb(colorR, colorG, colorB));
                                field.FillRectangle(d, x0, y0, w, h);
                                return "Fill Rectangle";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 6:
                        if (command.Length >= 8)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            h = Convert.ToInt16(command[4]);
                            colorR = Convert.ToInt16(command[5]);
                            colorG = Convert.ToInt16(command[6]);
                            colorB = Convert.ToInt16(command[7]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + h) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + h) < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, w, h);
                                pen = new Pen(Color.FromArgb(colorR, colorG, colorB));
                                field.DrawEllipse(pen, figure);
                                return "Draw ellipse";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 7:
                        if (command.Length >= 8)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            h = Convert.ToInt16(command[4]);
                            colorR = Convert.ToInt16(command[5]);
                            colorG = Convert.ToInt16(command[6]);
                            colorB = Convert.ToInt16(command[7]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || colorR > 255
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + h) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + h) < 0
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, w, h);
                                d = new SolidBrush(Color.FromArgb(colorR, colorG, colorB));
                                field.FillEllipse(d, figure);
                                return "Fill ellipse";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 8:
                        if (command.Length >= 7)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            colorR = Convert.ToInt16(command[4]);
                            colorG = Convert.ToInt16(command[5]);
                            colorB = Convert.ToInt16(command[6]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || colorR > 255
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + w) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + w) < 0
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, w, w);
                                pen = new Pen(Color.FromArgb(colorR, colorG, colorB));
                                field.DrawEllipse(pen, figure);
                                return "Draw circle";
                            }
                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 9:
                        if (command.Length >= 7)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            w = Convert.ToInt16(command[3]);
                            colorR = Convert.ToInt16(command[4]);
                            colorG = Convert.ToInt16(command[5]);
                            colorB = Convert.ToInt16(command[6]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || (x0 + w) > pictureBox1.Width
                                || (y0 + w) > pictureBox1.Height
                                || (x0 + w) < 0
                                || (y0 + w) < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                figure = new Rectangle(x0, y0, w, w);
                                d = new SolidBrush(Color.FromArgb(colorR, colorG, colorB));
                                field.FillEllipse(d, figure);
                                return "Fill circle";
                            }

                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 10:
                        if (command.Length >= 9)
                        {
                            x0 = Convert.ToInt16(command[1]);
                            y0 = Convert.ToInt16(command[2]);
                            drawText = command[3];
                            font = command[4];
                            size = Convert.ToInt16(command[5]);
                            colorR = Convert.ToInt16(command[6]);
                            colorG = Convert.ToInt16(command[7]);
                            colorB = Convert.ToInt16(command[8]);
                            colR = Convert.ToInt16(command[9]);
                            colG = Convert.ToInt16(command[10]);
                            colB = Convert.ToInt16(command[11]);
                            if (x0 > pictureBox1.Width
                                || y0 > pictureBox1.Height
                                || x0 < 0
                                || y0 < 0
                                || colorR > 255
                                || colorG > 255
                                || colorB > 255
                                || colorR < 0
                                || colorG < 0
                                || colorB < 0
                                || colR > 255
                                || colG > 255
                                || colB > 255
                                || colR < 0
                                || colG < 0
                                || colB < 0)
                            {
                                return "Параметры имеют недопустимые значения";
                            }
                            else
                            {
                                Font drawFont = new Font(font, size);

                                Size sizeOfText = TextRenderer.MeasureText(drawText, drawFont);
                                Rectangle rect = new Rectangle(new Point(x0, y0), sizeOfText);
                                b = new SolidBrush(Color.FromArgb(colR, colG, colB));
                                field.FillRectangle(b, rect);

                                d = new SolidBrush(Color.FromArgb(colorR, colorG, colorB));
                                field.DrawString(drawText, drawFont, d, x0, y0);
                                return "Draw text";
                            }

                        }
                        else
                        {
                            return errorCountParams;
                        }
                    case 11:
                        return "Ширина: " + pictureBox1.Width + "px";
                    case 12:
                        return "Высота: " + pictureBox1.Height + "px";

                    default:
                        return errorMsg;
                }
            }
            catch (FormatException e)
            {
                return e.Message;//"PARSING ERROR";
            }


        }
    }
}