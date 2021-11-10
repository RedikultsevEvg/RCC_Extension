using RDBLL.Common.ErrorProcessing.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIntegrationProject.Infrastracture.Factories
{
    public enum PathType
    {
        SteelBase,
        Fouhdation,
        Punching,
        Ancorage
    }
    public static class FilePathFactory
    {
        public static string GetFilePath(PathType type)
        {
            if (type == PathType.Ancorage)
            {
                return GetAncorageFilePath();
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
        }

        private static string GetAncorageFilePath()
        {
            string filePath = GetRCCFilePath();
            filePath += "\\Расчет анкеровки";
            return filePath;
        }

        private static string GetRCCFilePath()
        {
            string filePath = GetExampleFilePath();
            filePath += "\\Железобетонные конструкции";
            return filePath;
        }

        private static string GetExampleFilePath()
        {
            string filePath = "E:\\Repos";
            filePath += "\\Примеры";
            return filePath;
        }
    }
}
