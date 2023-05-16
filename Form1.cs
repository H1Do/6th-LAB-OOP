using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StorageClassLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _6th_LAB_OOP
{
    public partial class Form1 : Form
    {
        private MyStorage shapes;
        private Designer designer;
        private Color current_color = Color.White;
        private String current_shape;

        private bool is_ctrl_pressed = false;

        public Form1()
        {
            InitializeComponent();
            shapes = new MyStorage();
            designer = new Designer(pictureBox.Width, pictureBox.Height);
            this.MouseWheel += new MouseEventHandler(this_MouseWheel); // Изменение размера фигуры вращением колёсика
            this.MouseWheel += new MouseEventHandler(shapesComboBox_MouseWheel);
            shapesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; // Запрещаем ввод своих значений в combobox

        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        /*private void NewCircle(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CCircle circle = new CCircle(x, y, designer, current_color);
            circle.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.add(circle);
        }

        private void NewTriangle(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CTriangle triangle = new CTriangle(x, y, designer);
            triangle.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.add(triangle);
        }

        private void NewSquare(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CSquare square = new CSquare(x, y, designer);
            square.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.add(square);
        }*/

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            designer.DrawAll(shapes);
        }

        private void NewShare(int x, int y)
        {
            if (current_shape == null)
                return;

            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CShape new_obj;

            if (this.current_shape == "Circle")
                new_obj = new CCircle(x, y, designer, current_color);
            else if (this.current_shape == "Triangle")
                new_obj = new CTriangle(x, y, designer, current_color);
            else if (this.current_shape == "Square")
                new_obj = new CSquare(x, y, designer, current_color);
            else
                return;

            new_obj.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.add(new_obj);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            designer.Clear();
            bool was_clicked = false;

            for (shapes.first(); !shapes.isEOL(); shapes.next())
                if (shapes.getObject() is CShape shape)
                    if (shape.WasClicked(e.X, e.Y))
                    {
                        was_clicked = true;
                        if (!is_ctrl_pressed)
                            designer.UnselectAll(shapes);
                        shape.Select();
                    }
                        
            if (!was_clicked)
            {
                NewShare(e.X, e.Y);
                return;
            }

            designer.Clear(); // Очищаем изображение, отрисовываем все окружности и передаём изобажение pictureBox'у
            designer.DrawAll(shapes);
            pictureBox.Image = designer.GetBitmap();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey: // Если это CTRL, то мы это запоминаем и выходим
                    is_ctrl_pressed = true;
                    break;
                case Keys.Delete: // Если это DEL, то мы удаляем все выделенные элементы 
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shapes.del(shape);
                    break;
                case Keys.Up: // Если это Up, то все выделенные фигуры движутся наверх
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.Move((sbyte)'u');
                    break;
                case Keys.Down: // Если это Down, то все выделенные фигуры движутся вниз
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.Move((sbyte)'d');
                    break;
                case Keys.Left: // Если это Left, то все выделенные фигуры движутся влево
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.Move((sbyte)'l');
                    break;
                case Keys.Right: // Если это Right, то все выделенные фигуры движутся вправо
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.Move((sbyte)'r');
                    break; 
            }

            designer.Clear(); // Очищаем изображение, отрисовываем все окружности и передаём изобажение pictureBox'у
            designer.DrawAll(shapes);
            pictureBox.Image = designer.GetBitmap();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                is_ctrl_pressed = false;
        }

        void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (is_ctrl_pressed)
            {
                if (e.Delta < 0) // Уменьшаем все фигуры
                {
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.ChangeSize((sbyte)'+');
                }
                else // Увеличиваем все фигуры
                {
                    for (shapes.first(); !shapes.isEOL(); shapes.next())
                        if (shapes.getObject() is CShape shape)
                            if (shape.IsSelected())
                                shape.ChangeSize((sbyte)'-');
                }
            }
            designer.Clear(); // Очищаем изображение, отрисовываем все окружности и передаём изобажение pictureBox'у
            designer.DrawAll(shapes);
            pictureBox.Image = designer.GetBitmap();
        }

        private void colorBtn_Click(object sender, EventArgs e)
        {
            var clr_dialog = new ColorDialog();
            if (clr_dialog.ShowDialog() == DialogResult.OK)
            {
                colorBtn.BackColor = clr_dialog.Color;
                current_color = clr_dialog.Color; // Запоминаем для появляющихся фигур
            }
             
            chosedShare.ForeColor = current_color; 

            for (shapes.first(); !shapes.isEOL(); shapes.next()) // Перекрашиваем выделенные
                if (shapes.getObject() is CShape shape)
                    if (shape.IsSelected())
                        shape.ChangeColor(current_color.ToString());
        }

        private void shapesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            current_shape = shapesComboBox.SelectedItem.ToString();
            chosedShare.Text = current_shape;
            chosedShare.ForeColor = current_color;
        }

        private void shapesComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Left))
                e.Handled = true;
        }

        private void shapesComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((e.Delta > 0) || e.Delta < 0)
                ((HandledMouseEventArgs)e).Handled = true;
        }
    }
}
