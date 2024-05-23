using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonPlantation
{
    internal class Items
    {

        public int positionX;
        public int positionY;
        public Image item_image;
        public int height;
        public int width;
        public string name;

        Random range = new Random();
        int lifeTime = 200;
        public bool expired = false;

        public Items()
        {
            item_image = Image.FromFile("items/item_01.png");

            positionX = range.Next(10, 700);
            positionY = range.Next(10, 500);

            height = 50;
            width = 50;


        }

        public void CheckLifeTime()
        {

            lifeTime--;
            if (lifeTime < 1)
            {
                expired = true;
            }

        }
    }
}
