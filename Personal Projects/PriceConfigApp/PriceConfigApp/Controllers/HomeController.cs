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
        static List<PriceItem> Prices = new List<PriceItem>();

        // GET: Home
        public ActionResult Index()
        {
            //Sets up the model and checks to see if there is info uploaded to the database
            PriceConfigVM model = new PriceConfigVM();

            using (var cn = new SqlConnection(_connString))
            {
                try
                {
                    Prices = cn.Query<PriceItem>("select * from Prices", commandType: System.Data.CommandType.Text).ToList();
                    model.Uploaded = (Prices.Count > 0) ? true : false;
                    model.Prices = Prices;
                }
                catch(Exception ex)
                {
                    model.ErrorMessage = $"{ex.Source}: {ex.Message}";
                }
                
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PriceConfigVM model)
        {
            model.Prices = Prices;

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
                        if(row.Length == 1)
                        {
                            query = "insert into Prices values('bad', 'data')";
                            break;
                        }

                        query += $"insert into Prices values('{row[0]}', {row[1]}) ";
                    }

                    //Upload section
                    using (var cn = new SqlConnection(_connString))
                    {
                        try
                        {
                            //Delete data in database if there's already existing data
                            if (model.Uploaded) DeleteData();

                            cn.Execute(query, commandType: System.Data.CommandType.Text);
                        }
                        catch(Exception ex)
                        {
                            if(ex.Message.Contains("syntax")) ModelState.AddModelError("", "The csv file could not be uploaded due to invalid data.");

                            model.ErrorMessage = $"{ex.Source}: {ex.Message}";

                            return View(model);
                        }
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
            Prices.Clear();
            using (var cn = new SqlConnection(_connString))
            {
                cn.Execute("delete from Prices", commandType: System.Data.CommandType.Text);
            }
        }
    }
}