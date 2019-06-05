using Gematrianator.Data;
using Gematrianator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gematrianator.Controllers
{
    public class HomeController : Controller
    {
        private static CharacterRepo repo = new CharacterRepo();

        public ActionResult Index()
        {
            if (!repo.Set) repo.SetAll();
            return View(new GematrianatorVM());
        }

        [HttpPost]
        public ActionResult Index(GematrianatorVM model)
        {
            if (ModelState.IsValid)
            {
                var wr = new WordsRepo();
                if(int.TryParse(model.UserInput, out int result))
                {
                    var retrievedWords = wr.GetWords(result).Select(w => w.WordText).Distinct();
                    model.DecodedWords = new List<WordCipher>();
                    foreach(var word in retrievedWords) model.DecodedWords.AddRange(wr.GetWordCiphers(word));
                }
                else
                {
                    var word = model.UserInput.ToUpper();
                    var ciphers = wr.GetAllCiphers();
                    var cipherValues = new List<WordCipherInfo>();
                    foreach(var cipher in ciphers)
                    {
                        cipherValues.Add(new WordCipherInfo { CipherID = cipher.CipherID, CipherName = cipher.CipherName, WordText = model.UserInput });
                    }
                    foreach(var letter in word)
                    {
                        var character = repo.CharList.FirstOrDefault(c => c.CharacterName == letter);
                        if(character != null)
                        {
                            foreach(var cipher in cipherValues) cipher.CipherValue += character.Ciphers[cipher.CipherID];
                        }
                    }
                    if (model.EnglishOrdinal) model.EnglishOrdinalNum = cipherValues.First(c => c.CipherID == "EO").CipherValue;
                    if (model.ReverseEnglishOrdinal) model.ReverseEnglishOrdinalNum = cipherValues.First(c => c.CipherID == "REO").CipherValue;
                    if (model.EnglishFullReduction) model.EnglishFullReductionNum = cipherValues.First(c => c.CipherID == "EFR").CipherValue;
                    if (model.ReverseEnglishFullReduction) model.ReverseEnglishFullReductionNum = cipherValues.First(c => c.CipherID == "REFR").CipherValue;

                    var wordCiphers = wr.GetWordCiphers(model.UserInput);
                    if (wordCiphers.Count < ciphers.Count)
                    {
                        if(wordCiphers.Count == 0) wr.AddWord(model.UserInput);
                        foreach (var cipher in cipherValues) wr.AddWordCipher(model.UserInput, cipher.CipherID, cipher.CipherValue);
                    }
                }
                return View(model);
            }
            return View(model);
        }

        public ActionResult DeleteWord(string word)
        {
            var wr = new WordsRepo();
            wr.DeleteWord(word);
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}