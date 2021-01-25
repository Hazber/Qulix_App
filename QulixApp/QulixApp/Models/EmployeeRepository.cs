using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QulixApp.Domain;

namespace QulixApp.Models
{
    public class EmployeeRepository
    {
        public bool AddEmployee(EmployeeClass Employee)
        {
            Employee.EmployeeID = AddEmployee(Name: Employee.Name, Surname: Employee.Surname,Patronymic: Employee.Patronymic, CompanyID: Employee.Company.CompanyID, EmployeePosition: Employee.EmployeePosition);
            return Employee.EmployeeID > 0;
        }

        public int AddEmployee(string Name, string Surname,string Patronymic ,int CompanyID, Position EmployeePosition)
        {
            int ID = 0;
            using (MySqlConnection connect = new MySqlConnection(Base.strConnect))
            {
                string sql = "INSERT INTO `Employees` (`Name`,`Surname`,`Patronymic`, `CompanyID`,`EmployeePosition`) VALUES (@Name,@Surname,@Patronymic, @CompanyID, @Email, @EmployeePosition)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.Add("Name", MySqlDbType.String).Value = Name;
                    cmd.Parameters.Add("Surname", MySqlDbType.String).Value = Surname;
                    cmd.Parameters.Add("Patronymic", MySqlDbType.String).Value = Patronymic;
                    cmd.Parameters.Add("CompanyID", MySqlDbType.Int32).Value = CompanyID;
                    cmd.Parameters.Add("EmployeePosition", MySqlDbType.Int32).Value = EmployeePosition;
                    connect.Open();
                    if (cmd.ExecuteNonQuery() >= 0)
                    {
                        sql = "SELECT LAST_INSERT_ID() AS ID";
                        cmd.CommandText = sql;
                        int.TryParse(cmd.ExecuteScalar().ToString(), out ID);
                    }
                }
            }
            return ID;
        }

        public bool ChangeEmployee(EmployeeClass Employee)
        {
            return ChangeEmployee(ID: Employee.EmployeeID, Name: Employee.Name, Surname: Employee.Surname, Patronymic: Employee.Patronymic, CompanyID: Employee.Company.CompanyID, EmployeePosition: Employee.EmployeePosition);
        }

        public bool ChangeEmployee(int ID, string Name, string Surname, string Patronymic, int CompanyID, Position EmployeePosition)
        {
            bool result = false;
            if (ID > 0)
            {
                using (MySqlConnection connect = new MySqlConnection(Base.strConnect))
                {
                    string sql = "UPDATE `Employees` SET `Name`=@Name,`Surname`=@Surname,`Patronymic`=@Patronymic, `CompanyID`=@CompanyID,`EmployeePosition`=@EmployeePosition WHERE EmployeeID=@EmployeeID";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                    {
                        cmd.Parameters.Add("EmployeeID", MySqlDbType.Int32).Value = ID;
                        cmd.Parameters.Add("Loginname", MySqlDbType.String).Value = Name;
                        cmd.Parameters.Add("Surname", MySqlDbType.String).Value = Surname;
                        cmd.Parameters.Add("Patronymic", MySqlDbType.String).Value = Patronymic;
                        cmd.Parameters.Add("CompanyID", MySqlDbType.Int32).Value = CompanyID;
                        cmd.Parameters.Add("EmployeePosition", MySqlDbType.Int32).Value = EmployeePosition;
                        connect.Open();
                        result = cmd.ExecuteNonQuery() >= 0;
                    }
                }
            }
            return result;
        }

        public bool RemoveEmployee(EmployeeClass Employee)
        {
            return RemoveEmployee(Employee.EmployeeID);
        }

