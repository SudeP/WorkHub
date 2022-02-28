﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using System.Reflection;

namespace Api.ASPNET.Service.Inheritance
{
	public interface IHaveCustomMapping
	{
		void CreateMappings(Profile configuration);
	}

	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			LoadCustomMappings();
		}

		private void LoadCustomMappings()
		{
			var mapsFrom = MapperProfileHelper.LoadCustomMappings(Assembly.GetExecutingAssembly());

			foreach (var map in mapsFrom)
			{
				map.CreateMappings(this);
			}
		}
	}

	public sealed class Map
	{
		public Type Source { get; set; }
		public Type Destination { get; set; }
	}

	public static class MapperProfileHelper
	{

		public static IList<IHaveCustomMapping> LoadCustomMappings(Assembly rootAssembly)
		{
			var types = rootAssembly.GetExportedTypes();

			var mapsFrom = (
				from type in types
				from instance in type.GetInterfaces()
				where
					typeof(IHaveCustomMapping).IsAssignableFrom(type) &&
					!type.IsAbstract &&
					!type.IsInterface
				select (IHaveCustomMapping)Activator.CreateInstance(type)).ToList();

			return mapsFrom;
		}
	}
}
