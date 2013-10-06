using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDI;
using RDI.Concrete.Containers;
using RdiTests.RdiContainerTests.TestClasses;

namespace RdiTests
{
    [TestClass]
    public class RdiContainerTest
    {
        RdiContainer diContainer;

        public RdiContainerTest()
        {
            diContainer = new RdiContainer();  
        }

        #region Interfaces

        [TestMethod]
        public void CanResolveSimpleInterfaces()
        {   
            diContainer.Resolve<IClassB>().With<ClassB2>();
            var res = diContainer.Get<IClassB>();
            Assert.AreEqual(typeof(ClassB2), res.GetType());
        }

        [TestMethod]
        public void CanResolveStructuralInterfaces_AndCallNotDefaultConstructors()
        {           
            var res= GetA2();
            Assert.AreEqual(typeof(ClassA2), res.GetType());           
        }

        [TestMethod]
        public void CanResolveInterfaceDependencies()
        {
            diContainer.Resolve<IClassB>().With<IClassBDerived>();
            diContainer.Resolve<IClassBDerived>().With<ClassBDervived>();

            var res = diContainer.Get<IClassB>();
            Assert.AreEqual(typeof(ClassBDervived),res.GetType());
        }

        [TestMethod]
        public void CanCallDefaultConstructor()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            diContainer.Resolve<IClassA>().With<ClassA>();
            var resA = diContainer.Get<IClassA>();
            Assert.AreEqual(null, resA.B);
        }

        [TestMethod]
        public void CanAddParameterConstructors()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            int k = 123;
            diContainer.Resolve<IClassA>().With<ClassA2>().AddConstructorParameter("b",new ClassB2()).AddConstructorParameter("k", k);
            var res = diContainer.Get<IClassA>();
            Assert.AreEqual(typeof(ClassB2),res.B.GetType() );
            Assert.AreEqual(k, res.K);
        }

        [TestMethod]
        public void CanRedefineConstructorParameters()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            int i = 111;
            diContainer.Resolve<IClassA>().With<ClassA2>().AddConstructorParameter("i", i);
            Assert.AreEqual(i, diContainer.Get<IClassA>().I);
        }

        #region RDInjectAttribute tests

        [TestMethod]
        public void CanResolveConstructorParameterTypeFromRDInjectAttrName()
        {
            diContainer.Resolve<IClassB>().With<ClassB>().WhenName("B1");
            diContainer.Resolve<IClassB>().With<ClassB2>().WhenName("B2");
            diContainer.Resolve<IClassA>().With<ClassA2>();

            var resA = diContainer.Get<IClassA>();

            Assert.AreEqual(typeof(ClassB),resA.B.GetType());
        }

        [TestMethod]
        public void CanSelectConstructorWithRDInjectAttr()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            diContainer.Resolve<IClassA>().With<ClassA3>();

            var resA = diContainer.Get<IClassA>();

            Assert.AreEqual(2,resA.I);
            Assert.AreEqual(typeof(ClassB),resA.B.GetType());
        }

        [TestMethod]
        public void IsSetInjectionPropertiesWithRDInjectAttr_AndNotSetUnInjected()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            diContainer.Resolve<IClassA>().With<ClassA3>();

            var resA = diContainer.Get<IClassA>();

            Assert.AreEqual(typeof(ClassB),resA.BInj.GetType());
            Assert.AreEqual(null, resA.BNotInj);
        }

        #endregion

        private IClassA GetA()
        {
            diContainer.Resolve<IClassB>().With<ClassB2>();
            diContainer.Resolve<IClassA>().With<ClassA>();
            return diContainer.Get<IClassA>();
        }

        private IClassA GetA2()
        {
            diContainer.Resolve<IClassB>().With<ClassB2>();
            diContainer.Resolve<IClassA>().With<ClassA2>();
            return diContainer.Get<IClassA>();
        }

        #endregion


        #region Classes

        [TestMethod]
        public void CanResolveSimpleClasses()
        {
            var res = diContainer.Get<ClassB>();
            Assert.AreEqual(typeof(ClassB), res.GetType());
        }

        [TestMethod]
        public void CanResolveStructuralClasses()
        {
            diContainer.Resolve<IClassB>().With<ClassB>();
            var res = diContainer.Get<ClassA2>();
            Assert.AreEqual(typeof(ClassA2), res.GetType());
            Assert.AreEqual(typeof(ClassB), res.B.GetType());
        }

        #endregion       
    }
}
