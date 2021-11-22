-SEO Tool-

.NET 5 WPF Application coded with VS2019

Searches google using the base URL: https://www.google.com.au/search?num=100&q=
with the input 
- keywords - the string to use for google search
- search string - the string to search within google search result

Start button starts a scheduler job to search google with 1 day interval until stop button is clicked. Out of the scheduled searches are printed on the textblock below.

- Search string allows any string for flexibility (i.e. not limited to just www.smokeball.com.au but can also search for smokeball)
- Search only searches the URLS (anchor hrefs) returned from the google search (not including the header and div content)
- Manual string parsing is used for reading the html result from google search to meet the requirements. Ideally, a good html library can be used for more robust parsing.

Note: 
Google blocks bots/machines from doing scraping work and can result in 429 results. This project is only a POC for oop dev work. 
