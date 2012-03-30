using System;
using Microsoft.Practices.Unity;

namespace IoC
{
    public class Container
    {
        private static Container _container;
        private readonly IUnityContainer _unityContainer = new UnityContainer();

        private Container()
        {
        }

        public IUnityContainer UnityContainer
        {
            get { return _unityContainer; }
        }

        public static Container Instance
        {
            get { return _container ?? (_container = new Container()); }
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public Container RegisterType<T>(params InjectionMember[] injectionMembers)
        {
            _unityContainer.RegisterType<T>(injectionMembers);
            return this;
        }

        public Container RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>(injectionMembers);
            return this;
        }

        public Container RegisterType(Type tFrom, Type tTo, params InjectionMember[] injectionMembers)
        {
            _unityContainer.RegisterType(tFrom, tTo, injectionMembers);
            return this;
        }

        public Container RegisterInstance<TInterface>(TInterface instance)
        {
            _unityContainer.RegisterInstance(instance);
            return this;
        }
    }
}