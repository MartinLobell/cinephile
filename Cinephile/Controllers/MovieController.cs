using Cinephile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Cinephile.Controllers
{
    public class MovieController : Controller
    {
        // Lagrar directory till var movies.xml-filen befinner sig i en variabel.
        string xmlPath = "~/XML/movies.xml";

        [HttpGet]
        public ActionResult Index()
        {
            // Skapar en filmlista att presentera filmerna från.
            List<MovieModels> movieList = new List<MovieModels>();

            // Skapar ett nytt dataset att läsa in XML-filen till.
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(xmlPath));

            // Bildar en dataview, en översikt äver datan och hämtar den första tabellen i datasetet.
            // Sorterar efter titel och hämtar varje rad och lägger till i Modellen, och varje modell
            // Läggs till i filmlistan
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.Sort = "Title";
            foreach (DataRowView dr in dv)
            {
                MovieModels model = new MovieModels();
                model.Title = Convert.ToString(dr[0]);
                model.Description = Convert.ToString(dr[1]);
                model.Length = Convert.ToInt32(dr[2]);
                model.Year = Convert.ToInt32(dr[3]);
                model.Genre = Convert.ToString(dr[4]);
                model.HasSeen = Convert.ToBoolean(dr[5]);
                model.IsFavourite = Convert.ToBoolean(dr[6]);

                movieList.Add(model);
            }

            // Om det finns fler än 0 filmer i filmlistan, visa dem.
            // Annars, visa en tom view.
            if (movieList.Count > 0)
            {
                return View(movieList);
            }
            return View();
        }

        MovieModels model = new MovieModels();

        [HttpGet]
        public ActionResult EditMovie(string title)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult EditMovie(MovieModels mm)
        {
                // Laddar XML-filen och lagrar i variabel
                XDocument xDoc = XDocument.Load(Server.MapPath(xmlPath));
                // Lista de element i XML-filen som är av slaget "movie" och lista dem.
                var items = (from item in xDoc.Descendants("movie") select item).ToList();
                //System.Diagnostics.Debug.WriteLine(items.Count);

                // Leta upp den film som har samma namn som den som redigeras, ta bort den och spara den nya.
                XElement selected = items.Where(p => p.Element("title").Value == mm.Title.ToString()).FirstOrDefault();
                selected.Remove();
                xDoc.Save(Server.MapPath(xmlPath));
                xDoc.Element("movies").Add(new XElement("movie", new XElement("title", mm.Title), new XElement("description", mm.Description), new XElement("length", mm.Length), new XElement("year", mm.Year), new XElement("genre", mm.Genre), new XElement("hasSeen", mm.HasSeen), new XElement("isFavourite", mm.IsFavourite)));
                xDoc.Save(Server.MapPath(xmlPath));

                // Återvänd till start-/listsidan.
                return RedirectToAction("Index", "Movie");
        }
    }
}