using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Streams;

namespace win2d_text_game
{
    public class World
    {
        public List<Region> Regions = new List<Region>();

        public async Task Load()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\world");
                var file = await folder.GetFileAsync("world_save.xml");
                var stream = await file.OpenStreamForReadAsync();

                XDocument worldDocument = XDocument.Load(stream);
                await stream.FlushAsync();

                var regionNodes = from regions in worldDocument
                                      .Elements("world")
                                        .Elements("regions")
                                          .Elements("region")
                                  select regions;
                foreach (var regionNode in regionNodes)
                {
                    // Regions.Add(new Region(regionNode));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task LoadCompressed()
        {
            try
            {
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml");
                var file = await folder.GetFileAsync("world.compressed");
                //var stream = await file.OpenStreamForReadAsync();

                var decompressedFilename = "world.decompressed";
                var decompressedFile = await folder.CreateFileAsync(decompressedFilename, CreationCollisionOption.ReplaceExisting);

                using (var compressedInput = await file.OpenSequentialReadAsync())
                using (var decompressor = new Decompressor(compressedInput))
                using (var decompressedOutput = await decompressedFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var bytesDecompressed = await RandomAccessStream.CopyAsync(decompressor, decompressedOutput);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            await Load();
        }

        public void Update()
        {
            foreach(Region region in Regions)
            {
                region.Update();
            }
        }
    }
}
