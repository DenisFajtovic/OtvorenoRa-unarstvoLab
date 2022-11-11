using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebApplication1.Pages
{

    

    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }
        public List<string> attrib = new List<string>();
        public List<List<string>> tabledata = new List<List<string>>();
        public void OnGet()
        {
            //LoadInitiaDataAsyinc();
        }
        public void LoadInitiaDataAsyinc()
        {
            //string strConnString = "Server=remote_server;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();
                string strpostgracecommand = "select * from \"Book\"";
                NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                DataTable dat = new DataTable();
                objDataAdapter.Fill(dat);
                for (int i = 0; i < dat.Columns.Count; i++)
                    attrib.Add(dat.Columns[i].ColumnName);
                for (int i = 0; i < dat.Rows.Count; i++)
                {
                    List<string> t = new List<string>();
                    var datt = dat.Rows[i].ItemArray;
                    for (int j = 0; j < datt.Length; j++)
                    {
                        t.Add(datt[j].ToString());
                    }
                    tabledata.Add(t);
                    Debug.WriteLine(i);
                    //for(string s in a.)
                }
                objpostgraceConn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
