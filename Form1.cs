using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StorageClassLibrary;

namespace _6th_LAB_OOP
{
    public partial class Form1 : Form
    {
        Storage shares;
        Designer designer;

        bool is_ctrl_pressed = false;

        public Form1()
        {
            InitializeComponent();
            shares = new Storage();
            designer = new Designer(pictureBox.Width, pictureBox.Height);
        }

        private void NewCircle(int x, int y)
        {
            designer.UnselectAll(shares);
            designer.DrawAll(shares);

            CCircle circle = new CCircle(x, y, designer);
            circle.Draw();
            pictureBox.Image = designer.GetBitmap();
            shares.pushBack(circle);
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
}
