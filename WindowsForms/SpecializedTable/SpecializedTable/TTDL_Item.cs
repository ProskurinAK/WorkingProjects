using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
// ************************************************************************************************************

namespace SpecializedTable
{
    class TTDL_Item
    {
        /// <summary>
        /// Вкл/Выкл работу фильтра для ячеек
        /// </summary>
        public bool Enable_Filtration;

        /// <summary>
        /// Уникальный индентификатор (ячейки)
        /// </summary>
        public int UID;

        /// <summary>
        /// Уникальный индентификатор (столбца)
        /// </summary>
        public int UID_Column;

        /// <summary>
        /// Уникальный идентификатор (строки)
        /// </summary>
        public int UID_Row;

        /// <summary>
        /// Номер столбца в строке для текущего элемента
        /// </summary>
        public int X;

        /// <summary>
        /// Номер строки для текущего элемента
        /// </summary>
        public int Y;

        /// <summary>
        /// Уникальный идентификатор родителя (-1 если корень таблицы)
        /// </summary>
        public int Parent_UID;

        /// <summary>
        /// Цвет окна ячейки
        /// </summary>
        public Color Background;

        /// <summary>
        /// Цвет шрифта
        /// </summary>
        public Color Foreground;

        /// <summary>
        /// Шрифт
        /// </summary>
        public Font FontText;

        /// <summary>
        /// Значение в ячейке
        /// </summary>
        public string Value;

        /// <summary>
        /// true если ячейки в строке, которая является узлом. false если нет
        /// </summary>
        public bool IsNode;
        // ------------------------------------------------------------------------------------------------------------

        public TTDL_Item()
        {

        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Создание универсального объекта в таблице (ячейки) со значением поля Value
        /// </summary>
        /// <param name="Enable_Filtration"></param>
        /// <param name="UID"></param>
        /// <param name="UID_Column"></param>
        /// <param name="UID_Row"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Parent_UID"></param>
        /// <param name="Background"></param>
        /// <param name="Foreground"></param>
        /// <param name="FontText"></param>
        /// <param name="Value"></param>
        /// <param name="IsNode"></param>
        public TTDL_Item(bool Enable_Filtration, int UID, int UID_Column, int UID_Row, int X, int Y, int Parent_UID, Color Background, Color Foreground, Font FontText, string Value, bool IsNode = false)
        {
            this.Enable_Filtration = Enable_Filtration;
            this.UID = UID;
            this.UID_Column = UID_Column;
            this.UID_Row = UID_Row;
            this.X = X;
            this.Y = Y;
            this.Parent_UID = Parent_UID;
            this.Background = Background;
            this.Foreground = Foreground;
            this.FontText = FontText;
            this.Value = Value;
            this.IsNode = IsNode;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Создание универсального объекта в таблице (ячейки) без значения поля Value
        /// </summary>
        /// <param name="Enable_Filtration"></param>
        /// <param name="UID"></param>
        /// <param name="UID_Column"></param>
        /// <param name="UID_Row"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Parent_UID"></param>
        /// <param name="Background"></param>
        /// <param name="Foreground"></param>
        /// <param name="FontText"></param>
        /// <param name="IsNode"></param>
        public TTDL_Item(bool Enable_Filtration, int UID, int UID_Column, int UID_Row, int X, int Y, int Parent_UID, Color Background, Color Foreground, Font FontText, bool IsNode = false)
        {
            this.Enable_Filtration = Enable_Filtration;
            this.UID = UID;
            this.UID_Column = UID_Column;
            this.UID_Row = UID_Row;
            this.X = X;
            this.Y = Y;
            this.Parent_UID = Parent_UID;
            this.Background = Background;
            this.Foreground = Foreground;
            this.FontText = FontText;
            this.IsNode = IsNode;
        }
        // ------------------------------------------------------------------------------------------------------------
    }
}