        public bool RemoveEmployee(int ID)
        {
            using (MySqlConnection connect = new MySqlConnection(Base.strConnect))
            {
                string sql = "DELETE FROM `Employees` WHERE `EmployeeID`=@EmployeeID";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.Add("EmployeeID", MySqlDbType.Int32).Value = ID;
                    connect.Open();
                    return cmd.ExecuteNonQuery() >= 0;
                }
            }
        }

        public EmployeeClass FetchByID(int ID)
        {
            EmployeeClass Employee = null;
            using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
            {
                string strSQL = "SELECT e.`EmployeeID`, e.`Loginname`, c.`CompanyID`, c.`CompanyName`,c`CompanyOrganizationalForm`, e.`EmploymentDate`, CAST(e.`EmployeePosition` AS UNSIGNED) as `EmployeePosition` FROM `Employees` e LEFT JOIN `Companys` c ON c.CompanyID=c.CompanyID WHERE `EmployeeID`=@EmployeeID";
                using (MySqlCommand cmd = new MySqlCommand(strSQL, objConnect))
                {
                    objConnect.Open();
                    int EmployeeID = 0, CompanyID = 0;
                    string Name = null, Surname = null, Patronymic = null, CompanyName = null;
                    Position EmployeePosition = Position.None;
                    OrganizationalForm CompanyOrganizationalForm = OrganizationalForm.None;
                    DateTime? EmploymentDate = null;
                    cmd.Parameters.Add("EmployeeID", MySqlDbType.Int32).Value = ID;
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            EmployeeID = dr.GetInt32("EmployeeID");
                            Name = dr.GetString("Name").ToString();
                            Surname = dr.GetString("Surname").ToString();
                            Patronymic = dr.GetString("Patronymic").ToString();
                            CompanyID = dr.GetInt32("CompanyID");
                            CompanyName = dr.GetString("CompanyName").ToString();
                            EmploymentDate = dr.GetDateTime("EmploymentDate");
                            if (!dr.IsDBNull(dr.GetOrdinal("CompanyOrganizationalForm"))) CompanyOrganizationalForm = (OrganizationalForm)dr.GetInt32("CompanyOrganizationalForm");
                            if (!dr.IsDBNull(dr.GetOrdinal("EmployeePosition"))) EmployeePosition = (Position)dr.GetInt32("EmployeePosition");
                        }
                        CompanyClass Company = null;
                        if (CompanyID > 0) Company = new CompanyClass(CompanyID: CompanyID, CompanyName: CompanyName,CompanyOrganizationalForm: CompanyOrganizationalForm);
                        if (EmployeeID > 0 && Company != null && Company.CompanyID > 0) Employee = new EmployeeClass(EmployeeID: EmployeeID, Name: Name, Surname: Surname, Patronymic: Patronymic, Company: Company,EmploymentDate: EmploymentDate, EmployeePosition: EmployeePosition);
                    }
                }
            }
            return Employee;
        }

        public EmployeeClass FetchByLoginname(string Name)
        {
            EmployeeClass Employee = null;
            using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
            {
                string strSQL = "SELECT e.`EmployeeID`, e.`Name`,e.`Surname`,e.`Patronymic`, c.`CompanyID`, c.`CompanyName`,c.`CompanyOrganizationalForm`, e.`EmploymentDate`, CAST(e.`EmployeePosition` AS UNSIGNED) as `EmployeePosition` FROM `Employees` e LEFT JOIN `Companys` c ON c.CompanyID=e.CompanyID WHERE `Name`=@Name";
                using (MySqlCommand cmd = new MySqlCommand(strSQL, objConnect))
                {
                    objConnect.Open();
                    int EmployeeID = 0, CompanyID = 0;
                    string EmployeeName = null, Surname = null, Patronymic = null, CompanyName = null;
                    Position EmployeePosition = Position.None;
                    OrganizationalForm CompanyOrganizationalForm = OrganizationalForm.None;
                    DateTime? EmploymentDate = null;
                    cmd.Parameters.Add("Name", MySqlDbType.Int32).Value = Name;
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            EmployeeID = dr.GetInt32("EmployeeID");
                            EmployeeName = dr.GetString("Name").ToString();
                            Surname = dr.GetString("Surname").ToString();
                            Patronymic = dr.GetString("Patronymic").ToString();
                            CompanyID = dr.GetInt32("CompanyID");
                            CompanyName = dr.GetString("CompanyName").ToString();
                            EmploymentDate = dr.GetDateTime("EmploymentDate");
                            if (!dr.IsDBNull(dr.GetOrdinal("CompanyOrganizationalForm"))) CompanyOrganizationalForm = (OrganizationalForm)dr.GetInt32("CompanyOrganizationalForm");
                            if (!dr.IsDBNull(dr.GetOrdinal("EmployeePosition"))) EmployeePosition = (Position)dr.GetInt32("EmployeePosition");
                        }
                        CompanyClass Company = null;
                        if (CompanyID > 0) Company = new CompanyClass(CompanyID: CompanyID, CompanyName: CompanyName, CompanyOrganizationalForm: CompanyOrganizationalForm);
                        if (EmployeeID > 0 && Company != null && Company.CompanyID > 0) Employee = new EmployeeClass(EmployeeID: EmployeeID, Name: EmployeeName, Surname: Surname, Patronymic: Patronymic, Company: Company, EmploymentDate: EmploymentDate, EmployeePosition: EmployeePosition);
                    }
                }
            }
            return Employee;
        }

        // Стандартная и очень привлекательная практика для ASP.NET WebForms, поскольку позволяет в компонентах для отображения данных напрямую обращаться к данным в ObjectDataSource без образования специальной типизированной модели
        // Но так писать не надо, потому что в представлении мы вынуждены будем использовать "магические строки" для получения доступа к значениям в строке данных
        //public IEnumerable<DataRow> List()
        //{
        //    using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
        //    {
        //        string strSQL = "select * from Employees";
        //        using (MySqlCommand objCommand = new MySqlCommand(strSQL, objConnect))
        //        {
        //            objConnect.Open();
        //            using (MySqlDataAdapter da = new MySqlDataAdapter(objCommand))
        //            {
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);
        //                return dt.AsEnumerable();
        //            }
        //        }
        //    }
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Проверка запросов SQL на уязвимости безопасности")]
        public IList<EmployeeClass> List(string sortOrder, System.Web.Helpers.SortDirection sortDir, int page, int pagesize, out int count)
        {
            List<EmployeeClass> Employees = new List<EmployeeClass>();
            using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
            {
                // добавляем в запрос сортировку
                string sort = " ORDER BY ";
                // это плохая практика, потому что запрос может быть взломан при удачном встраивании в него некоей текстовой строки (inject)
                // но, к сожалению, MySQL не дает возможности использовать параметры для сортировки
                // поэтому надо экранировать кавычками, но перед этим обеспечить сначала проверку входного значения (чтобы тех же кавычек в нём не было)
                // в нашем проекте проверка значения идет в контроллере, перед простроением модели
                if (sortOrder != null && sortOrder != String.Empty)
                {
                    sort += "`" + sortOrder + "`";
                    if (sortDir == System.Web.Helpers.SortDirection.Descending) sort += " DESC";
                    sort += ",";
                }
                sort += "`EmployeeID`"; // по умолчанию
                // добавляем в запрос отображение только части записей (отображение страницами)
                string limit = "";
                if (pagesize > 0)
                {
                    int start = (page - 1) * pagesize;
                    limit = string.Concat(" LIMIT ", start.ToString(), ", ", pagesize.ToString());
                }
                string strSQL = "SELECT SQL_CALC_FOUND_ROWS e.`EmployeeID`, e.`Loginname`,e.`Surname`,e.`Patronymic`, c.`CompanyID`,c.`CompanyOrganizationalForm`, c.`CompanyName` as `Company`, e.`Employment`, CAST(e.`EmployeePosition` AS UNSIGNED) as `EmployeePosition` FROM `Employees` e LEFT JOIN `Companys` c ON c.CompanyID=e.CompanyID" + sort + limit;
                using (MySqlCommand cmd = new MySqlCommand(strSQL, objConnect))
                {
                    objConnect.Open();
                    cmd.Parameters.Add("page", MySqlDbType.Int32).Value = page;
                    cmd.Parameters.Add("pagesize", MySqlDbType.Int32).Value = pagesize;
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                           
                            CompanyClass Company = new CompanyClass(CompanyID: dr.GetInt32("CompanyID"), CompanyName: dr.GetString("Company").ToString(),CompanyOrganizationalForm: dr.IsDBNull(dr.GetOrdinal("EmployeePosition")) ? (OrganizationalForm)OrganizationalForm.None : (OrganizationalForm)dr.GetInt32("CompanyOrganizationalForm"));

                            Employees.Add(new EmployeeClass(
                                EmployeeID: dr.GetInt32("EmployeeID"),
                                Name: dr.GetString("Name"),
                                Surname: dr.GetString("Surname"),
                                Patronymic: dr.GetString("Patronymic"),
                                Company: Company,
           
                                EmploymentDate: dr.IsDBNull(dr.GetOrdinal("EmploymentDate")) ? (DateTime?)null : dr.GetDateTime("EmploymentDate"),
                                EmployeePosition: dr.IsDBNull(dr.GetOrdinal("EmployeePosition")) ? (Position)Position.None : (Position)dr.GetInt32("EmployeePosition")));
                        }
                    }
                }
                // получаем общее количество пользователей
                using (MySqlCommand cmdrows = new MySqlCommand("SELECT FOUND_ROWS()", objConnect))
                {
                    int.TryParse(cmdrows.ExecuteScalar().ToString(), out count);
                }
            }
            return Employees;
        }




    }
}