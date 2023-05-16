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

namespace _6th_LAB_OOP
{
    public partial class Form1 : Form
    {
        private Storage shapes;
        private Designer designer;
        private Color current_color;
        private String current_shape;

        private bool is_ctrl_pressed = false;

        public Form1()
        {
            InitializeComponent();
            shapes = new Storage();
            designer = new Designer(pictureBox.Width, pictureBox.Height);
        }

        private void NewCircle(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CCircle circle = new CCircle(x, y, designer);
            circle.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.pushBack(circle);
        }

        private void NewTriangle(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CTriangle triangle = new CTriangle(x, y, designer);
            triangle.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.pushBack(triangle);
        }

        private void NewSquare(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CSquare square = new CSquare(x, y, designer);
            square.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.pushBack(square);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            designer.DrawAll(shapes);
        }

        private void NewShare(int x, int y)
        {
            designer.UnselectAll(shapes);
            designer.DrawAll(shapes);

            CShape new_obj;

            if (this.current_shape == "Circle")
                new_obj = new CCircle(x, y, designer);
            else if (this.current_shape == "Triangle")
                new_obj = new CTriangle(x, y, designer);
            else if (this.current_shape == "Square")
                new_obj = new CSquare(x, y, designer);
            else
                return;

            new_obj.Draw();
            pictureBox.Image = designer.GetBitmap();
            shapes.pushBack(new_obj);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            designer.Clear();
            bool was_clicked = false;
            CShape current_shape = null;

            for (shapes.setFirst(); !shapes.isLast(); shapes.next())
                if (shapes.getCurrent() is CShape shape)
                    if (shape.WasClicked(e.X, e.Y))
                        current_shape = shape;
                        
            if (current_shape == null)
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
                    for (shapes.setFirst(); !shapes.isLast(); shapes.next())
                        if (shapes.getCurrent() is CShape shape)
                            if (shape.IsSelected())
                                shapes.deleteCurrent();
                    break;
            }

            designer.Clear(); // Очищаем изображение, отрисовываем все окружности и передаём изобажение pictureBox'у
            designer.DrawAll(shapes);
            pictureBox.Image = designer.GetBitmap();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.ControlKey)
            { // Если это была CTRL, то мы запоминаем это (запоминаем, что она уже не зажата)
                is_ctrl_pressed = false;
                isCtrlCheckBox.Checked = false;
            }*/
        }

        private void colorBtn_Click(object sender, EventArgs e)
        {
            var clr_dialog = new ColorDialog();
            if (clr_dialog.ShowDialog() == DialogResult.OK)
                colorBtn.BackColor = clr_dialog.Color;
            current_color = clr_dialog.Color;
            chosedShare.ForeColor = current_color;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            current_shape = sharesListBox.SelectedItem.ToString();
            chosedShare.Text = current_shape;
            chosedShare.ForeColor = current_color;
        }
    }
}
