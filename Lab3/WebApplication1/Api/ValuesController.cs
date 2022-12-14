using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class DataStructure
    {
        public List<string> RowNames { get; set; } 
        public List<List<string>> Data { get; set; }
        public DataStructure()
        {
            RowNames = new List<string>();
        }
        public DataStructure(List<string> rn, List<List<string>> d)
        {
            RowNames = rn;
            Data = d;
        }
    }
    //[Route("api/database")]
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {

        public DatabaseController()
        { }


        // GET: api/<ValuesController>
        //api za sve podatke
        [HttpGet]
        public DataStructure Get()
        {
            /*
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            return new string[] { "value1", "value2" };
            */
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                List<string> attrib = new List<string>();
                List<List<string>> tabledata = new List<List<string>>();
                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();
                string strpostgracecommand = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";
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
                    //for(string s in a.)
                }
                objpostgraceConn.Close();
                //DataStructure[] rt = { new DataStructure(attrib, tabledata) };
                DataStructure rt = new DataStructure(attrib, tabledata);
                return rt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;// new DataStructure[]{ new DataStructure() };
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        //api za sve podatke s odabirom u scv ili json formatu
        [HttpGet("{mode}")]
        public async Task<ActionResult> Get(string mode) //DataStructure
        {

            /*
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            return new string[] { "value1", "value2" };
            */
            if (mode != "1" && mode != "0")
                return null;
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                string jsoncmd = "SET client_encoding TO 'UTF8'; select array_to_json(array_agg(row_to_json(t))) from ( select b.\"Id\", json_build_object( 'Ime', f.\"Ime\", 'Prezime', f.\"Prezime\" ) as Fotograf, json_build_object( 'Ime', m.\"Ime\", 'Prezime', m.\"Prezime\" ) as Mladenac, json_build_object( 'Ime', a.\"Ime\", 'Prezime', a.\"Prezime\" ) as Mladenka, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\" ) t";
                string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";
                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();

                string strpostgracecommand = jsoncmd;
                string ext = "json";
                if (mode == "1")
                {
                    strpostgracecommand = csvcmd;
                    ext = "csv";
                }

                NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                DataTable dat = new DataTable();
                objDataAdapter.Fill(dat);

                if (!Directory.Exists("tmp"))
                    Directory.CreateDirectory("tmp");

                string filept = "tmp\\data." + ext;

                StreamWriter sw = new StreamWriter(filept);

                if (mode == "1")
                {
                    for (int i = 0; i < dat.Columns.Count; i++)
                    {
                        string s = dat.Columns[i].ColumnName;
                        sw.Write(dat.Columns[i].ColumnName);
                        if (i < dat.Columns.Count-1)
                            sw.Write(',');
                    }
                    sw.WriteLine();
                }
                for (int i = 0; i < dat.Rows.Count; i++)
                {
                    //List<string> t = new List<string>();
                    var datt = dat.Rows[i].ItemArray;
                    for (int j = 0; j < datt.Length; j++)
                    {
                        if (mode == "0")
                            sw.Write(datt[j].ToString());
                        else
                        {
                            sw.Write(datt[j].ToString());
                            if (j < datt.Length-1)
                                sw.Write(',');
                        }
                        
                    }
                    sw.WriteLine();
                }
                objpostgraceConn.Close();
                sw.Dispose();
                /*
                //slanje datoteke
                FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(filept), "application/text")
                {
                    FileDownloadName = "myFile.json"
                };

                return result;
                */


                //Stream stream = new FileStream(filept, FileMode.Open);
                var bytes = await System.IO.File.ReadAllBytesAsync(filept);
                return File(bytes, "text/plain", Path.GetFileName(filept));

                /*
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(filept, FileMode.Open, FileAccess.Read);
                result.Content = new StreamContent(stream);
                //result.Content = System.IO.File.ReadAllBytes(filept);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "data." + ext
                };
                
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                return result;
                */

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
            //return null;
        }

        //api za wildcard filtrirano biranje
        // GET api/<ValuesController>/5
        [HttpGet("{mode}/{arg}")]
        public async Task<ActionResult> Get(string mode, string arg)
        {

            if (mode != "1" && mode != "0")
                return null;
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                string jsoncmd = "SET client_encoding TO 'UTF8'; select array_to_json(array_agg(row_to_json(t))) from ( select b.\"Id\", json_build_object( 'Ime', f.\"Ime\", 'Prezime', f.\"Prezime\" ) as Fotograf, json_build_object( 'Ime', m.\"Ime\", 'Prezime', m.\"Prezime\" ) as Mladenac, json_build_object( 'Ime', a.\"Ime\", 'Prezime', a.\"Prezime\" ) as Mladenka, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\" ) t";
                string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";
                
                
                
                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();


                string strpostgracecommand = csvcmd;
                string ext = "csv";

                if (mode == "0")
                {
                    ext = "json";
                }
                
                NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                DataTable dat = new DataTable();
                objDataAdapter.Fill(dat);

                if (!Directory.Exists("tmp"))
                    Directory.CreateDirectory("tmp");

                string filept = "tmp\\data." + ext;

                StreamWriter sw = new StreamWriter(filept);

                if (mode == "1")
                {
                    for (int i = 0; i < dat.Columns.Count; i++)
                    {
                        string s = dat.Columns[i].ColumnName;
                        sw.Write(dat.Columns[i].ColumnName);
                        if (i < dat.Columns.Count - 1)
                            sw.Write(',');
                    }
                    sw.WriteLine();
                }
                arg = arg.ToLower();
                List<string> validId = new List<string>();
                for (int i = 0; i < dat.Rows.Count; i++)
                {
                    string id;
                    //List<string> t = new List<string>();
                    var datt = dat.Rows[i].ItemArray;

                    id = datt[0].ToString();
                    bool test = true;
                    string s;
                    foreach (var ss in datt)
                    {
                        s = ss.ToString();
                        if (s.ToLower().Contains(arg))
                        {
                            test = false;
                            validId.Add(id);
                            break;
                        }
                    }
                    if (test)
                        continue;
                    if (mode == "1")
                    {
                        for (int j = 0; j < datt.Length; j++)
                        {
                            sw.Write(datt[j].ToString());
                            if (j < datt.Length - 1)
                                sw.Write(',');
                        }
                        sw.WriteLine();
                    }
                }
                objpostgraceConn.Close();
                if(mode=="1")
                sw.Dispose();

                if (mode == "0")
                {
                    string where = " where \"Id\" in(";
                    bool t = false;
                    foreach (string s in validId)
                    {
                        if (t)
                            where += ",";
                        t = true;
                        where += "'" + s + "'";

                    }
                    where += ")";


                    NpgsqlDataAdapter objDataAdapter2 = new NpgsqlDataAdapter(jsoncmd+where, objpostgraceConn);
                    DataTable dat2 = new DataTable();
                    objDataAdapter2.Fill(dat2);

                    for (int i = 0; i < dat2.Rows.Count; i++)
                    {
                        //List<string> t = new List<string>();
                        var datt2 = dat2.Rows[i].ItemArray;
                        for (int j = 0; j < datt2.Length; j++)
                        {
                                sw.Write(datt2[j].ToString());
                        }
                        sw.WriteLine();
                    }
                    objpostgraceConn.Close();
                    sw.Dispose();
                }
                
                //slanje datoteke

                //Stream stream = new FileStream(filept, FileMode.Open);
                var bytes = await System.IO.File.ReadAllBytesAsync(filept);
                return File(bytes, "text/plain", Path.GetFileName(filept));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        //api za biranje po atributu
        // GET api/<ValuesController>/5
        [HttpGet("{mode}/{arg}/{col}")]
        public async Task<ActionResult> Get(string mode, string arg, string col)
        {
            int collum;
            if (mode != "1" && mode != "0")
                return null;
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                string jsoncmd = "SET client_encoding TO 'UTF8'; select array_to_json(array_agg(row_to_json(t))) from ( select b.\"Id\", json_build_object( 'Ime', f.\"Ime\", 'Prezime', f.\"Prezime\" ) as Fotograf, json_build_object( 'Ime', m.\"Ime\", 'Prezime', m.\"Prezime\" ) as Mladenac, json_build_object( 'Ime', a.\"Ime\", 'Prezime', a.\"Prezime\" ) as Mladenka, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\" ) t";
                string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";

                collum = int.Parse(col);


                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();


                string strpostgracecommand = csvcmd;
                string ext = "csv";

                if (mode == "0")
                {
                    ext = "json";
                }

                NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                DataTable dat = new DataTable();
                objDataAdapter.Fill(dat);

                if (!Directory.Exists("tmp"))
                    Directory.CreateDirectory("tmp");

                string filept = "tmp\\data." + ext;

                StreamWriter sw = new StreamWriter(filept);

                if (mode == "1")
                {
                    for (int i = 0; i < dat.Columns.Count; i++)
                    {
                        string s = dat.Columns[i].ColumnName;
                        sw.Write(dat.Columns[i].ColumnName);
                        if (i < dat.Columns.Count - 1)
                            sw.Write(',');
                    }
                    sw.WriteLine();
                }
                arg = arg.ToLower();
                List<string> validId = new List<string>();
                for (int i = 0; i < dat.Rows.Count; i++)
                {
                    string id;
                    //List<string> t = new List<string>();
                    var datt = dat.Rows[i].ItemArray;

                    id = datt[0].ToString();
                    bool test = true;
                    string s;
                    s = datt[collum].ToString();
                    if (s.ToLower().Contains(arg))
                    {
                        test = false;
                        validId.Add(id);
                    }
                    if (test)
                        continue;
                    if (mode == "1")
                    {
                        for (int j = 0; j < datt.Length; j++)
                        {
                            sw.Write(datt[j].ToString());
                            if (j < datt.Length - 1)
                                sw.Write(',');
                        }
                        sw.WriteLine();
                    }
                }
                objpostgraceConn.Close();
                if (mode == "1")
                    sw.Dispose();

                if (mode == "0")
                {
                    string where = " where \"Id\" in(";
                    bool t = false;
                    foreach (string s in validId)
                    {
                        if (t)
                            where += ",";
                        t = true;
                        where += "'" + s + "'";

                    }
                    where += ")";


                    NpgsqlDataAdapter objDataAdapter2 = new NpgsqlDataAdapter(jsoncmd + where, objpostgraceConn);
                    DataTable dat2 = new DataTable();
                    objDataAdapter2.Fill(dat2);

                    for (int i = 0; i < dat2.Rows.Count; i++)
                    {
                        //List<string> t = new List<string>();
                        var datt2 = dat2.Rows[i].ItemArray;
                        for (int j = 0; j < datt2.Length; j++)
                        {
                            sw.Write(datt2[j].ToString());
                        }
                        sw.WriteLine();
                    }
                    objpostgraceConn.Close();
                    sw.Dispose();
                }

                //slanje datoteke

                //Stream stream = new FileStream(filept, FileMode.Open);
                var bytes = await System.IO.File.ReadAllBytesAsync(filept);
                return File(bytes, "text/plain", Path.GetFileName(filept));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        //api za id odabir
        [HttpGet("id/{id}")]
        public async Task<ActionResult> Get(long id)
        {
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                string jsoncmd = "SET client_encoding TO 'UTF8'; select array_to_json(array_agg(row_to_json(t))) from ( select b.\"Id\", json_build_object( 'Ime', f.\"Ime\", 'Prezime', f.\"Prezime\" ) as Fotograf, json_build_object( 'Ime', m.\"Ime\", 'Prezime', m.\"Prezime\" ) as Mladenac, json_build_object( 'Ime', a.\"Ime\", 'Prezime', a.\"Prezime\" ) as Mladenka, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\" ) t";
                //string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";

                jsoncmd += " where \"Id\" = '"+id.ToString()+"';";

                NpgsqlConnection objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();


                string strpostgracecommand = jsoncmd;
                string ext = "json";
                /*
                if (mode == "0")
                {
                    ext = "json";
                }
                */
                NpgsqlDataAdapter objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                DataTable dat = new DataTable();
                objDataAdapter.Fill(dat);

                if (!Directory.Exists("tmp"))
                    Directory.CreateDirectory("tmp");

                string filept = "tmp\\data." + ext;

                if (dat.Columns.Count != 1)
                    throw new Exception();


                

                var datt = dat.Rows[0].ItemArray;

                if(datt[0].ToString()==string.Empty)
                    return new BadRequestResult();
                StreamWriter sw = new StreamWriter(filept);
                sw.Write(datt[0].ToString());


                objpostgraceConn.Close();
                    sw.Dispose();
                

                //slanje datoteke

                //Stream stream = new FileStream(filept, FileMode.Open);
                var bytes = await System.IO.File.ReadAllBytesAsync(filept);
                return File(bytes, "text/plain", Path.GetFileName(filept));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //return new HttpResponseMessage(HttpStatusCode.NoContent);
                //return new HttpUnauthorizedResult();
            }
            return null;
        }


        [HttpPost]
        public IActionResult Post([FromQuery] string oib, [FromQuery] string ime, [FromQuery] string prezime)
        {
            if (oib == null || ime == null || prezime == null)
                return BadRequest();
            else
            {
                string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
                try
                {
                    
                    string jsoncmd = "select \"OIB\" from \"Osobe\" where \"OIB\" = '"+oib+"'";
                    //string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";

                    NpgsqlConnection objpostgraceConn_ = new NpgsqlConnection(strConnString);
                    objpostgraceConn_.Open();


                    string strpostgracecommand = jsoncmd;

                    NpgsqlDataAdapter objDataAdapter_ = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn_);
                    DataTable dat = new DataTable();
                    objDataAdapter_.Fill(dat);
                    objpostgraceConn_.Close();

                    //var datt = dat.Rows[0].ItemArray;

                    if (dat.Rows.Count>0)
                        return new ConflictResult();



                    //
                    var objpostgraceConn = new NpgsqlConnection(strConnString);
                    objpostgraceConn.Open();
                    string inserts = "insert into \"Osobe\"(\"OIB\",\"Ime\",\"Prezime\") values('";
                    inserts += oib + "','";
                    inserts += ime + "','";
                    inserts += prezime + "')";

                    //var objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                    var cmd = new NpgsqlCommand(inserts, objpostgraceConn);
                    cmd.ExecuteNonQuery();
                    objpostgraceConn.Dispose();
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                    //return new HttpResponseMessage(HttpStatusCode.NoContent);
                    //return new HttpUnauthorizedResult();
                }
            }
        }
        /*
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] string value)
        {
            
            //Request.Body.
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var val = await reader.ReadToEndAsync();
            }
            return null;
        }
        */
        
        // PUT api/<ValuesController>/5
        [HttpPut]
        public IActionResult Put([FromQuery] string oib, [FromQuery] string ime, [FromQuery] string prezime)
        {
            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                var id = oib;
                string jsoncmd = "select \"OIB\" from \"Osobe\" where \"OIB\" = '" + id + "'";
                //string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";

                NpgsqlConnection objpostgraceConn_ = new NpgsqlConnection(strConnString);
                objpostgraceConn_.Open();


                string strpostgracecommand = jsoncmd;

                NpgsqlDataAdapter objDataAdapter_ = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn_);
                DataTable dat = new DataTable();
                objDataAdapter_.Fill(dat);
                objpostgraceConn_.Close();

                //var datt = dat.Rows[0].ItemArray;

                if (dat.Rows.Count != 1)
                    return new NotFoundResult();



                //
                var objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();
                string inserts = "update \"Osobe\" set \"Ime\" = '"+ime+ "', \"Prezime\"='"+prezime+ "' where \"OIB\"='"+oib+"'";

                //var objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                var cmd = new NpgsqlCommand(inserts, objpostgraceConn);
                cmd.ExecuteNonQuery();
                objpostgraceConn.Dispose();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
                //return new HttpResponseMessage(HttpStatusCode.NoContent);
                //return new HttpUnauthorizedResult();
            }
        }
        
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            string strConnString = "Host=localhost;Port=5432;User Id=postgres;Password=data;Database=Otvoreno";
            try
            {
                    
                string jsoncmd = "select \"OIB\" from \"Osobe\" where \"OIB\" = '"+id+"'";
                //string csvcmd = "select b.\"Id\", f.\"Ime\" as FotografIme, f.\"Prezime\" FotografPrezime, m.\"Ime\" as MladenacIme, m.\"Prezime\" as MladenacPrezime, a.\"Ime\" as MladenkaIme, a.\"Prezime\" as MladenkaPrezime, b.\"Format\", b.\"Korice\", b.\"Broj listova\", b.\"Paket\", b.\"Faceoff\", b.\"Cijena\" from \"Book\" b inner join \"Osobe\" f on b.\"Fotograf\" = f.\"OIB\" inner join \"Osobe\" m on b.\"Mladenac\" = m.\"OIB\" inner join \"Osobe\" a on b.\"Mladenka\" = a.\"OIB\"";

                NpgsqlConnection objpostgraceConn_ = new NpgsqlConnection(strConnString);
                objpostgraceConn_.Open();


                string strpostgracecommand = jsoncmd;

                NpgsqlDataAdapter objDataAdapter_ = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn_);
                DataTable dat = new DataTable();
                objDataAdapter_.Fill(dat);
                objpostgraceConn_.Close();

                //var datt = dat.Rows[0].ItemArray;

                if (dat.Rows.Count!=1)
                    return new NotFoundResult();



                //
                var objpostgraceConn = new NpgsqlConnection(strConnString);
                objpostgraceConn.Open();
                string inserts = "delete from \"Osobe\" where \"OIB\"='"+id+"'";

                //var objDataAdapter = new NpgsqlDataAdapter(strpostgracecommand, objpostgraceConn);
                var cmd = new NpgsqlCommand(inserts, objpostgraceConn);
                cmd.ExecuteNonQuery();
                objpostgraceConn.Dispose();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
                //return new HttpResponseMessage(HttpStatusCode.NoContent);
                //return new HttpUnauthorizedResult();
            }
            
        }
        
    }
}
