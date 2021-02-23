using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Common.Params
{
    /// <summary>
    /// Процессор для сохраняемых параметров
    /// </summary>
    public static class StoredParamProc
    {
        /// <summary>
        /// Возвращает строку из значений параметров
        /// </summary>
        /// <param name="hasStoredParams"></param>
        /// <returns></returns>
        public static string GetValueString(IHasStoredParams hasStoredParams)
        {
            return GetValueString(hasStoredParams.StoredParams);
        }
        /// <summary>
        /// Возвращает коллекцию параметров по входной строке нужного формата
        /// </summary>
        /// <param name="paramString"></param>
        /// <returns></returns>
        public static List<StoredParam> GetFromString(string paramString)
        {
            List<StoredParam> storedParams = new List<StoredParam>();
            string[] parameters = paramString.Split(new string[] { "~/~" }, StringSplitOptions.None);
            for (int i = 0; i < parameters.Count() - 1; i++)
            {
                StoredParam storedParam = new StoredParam();
                string[] fields = parameters[i].Split(new string[] { ";" }, StringSplitOptions.None);
                storedParam.Id = Convert.ToInt32(fields[0]);
                storedParam.Name = fields[1];
                storedParam.SetValueType(fields[2]);
                storedParam.ParameterValue = fields[3];
                storedParams.Add(storedParam);
            }
            return storedParams;
        }
        
        private static string GetValueString(List <StoredParam> storedParams)
        {
            string s = "";
            foreach (StoredParam parameter in storedParams)
            {
                s += parameter.Id + ";";
                s += parameter.Name + ";";
                s += parameter.ValueType + ";";
                s += parameter.ParameterValue;
                s += "~/~";
            }
            return s;
        }
    }
}
