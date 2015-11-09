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
    public delegate void GameCommand(string strAppend);

    public static class Game
    {
        public static event GameCommand OnGameCommand;

        public static World World;

        #region Initialization
        public static void Initialize()
        {

        }
        #endregion
    }
}
