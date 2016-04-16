using System.Collections.Generic;
using FS.MediaLibrary.CloudStorage.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Attach;

namespace FS.MediaLibrary.CloudStorage.Pipelines.attachFile
{
    /// <summary>
    /// Kicks off process to start media upload job
    /// </summary>
    public class ProcessMedia
    {
        public void Process(AttachArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (!args.MediaItem.FileBased)
                return;

            var helper = new PipelineHelper();
            helper.StartMediaProcessorJob(new List<Item> { args.MediaItem });
        }
    }
}
