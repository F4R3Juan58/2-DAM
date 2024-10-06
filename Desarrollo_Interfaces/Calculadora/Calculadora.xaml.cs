using System;

namespace Calculadora
{
    public partial class Calculadora : ContentPage
    {
        double memory = 0;
        string currentExpression = string.Empty;

        public Calculadora()
        {
            InitializeComponent();
        }

        void OnNumberClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                currentExpression += button.Text;
                display.Text = currentExpression;
            }
        }

        void OnOperatorClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (currentExpression.Length > 0)
                {
                    currentExpression += button.Text;
                    display.Text = currentExpression;
                }
            }
        }

        void OnEqualClicked(object sender, EventArgs e)
        {
            try
            {
                double result = EvaluateExpression(currentExpression);
                display.Text = result.ToString();
                currentExpression = result.ToString();
            }
            catch
            {
                display.Text = "Error";
                currentExpression = string.Empty;
            }
        }

        void OnClearClicked(object sender, EventArgs e)
        {
            currentExpression = string.Empty;
            display.Text = "0";
        }

        void OnMemoryAddClicked(object sender, EventArgs e)
        {
            try
            {
                double currentValue = EvaluateExpression(currentExpression);
                memory += currentValue;
                display.Text = "Memoria: " + memory;
            }
            catch
            {
                display.Text = "Error";
            }
        }

        void OnMemorySubtractClicked(object sender, EventArgs e)
        {
            try
            {
                double currentValue = EvaluateExpression(currentExpression);
                memory -= currentValue;
                display.Text = "Memoria: " + memory;
            }
            catch
            {
                display.Text = "Error";
            }
        }

        void OnMemoryClearClicked(object sender, EventArgs e)
        {
            memory = 0;
            display.Text = "Memoria Limpiada";
        }

        double EvaluateExpression(string expression)
        {
            double total = 0;
            char currentOperator = '+';

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                if (char.IsDigit(ch))
                {
                    string number = string.Empty;

                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }

                    double value = Convert.ToDouble(number);

                    switch (currentOperator)
                    {
                        case '+':
                            total += value;
                            break;
                        case '-':
                            total -= value;
                            break;
                        case '*':
                            total *= value;
                            break;
                        case '/':
                            if (value != 0)
                                total /= value;
                            else
                                throw new DivideByZeroException();
                            break;
                    }
                    i--;
                }
                else if ("+-*/".Contains(ch))
                {
                    currentOperator = ch;
                }
            }
            return total;
        }
    }
}
