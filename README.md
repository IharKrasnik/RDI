RDI
===

** .NET Dependency Injection Container **

*You can see current main features at RDI.Tests*


<pre>RDI.Concrete.Containers.RdiContainer  - main logic and dependency settings.</pre>
<pre>RDI.Concrete.Containers.RdiResolver is used for global access  to created at program entry point
to RdiContainer</pre>

Current features:
<ul>
<li> Resolve as Interfaces, as Classes</li>
<li> Constructor and Property injection support via <pre>RDI.Attributes.RDInjectAttribute</pre> </li>
<li> Support multiple dependency configuration for one Interface, controlled with 
 <pre>RDI.Attributes.RDInjectAttribute .Name</pre> property and WhenName method </li>
<li> Support resolving through resolving </li>
<li> Easy usage Asp.Net MVC when redefining conroller factory and resolving controller or another ways!</li>
</ul>
Examples:

- Simple usage
<pre>
  var rdiContainer = new RDI.Concrete.Containers.RdiContainer();
  rdiContainer.Resolve&lt;IBase&gt;().With&lt;Base&gt;();
  
  var resolved = rdiContainer.Get&lt;IBase&gt;(); // resolved.GetType() == Base
  
  RdiResolver.UseContainer(rdiContainer);
</pre>
  ***********
another application file, usually entry point, like controller factory:
<pre> 
  var resolved = RDI.Concrete.Containers.RdiResolver.Get&lt;IBase&gt;(); // GOTCHA!
</pre>

- Constructor Injection
<pre>
   public interface IBase1 {};
   public interface IBase2 {};
   public class Derived1: IBase1 {}; 
   public class Derived2: IBase2 { 
      public Derived2(IBase1 b1){}
   };
   rdiContainer.Resolve&lt;IBase1&gt;().With&lt;Derived1&gt;();
   rdiContainer.Resolve&lt;IBase2&gt;().With&lt;Derived2&gt;();
   
   var resolved = rdiContainer.Get&lt;IBase2&gt;(); // resolved.GetType() == Derived2, new Derived1() object 
                                                 was passed to the constructor
<pre>

- Property Injection with multiple configurations set
<pre>
  public interface IBase2 
  {
      [RDInject(Name="SomeConfig")]
      public IBase1 ToInject {get;set;}
  }
  
  rdiContainer.Resolve&lt;IBase1&gt;().With&lt;Derived1&gt;().WhenName("SomeConfig"); // specifuing config name needed 
  rdiContainer.Resolve&lt;IBase1&gt;().With&lt;SOMEANOTHERCLASS&gt;().WhenName("ANOTHERCONFIG");
  rdiContainer.Resolve&lt;IBase2&gt;().With&lt;Derived2&gt;();
  var resolved = rdiContainer.Get&lt;IBase2>(); // resolved.GetType() == Derived2,
                                                   resolved.ToInject.GetType() == Derived1
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
  rdiContainer.Resolve&lt;ISomeClass&gt;().With&lt;SomeClass1&gt;().AddConstructorParameter("i",100)
                                                       .AddConstructorParameter("o", new List<int>());
  
  var resolved = rdiContainer.Get&lt;ISomeClass&gt;();// resolved.i ==100, resolved.o == List<int>
</pre>
