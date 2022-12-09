using System.Drawing;

namespace GearManager
{
    public struct BitmapOrigin
    {
        public BitmapOrigin(Bitmap bitmap, Point point)
        {
            Bitmap = bitmap;
            Origin = point;
        }

        public BitmapOrigin(Bitmap bitmap, int x, int y)
        {
            Bitmap = bitmap;
            Origin = new Point(x, y);
        }

        public BitmapOrigin(Bitmap bitmap)
        {
            Bitmap = bitmap;
            Origin = new Point(0, 0);
        }

        public Bitmap Bitmap { get; set; }
        public Point Origin { get; set; }

        public bool IsEmpty
        {
            get { return Bitmap == null || Origin == null; }
        }

        public BitmapOrigin Clone()
        {
            return IsEmpty ? new BitmapOrigin() : new BitmapOrigin(new Bitmap(Bitmap), Origin);
        }
    }
}
