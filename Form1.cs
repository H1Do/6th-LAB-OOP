using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6th_LAB_OOP
{
    public partial class Form1 : Form
    {
        List<CCircle> circles;
        Designer designer;

        bool is_ctrl_pressed = false;

        public Form1()
        {
            InitializeComponent();
            circles = new List<CCircle>();
            designer = new Designer(pictureBox.Width, pictureBox.Height);
        }

        private void NewCircle(int x, int y)
        {
            designer.UnselectAll(circles);
            designer.DrawAll(circles);

            CCircle circle = new CCircle(x, y);
            circle.DrawCircle(designer);
            pictureBox.Image = designer.GetBitmap();
            circles.Add(circle);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            designer.Clear();

            bool was_clicked = false;

            if (!is_ctrl_pressed) // Если CTRL не зажата, значит убираем выделение со всех выделенным окружностей
                designer.UnselectAll(circles);

            if (!designer.SelectCircleByCoord(circles, e.X, e.Y, !isCrossSelectCheckBox.Checked)) // Выделяем попавшийся (попавшиеся) окружности, нашли?
            {
                NewCircle(e.X, e.Y); // Нет - создаём новую
            }
            else
            {
                designer.DrawAll(circles); // Да - Отрисовываем все (ту окружность, что мы нашли, мы уже выделели)
                pictureBox.Image = designer.GetBitmap();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            designer.DrawAll(circles);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey: // Если это CTRL, то мы это запоминаем и выходим
                    is_ctrl_pressed = true;
                    isCtrlCheckBox.Checked = true;
                    break;
                case Keys.Delete: // Если это DEL, то мы удаляем все выделенные элементы 
                    List<CCircle> order_to_delete = new List<CCircle>(); // Создаём список на удаление
                    foreach (CCircle circle in circles)
                        if (circle.IsSelected())
                            order_to_delete.Add(circle); // Заполняем этот список
                    foreach (CCircle circle in order_to_delete)
                        if (circles.Contains(circle))
                            circles.Remove(circle); // Проходимся по этому списку и удаляем все окружности уже из списка всех окружностей
                    break;
            }

            designer.Clear(); // Очищаем изображение, отрисовываем все окружности и передаём изобажение pictureBox'у
            designer.DrawAll(circles);
            pictureBox.Image = designer.GetBitmap();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            { // Если это была CTRL, то мы запоминаем это (запоминаем, что она уже не зажата)
                is_ctrl_pressed = false;
                isCtrlCheckBox.Checked = false;
            }
        }
    }
    public class Designer // Класс, отвечающий за отрисовку и получения изображения в bitmap (растровое изображение)
    {
        private Bitmap bitmap; // Растровое изображение
        private Pen blackPen; // Ручка для рисования черным цветом
        private Pen redPen; // Ручка для рисования черным цветом
        private Graphics g; // Класс, предоставляющий методы для рисования объектов

        public Designer(int width, int height) // Конструктор
        {
            bitmap = new Bitmap(width, height); // Определяем растровое изображение
            g = Graphics.FromImage(bitmap); // Определяем класс, отвечающий за рисование
            blackPen = new Pen(Color.Black); blackPen.Width = 2; // Определяем черную ручку
            redPen = new Pen(Color.Red); redPen.Width = 2; // Определяем красную ручку
        }

        public Bitmap GetBitmap() // Получить растровое изображение 
        {
            return bitmap;
        }

        public void Clear() // Очистить изображение
        {
            g.Clear(Color.White);
        }

        public void DrawCircle(int x, int y, int radius) // Нарисовать окружность (черного цвета)
        {
            g.DrawEllipse(blackPen, (x - radius), (y - radius), 2 * radius, 2 * radius);
        }

        public void DrawSelectedCircle(int x, int y, int radius) // Нарисовать выделенную окружность (красного цвета)
        {
            g.DrawEllipse(redPen, (x - radius), (y - radius), 2 * radius, 2 * radius);
        }

        public void DrawAll(List<CCircle> circles) // Отрисовать всех окружности
        {
            foreach (CCircle current_circle in circles)
                current_circle.DrawCircle(this);
        }

        public void UnselectAll(List<CCircle> circles) // Убираем подчеркивание со всех окружностей
        {
            foreach (CCircle current_circle in circles)
                current_circle.Unselect();
        }

        public bool SelectCircleByCoord(List<CCircle> circles, int x, int y, bool is_single) // Выделяем окружность (окружности) попавшуюся (попавшиеся) по координатам
        {
            bool was_clicked = false;
            foreach (CCircle circle in circles) // Ищем окружность, на который мы попали (если вообще попали)
            {
                if (circle.WasClicked(x, y))
                {
                    was_clicked = true;
                    circle.Select(); // Выделяем окружность

                    if (is_single) return true; // Если перекрестное выделение не выбрано, то останавливаемся на выделении одной окружности
                }
            }
            return was_clicked; // На выходе передаём успешность выделения
        }
    }
    public class CCircle
    {
        private int x, y, radius; // Параметры окружности
        private bool is_selected; // Параметр указывающий на выделенность окружности

        public CCircle(int x, int y) // Конструктор окружности 
        {
            this.x = x;
            this.y = y;
            radius = 33;
        }

        public CCircle(CCircle obj) // Констуктор копирования
        {
            this.x = obj.x;
            this.y = obj.y;
            this.radius = obj.radius;
        }

        public void Select() // Выделяем окружность
        {
            is_selected = true;
        }

        public void Unselect() // Убираем выделенность окружности
        {
            is_selected = false;
        }

        public bool IsSelected() // Запрашиваем выделенность окружности
        {
            return is_selected;
        }

        public void DrawCircle(Designer designer) // Отрисовать окружность с помощью класса, отв. за рисование
        {
            if (IsSelected()) designer.DrawSelectedCircle(x, y, radius);
            else designer.DrawCircle(x, y, radius);
        }

        public bool WasClicked(int coordX, int coordY) // Запрашиваем у окружности, попала ли она во входные координаты
        {
            return (Math.Pow(x - coordX, 2) + Math.Pow(y - coordY, 2) <= radius * radius);
        }
    }
}
