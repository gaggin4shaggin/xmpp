//XMPP .NET Library Copyright (C) 2006 Dieter Lunn
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Collections;
using System.Reflection;

namespace xmpp.registries
{
	public class Registry<T, Allocator> : IDisposable where T : class where Allocator : Allocator<T>
	{
		private static readonly Allocator<T> allocator;
		
		protected Hashtable _registeredItems = new Hashtable();
		
		static Registry()
		{
			ConstructorInfo constructor = typeof(Allocator).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], new ParameterModifier[0]);
			if (constructor == null)
			{
				throw new Exception("The allocator that you want to create doesn't have a protected/private constructor");
			}
			
			try
			{
				allocator = constructor.Invoke(new object[0]) as Allocator<T>;
			}
			catch (Exception e)
			{
				throw new Exception("The allocator couldn't be constructed, make sure it has a default constructor.", e);
			}
		}
		
		public static T Instance
		{
			get { return allocator.Instance; }
		}
		
		protected object[] GetAttributes(Assembly ass, Type attribute) 
		{
			object[] attrs;
			Type[] types = ass.GetTypes();
			
			foreach (Type type in types)
			{
				object[] temp = type.GetCustomAttributes(attribute, false);
				attrs
			}
			
			return attrs;
		}
		
		public virtual void Dispose() 
		{
			allocator.Dispose();
			_registeredItems.Clear();
		}
	}
}