using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using urlShortenerApi.Data;
using urlShortenerApi.Models;
using UrlShortenerApi.Models;

namespace urlShortenerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenerController : ControllerBase
    {
        private readonly Context _context;
        public ShortenerController(Context context)
        {
            _context = context;
        }

        [HttpGet("{shortened}")]
        public async Task<ActionResult<string>> GetOriginalUrl([FromRoute] string shortened)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.Shortened == shortened);
            return url == null ? NotFound() : Ok(url.Original);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateShortenedUrl([FromBody] UrlRequest request)
        {
            var url = new Url
            {
                Original = request.Original,
                Shortened = GenerateShortenedCode()
            };

            _context.Urls.Add(url);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetOriginalUrl),
                new { shortened = url.Shortened },
                url.Shortened
            );
        }

        private string GenerateShortenedCode(int length = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            string shortened;

            while(true)
            {
                shortened = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                if (!UrlExists(shortened)) break;
            }

            return shortened;
        }

        private bool UrlExists(string shortened)
        {
            return _context.Urls.Any(e => e.Shortened == shortened);
        }
    }
}
