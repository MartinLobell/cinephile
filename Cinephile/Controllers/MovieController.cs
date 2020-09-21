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
        // Stores the path to the XML-file in a variable.
        string xmlPath = "~/XML/movies.xml";

        [HttpGet]
        public ActionResult Index()
        {
            List<MovieModels> movieList = new List<MovieModels>();
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath(xmlPath));
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
            MovieModels mm = new MovieModels();

            XDocument xDoc = XDocument.Load(Server.MapPath(xmlPath));
            var items = (from item in xDoc.Descendants("movie") select item).ToList();
            XElement selected = items.Where(p => p.Element("title").Value == title).FirstOrDefault();

            // TO DO: Description and Genre gets returned with their XELEMENT-tags.
            model.Title = Convert.ToString(selected.Element("title"));
            model.Description = Convert.ToString(selected.Element("description"));
            model.Length = int.Parse(selected.Element("length").Value);
            model.Year = int.Parse(selected.Element("year").Value);
            model.Genre = Convert.ToString(selected.Element("genre"));
            model.HasSeen = bool.Parse(selected.Element("hasSeen").Value);
            model.IsFavourite = bool.Parse(selected.Element("isFavourite").Value);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMovie(MovieModels mm)
        {
                XDocument xDoc = XDocument.Load(Server.MapPath(xmlPath));
                var items = (from item in xDoc.Descendants("movie") select item).ToList();

                XElement selected = items.Where(p => p.Element("title").Value == mm.Title.ToString()).FirstOrDefault();
                selected.Remove();
                xDoc.Save(Server.MapPath(xmlPath));
                xDoc.Element("movies").Add(new XElement("movie", new XElement("title", mm.Title), new XElement("description", mm.Description), new XElement("length", mm.Length), new XElement("year", mm.Year), new XElement("genre", mm.Genre), new XElement("hasSeen", mm.HasSeen), new XElement("isFavourite", mm.IsFavourite)));
                xDoc.Save(Server.MapPath(xmlPath));

                return RedirectToAction("Index", "Movie");
        }
    }
}