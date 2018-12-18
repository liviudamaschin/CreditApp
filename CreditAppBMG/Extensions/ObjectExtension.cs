using Newtonsoft.Json;

namespace CreditAppBMG.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(this object obj)
        {
            //return JsonConvert.SerializeObject(obj);
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}
