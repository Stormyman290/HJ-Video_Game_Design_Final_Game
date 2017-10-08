using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Beta
{
    class Season
    {
        private String name;
        private int tilesStart, tilesNum;
        private Random rnd;
        private Hazard hazard;

        public Season(String name, int numOfTiles, int startAt, Hazard hazard)
        {
            this.name = name;
            this.hazard = hazard;
            tilesStart = startAt;
            tilesNum = numOfTiles;
            rnd = new Random();
        }

        public Hazard getCurrentHazard()
        {
            return hazard;
        }

        /*public bool doWeDrawAHazardRightNow()
        {
            return isHazardHappening;
        }*/

        public int getTilesStartPoint()
        {
            return tilesStart;
        }

        public int getNumberOfTiles()
        {
            return tilesNum;
        }

        public string getName()
        {
            return name;
        }
    }
}