using SeoTool.Services.Interface;
using System.Linq;

namespace SeoTool.Services.HtmlParsing
{
    public class GoogleResultHtmlParsingService : IHtmlParsingService
    {
        private const string LinkIdentifier = "<div class=\"kCrYT\"><a href=";

        public string FindTextInHtml(string htmlString, string text)
        {
            var splitText = htmlString.Split(LinkIdentifier);

            if (splitText.Length > 0)
            {
                //remove all other texts after the href url
                //this will only find text within the a tag href urls
                var links = splitText.Where((x, index) => index != 0).Select(x => x.Substring(0, x.IndexOf('>')));

                var rankingString = string.Join(",", links.Select((x, index) => new { value = x, index })
                    .Where(x => x.value.Contains(text, System.StringComparison.InvariantCultureIgnoreCase))
                    .Select(x=>x.index + 1));

                if (!string.IsNullOrEmpty(rankingString))
                    return rankingString;
            }   

            return "0";
        }
    }
}
