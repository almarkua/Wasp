using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;


namespace Front_End_WASP
{
    class DB
    {
        private static DB instance = null;
        private SqlCeConnection connect=null;
        private DB() {
            try
            {
                Connect("db_wasp.sdf");
                connect.Open();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Помилка підключення до бази даних: " + ex.Message);
            }
            catch (SqlCeException ex) {
                MessageBox.Show("Помилка відкриття зєднання з базою даних! " + ex.Message);
            }
        }

        private void Connect(string dataBase) {
            try
            {
                string dataSource = "Data Source=" + dataBase;
                connect = new SqlCeConnection(dataSource);
            }
            catch (Exception ex) {
                throw new ArgumentException("Database not found! "+ex.Message);
            }
             
        }

        public static DB getInstance() {
            if (instance == null)
            {
                instance = new DB();
            }
            return instance;
        }

        //повертає кількість змінених/доданих рядків
        public int executeNonQuery(string sql) {
            try
            {
                SqlCeCommand command = new SqlCeCommand(sql, connect);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(sql + " " + ex.Message);
            }
            return 0;
        }
        
        //повертає результат select
        public IDataReader execute(string sql) {
            try
            {
                SqlCeCommand command = new SqlCeCommand(sql, connect);
                return command.ExecuteReader();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        static SqlCeDataAdapter dataAdapter;

        public DataTable executeForUpdate(string sql) {
            dataAdapter = new SqlCeDataAdapter(sql, connect);
            SqlCeCommandBuilder cmdBuilder = new SqlCeCommandBuilder(dataAdapter);
            string ts=cmdBuilder.GetInsertCommand().CommandText;
            DataSet dSet=new DataSet("dt");
            dataAdapter.Fill(dSet,"dt");
            return dSet.Tables[0];
        }

        public DataTable exec(string sql) {
            try
            {
                dataAdapter = new SqlCeDataAdapter(sql, connect);
                DataSet dSet = new DataSet("dt");
                dataAdapter.Fill(dSet, "dt");
                return dSet.Tables[0];
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public DataSet execMoreQuery(string[] sqls, string[] names)
        {
            DataSet resultSet=new DataSet();
            try
            {
                for (int i = 0; i < sqls.Length; i++)
                {
                    dataAdapter = new SqlCeDataAdapter(sqls[i], connect);
                    dataAdapter.Fill(resultSet, names[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return resultSet;

        }

        public void update(DataTable dt) {
            dataAdapter.Update(dt);
        }

        ~DB() {
            connect.Close();
        }
    }
}
