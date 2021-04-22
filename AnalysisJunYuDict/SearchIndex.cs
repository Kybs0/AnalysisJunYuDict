using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using Lucene.Net.Store;
using Directory = System.IO.Directory;
using Version = Lucene.Net.Util.Version;

namespace AnalysisJunYuDict
{
    public class SearchIndex
    {
        public static List<SearchItem> GetAllItems()
        {
            string path = Directory.GetCurrentDirectory() + @"\mtad\";
            //建立索引搜索，指定索引目录
            IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(new System.IO.DirectoryInfo(path)), true);
            //获取最大文档数量
            var count = searcher.MaxDoc();
            var searchItems = new List<SearchItem>();
            for (int i = 0; i < count; i++)
            {
                var document = searcher.Doc(i);
                //var fields = document.GetFields();
                var id = document.GetValues("id")[0];
                var tablename = document.GetValues("tablename")[0];
                var acronym = document.GetValues("acronym")[0];
                var english = document.GetValues("english")[0];
                var chinese = document.GetValues("chinese")[0];
                var explain = document.GetValues("explain")[0];

                var searchItem = new SearchItem
                {
                    Id = id,
                    TableName = tablename,
                    Acronym = acronym,
                    English = english,
                    Chinese = chinese,
                    Explain = explain
                };
                searchItems.Add(searchItem);
            }
            return searchItems;
        }
        // Methods
        public static int GetCount(string tablename, string keywords)
        {
            string path = Directory.GetCurrentDirectory() + @"\mtad\";
            keywords = (keywords.Length != 0) ? ("\"" + keywords + "\"") : "kNoWfAr";
            DateTime now = DateTime.Now;
            IndexSearcher searcher = new IndexSearcher(path);
            BooleanQuery query = new BooleanQuery();
            if (tablename != "")
            {
                query.Add(new TermQuery(new Term("tablename", tablename)), BooleanClause.Occur.MUST);
            }
            string[] fields = new string[] { "acronym", "english", "chinese", "fornull" };
            query.Add(new MultiFieldQueryParser(fields, new StandardAnalyzer()).Parse(keywords), BooleanClause.Occur.MUST);
            var hits = searcher.Search(query);
            searcher.Close();
            return hits.Length();
        }
        public static List<SearchItem> Search(string tablename, string keywords, out int recCount, out double eventTime)
        {
            string path = Directory.GetCurrentDirectory() + @"\mtad\";
            keywords = (keywords.Length != 0) ? ("\"" + keywords + "\"") : "kNoWfAr";
            DateTime now = DateTime.Now;
            IndexSearcher searcher = new IndexSearcher(path, true);
            BooleanQuery query = new BooleanQuery();
            if (tablename != "")
            {
                query.Add(new TermQuery(new Term("tablename", tablename)), BooleanClause.Occur.MUST);
            }
            string[] fields = new string[] { "acronym", "english", "chinese", "chinese" };
            query.Add(new MultiFieldQueryParser(fields, new StandardAnalyzer()).Parse(keywords), BooleanClause.Occur.MUST);
            Sort sort1 = new Sort(new SortField(null, 1, true));
            var hits = searcher.Search(query);
            List<SearchItem> list = new List<SearchItem>();
            recCount = hits.Length();
            if (recCount <= 0)
            {
                searcher.Close();
                TimeSpan span2 = (TimeSpan)(DateTime.Now - now);
                eventTime = span2.TotalMilliseconds;
                return null;
            }

            for (int n = 0; n < recCount; n++)
            {
                SearchItem item = null;
                try
                {
                    item = new SearchItem
                    {
                        Id = hits.Doc(n).Get("id"),
                        TableName = hits.Doc(n).Get("tablename"),
                        Acronym = hits.Doc(n).Get("acronym"),
                        English = hits.Doc(n).Get("english"),
                        Chinese = hits.Doc(n).Get("chinese"),
                        Explain = hits.Doc(n).Get("explain")
                    };
                }
                catch (Exception exception1)
                {
                    Console.WriteLine(exception1.Message);
                }
                finally
                {
                    list.Add(item);
                }
            }

            searcher.Close();
            TimeSpan span = (TimeSpan)(DateTime.Now - now);
            eventTime = span.TotalMilliseconds;
            return list;
        }
    }
}
