using System;
using ModestTree;

#if !ZEN_NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public interface IGenericBinder<TContract> : ITypeBinder
    {
        // _____ ToSingle _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSingle<TConcrete>()
            where TConcrete : TContract;

        BindingConditionSetter ToSingle<TConcrete>(string concreteIdentifier)
            where TConcrete : TContract;

        // _____ ToInstance _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToInstance<TConcrete>(TConcrete instance)
            where TConcrete : TContract;

        // _____ ToTransient _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToTransient<TConcrete>()
            where TConcrete : TContract;

#if !ZEN_NOT_UNITY3D
        // _____ ToSinglePrefab _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSinglePrefab<TConcrete>(GameObject prefab)
            where TConcrete : TContract;

        BindingConditionSetter ToSinglePrefab<TConcrete>(
            string concreteIdentifier, GameObject prefab)
            where TConcrete : TContract;

        // _____ ToTransientPrefab _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToTransientPrefab<TConcrete>(GameObject prefab)
            where TConcrete : TContract;

        // _____ ToSinglePrefabResource _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSinglePrefabResource<TConcrete>(string resourcePath)
            where TConcrete : TContract;

        BindingConditionSetter ToSinglePrefabResource<TConcrete>(string concreteIdentifier, string resourcePath)
            where TConcrete : TContract;

        // _____ ToTransientPrefabResource _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToTransientPrefabResource<TConcrete>(string resourcePath)
            where TConcrete : TContract;

        // _____ ToSingleGameObject _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSingleGameObject<TConcrete>()
            where TConcrete : Component, TContract;

        BindingConditionSetter ToSingleGameObject<TConcrete>(string concreteIdentifier)
            where TConcrete : Component, TContract;

        // _____ ToTransientGameObject _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToTransientGameObject<TConcrete>()
            where TConcrete : Component, TContract;

        // _____ ToSingleMonoBehaviour _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSingleMonoBehaviour<TConcrete>(GameObject gameObject)
            where TConcrete : TContract;

        BindingConditionSetter ToSingleMonoBehaviour<TConcrete>(string concreteIdentifier, GameObject gameObject)
            where TConcrete : TContract;

        // _____ ToResource _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToResource<TConcrete>(string resourcePath)
            where TConcrete : TContract;
#endif

        // _____ ToMethod _____  - Inject by custom method
        //
        //  This binding allows you to customize creation logic yourself by defining a method.
        //  For example:
        //
        //  Container.Bind<IFoo>().ToMethod(SomeMethod);
        //
        //  public IFoo SomeMethod(InjectContext context)
        //  {
        //      ...
        //      return new Foo();
        //  }
        //
        BindingConditionSetter ToMethod(Func<InjectContext, TContract> method);

        // _____ ToSingleMethod _____  - Inject using a custom method but only call that method once
        //
        //  This binding works similar to ToMethod except that the given method will only be
        //  called once. The value returned from the method will then be used for every
        //  subsequent request for the given dependency.
        //
        //  Example:
        //
        //  Container.Bind<IFoo>().ToSingleMethod(SomeMethod);
        //
        //  public IFoo SomeMethod(InjectContext context)
        //  {
        //      ...
        //      return new Foo();
        //  }
        //
        //
        BindingConditionSetter ToSingleMethod<TConcrete>(
            string concreteIdentifier, Func<InjectContext, TConcrete> method)
            where TConcrete : TContract;

        BindingConditionSetter ToSingleMethod<TConcrete>(Func<InjectContext, TConcrete> method)
            where TConcrete : TContract;

        // _____ ToGetter _____  - Inject by getter.
        //
        //  This method can be useful if you want to bind to a property of another object.
        //
        //  Example:
        //
        //  Container.Bind<IFoo>().ToSingle<Foo>()
        //  Container.Bind<Bar>().ToGetter<IFoo>(x => x.GetBar())
        //
        //  Note here that it gets IFoo by doing a recursive lookup on the container for whatever
        //  value is bound to <IFoo>
        //
        BindingConditionSetter ToGetter<TObj>(Func<TObj, TContract> method);

        BindingConditionSetter ToGetter<TObj>(string identifier, Func<TObj, TContract> method);

        // _____ ToResolve _____  - Inject by recursive resolve
        //
        //  Examples
        //
        //    Container.Bind<IFoo>().ToLookup<IBar>()
        //    Container.Bind<IBar>().ToLookup<Foo>()
        //
        //  In some cases it is useful to be able to bind an interface to another interface.
        //  However, you cannot use ToSingle or ToTransient because they both require concrete
        //  types.
        //
        //  In the example code above we assume that Foo inherits from IBar, which inherits
        //  from IFoo. The result here will be that all dependencies for IFoo will be bound
        //  to whatever IBar is bound to (in this case, Foo).
        //
        BindingConditionSetter ToResolve<TConcrete>()
            where TConcrete : TContract;

        BindingConditionSetter ToResolve<TConcrete>(string identifier)
            where TConcrete : TContract;

        // _____ ToSingleInstance _____  - Treat the given instance as a singleton.
        //
        //  This is the same as ToInstance except it will ensure that there is only ever one
        //  instance for the given type.
        //
        //  When using ToInstance you can do the following:
        //
        //  Container.Bind<Foo>().ToInstance(new Foo());
        //  Container.Bind<Foo>().ToInstance(new Foo());
        //  Container.Bind<Foo>().ToInstance(new Foo());
        //
        //  Or, equivalently:
        //
        //  Container.BindInstance(new Foo());
        //  Container.BindInstance(new Foo());
        //  Container.BindInstance(new Foo());
        //
        //  And then have a class that takes all of them as a list like this:
        //
        //  public class Bar
        //  {
        //      public Bar(List<Foo> foos)
        //      {
        //      }
        //  }
        //
        //  Whereas, if you use ToSingleInstance this would trigger an error.
        //
        BindingConditionSetter ToSingleInstance<TConcrete>(TConcrete instance)
            where TConcrete : TContract;

        BindingConditionSetter ToSingleInstance<TConcrete>(
            string concreteIdentifier, TConcrete instance)
            where TConcrete : TContract;

        // _____ ToSingleFactory _____  - Define a custom factory for a singleton
        //
        //  Example:
        //
        //  Container.Bind<IFoo>().ToSingleFactory<MyCustomFactory>();
        //
        //  class MyCustomFactory : IFactory<IFoo>
        //  {
        //      Bar _bar;
        //
        //      public MyCustomFactory(Bar bar)
        //      {
        //          _bar = bar;
        //      }
        //
        //      public IFoo Create()
        //      {
        //          ...
        //      }
        //  }
        //
        //  The ToSingleFactory binding can be useful when you want to define a singleton,
        //  but it has complex construction logic that you want to define yourself. You could
        //  use ToSingleMethod, but this can get ugly if your construction logic itself has
        //  its own dependencies that it needs. Using ToSingleFactory for this case it is nice
        //  because any dependencies that you require for construction can be simply added to
        //  the factory constructor
        //
        BindingConditionSetter ToSingleFactory<TFactory>()
            where TFactory : IFactory<TContract>;

        BindingConditionSetter ToSingleFactory<TFactory>(string concreteIdentifier)
            where TFactory : IFactory<TContract>;

        BindingConditionSetter ToSingleFactory<TFactory, TConcrete>()
            where TFactory : IFactory<TConcrete>
            where TConcrete : TContract;

        BindingConditionSetter ToSingleFactory<TFactory, TConcrete>(string concreteIdentifier)
            where TFactory : IFactory<TConcrete>
            where TConcrete : TContract;

        // _____ ToSingleFacadeMethod _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSingleFacadeMethod<TConcrete>(Action<DiContainer> installerFunc)
            where TConcrete : TContract;

        BindingConditionSetter ToSingleFacadeMethod<TConcrete>(
            string concreteIdentifier, Action<DiContainer> installerFunc)
            where TConcrete : TContract;

        // _____ ToSingleFacadeInstaller _____
        //
        //  See description in ITypeBinder
        //
        BindingConditionSetter ToSingleFacadeInstaller<TConcrete, TInstaller>()
            where TConcrete : TContract
            where TInstaller : Installer;

        BindingConditionSetter ToSingleFacadeInstaller<TConcrete, TInstaller>(
            string concreteIdentifier)
            where TConcrete : TContract
            where TInstaller : Installer;
    }
}

