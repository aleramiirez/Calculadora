using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace Calculadora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isNegative = false;

        public MainWindow()
        {
            InitializeComponent();
            Display.Text = "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (Display.Text == "0" && button.Content.ToString() != "+" && button.Content.ToString() != "-"
                && button.Content.ToString() != "x" && button.Content.ToString() != "/")
            {
                Display.Text = button.Content.ToString();
            }
            else
            {
                Display.Text += button.Content.ToString();
            }
        }

        private bool AreParenthesesBalanced(string expression)
        {
            int countOpen = expression.Count(c => c == '(');
            int countClose = expression.Count(c => c == ')');
            return countOpen == countClose;
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text.Replace("x", "*"); // Reemplazar '*' con 'x'
                expression = expression.Replace(",", "."); // Reemplazar comas con puntos
                expression = expression.Replace("÷", "/"); // Reemplazar comas con puntos
                if (AreParenthesesBalanced(expression))
                {
                    DataTable table = new DataTable();
                    string result = table.Compute(expression, "").ToString();
                    Display.Text = result;
                }
                else
                {
                    Display.Text = "SyntaxError: Paréntesis no balanceados";
                }
            }
            catch (Exception ex)
            {
                Display.Text = "SyntaxError";
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            isNegative = false; // Restablecer a positivo
            Display.Text = "0";
        }

        private void ClearLastButton_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 0)
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1); // Elimina el último carácter
                if (Display.Text.Length == 0) Display.Text = "0"; // Si no quedan caracteres, restaurar a "0"
            }
            isNegative = false; // Restablecer a positivo
        }

        private void NegateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Display.Text.Length > 0)
            {
                if (Display.Text[0] == '-') // Si el primer carácter es un "-", quitar la negación
                {
                    Display.Text = Display.Text.Substring(1); // Elimina el signo negativo
                }
                else
                {
                    Display.Text = "-" + Display.Text; // Agrega un signo negativo
                }
            }
        }

        private void ParenthesesButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Verificar si el botón clicado es un paréntesis de cierre y hay un paréntesis de apertura correspondiente en el Display
            if (button.Content.ToString() == ")" && Display.Text.Contains("("))
            {
                // Encontrar el índice del último paréntesis de apertura en el contenido del Display
                int lastOpenParenthesisIndex = Display.Text.LastIndexOf("(");

                // Obtener la parte del contenido del Display desde el último paréntesis de apertura hasta el final
                string lastPart = Display.Text.Substring(lastOpenParenthesisIndex);

                // Verificar si la parte obtenida no contiene un paréntesis de cierre
                if (!lastPart.Contains(")"))
                {
                    // Agregar el paréntesis de cierre al contenido del Display
                    Display.Text += button.Content.ToString();
                }
            }
            // Si el Display no está vacío y el último carácter es un dígito
            else if (Display.Text != "" && char.IsDigit(Display.Text.Last()))
            {
                // Agregar un operador de multiplicación ('x') seguido del paréntesis al contenido del Display
                Display.Text += "x" + button.Content.ToString();
            }
            // Si ninguna de las condiciones anteriores se cumple
            else
            {
                // Agregar el contenido del botón al Display
                Display.Text += button.Content.ToString();
            }
        }

        private void BoldMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Display.FontWeight = Display.FontWeight == FontWeights.Bold ? FontWeights.Normal : FontWeights.Bold;
        }

        private void ItalicMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Display.FontStyle = Display.FontStyle == FontStyles.Italic ? FontStyles.Normal : FontStyles.Italic;
        }

        private void EuroMenuItem_Click(object sender, RoutedEventArgs e)
        {
            double currentValue;
            if (double.TryParse(Display.Text, out currentValue))
            {
                Display.Text = (currentValue * 0.94).ToString("€0.00"); // Euro
            }
        }

        private void DollarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            double currentValue;
            if (double.TryParse(Display.Text, out currentValue))
            {
                Display.Text = (currentValue / 0.94).ToString("$0.00"); // Dólar
            }
        }
    }
}
