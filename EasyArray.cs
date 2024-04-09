using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Optomo
{
	public class EasyArray<T> : IList, IList<T>, IEnumerable<T> , IAsyncEnumerable<T>
	{
		private List<T> list;

		public EasyArray() => list = new List<T>();
		public EasyArray(T[] arrays) => list = new List<T>(arrays);
		public EasyArray(IEnumerable<T> arrays) => list = new List<T>(arrays);
		public EasyArray(IList lists) => list = new List<T>(lists as List<T>);
		public EasyArray(IList<T> lists) => list = new List<T>(lists as List<T>);
		public EasyArray(Array array) => this.list = new List<T>(array as T[]);
		public EasyArray(EasyArray<T> array) => list = array;
		public EasyArray(params T[][] arrays) => list = arrays.SelectMany(a => a).ToList();
		public EasyArray([Optional]bool param, params T[] array) => list = array.ToList();
		public EasyArray([Optional]bool param, params IEnumerable<T>[] arrays) => list = arrays.SelectMany(x=>x).ToList();
		public EasyArray([Optional]bool param, params IList<T>[] arrays) => list = (List<T>?)( arrays as List<List<T>>).SelectMany(x => x);
		public EasyArray([Optional]bool param, params IList[] arrays) => list = (List<T>?)( arrays as List<List<T>>).SelectMany(x => x);
		public EasyArray([Optional]bool param, params Array[] array ) => this.list = new List<T>( array as T [] );
		public EasyArray([Optional]bool param, params EasyArray<T>[] array) => list = array.SelectMany(x=>x).ToList();
		public EasyArray( params EasyValue<T> [] array )
		{
			list = (array as T[]).ToList();
		}

		public T this [ int index ] 
		{ 
			get => ( (IList<T>)this.list ) [ index ]; 
			set => ( (IList<T>)this.list ) [ index ]=value; 
		}
		public T[] this [ int startIndex, int endIndex ]
		{
			get => this[GetPossibleIndices(startIndex, endIndex)];
			set => this[GetPossibleIndices(startIndex, endIndex)] = value;
		}
		public T[] this [ int[] indices ] 
		{
			get
			{
				List<T> items = new List<T>();
				foreach ( var index in indices )
				{
					items.Add(( (IList<T>)this.list ) [ index ]);
				}
				return items.ToArray();
			}
			set
			{
				for (int i = 0; i < indices.Length; i++)
				{
					( (IList<T>)this.list ) [ indices[i] ] = value[i];
				}
			}
		}
		public T[] this [ params int[][] indices ]
		{
			get => this[indices.SelectMany(a => a).ToArray()];
			set => this[indices.SelectMany(a => a).ToArray()] = value;
		}
		public T[] this [ Range range ]
		{
			get => this [ GetPossibleIndices(range) ];
			set => this [ GetPossibleIndices(range) ] = value;
		}
		public T[] this [ Range[] ranges ]
		{
			get => this[GetPossibleIndices(ranges)];
			set => this[GetPossibleIndices(ranges)] = value;
		}
		public T[] this [ [Optional]bool param, params int[] indices ]
		{
			get => this[indices];
			set => this[indices] = value;
		}
		public T[] this [ [Optional]bool param, params Range[] ranges ]
		{
			get => this[ranges];
			set => this[ranges] = value;
		}
		public int this [ T values ]
		{
			get => IndexOf(values);
			set 
			{
				if (value > list.Count-1)
				{
					list.InsertRange(list.Count, Enumerable.Repeat((T) default, value+1 - this.list.Count));
					list[value] = values;
				}
				else if (value < 0)
					list[0] = values;
				else
					list[value] = values;
			}
		}
		object? IList.this [ int index ] { get => ( (IList)this.list ) [ index ]; set => ( (IList)this.list ) [ index ]=value; }


		public int Count => ( (ICollection<T>)this.list ).Count;
		public bool IsReadOnly => ( (ICollection<T>)this.list ).IsReadOnly;
		public bool IsFixedSize => ( (IList)this.list ).IsFixedSize;
		public bool IsSynchronized => ( (ICollection)this.list ).IsSynchronized;
		public object SyncRoot => ( (ICollection)this.list ).SyncRoot;





		public void Add( T item ) => ( (ICollection<T>)this.list ).Add( item );
		public int Add( object? value ) => ( (IList)this.list ).Add( value );
		public void Add( T[] items ) => this.list.AddRange(items);
		public void Add( [Optional]bool param, params T[] arrays ) => Add(arrays);
		public EasyArray<T> AddReturn( T item ) 
		{	
			this.Add(item); return this;
		}
		public EasyArray<T> AddReturn( T[] items ) 
		{
			this.list.AddRange(items);
			return this;
		}
		public EasyArray<T> AddReturn([Optional]bool param, params T[] items ) => AddReturn(items);
		public void Clear() => ( (ICollection<T>)this.list ).Clear();
		public bool Contains( T item ) => ( (ICollection<T>)this.list ).Contains( item );
		public bool Contains( out bool anyFound, T[] items )
		{
			bool allFound;
			bool currentFound;
			if ( items.Length < 1 )
			{
				allFound = false;
				anyFound = false;
				return allFound;
			}
			else
			{
				allFound = true;
				anyFound = false;
			}
			foreach ( var item in items )
			{
				currentFound = this.list.Contains( item );
				if ( currentFound )
					anyFound = true;
				else
					allFound = false;
			}
			return allFound;
		}
		public bool Contains( out bool anyFound, params T[][] items ) 
			=> Contains( out anyFound, items.SelectMany(a => a).ToArray());
		public bool Contains( object? value ) => ( (IList)this.list ).Contains( value );
		public void CopyTo( T [] array, int arrayIndex ) => ( (ICollection<T>)this.list ).CopyTo( array, arrayIndex );
		public void CopyTo( [Optional]bool param, int arrayIndex, params T[] items ) => CopyTo(items, arrayIndex);
		public void CopyTo( Array array, int index ) => ( (ICollection)this.list ).CopyTo(array, index);
		public EasyArray<T> CopyToReturn( int arrayIndex, T[] array ) 
		{
			CopyTo(array, arrayIndex);
			return this;
		}
		public EasyArray<T> CopyToReturn( [Optional]bool param, int arrayIndex, params T[] items ) => CopyToReturn(arrayIndex, items);
		public EasyArray<T> CopyToReturn( Array array, int index )
		{
			CopyTo(array, index); return this;
		}
		public IEnumerator<T> GetEnumerator() => ( (IEnumerable<T>)this.list ).GetEnumerator();
		public int IndexOf( T item ) => ( (IList<T>)this.list ).IndexOf( item );
		public int[] IndexOf( T[] items ) 
		{
			List<int> indices = new List<int>();
			foreach( var item in items )
			{
				int index = IndexOf( item );
				if ( index != -1 )
				{
					indices.Add(index);
				}
			}
			return indices.ToArray();
		}
		public int[] IndexOf( params T[][] items ) => IndexOf(items.SelectMany(x => x).ToArray());
		public int IndexOf( object? value ) => ( (IList)this.list ).IndexOf( value );
		public void Insert( int index, T item ) => ( (IList<T>)this.list ).Insert( index, item );
		public void Insert( (int, T)[] itemsInsert )
		{
			foreach( var item in itemsInsert )
			{
				((IList<T>)this.list).Insert(item.Item1, item.Item2);
			}
		}
		public void Insert( [Optional]bool param, params (int, T)[] itemsInsert ) => Insert(itemsInsert);
		public void Insert( int index, object? value ) => ( (IList)this.list ).Insert( index, value );
		public EasyArray<T> InsertReturn( int index, T item ) 
		{
			( (IList<T>)this.list).Insert(index, item);
			return this;
		}
		public EasyArray<T> InsertReturn( (int, T)[] itemsInsert )
		{
			Insert(itemsInsert);
			return this;
		}
		public EasyArray<T> InsertReturn([Optional]bool param, params (int, T)[] itemsInsert ) 
		{
			Insert(itemsInsert); return this;
		}
		public bool Remove( T item ) => ( (ICollection<T>)this.list ).Remove( item );
		public bool Remove( out bool anyFound, T[] items ) 
		{
			bool allFound;
			bool currentFound;
			if (items.Length < 1)
			{
				allFound = false;
				anyFound = false;
				return allFound;
			}
			else
			{
				allFound = true;
				anyFound = false;
			}
			foreach (var item in items) 
			{
				currentFound = this.list.Remove( item );	
				if (currentFound)
					anyFound = true;
				else
					allFound = false;
			}
			return allFound;
		}
		public bool Remove( [Optional] bool param, out bool anyFound, params T[] items ) 
			=> Remove( out anyFound, items );	
		public void Remove( object? value ) => ( (IList)this.list ).Remove( value );
		public EasyArray<T> RemoveReturn( T item )
		{
			Remove(item); return this;
		}
		public EasyArray<T> RemoveReturn( out bool anyFound, T[] items )
		{
			_ = Remove(out anyFound, items ); 
			return this;
		}
		public EasyArray<T> RemoveReturn([Optional]bool param, out bool anyFound, params T[] items )
		{
			_ = Remove(out anyFound, items);
			return this;
		}
		public void RemoveAt( int index ) => ( (IList<T>)this.list ).RemoveAt( index );
		public void RemoveAt( int[] indices ) 
		{
			foreach( int index in indices )
			{
				((IList<T>)this.list).RemoveAt( index );
			}
		}
		public void RemoveAt( [Optional]bool param, params int[] indices ) => RemoveAt( indices );
		public EasyArray<T> RemoveAtReturn( int index )
		{
			RemoveAt( index ); return this;
		}
		public EasyArray<T> RemoveAtReturn( int [] indices )
		{
			RemoveAt( indices );
			return this;
		}
		public EasyArray<T> RemoveAtReturn( [Optional]bool param, params int[] indices ) => RemoveAtReturn( indices );
		IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)this.list ).GetEnumerator();

		public int[] GetPossibleIndices( int startIndex, int endIndex )
		{
			if ( startIndex < 0 ) startIndex = 0;
			if ( endIndex < 0 ) endIndex = 0;
            if ( startIndex > list.Count - 1 ) startIndex = list.Count-1;
            if ( endIndex > list.Count - 1 ) endIndex = list.Count-1;
			
			if (startIndex < endIndex)
			{
				return Enumerable.Range(startIndex, (endIndex+1 - startIndex)).ToArray();
			}
			else
			{
				List<int> ints = new List<int>();
				for (int i = startIndex; i >= endIndex; i-- )
				{
					ints.Add(i);
				}
				return ints.ToArray();
			}
		}
		public int[] GetPossibleIndices( Range range ) => GetPossibleIndices(range.Start.Value, range.End.Value );
		public int[] GetPossibleIndices( params Range[] ranges ) 
		{
			List<List<int>> result = new();
			foreach (var range in ranges ) 
			{
				result.Add(GetPossibleIndices(range).ToList());
			}
			return result.SelectMany(x => x).ToArray();
		}

		public async IAsyncEnumerator<T> GetAsyncEnumerator( CancellationToken cancellationToken = default ) 
		{
			try
			{
				for (int i = 0; i < list.Count; i++)
				{
					yield return list[i];
				}
			} 
			finally {}
		}
		

		public static implicit operator List<T>( EasyArray<T> v ) => new List<T>( v );
		public static implicit operator Array( EasyArray<T> v ) => v.ToArray();
		public static implicit operator T[]( EasyArray<T> v ) => v.ToArray();
		public static explicit operator EasyArray<T>( List<T> v ) => new EasyArray<T> ((IList<T>)v );
		public static explicit operator EasyArray<T>( Array v ) => new EasyArray<T>( v );
		public static explicit operator EasyArray<T>( T[] v ) => new EasyArray<T>( v );
	}
}
