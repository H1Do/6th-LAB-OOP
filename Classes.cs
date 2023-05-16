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

        public void DrawCircle(int x, int y, int radius, bool is_selected, string color) // Нарисовать окружность 
        {
            g.DrawEllipse(((is_selected) ? redPen : blackPen), (x - radius), (y - radius), 2 * radius, 2 * radius);
            brush = new SolidBrush(Color.FromName(color));
            g.FillEllipse(brush, (x - radius), (y - radius), 2 * radius, 2 * radius);
            brush.Dispose();
        }

        public void DrawTriangle(Point[] points, bool is_selected, string color) // Нарисовать треугольник
        {
            g.DrawPolygon(((is_selected) ? redPen : blackPen), points);
            brush = new SolidBrush(Color.FromName(color));
            g.FillPolygon(brush, points);
            brush.Dispose();
        }

        public void DrawSquare(int x, int y, int length, bool is_selected, string color) // Нарисовать квадрат
        {
            g.DrawRectangle(((is_selected) ? redPen : blackPen), x, y, length, length);
            brush = new SolidBrush(Color.FromName(color));
            g.FillRectangle(brush, x, y, length, length);
            brush.Dispose();
        }

        public void DrawLine(Point point1, Point point2, bool is_selected, string color) // Нарисовать линию
        {
            Pen current_color_pen = new Pen(Color.FromName(color));
            g.DrawLine(((is_selected) ? redPen : current_color_pen), point1, point2);
        }

        public void DrawAll(Storage storage) // Отрисовать всех фигуры
        {
            if (storage.isEmpty()) return;
            for (storage.setFirst(); storage.isLast(); storage.next())
                storage.getCurrent().Draw();
        }

        public void UnselectAll(Storage storage) // Убираем подчеркивание со всех окружностей
        {
            for (storage.setFirst(); storage.isLast(); storage.next())
                storage.getCurrent().Unselect();
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
        private Point[] bound_points = new Point[4];
        private Designer designer;

        public CCircle(int x, int y, Designer designer) // Конструктор окружности 
        {
            this.center_point.X = x;
            this.center_point.Y = y;
            this.designer = designer;
            radius = 33;

            bound_points[0].X = x; bound_points[0].Y = y + radius;
            bound_points[1].X = x + radius; bound_points[1].Y = y;
            bound_points[2].X = x; bound_points[1].Y = y - radius;
            bound_points[3].X = x - radius; bound_points[1].Y = y;
        }

        public override void Draw()
        {
            designer.DrawCircle(center_point.X, center_point.Y, radius, is_selected, color);
        }

        public override void ChangeSize(sbyte type)
        {
            radius += (type == '+') ? 5 : -5;
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

            bound_points[0].X = this.center_point.X; bound_points[0].Y = this.center_point.Y + radius;
            bound_points[1].X = this.center_point.X + radius; bound_points[1].Y = this.center_point.Y;
            bound_points[2].X = this.center_point.X; bound_points[1].Y = this.center_point.Y - radius;
            bound_points[3].X = this.center_point.X - radius; bound_points[1].Y = this.center_point.Y;
        }

        public override bool CanMove(sbyte direction)
        {
            if (direction == 'u')
                return bound_points[1].Y - 5 >= 0 ? true : false;
            else if (direction == 'r')
                return bound_points[2].X + 5 <= (designer.getWidth()) ? true : false;
            else if (direction == 'd')
                return bound_points[3].Y + 5 <= (designer.getHeight()) ? true : false;
            else if (direction == 'l')
                return bound_points[0].X - 5 >= 0 ? true : false;
            return false;
        }
    }

    public class CTriangle : CShape
    {
        private Point[] points = new Point[3];
        private Designer designer;

        public CTriangle(int x, int y, Designer designer)
        {
            points[0].X = x; points[0].Y = y - 35; // Верхняя точка
            points[1].X = x - 35; points[1].Y = y + 25; // Левая точка
            points[2].X = x + 35; points[2].Y = y + 25; // Правая точка

            this.designer = designer;
        }

        public override void Draw()
        {
            designer.DrawTriangle(points, is_selected, color);
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
                    points[0].Y -= 5;
                    points[1].Y -= 5;
                    points[2].Y -= 5;
                }
                if (direction == 'd')
                {
                    points[0].Y += 5;
                    points[1].Y += 5;
                    points[2].Y += 5;
                }
                if (direction == 'l')
                {
                    points[0].X -= 5;
                    points[1].X -= 5;
                    points[2].X -= 5;
                }
                if (direction == 'r')
                {
                    points[0].X += 5;
                    points[1].X += 5;
                    points[2].X += 5;
                }
            }
        }

        public override bool CanMove(sbyte direction)
        {
            if (direction == 'u')
                return points[0].Y - 5 >= 0 ? true : false;
            else if (direction == 'd')
                return points[2].Y + 5 <= designer.getHeight() ? true : false;
            else if (direction == 'r')
                return points[2].X + 5 <= designer.getWidth() ? true : false;
            else if (direction == 'l')
                return points[1].X - 5 >= 0 ? true : false;
            else
                return false;
        }

        public override void ChangeSize(sbyte type)
        {
            points[0].X += (type == '+') ? -5 : 5;
            points[1].X += (type == '+') ? -5 : 5;
            points[1].Y += (type == '+') ? 5 : -5;
            points[2].X += (type == '+') ? 5 : -5;
            points[2].Y += (type == '+') ? 5 : -5;
        }
    }

    public class CSquare : CShape
    {
        private Point center_point;
        private Point[] points = new Point[2];
        private Designer designer;
        private int length;

        public CSquare(int x, int y, Designer designer)
        {
            length = 50;

            center_point = new Point(x, y);

            points[0].X = x - length / 2;
            points[0].Y = y - length / 2;
            points[1].X = x + length / 2;
            points[1].Y = y + length / 2;

            this.designer = designer;
        }

        public override void Draw()
        {
            designer.DrawSquare(points[0].X, points[0].Y, length, is_selected, color);
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
                return points[0].Y - 5 >= 0 ? true : false;
            else if (direction == 'u')
                return points[1].Y + 5 <= designer.getHeight() ? true : false;
            else if (direction == 'u')
                return points[1].X + 5 <= designer.getWidth() ? true : false;
            else if (direction == 'u')
                return points[0].X - 5 >= 0 ? true : false;
            else
                return false;

        }

        public override void ChangeSize(sbyte type)
        {
            length += (type == '+') ? 10 : -10;
        }

        public override bool WasClicked(int x, int y)
        {
            return x >= center_point.X - length / 2 && y >= center_point.Y - length / 2 && x <= center_point.X + length / 2 && y <= center_point.Y + length / 2;
        }
    }

    public class CLine : CShape
    {
        private Point[] points = new Point[2];
        private Designer designer;
        public CLine(int x1, int y1, int x2, int y2, Designer designer)
        {
            points[0] = new Point(x1, y1);
            points[1] = new Point(x2, y2);
        }

        public override void ChangeSize(sbyte type)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void Move(sbyte direction)
        {
            throw new NotImplementedException();
        }

        public override bool CanMove(sbyte direction)
        {
            throw new NotImplementedException();
        }

        public override bool WasClicked(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}