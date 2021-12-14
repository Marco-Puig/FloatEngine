using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Serialization;

namespace FloatEngine
{
    public class LevelData
    {
        //Xml element: a tag so xml can tell the difference between for ex: player and enemies even if they are both a Gameobject.
        [XmlElement("Player", Type = typeof(Player))]
        [XmlElement("Enemy", Type = typeof(Enemy))]
        [XmlElement("PowerUp", Type = typeof(PowerUp))]
        [XmlElement("Snow", Type = typeof(Snow))]

        public List<GameObject> objects { get; set; }
        public List<Wall> walls { get; set; }
        public List<Decor> decor { get; set; }

        public int mapWidth { get; set; }
        public int mapHeight { get; set; }

    }
}
