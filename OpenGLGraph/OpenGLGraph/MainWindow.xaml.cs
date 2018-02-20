using SharpGL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace OpenGLGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        FileReader fRead = new FileReader();

        FileProperties fProperties = new FileProperties();

        List<double> tempList = new List<double>();

        bool isMouseDown = false;
        System.Windows.Point mouseClick;
        double anglX = 0;
        double anglY = 0;


        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            ///<summary>
            ///Constantly draws a graph based on values in the list.
            ///
            ///Currently draws a correct graph for a second, possibly because removing values from a list's copy removes them from the original one.
            ///</summary>
            OpenGL gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Rotate(anglX, 0, 1, 0);
            gl.Rotate(anglY, 1, 0, 0);

            gl.Translate(-128, -128, 0);

            for (int x = 0; x < fProperties.Rows; x++)
            {
                for (int y = 0; y < fProperties.Columns; y++)
                {
                    double z = tempList.First();
                    if (z != fProperties.Last)
                    {
                        tempList.Remove(fProperties.Data.First());
                    }
                    if (z == fProperties.Last)
                    {
                        tempList = fProperties.Data;
                    }


                    z = Math.Round(z / fProperties.Highest * 255);

                    System.Drawing.Color colour = System.Drawing.Color.FromArgb((int)z, (int)z, (int)z);

                    gl.Color(colour.R, colour.G, colour.B);

                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Vertex(x, y, 0);
                    gl.Vertex(x + 1, y, 0);
                    gl.Vertex(x + 1, y + 1, 0);
                    gl.Vertex(x, y + 1, 0);


                    gl.End();


                }


            }


        }
        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(45, (double)Width / (double)Height, 0.01, 1300);
            gl.LookAt(0, 0, 350, 0, 0, 255, 0, 1, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            ///<summary>
            /// This allows to open the test file. As the file reader saves everything inside the list,
            /// the first two values are columns and rows. We save them inside corresponding properties and remove them from the list. 
            /// The list is saved inside an object. 
            /// </summary>


            fProperties.Data = fRead.Reader();
            tempList = fProperties.Data;

            fProperties.Columns = fProperties.Data.First();
            fProperties.Data.Remove(fProperties.Data.First());

            fProperties.Rows = fProperties.Data.First();
            fProperties.Data.Remove(fProperties.Data.First());
            fProperties.Highest = fProperties.getHighest(fProperties.Data);
            fProperties.Last = fProperties.Data.Last();

        }

        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                System.Windows.Point tempMousePoint = e.GetPosition(this);
                anglX += (tempMousePoint.X - mouseClick.X) / 1000;
                anglY += (mouseClick.Y - tempMousePoint.Y) / 1000;
            }
        }

        private void OpenGLControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            mouseClick = e.GetPosition(this);
        }
    }
}
