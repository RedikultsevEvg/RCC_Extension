using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ErrorProcessing;

namespace RDBLL.Common.Service
{
    public static class CommonErrorProcessor
    {
        public static void ShowErrorMessage(String mainText, String extendedText)
        {
            wndErrorMessage newErrorMessage = new wndErrorMessage(mainText, extendedText);
            newErrorMessage.ShowDialog();
        }

        public static void ShowErrorMessage(String mainText, Exception ex)
        {
            ShowErrorMessage(mainText, "Exception: \n" + ex);
        }

        public static void ShowErrorMessage(String mainText, String extendedText, Exception ex)
        {
            wndErrorMessage newErrorMessage = new wndErrorMessage(mainText, extendedText);
            ShowErrorMessage(mainText, "Exception: \n" + extendedText + "\n" + ex);
        }
    }
}
