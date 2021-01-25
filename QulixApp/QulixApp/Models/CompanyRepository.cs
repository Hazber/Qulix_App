using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QulixApp.Domain;


namespace QulixApp.Models
{
    public class CompanyRepository
    {
        public IList<CompanyClass> List()
        {
            List<CompanyClass> companies = new List<CompanyClass>();
            using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
            {
                string strSQL = "SELECT `CompanyID`,`CompanyOrganizationalForm`, `CompanyName` as `Company` FROM `Companys` ORDER BY `CompanyName`";
                using (MySqlCommand cmd = new MySqlCommand(strSQL, objConnect))
                {
                    objConnect.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CompanyClass language = new CompanyClass(CompanyID: dr.GetInt32("LanguageID"), CompanyName: dr.GetString("Language").ToString(), CompanyOrganizationalForm : (OrganizationalForm)dr.GetInt32("CompanyOrganizationalForm"));
                            companies.Add(language);
                        }
                    }
                }
            }
            return companies;
        }
    }
}