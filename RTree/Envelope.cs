using System;

namespace RTree
{
	public class Envelope
	{
		public double MinX;
		public double MinY;
		public double MaxX;
		public double MaxY;

		private double? _centerX;
		public double CenterX
		{
			get
			{
				if (Math.Abs(MinX - MaxX) < 0.1)
					_centerX = MinX;
				else
				{
					_centerX = MinX + MaxX * 0.5f;
				}
				return _centerX.Value;
			}
		}

		private double? _centerY;
		public double CenterY
		{
			get
			{
				if (_centerY == null)
				{
					if (Math.Abs(MinY - MaxY) < 0.1)
						_centerY = MinY;
					else
					{
						_centerY = MinY + MaxY * 0.5f;
					}
				}
				return _centerY.Value;
			}
			set { _centerY = value; }
		}


        // NB: Feature `expression bodied members' cannot be used because it is not part of the C# 4.0 language specification
        public double Area
        {
            get
            {
                return Math.Max(this.MaxX - this.MinX, 0) * Math.Max(this.MaxY - this.MinY, 0);
            }
        }

		public double Margin
        {
            get
            {
                return Math.Max(this.MaxX - this.MinX, 0) + Math.Max(this.MaxY - this.MinY, 0);
            }
        }

		public Envelope() { }
		public Envelope(double minX, double minY, double maxX, double maxY)
		{
			MinX = minX;
			MinY = minY;
			MaxX = maxX;
			MaxY = maxY;
		}

		public Envelope(float x, float y)
		{
			MinX = x;
			MaxX = x;
			MinY = y;
			MaxY = y;
		}

		public void Extend(Envelope other)
		{
			this.MinX = Math.Min(this.MinX, other.MinX);
			this.MinY = Math.Min(this.MinY, other.MinY);
			this.MaxX = Math.Max(this.MaxX, other.MaxX);
			this.MaxY = Math.Max(this.MaxY, other.MaxY);
		}
		
		public Envelope Clone()
		{
			return new Envelope
			{
				MinX = this.MinX,
				MinY = this.MinY,
				MaxX = this.MaxX,
				MaxY = this.MaxY,
			};
		}

		public Envelope Intersection(Envelope other)
		{
			return new Envelope
			{
				MinX = Math.Max(this.MinX, other.MinX),
				MinY = Math.Max(this.MinY, other.MinY),
				MaxX = Math.Min(this.MaxX, other.MaxX),
				MaxY = Math.Min(this.MaxY, other.MaxY),
			};
		}

		public Envelope Enlargement(Envelope other)
		{
			var clone = this.Clone();
			clone.Extend(other);
			return clone;
		}

		public bool Contains(Envelope other)
		{
			return
				this.MinX <= other.MinX &&
				this.MinY <= other.MinY &&
				this.MaxX >= other.MaxX &&
				this.MaxY >= other.MaxY;
		}

		public bool Intersects(Envelope other)
		{
			return
				this.MinX <= other.MaxX &&
				this.MinY <= other.MaxY &&
				this.MaxX >= other.MinX &&
				this.MaxY >= other.MinY;
		}

		public static Envelope InfiniteBounds
        {
            get
            {
                return new Envelope
                {
                    MinX = double.NegativeInfinity,
                    MinY = double.NegativeInfinity,
                    MaxX = double.PositiveInfinity,
                    MaxY = double.PositiveInfinity,
                };
            }
        }
		

		public static Envelope EmptyBounds
        {
            get
            {
                return new Envelope
                {
                    MinX = double.PositiveInfinity,
                    MinY = double.PositiveInfinity,
                    MaxX = double.NegativeInfinity,
                    MaxY = double.NegativeInfinity,
                };
            }
        }
			

		public static double Distance(Envelope a, Envelope b)
		{
			var aLength = Math.Sqrt(Math.Pow(a.CenterX, 2) + Math.Pow(a.CenterY, 2));
			var bLength = Math.Sqrt(Math.Pow(b.CenterX, 2) + Math.Pow(b.CenterY, 2));
			return Math.Abs(aLength - bLength);
		}

		public override string ToString()
		{
			return String.Format("Envelope: MinX={0}, MinY={2}, MaxX{2}, MaxY{3}", MinX, MinY, MaxX, MaxY);
		}
	}
}