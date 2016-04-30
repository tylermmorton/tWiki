using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tWiki
{
    class Link
    {
        static string bitlyToken = "72107fafc4dda52519a18db8bee00445154cf4c3";

        public string url;

        public Link(string url)
        {
            this.url = url;
        }

        public Link(Link copy)
        {
            this.url = copy.url;
        }

        /* Using the given URL, returns a shortened version using the 
        // bit.ly online API. If the function fails, return the given
        // string.
        */
        public static Link Shorten(Link link)
        {
            // Create strings referencing the bitly API
            string apiAddress = "https://api-ssl.bitly.com";
            string apiGetQuery = "/v3/shorten?access_token={0}&longUrl={1}&format=txt";

            // Put all of the strings and the API auth together
            // Also make sure to encode the provided URL into % encoding.
            string request = string.Format(apiAddress + apiGetQuery, bitlyToken, WebUtility.UrlEncode(link.url));

            // Create and use a WebClient to retrieve the queried link.
            // The response should be a one line string containing the new link.
            try
            {
                WebClient client = new WebClient();
                return new Link(link) { url = client.DownloadString(request) };
            }
            catch (Exception e)
            {
                return link;
            }
        }
    }
}
