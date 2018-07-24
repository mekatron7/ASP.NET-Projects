using Dapper;
using PriceConfigApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriceConfigApp.Controllers
{
    public class HomeController : Controller
    {
        string _connString = ConfigurationManager.ConnectionStrings["PriceConfig"].ConnectionString;

        // GET: Home
        public ActionResult Index()
        {
            //Sets up the model and checks to see if there is info uploaded to the database
            PriceConfigVM model = new PriceConfigVM();

            using (var cn = new SqlConnection(_connString))
            {
                int rows = cn.QueryFirst<int>("select count(*) from Prices", commandType: System.Data.CommandType.Text);
                model.Uploaded = (rows > 0) ? true : false;

                if (model.Uploaded)
                {
                    model.Prices = cn.Query<PriceItem>("select * from Prices", commandType: System.Data.CommandType.Text).ToList();
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PriceConfigVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Mode == "csv")
                {
                    if (model.CSVFile == null || model.CSVFile.ContentLength == 0)
                    {
                        ModelState.AddModelError("", "You must choose a valid CSV file before you hit the 'Upload' button.");

                        return View(model);
                    }
                    else if (!model.CSVFile.FileName.EndsWith(".csv"))
                    {
                        ModelState.AddModelError("", "The file must be in the format of .csv");

                        return View(model);
                    }

                    //Delete data in database if there's already existing data
                    if (model.Uploaded) DeleteData();

                    //Parse CSV and upload to database
                    //Parsing section
                    FileInfo csv = new FileInfo(model.CSVFile.FileName);
                    TextReader reader = csv.OpenText();
                    reader.ReadLine();

                    string line;
                    string query = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] row = line.Split(',');
                        query += $"insert into Prices values('{row[0]}', {row[1]}) ";
                    }

                    using (var cn = new SqlConnection(_connString))
                    {
                        cn.Execute(query, commandType: System.Data.CommandType.Text);
                    }
                }
                else
                {
                    //Alter price in database
                    using (var cn = new SqlConnection(_connString))
                    {
                        string query = $"update Prices set Price = {model.NewPrice} where PriceId = {model.PriceId}";
                        cn.Execute(query, commandType: System.Data.CommandType.Text);
                    }

                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string a)
        {
            DeleteData();

            return RedirectToAction("Index");
        }

        public void DeleteData()
        {
            using (var cn = new SqlConnection(_connString))
            {
                cn.Execute("delete from Prices", commandType: System.Data.CommandType.Text);
            }
        }
    }
}