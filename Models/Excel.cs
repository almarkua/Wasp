using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exc=Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Drawing;

namespace Front_End_WASP
{
    class Excel
    {
        public static void writeStudyToExcel(string fileName, int idStudy) {
            Model model = new Model();
            Exc.Application ObjExcel=null;
            Exc.Workbook ObjWorkBook=null;
            Exc.Worksheet ObjWorkSheet=null;
            try
            {
                //Приложение самого Excel
                ObjExcel = new Exc.Application();
                //Книга.
                ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);
                //Таблица.
                ObjWorkSheet = (Exc.Worksheet)ObjWorkBook.Sheets[1];

                IDataReader study = model.getStudyForId(idStudy);
                IDataReader scenaries = model.getScanariosForStudy(idStudy);
                study.Read();
                ObjExcel.Cells[1, 1] = study.GetValue(0).ToString();
                ObjExcel.Cells[2, 1] = study.GetValue(1).ToString();

                int rowNumber = 5;
                int marginRow = 2;

                while (scenaries.Read())
                {
                    ObjExcel.Cells[rowNumber, 1] = "Year:";
                    ObjExcel.Cells[rowNumber, 2] = scenaries.GetValue(1).ToString();
                    ObjExcel.Cells[++rowNumber, 1] = "Period:";
                    ObjExcel.Cells[rowNumber, 2] = scenaries.GetValue(2).ToString();
                    ObjExcel.Cells[++rowNumber, 1] = "Hydrocondition:";
                    ObjExcel.Cells[rowNumber, 2] = scenaries.GetValue(3).ToString();
                    ObjExcel.Cells[++rowNumber, 1] = "Probality:";
                    ObjExcel.Cells[rowNumber, 2] = scenaries.GetValue(4).ToString();

                    IDataReader hydroData = new Model().getHydroDataForScenario(idStudy, int.Parse(scenaries.GetValue(1).ToString()), int.Parse(scenaries.GetValue(2).ToString()), int.Parse(scenaries.GetValue(3).ToString()));
                    
                    rowNumber += marginRow;
                    while (hydroData.Read())
                    {
                        for (int i = 1; i < 16; i++)
                        {
                            ObjExcel.Cells[rowNumber, i] = hydroData.GetValue(i - 1).ToString();
                        }
                        rowNumber++;
                    }

                    IDataReader thermalData = model.getThermalDataForScenario(idStudy, int.Parse(scenaries.GetValue(1).ToString()), int.Parse(scenaries.GetValue(2).ToString()), int.Parse(scenaries.GetValue(3).ToString()));
                    rowNumber += marginRow;
                    while (thermalData.Read())
                    {
                        for (int i = 1; i < 16; i++)
                        {
                            ObjExcel.Cells[rowNumber, i] = thermalData.GetValue(i - 1).ToString();
                        }
                        rowNumber++;
                    }
                    rowNumber += marginRow;
                }
                ObjWorkBook.SaveAs(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            finally {
                //Закрытие книгу Excel.
                ObjWorkBook.Close();
                //Закрытие приложения Excel.
                ObjExcel.Quit();
                //Обнуляем созданые объекты
                ObjWorkBook = null;
                ObjWorkSheet = null;
                ObjExcel = null;
                //Вызываем сборщик мусора для их уничтожения и освобождения памяти
                GC.Collect();
            }
        }

        public static void saveData(string file, DataTable data) {
            Exc.Application ObjExcel = null;
            Exc.Workbook ObjWorkBook = null;
            Exc.Worksheet ObjWorkSheet = null;
            try
            {
                int[] maxLengths = new int[data.Columns.Count];
                //Приложение самого Excel
                ObjExcel = new Exc.Application();
                //Книга.
                ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);
                //Таблица.
                ObjWorkSheet = (Exc.Worksheet)ObjWorkBook.Sheets[1];
                for (int i = 0; i < data.Columns.Count; i++) {
                    ObjExcel.Cells[1, i + 1] = data.Columns[i].ColumnName;
                }
                ObjWorkSheet.get_Range("A1", getStringCell(data.Columns.Count) + "1").HorizontalAlignment = Exc.XlHAlign.xlHAlignCenter;
                ObjWorkSheet.get_Range("A1", getStringCell(data.Columns.Count)+"1").Font.Bold = true;
                ObjWorkSheet.get_Range("A1", getStringCell(data.Columns.Count) + "1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray); 
                ObjWorkSheet.get_Range("A1", getStringCell(data.Columns.Count) + "1").Borders.Weight = 2;
                ObjWorkSheet.get_Range("A1", getStringCell(data.Columns.Count) + "1").Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                                if (data.Rows[i].ItemArray[j].ToString().Length > maxLengths[j])
                                {
                                    maxLengths[j] = data.Rows[i].ItemArray[j].ToString().Length;
                                }
                            ObjExcel.Cells[i + 2, j + 1] = data.Rows[i].ItemArray[j];
                        }
                    }
                    for (int i = 0; i < maxLengths.Length; i++)
                    {
                        ObjExcel.get_Range(getStringCell(i+1) + 1.ToString() + ":" + getStringCell(i+1) + 1.ToString()).ColumnWidth = maxLengths[i] * 1.4;
                    }

                    ObjExcel.Cells[data.Rows.Count + 3, data.Columns.Count - 1] = "Дата створення:";
                    ObjExcel.Cells[data.Rows.Count + 3, data.Columns.Count] = DateTime.Now.ToLongDateString().ToString();
                    ObjWorkSheet.get_Range(getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString(), getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString()).HorizontalAlignment = Exc.XlHAlign.xlHAlignCenter;
                    ObjWorkSheet.get_Range(getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString(), getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString()).Font.Bold = true;
                    ObjWorkSheet.get_Range(getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString(), getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    ObjWorkSheet.get_Range(getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString(), getStringCell(data.Columns.Count) + (data.Rows.Count + 3).ToString()).Borders.Weight = 2;
                    ObjWorkSheet.get_Range(getStringCell(data.Columns.Count - 1) + (data.Rows.Count + 3).ToString(), getStringCell(data.Columns.Count) + (data.Rows.Count + 3).ToString()).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                ObjWorkBook.SaveAs(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка роботи із Excel. Код помилки: "+ex.Message, "Error");
            }

            finally
            {
                //Закрытие книгу Excel.
                ObjWorkBook.Close();
                //Закрытие приложения Excel.
                ObjExcel.Quit();
                //Обнуляем созданые объекты
                ObjWorkBook = null;
                ObjWorkSheet = null;
                ObjExcel = null;
                //Вызываем сборщик мусора для их уничтожения и освобождения памяти
                GC.Collect();
                string appid = "Excel.Application";
                Type excelType = Type.GetTypeFromProgID(appid);
                object excelAppInstance = Activator.CreateInstance(excelType);
                object appWorkkbooks = excelType.InvokeMember("Workbooks", BindingFlags.GetProperty, null, excelAppInstance, null);
                object workbook = appWorkkbooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, appWorkkbooks, new object[] { @file });
                excelType.InvokeMember("Visible", BindingFlags.SetProperty, null, excelAppInstance, new object[] { true });
            }
        }

        private static string getStringCell(int number)
        {
            string result = "";
            int count = 0;
            while (number > 26)
            {
                number -= 26;
                count++;
            }
            if (count > 0) result += getStringCell(count);
            if (number > 0)
            {
                result += (char)(64 + number);
            }
            return result;
        }
    }
}
