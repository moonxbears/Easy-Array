using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Optomo
{
	public class InterpArray<T> : EasyArray<T> where T : unmanaged
	{
		
		public InterpArray( float f ) : base()
		{
			
		}
		public InterpArray(T data) : base(true, data) {}
		public InterpArray(T[] arrays) : base(arrays) {}
		public InterpArray(IEnumerable<T> arrays) : base(arrays) {}
		public InterpArray(IList lists) : base(lists) {}
		public InterpArray(IList<T> lists) : base(lists) {}
		public InterpArray(Array array) : base(array) {}
		public InterpArray(EasyArray<T> arrays) : base(arrays) {}
		public InterpArray(params T[][] arrays) : base(arrays) {}
		public InterpArray([Optional] bool param, params T [] array ) : base (param, array) {}
		public InterpArray([Optional] bool param, params IEnumerable<T> [] arrays ) : base (param, arrays) {}
		public InterpArray([Optional] bool param, params IList<T> [] arrays ) : base(param, arrays) {}
		public InterpArray([Optional] bool param, params IList [] arrays ) : base(param, arrays) {}
		public InterpArray([Optional] bool param, params Array [] array ) : base( param, array ) {}
		public InterpArray([Optional] bool param, params EasyArray<T> [] array ) : base(param, array) {}
		public InterpArray( params EasyValue<T> [] array ) : base(array) { }

		public T this [ float indexInterpolation ]
		{
			get 
			{
				if (Count < 2) throw new IndexOutOfRangeException("can't attach to indices");
				int lower = (int)MathF.Floor(indexInterpolation);
				int upper = (int)MathF.Ceiling(indexInterpolation);
				float actualDiff;

				if (upper > Count - 1)
				{
					upper = Count - 1;
					lower = Count - 2;
				}
				else if (lower < 0)
				{
					upper = 1;
					lower = 0;
				}
				actualDiff = ( indexInterpolation - lower );
				return ConvertToT(Lerp(base[lower], base[upper], actualDiff));
			}
		}

		public static double Lerp(double a, double b, double t)
		{
			return a + (b - a) * t;
		}
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}
		public dynamic Lerp(dynamic a, dynamic b, float t)
		{
			return (float)a + ((float)b - (float)a) * t;
		}
		public static double InvLerp(double a, double b, double v)
		{
			return (v - a) / (b - a);
		}
		public static float InvLerp(float a, float b, float v)
		{
			return (v - a) / (b - a);
		}
		public T InvLerp(dynamic a, dynamic b, dynamic v)
		{

			return (v - a) / (b - a);
		}
		public T ConvertToT(dynamic value) => (T)Convert.ChangeType(value, typeof(T));
		
		public static explicit operator InterpArray<T>(float f) 
		{
			return new InterpArray<T>(f);
		}

	}
}
