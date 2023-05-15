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
            for (storage.setFirst();  storage.isLast(); storage.next())
                storage.getCurrent().Draw();
        }

        public void UnselectAll(Storage storage) // Убираем подчеркивание со всех окружностей
        {
            for (storage.setFirst(); storage.isLast(); storage.next())
                storage.getCurrent().Unselect();
        }

        public Point MoveFigure(int x, int y, sbyte direction)
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
        private int radius;
        private Point[] bound_points = new Point[4];
        private Designer designer;

        public CCircle(int x, int y, Designer designer) // Конструктор окружности 
        {
            this.x = x;
            this.y = y;
            this.designer = designer;
            radius = 33;

            bound_points[0].X = x; bound_points[0].Y = y + radius;
            bound_points[1].X = x + radius; bound_points[1].Y = y;
            bound_points[2].X = x; bound_points[1].Y = y - radius;
            bound_points[3].X = x - radius; bound_points[1].Y = y;
        }

        public override void Draw()
        {
            designer.DrawCircle(x, y, radius, is_selected, color);
        }

        public override void ChangeSize(string type)
        {
            radius += (type == "+") ? 5 : -5;
        }

        public override bool WasClicked(int x, int y)
        {
            return ((this.x - x) * (this.x - x) + (this.y - y) * (this.y - y) <= radius * radius);
        }

        public override void Move(sbyte direction)
        {
            if (CanMove(direction))
            {
                x = designer.MoveFigure(x, y, direction).X;
                y = designer.MoveFigure(y, x, direction).Y;
            }

            bound_points[0].X = x; bound_points[0].Y = y + radius;
            bound_points[1].X = x + radius; bound_points[1].Y = y;
            bound_points[2].X = x; bound_points[1].Y = y - radius;
            bound_points[3].X = x - radius; bound_points[1].Y = y;
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
}