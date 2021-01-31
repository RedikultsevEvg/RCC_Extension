using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;

namespace RDBLL.Entity.Common.Materials.Interfaces
{
    interface IMaterialUsing :ISavableToDataSet
    {
        /// <summary>
        /// Код выбранного материала
        /// </summary>
        int SelectedId { get; set; }
        /// <summary>
        /// Коллекция доступных материалов
        /// </summary>
        List<IMaterialKind> MaterialKinds { get; set; }
        /// <summary>
        /// Ссылка на выбранный материал
        /// </summary>
        IMaterialKind MaterialKind { get; set; }
        /// <summary>
        /// Предварительная деформация
        /// </summary>
        double PreStrain { get; set; }
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        ISavableToDataSet Parent { get; set; }
        /// <summary>
        /// Обновление ссылки на выбранный материал
        /// </summary>
        void RenewMaterial();
    }
}
