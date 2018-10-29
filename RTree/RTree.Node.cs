using System;
using System.Collections.Generic;
using System.Linq;

namespace RTree
{
	public partial class RTree<T>
	{
		internal class Node : ISpatialData
		{
			public Node(List<ISpatialData> items, int height)
			{
				this.Height = height;
				this.Children = items;
				ResetEnvelope();
			}

			public void Add(ISpatialData node)
			{
				Children.Add(node);
				Envelope.Extend(node.Envelope);
			}

			public void ResetEnvelope()
			{
				Envelope = GetEnclosingEnvelope(Children);
			}

			public List<ISpatialData> Children { get; set; }
			public int Height { get; set; }
			public bool IsLeaf
            {
                get
                {
                    return Height == 1;
                }
            }
			public Envelope Envelope { get; set; }
		}
	}
}