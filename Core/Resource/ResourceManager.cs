using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Resource
{

    public class ResourceManager
    {

        private const string CoreResourcePath = "Resources/Core";

        private IDictionary<Type, IResourceCollection> resourceCollectionByType
            = new Dictionary<Type, IResourceCollection>();

        private IList<IResourceFactory> resourceFactories
            = new List<IResourceFactory>();

        public void AddResourceFactory(IResourceFactory factory)
        {
            resourceFactories.Add(factory);
        }

        public void RemoveResourceFactory(IResourceFactory factory)
        {
            resourceFactories.Remove(factory);
        }

        public void LoadResources(IEnumerable<string> resourcePaths, bool clearResources = false)
        {
            if (clearResources)
            {
                resourceCollectionByType.Clear();
            }

            LoadResources(CoreResourcePath);
            foreach (string resourcePath in resourcePaths)
            {
                LoadResources(resourcePath);
            }

        }

        private void LoadResources(string resourcePath)
        {
            foreach (IResourceFactory factory in resourceFactories)
            {
                foreach (IResource resource in factory.LoadResources(resourcePath))
                {
                    if (resource != null)
                    {
                        AddResource(resource);
                    }
                }
            }
        }

        public void AddResource(IResource resource)
        {
            IResourceCollection resourceCollection
                = GetResourceCollection(resource.GetType());
            resourceCollection.AddResource(resource);
        }

        public void AddResource<T>(T resource) where T : IResource
        {
            var resourceCollection = GetResourceCollection(typeof(T));
            resourceCollection.AddResource(resource);
        }

        public bool HasResource<T>(string name) where T : IResource
        {
            var resourceCollection = (ResourceCollection<T>)
                resourceCollectionByType[typeof(T)];
            return resourceCollection.HasResource(name);
        }

        public T GetResource<T>(string name) where T : IResource
        {
            var resourceCollection = (ResourceCollection<T>)
                resourceCollectionByType[typeof(T)];
            return resourceCollection.GetResource(name);

        }

        public void RemoveResource<T>(T resource) where T : IResource
        {
            var resourceCollection = (ResourceCollection<T>)
                resourceCollectionByType[typeof(T)];
            resourceCollection.RemoveResource(resource.Name);
        }

        public void SetDefault<T>(string name) where T : IResource
        {
            var resourceCollection = (ResourceCollection<T>)
                resourceCollectionByType[typeof(T)];
            resourceCollection.DefaultName = name;
        }

        private IResourceCollection GetResourceCollection(Type type)
        {
            if (!resourceCollectionByType.ContainsKey(type))
            {
                Type genericType = typeof(ResourceCollection<>).MakeGenericType(type);
                resourceCollectionByType[type] = (IResourceCollection)
                    Activator.CreateInstance(genericType);
            }

            return resourceCollectionByType[type];

        }

        private interface IResourceCollection
        {

            void AddResource(IResource resource);

        }

        private class ResourceCollection<T> : IResourceCollection where T : IResource
        {

            public string DefaultName { get; set; }

            private IDictionary<string, T> resources;

            public ResourceCollection()
            {
                resources = new Dictionary<string, T>();
            }

            public void AddResource(IResource resource)
            {
                resources[resource.Name] = (T)resource;
            }

            public bool HasResource(string name)
            {
                return resources.ContainsKey(name);
            }

            public T GetResource(string name)
            {
                if (resources.ContainsKey(name ?? string.Empty))
                {
                    return resources[name];
                }
                else if (resources.ContainsKey(DefaultName ?? string.Empty))
                {
                    return resources[DefaultName];
                }
                else
                {
                    throw new ArgumentException(string.Format(
                        "Could not find resource {0}", name));
                }
            }

            public void RemoveResource(string name)
            {
                resources.Remove(name);
            }

        }

    }

}

