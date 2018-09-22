using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Service
{
    //Класс передачи данных в окна списков
    public class DetailObjectList
    {
        public String DataType { get; set; } //Тип данных
        public Object ParentObject { get; set; } //Ссылка на родительский объект
        public Object ObjectList { get; set; } //Коллекция объектов для которых составляется список
        public bool IsSelectable { get; set; } //Опция окна - выбора или только редактировани

        //Конструктор
        public DetailObjectList(String dataType, Object parentObject, Object objectList, bool isSelectable)
        {
            DataType = dataType;
            ParentObject = parentObject;
            ObjectList = objectList;
            IsSelectable = isSelectable;
        }
    }
}
