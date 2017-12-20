using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class CacheHelper
    {
        public static System.Web.Caching.Cache Cache{
            get{ return HttpContext.Current.Cache;}
        }

        public static String EncodeText(String content){
            String result = HttpUtility.HtmlEncode(content);
            result = result.Replace("  ", " &nbsp;&nbsp;").Replace("\n", "<br>");
		    return  result;
        }

        public static String ConvertNullToEmptyString(String content){
		    return  (String.IsNullOrEmpty(content) ? "" : content);
        }

        public static void PurgeCacheItems(String startWith){
		    String itemName = startWith.ToLower();
            var temp = CacheHelper.Cache
                .Cast<DictionaryEntry>().ToList();
            var keysQuery = CacheHelper.Cache
                .Cast<DictionaryEntry>()
                .Select(entry => (string)entry.Key)
                .Where(key => key.ToString().ToLower().StartsWith(itemName));

            foreach (String item in keysQuery){
                CacheHelper.Cache.Remove(item);
            }
        }

        public static void PurgeCacheItems(String startWith, String endWith){
		    startWith = startWith.ToLower();
            endWith = endWith.ToLower();

            var keysQuery = CacheHelper.Cache
                .Cast<DictionaryEntry>()
                .Select(entry => (string)entry.Key)
                .Where(key => key.ToString().ToLower().StartsWith(startWith) && key.ToString().ToLower().EndsWith(endWith));

            foreach (String item in keysQuery){
                CacheHelper.Cache.Remove(item);
            }
        }


        //If ObjectBase.Cache(cacheKey) Is Nothing Then
        public static T Find<T>(string key) {
            object found = Cache.Get(key);
            if (found!=null && found.GetType() == typeof(T))
                return (T)found;
            return default(T);
        }
        public static void AddToCache<T>(string key, T item, TimeSpan expiration){
            if (Cache.Get(key) != null)
                Cache.Remove(key);
            Cache.Insert(key, item, null, System.Web.Caching.Cache.NoAbsoluteExpiration, expiration);
        }
        
         public static void AddToFileCache<T>(string key, T item, String filename){
            if (Cache.Get(key) != null)
                Cache.Remove(key);
            Cache.Insert(key, item, new System.Web.Caching.CacheDependency(filename), DateTime.MaxValue, TimeSpan.Zero);
        }
       // New Caching.CacheDependency(oManager.ConfigurationFilePatho)

    }
}


//        Dim itemsToRemove As New List(Of String)

//        Dim enumerator As IDictionaryEnumerator = ObjectBase.Cache.GetEnumerator()
//        While enumerator.MoveNext
//            If enumerator.Key.ToString.ToLower.StartsWith(prefix) Then
//                itemsToRemove.Add(enumerator.Key.ToString)
//            End If
//        End While

//        For Each itemToRemove As String In itemsToRemove
//            ObjectBase.Cache.Remove(itemToRemove)
//        Next


//    Protected Shared Sub PurgeCacheItems(ByVal startPrefix As String, ByVal endPrefix As String)
//        startPrefix = startPrefix.ToLower
//        endPrefix = endPrefix.ToLower

//        Dim itemsToRemove As New List(Of String)
//        Dim enumerator As IDictionaryEnumerator = ObjectBase.Cache.GetEnumerator()
//        While enumerator.MoveNext
//            If enumerator.Key.ToString.ToLower.StartsWith(startPrefix) AndAlso enumerator.Key.ToString.ToLower.EndsWith(endPrefix) Then
//                itemsToRemove.Add(enumerator.Key.ToString)
//            End If
//        End While
//        For Each itemToRemove As String In itemsToRemove
//            ObjectBase.Cache.Remove(itemToRemove)
//        Next
//    End Sub
//End Class