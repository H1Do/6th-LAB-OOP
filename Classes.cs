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
        private Point center_point;
        private int radius;
        private Designer designer;

        public CCircle(int x, int y, Designer designer, Color color) // Конструктор окружности 
        {
            this.center_point.X = x;
            this.center_point.Y = y;
            this.designer = designer;
            this.color = ColorTranslator.ToHtml(color);
            radius = 33;
        }

        public override void Draw()
        {
            designer.DrawCircle(center_point.X, center_point.Y, radius, is_selected, ColorTranslator.FromHtml(color));
        }

        public override void ChangeSize(sbyte type)
        {
            radius += (type == '+') ? (center_point.X + radius < designer.getWidth() &&
                                        center_point.Y + radius < designer.getHeight() &&
                                        center_point.X - radius > 0 &&
                                        center_point.Y - radius > 0) ? 5 : 0 : (radius > 5) ? -5 : 0;
        }

        public override bool WasClicked(int x, int y)
        {
            return ((this.center_point.X - x) * (this.center_point.X - x) + (this.center_point.Y - y) * (this.center_point.Y - y) <= radius * radius);
        }

        public override void Move(sbyte direction)
        {
            if (CanMove(direction))
            {
                this.center_point.X = designer.MoveFigure(this.center_point.X, this.center_point.Y, direction).X;
                this.center_point.Y = designer.MoveFigure(this.center_point.X, this.center_point.Y, direction).Y;
            }
        }

        public override bool CanMove(sbyte direction)
        {
            if (direction == 'u')
                return center_point.Y - radius > 0;
            else if (direction == 'd')
                return center_point.Y + radius < designer.getHeight();
            else if (direction == 'r')
                return center_point.X + radius < designer.getWidth();
            else if (direction == 'l')
                return center_point.X - radius > 0;
            else
                return false;
        }
    }

    public class CTriangle : CShape
    {
        private Point[] points = new Point[3];
        private Designer designer;
        private int height = 35;

        public CTriangle(int x, int y, Designer designer, Color color)
        {
            points[0].X = x; points[0].Y = y - 35; // Верхняя точка
            points[1].X = x - 35; points[1].Y = y + 25; // Левая точка
            points[2].X = x + 35; points[2].Y = y + 25; // Правая точка

            this.color = ColorTranslator.ToHtml(color);
            this.designer = designer;
        }

        public override void Draw()
        {
            designer.DrawTriangle(points, is_selected, ColorTranslator.FromHtml(color));
        }

        public override bool WasClicked(int x, int y)
        {
            int a = (points[0].X - x) * (points[1].Y - points[0].Y) - (points[1].X - points[0].X) * (points[0].Y - y);
            int b = (points[1].X - y) * (points[2].Y - points[1].Y) - (points[2].X - points[1].X) * (points[1].Y - y);
            int c = (points[2].X - x) * (points[0].Y - points[2].Y) - (points[0].X - points[2].X) * (points[2].Y - y);

            return (a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0);
        }
        public override void Move(sbyte direction)
        {
            if (CanMove(direction))
            {
                if (direction == 'u')
                {
                    points[0].Y -= 5; points[1].Y -= 5; points[2].Y -= 5;
                }
                if (direction == 'd')
                {
                    points[0].Y += 5; points[1].Y += 5; points[2].Y += 5;
                }
                if (direction == 'l')
                {
                    points[0].X -= 5; points[1].X -= 5; points[2].X -= 5;
                }
                if (direction == 'r')
                {
                    points[0].X += 5; points[1].X += 5; points[2].X += 5;
                }   
            }
        }



        public override bool CanMove(sbyte direction)
        {
            if (direction == 'u')
                return points[0].Y - 5 > 0;
            else if (direction == 'd')
                return points[2].Y + 5 < designer.getHeight();
            else if (direction == 'r')
                return points[2].X + 5 < designer.getWidth();
            else if (direction == 'l')
                return points[1].X - 5 > 0;
            else
                return false;
        }

        public override void ChangeSize(sbyte type)
        {
            if (type == '+' && !((points[0].Y - 5 > 0) && (points[2].Y + 5 < designer.getHeight()) &&
                                (points[2].X + 5 < designer.getWidth()) && points[1].X - 5 > 0)) return; // Проверяем, можем ли мы вообще увеличится
                
            points[0].Y += (type == '+') ? -5 : (height > 10) ? 5 : 0; // На каждом шаге при type == '-' проверяем, можем ли мы вообще уменьшится
            points[1].X += (type == '+') ? -5 : (height > 10) ? 5 : 0;
            points[1].Y += (type == '+') ? 5 : (height > 10) ? -5 : 0;
            points[2].X += (type == '+') ? 5 : (height > 10) ? -5 : 0;
            points[2].Y += (type == '+') ? 5 : (height > 10) ? -5 : 0;
            height += (type == '+') ? 5 : (height > 10) ? -5 : 0;
        }
    }

    public class CSquare : CShape
    {
        private Point center_point;
        private Point[] points = new Point[2];
        private Designer designer;
        private int length;

        public CSquare(int x, int y, Designer designer, Color color)
        {
            length = 50;

            center_point = new Point(x, y);

            this.color = ColorTranslator.ToHtml(color);

            points[0].X = x - length / 2;
            points[0].Y = y - length / 2;
            points[1].X = x + length / 2;
            points[1].Y = y + length / 2;

            this.designer = designer;
        }

        public override void Draw()
        {
            designer.DrawSquare(points[0].X, points[0].Y, length, is_selected, ColorTranslator.FromHtml(color));
        }

        public override void Move(sbyte direction)
        {
            if (CanMove(direction))
            {
                center_point.X = designer.MoveFigure(center_point.X, center_point.Y, direction).X;
                center_point.Y = designer.MoveFigure(center_point.X, center_point.Y, direction).Y;
            }

            points[0].X = center_point.X - length / 2;
            points[0].Y = center_point.Y - length / 2;
            points[1].X = center_point.X + length / 2;
            points[1].Y = center_point.Y + length / 2;
        }
        public override bool CanMove(sbyte direction)
        {
            if (direction == 'u')
                return center_point.Y - length / 2 - 5 > 0;
            else if (direction == 'd')
                return center_point.Y + length / 2 + 5 < designer.getHeight();
            else if (direction == 'r')
                return center_point.X + length / 2 + 5 < designer.getWidth();
            else if (direction == 'l')
                return center_point.X - length / 2 - 5 > 0;
            else
                return false;
        }

        public override void ChangeSize(sbyte type)
        {
            length += (type == '+') ? (center_point.X + length / 2 + 5 < designer.getWidth() &&
                                        center_point.Y + length / 2 + 5 < designer.getHeight() &&
                                        center_point.X - length / 2 - 5 > 0 &&
                                        center_point.Y - length / 2 - 5 > 0) ? 5 : 0 : (length > 5) ? -5 : 0;
            
            center_point.X = points[0].X + length / 2;
            center_point.Y = points[0].Y + length / 2;
        }

        public override bool WasClicked(int x, int y)
        {
            return x >= center_point.X - length / 2 && y >= center_point.Y - length / 2 && x <= center_point.X + length / 2 && y <= center_point.Y + length / 2;
        }
    }
}