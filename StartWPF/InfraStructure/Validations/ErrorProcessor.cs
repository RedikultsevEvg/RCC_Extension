using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;


namespace RDUIL.Validations
{
    public static class ErrorProcessor
    {
        public static string cmdGetErrorString(Grid grid)
        {
            StringBuilder sb = new StringBuilder();
            GetErrors(sb, grid);
            string message = sb.ToString();
            return message;
        }

        public static void GetErrors(StringBuilder sb, DependencyObject obj)
        {
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                GetErrors(sb, VisualTreeHelper.GetChild(obj, i));

                TextBox element = VisualTreeHelper.GetChild(obj, i) as TextBox;
                if (element == null) continue;

                if (Validation.GetHasError(element))
                {
                    sb.Append(element.Text + " найдена ошибка:\r\n");
                    foreach (ValidationError error in Validation.GetErrors(element))
                    {
                        sb.Append("  " + error.ErrorContent.ToString());
                        sb.Append("\r\n");
                    }

                }
            }
        }
    }
}
