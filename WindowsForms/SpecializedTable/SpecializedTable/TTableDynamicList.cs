using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
// ************************************************************************************************************

namespace SpecializedTable
{
    class TTableDynamicList : Control
    {
        UForm_Table Table_Form = new UForm_Table();

        public DataGridView RenderTable = new DataGridView();   // Тело отрисовываемой таблицы
        public List<List<TTDL_Item>> Items = new List<List<TTDL_Item>>();   // Все элементы таблицы
        public bool Enbale = true;  // вкл/выкл отображение компонента
        public bool Enable_Move = true; // Вкл/выкл разрешение на перемещение пользователем колонок
        public bool Enable_Filtration = true;   // Вкл/выкл работу фильтра для ячеек
        public bool Freeze_FirstColumn = true;  // Вкл/выкл заморозку первой конки
        public bool Freeze_FirstRow = true; // Вкл/выкл заморозку первой строчки

        Dictionary<int, List<int>> NestedRows = new Dictionary<int, List<int>>();   // Словарь, где ключи это индексы корней дерева, а значения это индексы вложенных строк
        List<int> NestedRowsKeys = new List<int>(); // Список ключей
        List<List<int>> NestedRowsValues = new List<List<int>>();   // Список значений
        // ------------------------------------------------------------------------------------------------------------
        public TTableDynamicList()
        {
            Table_Form.Controls.Add(RenderTable);

            if (Enbale == true)
            {
                RenderTable.Visible = true;
            }
            else
            {
                RenderTable.Visible = false;
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод заполнения элементов таблицы
        /// </summary>
        public void FillItems()
        {
            List<TTDL_Item> Row1 = new List<TTDL_Item>();
            List<TTDL_Item> Row2 = new List<TTDL_Item>();
            List<TTDL_Item> Row3 = new List<TTDL_Item>();
            List<TTDL_Item> Row4 = new List<TTDL_Item>();
            List<TTDL_Item> Row5 = new List<TTDL_Item>();
            List<TTDL_Item> Row6 = new List<TTDL_Item>();
            List<TTDL_Item> Row7 = new List<TTDL_Item>();
            List<TTDL_Item> Row8 = new List<TTDL_Item>();
            List<TTDL_Item> Row9 = new List<TTDL_Item>();


            Row1.Add(new TTDL_Item(false, 0, 0, 0, 0, 0, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Andrew", true));
            Row1.Add(new TTDL_Item(true, 1, 1, 0, 1, 0, -1, Color.White, Color.Black, new Font("Consoals", 10f), "22", true));
            Row1.Add(new TTDL_Item(false, 2, 2, 0, 2, 0, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Male", true));

            Row2.Add(new TTDL_Item(false, 3, 0, 1, 0, 1, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Alex"));
            Row2.Add(new TTDL_Item(false, 4, 1, 1, 1, 1, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "19"));
            Row2.Add(new TTDL_Item(false, 5, 2, 1, 2, 1, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Male"));

            Row3.Add(new TTDL_Item(false, 6, 0, 2, 0, 2, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Kirill"));
            Row3.Add(new TTDL_Item(true, 7, 1, 2, 1, 2, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "40"));
            Row3.Add(new TTDL_Item(false, 8, 2, 2, 2, 2, 0, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Male"));

            Row4.Add(new TTDL_Item(false, 9, 0, 3, 0, 3, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Natasha", true));
            Row4.Add(new TTDL_Item(false, 10, 1, 3, 1, 3, -1, Color.White, Color.Black, new Font("Consoals", 10f), "20", true));
            Row4.Add(new TTDL_Item(false, 11, 2, 3, 2, 3, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Female", true));

            Row5.Add(new TTDL_Item(false, 12, 0, 4, 0, 4, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Diana"));
            Row5.Add(new TTDL_Item(false, 13, 1, 4, 1, 4, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "1"));
            Row5.Add(new TTDL_Item(false, 14, 2, 4, 2, 4, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Female"));

            Row6.Add(new TTDL_Item(false, 15, 0, 5, 0, 5, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Ann", true));
            Row6.Add(new TTDL_Item(true, 16, 1, 5, 1, 5, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "27", true));
            Row6.Add(new TTDL_Item(false, 17, 2, 5, 2, 5, 3, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Female", true));

            Row7.Add(new TTDL_Item(false, 18, 0, 6, 0, 6, 5, Color.LightGreen, Color.Black, new Font("Consoals", 10f), "Alex"));
            Row7.Add(new TTDL_Item(false, 19, 1, 6, 1, 6, 5, Color.LightGreen, Color.Black, new Font("Consoals", 10f), "28"));
            Row7.Add(new TTDL_Item(false, 20, 2, 6, 2, 6, 5, Color.LightGreen, Color.Black, new Font("Consoals", 10f), "Male"));

            Row8.Add(new TTDL_Item(false, 21, 0, 7, 0, 7, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Dima", true));
            Row8.Add(new TTDL_Item(false, 22, 1, 7, 1, 7, -1, Color.White, Color.Black, new Font("Consoals", 10f), "24", true));
            Row8.Add(new TTDL_Item(false, 23, 2, 7, 2, 7, -1, Color.White, Color.Black, new Font("Consoals", 10f), "Male", true));

            Row9.Add(new TTDL_Item(false, 24, 0, 8, 0, 8, 7, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Tema"));
            Row9.Add(new TTDL_Item(false, 25, 1, 8, 1, 8, 7, Color.LightPink, Color.Black, new Font("Consoals", 10f), "28"));
            Row9.Add(new TTDL_Item(false, 26, 2, 8, 2, 8, 7, Color.LightPink, Color.Black, new Font("Consoals", 10f), "Male"));

            Items.Add(Row1);
            Items.Add(Row2);
            Items.Add(Row3);
            Items.Add(Row4);
            Items.Add(Row5);
            Items.Add(Row6);
            Items.Add(Row7);
            Items.Add(Row8);
            Items.Add(Row9);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод заполнения словаря данными
        /// </summary>
        public void FillNestedRowsDictionary()
        {
            // Заполнение словаря данными
            for (int i = 0; i < Items.Count; i++)
            {
                List<int> RowsToNested = new List<int>();

                if (Items[i][0].IsNode == true && i + 1 < Items.Count)
                {
                    for (int j = i + 1; j * Items[j][0].Parent_UID >= 0; )
                    {
                        RowsToNested.Add(j);

                        if (j < Items.Count - 1)
                        {
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    NestedRows.Add(i, RowsToNested);
                }
                else if (Items[i][0].IsNode == true)
                {
                    NestedRows.Add(i, RowsToNested);
                }
            }

            // Извлечение ключей в List
            for (int i = 0; i < NestedRows.Count; i++)
            {
                NestedRowsKeys = NestedRows.Keys.ToList();
            }

            // Извлечение информации в List
            for (int i = 0; i < NestedRows.Count; i++)
            {
                NestedRowsValues = NestedRows.Values.ToList();
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Добавить колонки с указанными именами
        /// </summary>
        /// <param name="Titles"></param>
        public void AddColumn(params string[] Titles)
        {
            for (int i = 0; i < Titles.Length; i++)
            {
                DataGridViewColumn NewColumn = new DataGridViewTextBoxColumn();

                NewColumn.Name = Titles[i];

                RenderTable.Columns.Add(NewColumn);
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод создания таблицы
        /// </summary>
        public void CreateDataGrid()
        {
            AddColumn("Name", "Age", "Gender");

            FillItems();

            for (int i = 0; i < Items.Count; i++)
            {
                RenderTable.Rows.Add(Items[i][0].Value, Items[i][1].Value, Items[i][2].Value);
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Метод настройки отображения таблицы
        /// </summary>
        public void CustomizeDataGrid()
        {
            RenderTable.Dock = DockStyle.Fill;  // Изменение размера DataGrid во весь элемент управления
            RenderTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Растяжение DataGrid на всю форму
            RenderTable.BackgroundColor = SystemColors.Window;
            RenderTable.AllowUserToAddRows = false;  // remove empty row
            // RenderTable.RowHeadersVisible = false;   // Отображение столбца заголовка строк

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    RenderTable.Rows[i].Cells[j].Style.BackColor = Items[i][j].Background;
                    RenderTable.Rows[i].Cells[j].Style.ForeColor = Items[i][j].Foreground;
                    RenderTable.Rows[i].Cells[j].Style.Font = Items[i][j].FontText;
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Добавить пустую строку
        /// </summary>
        /// <param name="Node">Node это строка в которую хотим вложить новую, если Parent_UID = -1 то вкладываем в корень таблицы</param>
        public void AddRow(List<TTDL_Item> Node)
        {
            Items.Add(Node);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить все элементы в колонке по ее номеру
        /// </summary>
        /// <param name="X">номер колонки</param>
        /// <returns></returns>
        public List<TTDL_Item> GetColumn(int X)
        {
            List<TTDL_Item> AllItemsInColumnByNumb = new List<TTDL_Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (X == Items[i][j].X)
                    {
                        AllItemsInColumnByNumb.Add(Items[i][j]);
                    }
                }
            }

            return AllItemsInColumnByNumb;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить все элементы в строке по ее номеру
        /// </summary>
        /// <param name="Y">номер строки</param>
        /// <returns></returns>
        public List<TTDL_Item> GetRow(int Y)
        {
            List<TTDL_Item> AllItemsInRowByNumb = new List<TTDL_Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (Y == Items[i][j].Y)
                    {
                        AllItemsInRowByNumb.Add(Items[i][j]);
                    }
                }
            }

            return AllItemsInRowByNumb;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить все элементы в колонке по ее идентификатору
        /// </summary>
        /// <param name="ID">идентификатор колонки</param>
        /// <returns></returns>
        public List<TTDL_Item> GetColumn_ID(int ID)
        {
            List<TTDL_Item> AllItemsInColumnByID = new List<TTDL_Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (ID == Items[i][j].UID_Column)
                    {
                        AllItemsInColumnByID.Add(Items[i][j]);
                    }
                }
            }

            return AllItemsInColumnByID;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить все элементы в строке по ее идентификатору
        /// </summary>
        /// <param name="ID">идентификатор строки</param>
        /// <returns></returns>
        public List<TTDL_Item> GetRow_ID(int ID)
        {
            List<TTDL_Item> AllItemsInRowByID = new List<TTDL_Item>();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (ID == Items[i][j].UID_Row)
                    {
                        AllItemsInRowByID.Add(Items[i][j]);
                    }
                }
            }

            return AllItemsInRowByID;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить строку как узел и все входящие в нее строки по номеру строки
        /// </summary>
        /// <param name="Y">номер строки</param>
        /// <returns></returns>
        public List<List<TTDL_Item>> GetNode(int Y)
        {
            List<List<TTDL_Item>> NodeByNumb = new List<List<TTDL_Item>>();

            for (int i = 0; i < NestedRowsKeys.Count; i++)
            {
                if (NestedRowsKeys[i] == Y)
                {
                    List<TTDL_Item> Tmp = new List<TTDL_Item>();
                    for (int j = 0; j < Items[0].Count; j++)
                    {
                        Tmp.Add(Items[NestedRowsKeys[i]][j]);
                    }
                    NodeByNumb.Add(Tmp);

                    for (int k = 0; k < NestedRowsValues[i].Count; k++)
                    {
                        List<TTDL_Item> Tmp2 = new List<TTDL_Item>();
                        for (int l = 0; l < Items[0].Count; l++)
                        {
                            Tmp2.Add(Items[NestedRowsValues[i][k]][l]);
                        }
                        NodeByNumb.Add(Tmp2);
                    }
                }
            }

            return NodeByNumb;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить строку как узел и все входящие в нее строки по идентификатору строки
        /// </summary>
        /// <param name="ID">идентификатор строки</param>
        /// <returns></returns>
        public List<List<TTDL_Item>> GetNode_ID(int ID)
        {
            List<List<TTDL_Item>> NodeByID = new List<List<TTDL_Item>>();
            int NumbOfRow = -1;

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (Items[i][j].UID_Row == ID)
                    {
                        NumbOfRow = Items[i][j].Y;
                    }
                }
            }

            for (int i = 0; i < NestedRowsKeys.Count; i++)
            {
                if (NestedRowsKeys[i] == NumbOfRow)
                {
                    List<TTDL_Item> Tmp = new List<TTDL_Item>();
                    for (int j = 0; j < Items[0].Count; j++)
                    {
                        Tmp.Add(Items[NestedRowsKeys[i]][j]);
                    }
                    NodeByID.Add(Tmp);

                    for (int k = 0; k < NestedRowsValues[i].Count; k++)
                    {
                        List<TTDL_Item> Tmp2 = new List<TTDL_Item>();
                        for (int l = 0; l < Items[0].Count; l++)
                        {
                            Tmp2.Add(Items[NestedRowsValues[i][k]][l]);
                        }
                        NodeByID.Add(Tmp2);
                    }
                }
            }

            return NodeByID;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить ячейку таблицы по номеру колонки и строки
        /// </summary>
        /// <param name="X">номер колонки</param>
        /// <param name="Y">номер строки</param>
        /// <returns></returns>
        public TTDL_Item GetCell(int X, int Y)
        {
            TTDL_Item Cell = new TTDL_Item();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (X == Items[i][j].X && Y == Items[i][j].Y)
                    {
                        Cell = Items[i][j];
                    }
                }
            }

            return Cell;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Получить ячейку таблицы по идентификатору ячейки
        /// </summary>
        /// <param name="ID">идентификатор ячейки</param>
        /// <returns></returns>
        public TTDL_Item GetCell_ID(int ID)
        {
            TTDL_Item CellByID = new TTDL_Item();

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (ID == Items[i][j].UID)
                    {
                        CellByID = Items[i][j];
                    }
                }
            }

            return CellByID;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Установить новое значение в ячейку по ее идентификатору
        /// </summary>
        /// <param name="ID">идентификатор ячейки</param>
        /// <param name="Item">новое значение ячейки</param>
        public void SetCell_ID(int ID, TTDL_Item Item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (ID == Items[i][j].UID)
                    {
                        Items[i][j] = Item;
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Установить новое значение в ячейку по номеру колонки и строки
        /// </summary>
        /// <param name="X">номер колонки</param>
        /// <param name="Y">номер строки</param>
        /// <param name="Item">новое значение ячейки</param>
        public void SetCell(int X, int Y, TTDL_Item Item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    if (X == Items[i][j].X && Y == Items[i][j].Y)
                    {
                        Items[i][j] = Item;
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Установить набор значений в ячейки по порядку от начала по номеру строки
        /// </summary>
        /// <param name="Y">номер строки</param>
        /// <param name="Values">набор значений в ячейки</param>
        public void SetRow(int Y, params string[] Values)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Y == i)
                {
                    for (int j = 0; j < Items[i].Count; j++)
                    {
                        Items[i][j].Value = Values[j];
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Кол-во строк
        /// </summary>
        /// <returns></returns>
        public int Count_Rows()
        {
            if (RenderTable.AllowUserToAddRows == true)
            {
                return RenderTable.Rows.Count - 1;
            }
            else
            {
                return RenderTable.Rows.Count;
            }
            
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Кол-во столбцов
        /// </summary>
        /// <returns></returns>
        public int Count_Columns()
        {
            return RenderTable.Columns.Count;
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Удалить строку по номеру и все вложенные строки в нее
        /// </summary>
        /// <param name="Y">номер строки</param>
        public void RemoveRow(int Y)
        {
            List<List<TTDL_Item>> NewItems = new List<List<TTDL_Item>>();

            for (int i = 0; i < Items.Count; i++)
            {
                List<TTDL_Item> NewItemsRow = new List<TTDL_Item>();

                if (i == Y)
                {
                    if (Items[i][0].IsNode == true)
                    {
                        while (i == Y || Items[i][0].Parent_UID != -1)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                for (int j = 0; j < Items[i].Count; j++)
                {
                    NewItemsRow.Add(Items[i][j]);
                }
                NewItems.Add(NewItemsRow);
            }

            Items.Clear();

            for (int i = 0; i < NewItems.Count; i++)
            {
                List<TTDL_Item> NewItemsRow = new List<TTDL_Item>();

                for (int j = 0; j < NewItems[i].Count; j++)
                {
                    NewItemsRow.Add(NewItems[i][j]);
                }
                Items.Add(NewItemsRow);
            }

            NestedRows.Clear();
            this.FillNestedRowsDictionary();

            RenderTable.Rows.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                RenderTable.Rows.Add(Items[i][0].Value, Items[i][1].Value, Items[i][2].Value);
            }

            this.CustomizeDataGrid();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Удалить колонку по номеру
        /// </summary>
        /// <param name="X">номер колонки</param>
        public void RemoveColumn(int X)
        {
            RenderTable.Columns.RemoveAt(X);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Удалить все
        /// </summary>
        public void RemoveAll()
        {
            RenderTable.Rows.Clear();
            RenderTable.Columns.Clear();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Экспортировать таблицу в Эксель
        /// </summary>
        /// <param name="FileName">имя файла Excel</param>
        public void ExportToExcel(string FileName)
        {
            Excel.Application XlApp = new Excel.Application();
            Excel.Workbook XlWorkBook = XlApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet XlWorkSheet = XlWorkBook.Worksheets.get_Item(1);

            XlApp.Visible = true;

            for (int i = 0; i < RenderTable.Columns.Count; i++)
            {
                XlWorkSheet.Cells[i + 2][1] = RenderTable.Columns[i].Name;
            }

            for (int i = 0; i < RenderTable.Rows.Count; i++)
            {
                XlWorkSheet.Cells[1][i + 2] = RenderTable.Rows[i].HeaderCell.Value;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                for (int j = 0; j < Items[i].Count; j++)
                {
                    XlWorkSheet.Cells[j + 2][i + 2] = RenderTable.Rows[i].Cells[j].Value;
                    XlWorkSheet.Cells[j + 2][i + 2].Interior.Color = Items[i][j].Background;
                    XlWorkSheet.Cells[j + 2][i + 2].Font.Color = Items[i][j].Foreground;
                    XlWorkSheet.Cells[j + 2][i + 2].Font.Name = Items[i][j].FontText.Name;
                    XlWorkSheet.Cells[j + 2][i + 2].Font.Size = Items[i][j].FontText.Size;
                }
            }

            Console.Write("Close excel - ");
            Console.ReadLine();

            // XlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            XlWorkBook.Close();
            XlApp.Quit();
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
            {
                proc.Kill();
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Копировать таблицу в буфер обмена
        /// </summary>
        public void CopyToClipboard()
        {
            RenderTable.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

            RenderTable.SelectAll();
            DataObject DataObj = RenderTable.GetClipboardContent();
            Clipboard.SetDataObject(DataObj, true);
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Вставить таблицу из буфера обмена
        /// </summary>
        public void PasteFromClipboard()
        {
            IDataObject IDataObj = Clipboard.GetDataObject();
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// метод позволяет методом перетягивания переставлять местами столбцы/строки в рамках узла дерева
        /// </summary>
        public void TableReorder()
        {
            RenderTable.AllowUserToOrderColumns = true; // Разрешение пользователю изменять порядок столбцов
            if (Enable_Move == true)
            {
                RenderTable.AllowDrop = true;   // Разрешить операцию перетаскивания
            }

            RenderTable.MouseDoubleClick += new MouseEventHandler(this.RenderTable_MouseDoubleClick);
            RenderTable.DragEnter += new DragEventHandler(this.RenderTable_DragEnter);
            RenderTable.DragDrop += new DragEventHandler(this.RenderTable_DragDrop);
        }

        int RowIndexFromMouseDown;
        DataGridViewRow Row;

        private void RenderTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (RenderTable.SelectedRows.Count == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Row = RenderTable.SelectedRows[0];
                    RowIndexFromMouseDown = RenderTable.SelectedRows[0].Index;

                    RenderTable.DoDragDrop(Row, DragDropEffects.Move);
                }
            }
        }
        private void RenderTable_DragEnter(object sender, DragEventArgs e)
        {
            if (RenderTable.SelectedRows.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
        }
        private void RenderTable_DragDrop(object sender, DragEventArgs e)
        {

            int RowIndexOfItemUnderMouseToDrop;
            Point ClientPoint = RenderTable.PointToClient(new Point(e.X, e.Y));
            RowIndexOfItemUnderMouseToDrop = RenderTable.HitTest(ClientPoint.X, ClientPoint.Y).RowIndex;

            if (RenderTable.Rows[0].Frozen == true)
            {
                if (e.Effect == DragDropEffects.Move && RowIndexOfItemUnderMouseToDrop >= 1 && RowIndexFromMouseDown != 0)
                {
                    RenderTable.Rows.RemoveAt(RowIndexFromMouseDown);
                    RenderTable.Rows.Insert(RowIndexOfItemUnderMouseToDrop, Row);
                }
            }
            else
            {
                if (e.Effect == DragDropEffects.Move && RowIndexOfItemUnderMouseToDrop >= 0)
                {
                    RenderTable.Rows.RemoveAt(RowIndexFromMouseDown);
                    RenderTable.Rows.Insert(RowIndexOfItemUnderMouseToDrop, Row);
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// метод позволяет делать невидимыми указанные столбцы
        /// </summary>
        /// <param name="Name"></param>
        public void HideColumns(params string[] Name)
        {
            for (int i = 0; i < RenderTable.Columns.Count; i++)
            {
                for (int j = 0; j < Name.Length; j++)
                {
                    if (RenderTable.Columns[i].Name == Name[j])
                    {
                        RenderTable.Columns[i].Visible = false;
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// метод позволяет сворачивать/разворачивать строки
        /// </summary>
        public void RollUpAndExpandRows()
        {
            
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i][0].IsNode == true && NestedRows[i].Count != 0)
                {
                    RenderTable.Rows[i].HeaderCell.Value = "+";
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i][0].Parent_UID != -1)
                {
                    RenderTable.Rows[i].Visible = false;
                }
            }

            RenderTable.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.RenderTable_RowHeaderMouseClick);
        }
        private void RenderTable_RowHeaderMouseClick(object sender, MouseEventArgs e)
        {
            if ((string)RenderTable.SelectedRows[0].HeaderCell.Value == "+")
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[RenderTable.SelectedRows[0].Index][0].Y == Items[i][0].Parent_UID)
                    {
                        if (RenderTable.Rows[i].Visible == false)
                        {
                            RenderTable.Rows[i].Visible = true;
                        }
                        else
                        {
                            int Tmp = 0;

                            for (int j = 0; j < NestedRowsKeys.Count; j++)
                            {
                                if (RenderTable.SelectedRows[0].Index == NestedRowsKeys[j])
                                {
                                    Tmp = j;
                                }
                            }

                            for (int j = 0; j < NestedRowsValues[Tmp].Count; j++)
                            {
                                for (int k = 0; k < Items.Count; k++)
                                {
                                    if (RenderTable.Rows[k].Index == NestedRowsValues[Tmp][j])
                                    {
                                        RenderTable.Rows[k].Visible = false;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// метод позволяет "замораживать" первую строку и первый столбец
        /// </summary>
        public void FreezeFirstRowAndColumn()
        {
            if (Freeze_FirstColumn == true)
            {
                RenderTable.Columns["Name"].Frozen = true;
            }

            if (Freeze_FirstRow == true)
            {
                RenderTable.Rows[0].Frozen = true;
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// фильтр на вводимые пользователем в ячейку значения
        /// </summary>
        public void Filter()
        {
            if (Enable_Filtration == true)
            {
                int IntValue = 0;

                for (int i = 0; i < Items.Count; i++)
                {
                    for (int j = 0; j < Items[i].Count; j++)
                    {
                        if (Items[i][j].Enable_Filtration == true)
                        {
                            IntValue = Convert.ToInt32(RenderTable.Rows[i].Cells[j].Value);
                            RenderTable.Rows[i].Cells[j].Value = IntValue;
                        }
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// отображение формы
        /// </summary>
        public void ShowForm()
        {
            Table_Form.Show();
        }
    }
}
