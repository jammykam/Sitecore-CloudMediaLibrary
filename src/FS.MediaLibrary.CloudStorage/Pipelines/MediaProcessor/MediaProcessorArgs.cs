using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace FS.MediaLibrary.CloudStorage.Pipelines.MediaProcessor
{
    public class MediaProcessorArgs : PipelineArgs
    {
        public IEnumerable<Item> UploadedItems { get; set; }
    }
}
