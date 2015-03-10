using CloudSample.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
using System.Net;
using Microsoft.AspNet.Mvc;
using CloudSample.Models;
using CloudSample.Entities;

namespace CloudSample.Controllers
{
    public class CloudConceptsController : Controller
    {
        #region Action methods

        public ActionResult ExHandling()
        {
            ViewBag.Message = "Exception Handling";

            return View();
        }

        public ActionResult BackOff()
        {
            ViewBag.Message = "Exponential Back Off";

            return View();
        }

        #endregion

        #region API methods

        public ActionResult FriendSearch(string firstName, string lastName)
        {
            var result = new FriendService().GetFriends(firstName, lastName)
                .Select(f => new 
                {
                    Firstname = f.RowKey,
                    Lastname = f.PartitionKey,
                    Phone = f.Phone,
                    Email = f.Email,
                    Twitter = f.Twitter
                })
                .ToArray();

            return Json(result);
        }

        public ActionResult SiteContent(string url) 
        {
            HttpClient httpClient = new HttpClient();
            string htmlString = string.Empty;

            // Linear BackOff
            Run.TightLoop(10, 2, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                htmlString = response.Content.ReadAsStringAsync().Result;
            });

            // Random BackOff
            Run.WithRandomInterval(10, 1, 5, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                htmlString = response.Content.ReadAsStringAsync().Result;
            });

            // Progressive BackOff
            Run.WithProgressBackOff(10, 1, 10, () =>
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode) throw new ApplicationException("Could not connect");
                htmlString = response.Content.ReadAsStringAsync().Result;
            });

            return Json(htmlString);
        }

        #endregion
    }
}