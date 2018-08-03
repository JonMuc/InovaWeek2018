using Automacao;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WebCrawler
{
    public class NetCoders : Robo
    {
        /// <summary>
        /// Construtor para instânciar o Client
        /// </summary>
        public NetCoders()
        {
            RoboWebClient = new RoboWebClient();
        }

        //public List<artigo> CarregaPosts()
        //{
        //    NameValueCollection parametros = new NameValueCollection();
        //    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //    this.RoboWebClient._allowAutoRedirect = false;
        //    var ret = this.HttpGet(@"https://www.zoom.com.br/notebook?q=notebook&resultsperpage=72");

        //    var artigosOrdenados = ret.DocumentNode.Descendants("li");
        //    //artigosOrdenados = artigosOrdenados.Where(n => n.Attributes["class"].Value == "item tp-default").ToArray();
        //    doc.LoadHtml(artigosOrdenados);
        //    artigosOrdenados = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-title entry-title").InnerText))
        //    List<artigo> artigos = new List<artigo>();

        //    foreach (var item in artigosOrdenados)
        //    {
        //        artigo art = new artigo();

        //        doc.LoadHtml(item.InnerHtml);

        //        art.Titulo = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-title entry-title").InnerText));
        //        art.Data = Convert.ToDateTime(HtmlAgilityPack.HtmlEntity.DeEntitize(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Name == "span" && d.Attributes["class"].Value == "post-time").InnerText));
        //        art.Descricao = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "entry-content").InnerText));
        //        art.Autor = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-author").InnerText));
        //        artigos.Add(art);
        //    }

        //    return artigos.OrderBy(d => d.Data).ToList();
        //}

        private string ConvertUTF(string texto)
        {
            byte[] data = Encoding.Default.GetBytes(texto);

            string ret = Encoding.UTF8.GetString(data);

            return ret;
        }

    }

    /// <summary>
    /// Classe espelho do artigo.
    /// </summary>
    public class artigo
    {
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Autor { get; set; }
    }
}