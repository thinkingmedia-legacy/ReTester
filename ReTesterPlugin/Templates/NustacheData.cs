using System.Collections.Generic;

namespace ReTesterPlugin.Templates
{
    public class NustacheData : Dictionary<string, object>
    {
        public new object this[string pKey]
        {
            get { return base[pKey]; }
            set
            {
                if (!ContainsKey(pKey))
                {
                    Add(pKey, null);
                }
                base[pKey] = value;
            }
        }

        public List<NustacheData> List(string pKey)
        {
            return (List<NustacheData>)this[pKey];
        }
    }
}