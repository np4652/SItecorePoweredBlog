using Sitecore.Web.UI.WebControls;
using SitecorePoweredBlog.Feature.Postcard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecorePoweredBlog.Feature.Postcard.Controllers
{
    public class PostcardController : Controller
    {
        // GET: Postcard
        public ActionResult Postcard()
        {
            
            var result = new List<Article>();
            try
            {
                Sitecore.Data.Database database = Sitecore.Configuration.Factory.GetDatabase("web");
                Sitecore.Data.Items.Item[] allItems = database.SelectItems(@"fast:/sitecore/content/SitecorePoweredBlog/Article//*[@@templateid='{1BF2A309-E3A4-49C0-9161-DE7C7BC55865}']");

                foreach (var item in allItems)
                {
                    Sitecore.Data.Fields.ReferenceField dl = item.Fields["Category"];
                    string str = dl != null && dl.TargetItem != null ? dl.TargetItem.Name : "";
                    result.Add(new Article
                    {
                        Id = FieldRenderer.Render(item, "Id"),
                        Title = FieldRenderer.Render(item, "Title"),
                        ShortDescription = FieldRenderer.Render(item, "ShortDescription"),
                        ShortImage = FieldRenderer.Render(item, "ShortImage"),
                        PostedDate = Convert.ToDateTime(FieldRenderer.Render(item, "PostedDate")),
                        Author = FieldRenderer.Render(item, "Author"),
                        Category = str//FieldRenderer.Render(item, "Category"),
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //var items = Sitecore.Context.Database.SelectItems("{56741DF6-7290-4E29-B0D1-11A05EE63AFF}");

            return View("~/Views/SitecorePoweredBlog/Postcard.cshtml", result);
        }

        public ActionResult blogdetail(string blogId)
        {
            ViewBag.blogId = blogId;
            var result = new Article();
            try
            {                
                var item = Sitecore.Context.Database.GetItem(blogId);
                Sitecore.Data.Fields.ReferenceField dl = item.Fields["Category"];
                string str = dl != null && dl.TargetItem != null ? dl.TargetItem.Name : "";
                List<string> tags = new List<string>();
                Sitecore.Data.Fields.MultilistField multilistField = item.Fields["Tags"];
                if (multilistField != null)
                {
                    foreach (Sitecore.Data.Items.Item tag in multilistField.GetItems())
                    {
                        tags.Add(tag.Name);
                    }
                }
                result = new Article
                {
                    Id = FieldRenderer.Render(item, "Id"),
                    Title = FieldRenderer.Render(item, "Title"),
                    LongDescription = FieldRenderer.Render(item, "LargeDescription"),
                    LargeImage = FieldRenderer.Render(item, "LargeImage"),
                    PostedDate = Convert.ToDateTime(FieldRenderer.Render(item, "PostedDate")),
                    Author = FieldRenderer.Render(item, "Author"),
                    Category = str,//FieldRenderer.Render(item, "Category"),
                    Tags = tags
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            result = result != null ? result : new Article();
            return View("~/Views/SitecorePoweredBlog/blog-detail.cshtml", result);
        }
    }
}