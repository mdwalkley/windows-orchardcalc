using System;
using System.Collections.Generic;
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

public class Dimensions
{
    private double width, length, depth;
    public Dimensions() {
        this.width = 0;
        this.length = 0;
        this.depth = 0.25;
    }
    public void setLength(double l){ this.length = l; }
    public void setWidth(double w){ this.width = w; }
    public void setDepth(double d){ this.depth = d; }
    public double getLength(){ return this.length; }
    public double getWidth(){ return this.width; }
    public double getDepth(){ return this.depth; }
    public double getArea(){ return this.length * this.width; }
    public double getVolume(){ return this.width * this.length * (this.depth / 12); }

}



namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    //Auto-Select TextBox (Credit: http://madprops.org/blog/wpf-textbox-selectall-on-focus/)
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_GotFocus));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseDownEvent, new RoutedEventHandler(TextBox_PreviewMouseDown));
            base.OnStartup(e);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void TextBox_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!textBox.IsFocused)
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }

        
    }


        //Main App
        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //MULCH TAB

        //Variables
        Dimensions mulchDim = new Dimensions();

        //Methods
        private void showMulchArea()
        {
            textBoxArea_Mulch.Text = mulchDim.getArea().ToString("F0");
        }

        //Events
        private void textBoxWidth_Mulch_TextChanged(object sender, TextChangedEventArgs e)
        {
            double width;
            if (Double.TryParse(textBoxWidth_Mulch.Text, out width))
            {
                mulchDim.setWidth(width);
                showMulchArea();
            }
        }

        private void textBoxLength_Mulch_TextChanged(object sender, TextChangedEventArgs e)
        {
            double length;
            if (Double.TryParse(textBoxLength_Mulch.Text, out length))
            {
                mulchDim.setLength(length);
                showMulchArea();
            }
        }

        private void comboBoxDepth_Mulch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mulchDim.setDepth(Double.Parse(comboBoxDepth_Mulch.SelectedValue.ToString()));
        }

        private void buttonCalculate_Mulch_Click(object sender, RoutedEventArgs e)
        {
            double vol = mulchDim.getVolume();
            double bags2Dbl = vol / 2;
            double bags3Dbl = vol / 3;
            int bags2Int = Convert.ToInt32(bags2Dbl);
            int bags3Int = Convert.ToInt32(bags3Dbl);

            bags2Int = (bags2Int < 1) ? ++bags2Int : bags2Int;
            bags3Int = (bags3Int < 1) ? ++bags3Int : bags3Int;

            textBoxResult_Mulch.Text = vol.ToString("N2");
            textBox2Bags.Text = bags2Int.ToString();
            label2Bags.Content = "(" + bags2Dbl.ToString("N2") + ")";
            textBox3Bags.Text = bags3Int.ToString();
            label3Bags.Content = "(" + bags3Dbl.ToString("N2") + ")";
            
        }



        //GROUND COVER TAB

        //Variables
        Dimensions gcDim = new Dimensions();

        //Methods
        private void showGCArea()
        {
            textBoxArea_GC.Text = gcDim.getArea().ToString("F0");
        }

        //Events
        private void textBoxWidth_GC_TextChanged(object sender, TextChangedEventArgs e)
        {
            double width;
            if (Double.TryParse(textBoxWidth_GC.Text, out width))
            {
                gcDim.setWidth(width);
                showGCArea();
            }
        }

        private void textBoxLength_GC_TextChanged(object sender, TextChangedEventArgs e)
        {
            double length;
            if (Double.TryParse(textBoxLength_GC.Text, out length))
            {
                gcDim.setLength(length);
                showGCArea();
            }
        }

        private void buttonCalculate_GC_Click(object sender, RoutedEventArgs e)
        {
            int sPlants = (int)(gcDim.getArea() * 144 / (double)(IntUpDown_GC.Value * IntUpDown_GC.Value));
            int tPlants = (int)(gcDim.getArea() * 144 / (0.866 * IntUpDown_GC.Value * IntUpDown_GC.Value));
            textBoxPlants_S.Text = sPlants.ToString();
            textBoxPlants_T.Text = tPlants.ToString();

            int sGPCP = (sPlants % 6 > 2) ? sPlants / 6 + 1 : sPlants / 6;
            int tGPCP = (tPlants % 6 > 2) ? tPlants / 6 + 1 : tPlants / 6;
            textBoxGPCP_S.Text = sGPCP.ToString();
            textBoxGPCP_T.Text = tGPCP.ToString();

            textBoxFlats_S.Text = (sGPCP / 8.0).ToString("N1");
            textBoxFlats_T.Text = (tGPCP / 8.0).ToString("N1");

        }
    }
}
