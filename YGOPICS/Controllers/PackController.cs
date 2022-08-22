using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using YGOPICS.Models;
using YGOPICS.Models.ViewModels;

namespace YGOPICS.Controllers
{
    
    public class PackController : Controller
    {

        private readonly ygopicsdbContext _context;

        public PackController(ygopicsdbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://db.ygoprodeck.com/api/v7/cardsets.php");
            var cardsetList = JsonConvert.DeserializeObject<List<Cardset>>(json);
            cardsetList.RemoveAt(0);
            return View(cardsetList);
        }

        public async Task<IActionResult> Card(string name)
        {
            if (HttpContext.Session.GetInt32("_Id") != null)
            {
                var httpClient = new HttpClient();
                name = name.ToLower();
                var jsonString = await httpClient.GetStringAsync("https://db.ygoprodeck.com/api/v7/cardinfo.php?cardset=" + name);
                var cardList = JsonConvert.DeserializeObject<Card>(jsonString);

                return View(cardList);
            }
            else
            {
                return RedirectToAction(nameof(Index), "Login");
            }
        }

        [HttpPost]
        public JsonResult getComment([FromBody] CardGetter card)
        {
            Console.WriteLine("aki t");
            Console.WriteLine(card.cardId);
            try
            {
                var commentdb = _context.Comments
                .Where(b => b.Cardid== card.cardId)
                .Include(b => b.User)
                .Select(b => new CommentViewModel
                {
                    UserName = b.User.Name,
                    Text = b.Text
                }).ToList();
               
                return Json(commentdb);
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Comment([FromBody] CommentViewModel newCommentary)
        {
            
                string texto = newCommentary.Text.Trim(' ');
                if (texto != null)
                {
                    var Userid = (int)HttpContext.Session.GetInt32("_Id");
                   
                    try  {
                        _context.Database.ExecuteSqlRaw("INSERT INTO COMMENT (cardid,text,userid) SELECT {0}, {1}, userid FROM USERS WHERE userid={2}", newCommentary.Cardid, texto, Userid);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch(Exception e) { 
                        Console.WriteLine(e.ToString());
                        return BadRequest();
                    }
                    
                }
                return NotFound();
            
            
        }

    }  
}
