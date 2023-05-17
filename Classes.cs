using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using StorageClassLibrary;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace _6th_LAB_OOP
{
    public class Designer // Класс, отвечающий за отрисовку и получения изображения в bitmap (растровое изображение)
    {
        private Bitmap bitmap; // Растровое изображение
        private Pen blackPen; // Ручка для рисования черным цветом
        private Pen redPen; // Ручка для рисования черным цветом
        private Brush brush; // Кисточка для заливки фигур цветом
        private Graphics g; // Класс, предоставляющий методы для рисования объектов
        private int height, width; // Храним высоту и ширину изображения

        public Designer(int width, int height) // Конструктор
        {
            this.width = width; this.height = height; // Будет нужно для ограничения в движении фигур
            bitmap = new Bitmap(width, height); // Определяем растровое изображение
            g = Graphics.FromImage(bitmap); // Определяем класс, отвечающий за рисование
            blackPen = new Pen(Color.Black); blackPen.Width = 2; // Определяем черную ручку
            redPen = new Pen(Color.Red); redPen.Width = 2; // Определяем красную ручку
            brush = new SolidBrush(Color.White);
        }

        public int getHeight() { return height; }

        public int getWidth() { return width; }

        public Bitmap GetBitmap() // Получить растровое изображение
        {
            return bitmap;
        }

        public void Clear() // Очистить изображение
        {
            g.Clear(Color.White);
        }

        public void DrawCircle(int x, int y, int radius, bool is_selected, Color color) // Нарисовать окружность 
        {
            g.DrawEllipse(((is_selected) ? redPen : blackPen), (x - radius), (y - radius), 2 * radius, 2 * radius);
            brush = new SolidBrush(color);
            g.FillEllipse(brush, (x - radius), (y - radius), 2 * radius, 2 * radius);
            brush.Dispose();
        }

        public void DrawTriangle(Point[] points, bool is_selected, Color color) // Нарисовать треугольник
        {
            g.DrawPolygon(((is_selected) ? redPen : blackPen), points);
            brush = new SolidBrush(color);
            g.FillPolygon(brush, points);
            brush.Dispose();
        }

        public void DrawSquare(int x, int y, int length, bool is_selected, Color color) // Нарисовать квадрат
        {
            g.DrawRectangle(((is_selected) ? redPen : blackPen), x, y, length, length);
            brush = new SolidBrush(color);
            g.FillRectangle(brush, x, y, length, length);
            brush.Dispose();
        }

        public void DrawLine(Point point1, Point point2, bool is_selected, string color) // Нарисовать линию
        {
            Pen current_color_pen = new Pen(Color.FromName(color));
            g.DrawLine(((is_selected) ? redPen : current_color_pen), point1, point2);
        }

        public void DrawAll(List storage) // Отрисовать всех фигуры
        {
            for (int i = 0; i < storage.GetSize(); i++)
                storage.Get(i).Draw();
        }

        public void UnselectAll(List storage) // Убираем подчеркивание со всех окружностей
        {
            for (int i = 0; i < storage.GetSize(); i++)
                storage.Get(i).Unselect();
        }

        public Point MoveFigure(int x, int y, sbyte direction) // Перемещение фигуры
        {
            if (direction == 'u') // вверх
                y = y - 5;
            else if (direction == 'd') // вниз
                y = y + 5;
            else if (direction == 'r') // вправо
                x = x + 5;
            else if (direction == 'l') // влево
                x = x - 5;

            return new Point(x, y);
        }
    }

    public class CCircle : CShape
    {
        private Designer designer;

        public CCircle(int x, int y, Designer designer, Color color) // Конструктор окружности 
        {
            this.x = x;
            this.y = y;
            this.designer = designer;
            this.color = ColorTranslator.ToHtml(color);
            this.length = 35; // Радиус
        }

        public override void Draw()
        {
            designer.DrawCircle(x, y, length, is_selected, ColorTranslator.FromHtml(color));
        }

        public override void ChangeSize(sbyte type)
        {
            length += (type == '+') ? (canChange(0, 0, 5) ? 5 : 0) : (canChange(0, 0, -5) ? -5 : 0);
        }

        public override bool WasClicked(int x, int y)
        {
            return ((this.x - x) * (this.x - x) + (this.y - y) * (this.y - y) <= length * length);
        }

        public override bool canChange(int dx, int dy, int dlength)
        {
            return (x + dx + length + dlength < designer.getWidth() &&
                    y + dy + length + dlength < designer.getHeight() &&
                    x + dx - length - dlength > 0 &&
                    y + dy - length - dlength > 0 &&
                    length + dlength > 5);
        }
    }

    public class CTriangle : CShape
    {
        private Point[] points = new Point[3];
        private Designer designer;

        public CTriangle(int x, int y, Designer designer, Color color)
        {
            this.x = x; this.y = y;
            this.color = ColorTranslator.ToHtml(color);
            this.designer = designer;
            this.length = 35; // Высота

            points[0].X = x; points[0].Y = y - length;
            points[1].X = x - length; points[1].Y = y + length / 2;
            points[2].X = x + length; points[2].Y = y + length / 2;
        }

        public override void Draw()
        {
            points[0].X = x; points[0].Y = y - length;
            points[1].X = x - length; points[1].Y = y + length / 2;
            points[2].X = x + length; points[2].Y = y + length / 2;

            designer.DrawTriangle(points, is_selected, ColorTranslator.FromHtml(color));
        }

        public override bool WasClicked(int x, int y)
        {
            int a = (points[0].X - x) * (points[1].Y - points[0].Y) - (points[1].X - points[0].X) * (points[0].Y - y);
            int b = (points[1].X - y) * (points[2].Y - points[1].Y) - (points[2].X - points[1].X) * (points[1].Y - y);
            int c = (points[2].X - x) * (points[0].Y - points[2].Y) - (points[0].X - points[2].X) * (points[2].Y - y);

            return (a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0);
        }

        public override bool canChange(int dx, int dy, int dlength)
        {
            
            return (y + dy - length - dlength > 0 &&
                    y + length / 2 + dy + dlength / 2 < designer.getHeight() &&
                    x + length + dx + dlength < designer.getWidth() &&
                    x - length + dx - dlength > 0 &&
                    length + dlength > 10);
        }

        public override void ChangeSize(sbyte type)
        {
            if (type == '+')
            {
                if (canChange(0, 0, 5))
                {
                    points[0].Y += -5;
                    points[1].X += -5;
                    points[1].Y += 5;
                    points[2].X += 5;
                    points[2].Y += 5;
                    length += 5;
                }
            } else
            {
                if (canChange(0, 0, -5))
                {
                    points[0].Y += 5;
                    points[1].X += 5;
                    points[1].Y += -5;
                    points[2].X += -5;
                    points[2].Y += -5;
                    length += -5;
                }
            }
        }
    }

    public class CSquare : CShape
    {
        private Point[] points = new Point[2];
        private Designer designer;

        public CSquare(int x, int y, Designer designer, Color color)
        {
            this.length = 50;
            this.x = x;
            this.y = y;

            this.color = ColorTranslator.ToHtml(color);

            points[0].X = x - length / 2;
            points[0].Y = y - length / 2;
            points[1].X = x + length / 2;
            points[1].Y = y + length / 2;

            this.designer = designer;
        }

        public override void Draw()
        {
            points[0].X = x - length / 2;
            points[0].Y = y - length / 2;
            points[1].X = x + length / 2;
            points[1].Y = y + length / 2;

            designer.DrawSquare(points[0].X, points[0].Y, length, is_selected, ColorTranslator.FromHtml(color));
        }

        public override void ChangeSize(sbyte type)
        {
            length += (type == '+' && canChange(0, 0, 5)) ? 5 : (type == '-' && canChange(0, 0, -5)) ? -5 : 0;

            x = points[0].X + length / 2;
            y = points[0].Y + length / 2;
        }

        public override bool canChange(int dx, int dy, int dlength)
        {
            return (x + length / 2 + dx + dlength / 2 < designer.getWidth() - 5 &&
                    y + length / 2 + dy + dlength < designer.getHeight() - 5 &&
                    x - length / 2 + dx - dlength > 5 &&
                    y - length / 2 + dy - dlength > 5 &&
                    length > 10);
        }

        public override bool WasClicked(int x, int y)
        {
            return x >= this.x - length / 2 && y >= this.y - length / 2 && x <= this.x + length / 2 && y <= this.y + length / 2;
        }
    }
}