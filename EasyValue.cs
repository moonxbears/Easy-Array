using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Optomo
{
	public class EasyValue<T> : IList<T>, IList, IConvertible, IComparable<EasyValue<T>>, IComparable<List<EasyValue<T>>>
	{
		private List<T> m_Values { get; set; }
		public T Value
		{
			get => m_Values[0];
			set => m_Values[0] = value;
		}

		public int Count => ( (ICollection<T>)this.m_Values ).Count;
		public bool IsReadOnly => ( (ICollection<T>)this.m_Values ).IsReadOnly;
		public bool IsFixedSize => ( (IList)this.m_Values ).IsFixedSize;
		public bool IsSynchronized => ( (ICollection)this.m_Values ).IsSynchronized;
		public object SyncRoot => ( (ICollection)this.m_Values ).SyncRoot;



		object? IList.this [ int index ] { get => ( (IList)this.m_Values ) [ index ]; set => ( (IList)this.m_Values ) [ index ]=value; }
		public T this [ int index ] 
		{ 
			get 
			{
				T Val = ( (IList<T>)this.m_Values ) [ index ];
				if ( Val is EasyValue<T> )
				{
					
				}
				return ( (IList<T>)this.m_Values ) [ index ]; 
			}
			set => ( (IList<T>)this.m_Values ) [ index ]=value; 
		}
		public EasyValue(T value) 
		{ 
			m_Values = new List<T>
			{
				value,
			}; 
		}
		public EasyValue(T[] value)
		{
			m_Values = new List<T>( value );
		}
		public EasyValue(IList<T> value)
		{
			m_Values = new List<T>(value);
		}
		public EasyValue(Array value)
		{
			m_Values = new List<T>(value as IList<T>);
		}
		public EasyValue(EasyArray<T> value)
		{
			m_Values = new EasyArray<T>(value);
		}

		public static bool IsNumeric<T>() => IsNumeric( typeof( T ) );
		public static bool IsNumeric(Type type)
		{
			switch(Type.GetTypeCode(type))
			{
				case TypeCode.Byte: case TypeCode.SByte: case TypeCode.Int16: 
				case TypeCode.Int32: case TypeCode.Int64: case TypeCode.Single:
				case TypeCode.Double: case TypeCode.Decimal: case TypeCode.UInt16: 
				case TypeCode.UInt32: case TypeCode.UInt64:
					return true;
				case TypeCode.Object:
					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof( Nullable<>))
					{
						return IsNumeric(Nullable.GetUnderlyingType(type));
					}
					return false;
			}
			return false;			
		}


		// array
		public int CompareTo( List<EasyValue<T>> other ) => ((IComparable<List<T>>)m_Values).CompareTo( (List<T>?)other.Select (x => x.m_Values));
		public int IndexOf( T item ) => ( (IList<T>)this.m_Values ).IndexOf( item );
		public int IndexOf( object? value ) => ( (IList)this.m_Values ).IndexOf( value );
		public void Insert( int index, T item ) => ( (IList<T>)this.m_Values ).Insert( index, item );
		public void Insert( int index, object? value ) => ( (IList)this.m_Values ).Insert( index, value );
		public void RemoveAt( int index ) => ( (IList<T>)this.m_Values ).RemoveAt( index );
		public void Add( T item ) => ( (ICollection<T>)this.m_Values ).Add( item );
		public int Add( object? value ) => ( (IList)this.m_Values ).Add( value );
		public void Clear() => ( (ICollection<T>)this.m_Values ).Clear();
		public bool Contains( T item ) => ( (ICollection<T>)this.m_Values ).Contains( item );
		public bool Contains( object? value ) => ( (IList)this.m_Values ).Contains( value );
		public void CopyTo( T [] array, int arrayIndex ) => ( (ICollection<T>)this.m_Values ).CopyTo( array, arrayIndex );
		public void CopyTo( Array array, int index ) => ( (ICollection)this.m_Values ).CopyTo( array, index );
		public bool Remove( T item ) => ( (ICollection<T>)this.m_Values ).Remove( item );
		public void Remove( object? value ) => ( (IList)this.m_Values ).Remove( value );
		public IEnumerator<T> GetEnumerator() 
		{
			foreach (var item in this.m_Values )
			{
				if ( item is EasyValue<T> )
				{
					yield return ( item as EasyValue<T> );
				}
			}
			//return ( (IEnumerable<T>)this.m_Values ).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)this.m_Values ).GetEnumerator();

		// value
		public int CompareTo( EasyValue<T> other ) => ((IComparable<T>)m_Values).CompareTo(other.m_Values [ 0 ] );
		public TypeCode GetTypeCode()											=> TypeCode.Object;
		public bool ToBoolean( IFormatProvider? provider )						=> Convert.ToBoolean(m_Values[0]);
		public byte ToByte( IFormatProvider? provider )							=> Convert.ToByte(m_Values[0]);
		public char ToChar( IFormatProvider? provider )							=> Convert.ToChar(m_Values[0]);
		public DateTime ToDateTime( IFormatProvider? provider )					=> Convert.ToDateTime(m_Values [ 0 ] );
		public decimal ToDecimal( IFormatProvider? provider )					=> Convert.ToDecimal(m_Values [ 0 ] );
		public double ToDouble( IFormatProvider? provider )						=> Convert.ToDouble(m_Values [ 0 ] );
		public short ToInt16( IFormatProvider? provider )						=> Convert.ToInt16(m_Values [ 0 ] );
		public int ToInt32( IFormatProvider? provider )							=> Convert.ToInt32(m_Values [ 0 ] );
		public long ToInt64( IFormatProvider? provider )						=> Convert.ToInt64(m_Values [ 0 ] );
		public sbyte ToSByte( IFormatProvider? provider )						=> Convert.ToSByte(m_Values [ 0 ] );
		public float ToSingle( IFormatProvider? provider )						=> Convert.ToSingle(m_Values [ 0 ] );
		public string ToString( IFormatProvider? provider )						=> Convert.ToString(m_Values [ 0 ] );
		public object ToType( Type conversionType, IFormatProvider? provider )	=> ((IConvertible)this).ToType(conversionType, provider);
		public ushort ToUInt16( IFormatProvider? provider )						=> Convert.ToUInt16(m_Values [ 0 ] );
		public uint ToUInt32( IFormatProvider? provider )						=> Convert.ToUInt32(m_Values [ 0 ] );
		public ulong ToUInt64( IFormatProvider? provider )						=> Convert.ToUInt64(m_Values [ 0 ] );

		// as array
		public static implicit operator List<T>( EasyValue<T> v ) => new List<T>( v );
		public static implicit operator Array( EasyValue<T> v ) => v.ToArray();
		public static implicit operator EasyArray<T>( EasyValue<T> v ) => (EasyArray<T>)v.m_Values;
		public static explicit operator EasyValue<T>( List<T> v ) => new EasyValue<T>( v );
		public static explicit operator EasyValue<T>( Array v ) => new EasyValue<T>( v );
		public static explicit operator EasyValue<T>( EasyArray<T> v ) => new EasyValue<T>( v );

		// as value
		public static implicit operator T ( EasyValue<T> v ) 
		{
			return v.Value;
		}
		public static explicit operator EasyValue<T>( T v ) => new EasyValue<T>( v as List<T> );
	}
}
