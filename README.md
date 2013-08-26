RDI
===

** .NET Dependency Injection Container **

*You can see current main features at RDI.Tests*


<pre>RDI.Concrete.Containers.RdiContainer</pre> - main logic and dependency settings.
<pre>RDI.Concrete.Containers.RdiResolver</pre> is used for global access  to created at program entry point <pre>RdiContainer</pre>

Current features:
- Resolve as Interfaces, as Classes
- Constructor and Property injection support via <pre>RDI.Attributes.RDInjectAttribute</pre>
- Support multiple dependency configuration for one Interface, controlled with 
<pre>RDI.Attributes.RDInjectAttribute .Name</pre> property and WhenName method
- Support resolving through resolving
- Easy usage Asp.Net MVC when redefining conroller factory and resolving controller or another ways!

Examples:

- Simple usage
<pre>
  var rdiContainer = new RDI.Concrete.Containers.RdiContainer();
  rdiContainer.Resolve<IBase>().With<Base>();
  
  var resolved = rdiContainer.Get<IBase>(); // resolved.GetType() == Base
  
  RdiResolver.UseContainer(rdiContainer);
</pre>
  --------
  another application file, usually entry point, like controller factory:
<pre> 
  var resolved = RDI.Concrete.Containers.RdiResolver.Get<IBase>(); // GOTCHA!
</pre>

- Constructor Injection
<pre>
   public interface IBase1 {};
   public interface IBase2 {};
   public class Derived1: IBase1 {}; 
   
   public class Derived2: IBase2 { 
      public Derived2(IBase1 b1){}
   };
   
   rdiContainer.Resolve<IBase1>().With<Derived1>();
   rdiContainer.Resolve<IBase2>().With<Derived2>();
   
   var resolved = rdiContainer.Get<IBase2>(); // resolved.GetType() == Derived2, new Derived1() object 
                                                 was passed to the constructor
<pre>

- Property Injection with multiple configurations set
<pre>
  public interface IBase2 
  {
      [RDInject(Name="SomeConfig")]
      public IBase1 ToInject {get;set;}
  }
  
  rdiContainer.Resolve<IBase1>().With<Derived1>().WhenName("SomeConfig"); // specifuing config name needed 
  rdiContainer.Resolve<IBase1>().With<SOMEANOTHERCLASS>().WhenName("ANOTHERCONFIG");
  rdiContainer.Resolve<IBase2>().With<Derived2>();
  
  var resolved = rdiContainer.Get<IBase2>(); // resolved.GetType() == Derived2, resolved.ToInject.GetType() == Derived1
</pre>

- You can specify constructor you want to be involved in injection. If no inject constructor specified, 
default constructor is used or , if it's not, another appropriate one
<pre>
  public class SomeClass
  {
     //Constructor1
     SomeClass() {}
     //Constructor2
     [RDInject]
     SomeClass(int i,object o)
  }
</pre>

- You can specify parameter passed to constructor with <pre>IRdiBindingSettings.AddConstructorParameter</pre> or
<pre>IRdiBindingSettings.WithConstructorParameters</pre> methods
<pre>
  ...
  rdiContainer.Resolve<ISomeClass>().With<SomeClass1>().AddConstructorParameter("i",100)
                                                       .AddConstructorParameter("o", new List<int>());
  
  var resolved = rdiContainer.Get<ISomeClass>();// resolved.i ==100, resolved.o == List<int>
</pre>
