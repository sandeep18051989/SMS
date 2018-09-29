﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using EF.Core.Mapper;

namespace EF.Core
{
	/// <summary>
	/// Engine
	/// </summary>
	public class CMSEngine : IEngine
	{
		#region Fields

		private ContainerManager _containerManager;

		#endregion

		#region Utilities

		/// <summary>
		/// Register dependencies
		/// </summary>
		/// <param name="config">Config</param>
		protected virtual void RegisterDependencies(CMSConfig config)
		{
			var builder = new ContainerBuilder();
			var container = builder.Build();
			this._containerManager = new ContainerManager(container);

			//we create new instance of ContainerBuilder
			//because Build() or Update() method can only be called once on a ContainerBuilder.

			//dependencies
			var typeFinder = new WebAppTypeFinder();
			builder = new ContainerBuilder();
			builder.RegisterInstance(config).As<CMSConfig>().SingleInstance();
			builder.RegisterInstance(this).As<IEngine>().SingleInstance();
			builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
			builder.Update(container);

			//register dependencies provided by other assemblies
			builder = new ContainerBuilder();
			var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
			var drInstances = new List<IDependencyRegistrar>();
			foreach (var drType in drTypes)
				drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
			//sort
			drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
			foreach (var dependencyRegistrar in drInstances)
				dependencyRegistrar.Register(builder, typeFinder, config);
			builder.Update(container);

			//set dependency resolver
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}

		protected virtual void RegisterMapperConfiguration(CMSConfig config)
		{
			//dependencies
			var typeFinder = new WebAppTypeFinder();

			//register mapper configurations provided by other assemblies
			var mcTypes = typeFinder.FindClassesOfType<IMapperConfiguration>();
			var mcInstances = new List<IMapperConfiguration>();
			foreach (var mcType in mcTypes)
				mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
			//sort
			mcInstances = mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
			//get configurations
			var configurationActions = new List<Action<IMapperConfigurationExpression>>();
			foreach (var mc in mcInstances)
				configurationActions.Add(mc.GetConfiguration());
			//register
			AutoMapperConfiguration.Init(configurationActions);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initialize components and plugins in the nop environment.
		/// </summary>
		/// <param name="config">Config</param>
		public void Initialize(CMSConfig config)
		{
			//register dependencies
			RegisterDependencies(config);

			// register auto mapping tasks
			RegisterMapperConfiguration(config);

			//startup tasks
			var settings = ContextHelper.Current.Resolve<DatabaseSettings>();
			if (settings != null && settings.IsValid())
			{
				var provider = ContextHelper.Current.Resolve<IDataProvider>();
				if (provider == null)
					throw new Exception("No IDataProvider found");

				provider.SetDatabaseInitializer();
			}

		}

		/// <summary>
		/// Resolve dependency
		/// </summary>
		/// <typeparam name="T">T</typeparam>
		/// <returns></returns>
		public T Resolve<T>() where T : class
		{
			return ContainerManager.Resolve<T>();
		}

		/// <summary>
		///  Resolve dependency
		/// </summary>
		/// <param name="type">Type</param>
		/// <returns></returns>
		public object Resolve(Type type)
		{
			return ContainerManager.Resolve(type);
		}

		/// <summary>
		/// Resolve dependencies
		/// </summary>
		/// <typeparam name="T">T</typeparam>
		/// <returns></returns>
		public T[] ResolveAll<T>()
		{
			return ContainerManager.ResolveAll<T>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Container manager
		/// </summary>
		public ContainerManager ContainerManager
		{
			get { return _containerManager; }
		}

		#endregion
	}
}
