using PriceConfig.Data;
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
        static List<PriceItem> Prices = new List<PriceItem>();
        DatabaseRepo repo = new DatabaseRepo();

        // GET: Home
        public ActionResult Index()
        {
            //Sets up the model and checks to see if there is info uploaded to the database
            PriceConfigVM model = new PriceConfigVM();

            try
            {
                Prices = repo.GetPrices();
                model.Uploaded = (Prices.Count > 0) ? true : false;
                model.Prices = Prices;
            }
            catch (Exception ex)
            {
                model.ErrorMessage = $"{ex.Source}: {ex.Message}";
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

                    //Try parsing CSV file and upload to the database
                    try
                    {
                        if (model.Uploaded) DeleteData();
                        repo.InsertData(ParseCSV(model.CSVFile));
                    }
                    catch(Exception ex)
                    {
                        if (!ex.Message.Contains("SQL Server")) ModelState.AddModelError("", "The csv file could not be uploaded due to invalid data.");

                        model.ErrorMessage = $"{ex.Source}: {ex.Message}";

                        return View(model);
                    }
                }
                else
                {
                    //Alter price in database
                    repo.UpdatePrice(model.PriceId, model.NewPrice);
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
            var model = new PriceConfigVM();

            try
            {
                DeleteData();
            }
            catch(Exception ex)
            {
                model.ErrorMessage = $"{ex.Source}: {ex.Message}";
            }


            return View("Index", model);
        }

        //Parse CSV data
        public List<string[]> ParseCSV(HttpPostedFileBase csvFile)
        {
            FileInfo csv = new FileInfo(csvFile.FileName);
            TextReader reader = csv.OpenText();
            reader.ReadLine();

            string line;
            List<string[]> inserts = new List<string[]>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] row = line.Split(',');
                if (row.Length == 1)
                {
                    inserts.Add(new string[] { row[0], "invalid" });
                    break;
                }
                inserts.Add(row);
            }

            return inserts;
        }

        public void DeleteData()
        {
            Prices.Clear();
            repo.DeleteData();
        }
    }
}