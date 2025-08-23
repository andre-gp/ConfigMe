using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ConfigMe
{
    /// <summary>
    /// Container passed to file-loading listeners to supply the deserialized JObject.
    /// </summary>
    public class FileLoadContext
    {
        JObject loadedObject;
        public JObject LoadedObject { get => loadedObject; set => loadedObject = value; }

        LoadResult loadType;
        public LoadResult LoadResult { get => loadType; set => loadType = value; }

        public FileLoadContext()
        {
            this.loadedObject = null;
            this.loadType = LoadResult.FileNotFound;
        }        
    }

    public enum LoadResult
    {
        FileNotFound = 0,
        Invalid = 1,
        Success = 2
    }
}
