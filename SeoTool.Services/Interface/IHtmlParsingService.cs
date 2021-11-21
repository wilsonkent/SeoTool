using System.Threading.Tasks;

namespace SeoTool.Services.Interface
{
    public interface IHtmlParsingService
    {
        string FindTextInHtml(string htmlString, string text);
    }
}