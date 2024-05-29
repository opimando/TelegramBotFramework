#region Copyright

/*
 * File: ActivatorExtensionsTests.cs
 * Author: denisosipenko
 * Created: 2023-08-13
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

using Moq;
using Xunit;

namespace TgBotFramework.Core.Tests;

public class ActivatorExtensionsTests
{
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    
    [Fact]
    public void CreateGeneric_WithoutArguments_ShouldCreate()
    {
        var someInstance = _serviceProvider.Object.Create<SomeClass>();
        Assert.IsType<SomeClass>(someInstance);
        Assert.NotNull(someInstance);
    }
    
    [Fact]
    public void CreateGeneric_WithExistArgument_ShouldCreate()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface))).Returns(Mock.Of<ISomeInterface>());
        
        var someInstance = _serviceProvider.Object.Create<SomeClass2>();
        
        Assert.IsType<SomeClass2>(someInstance);
        Assert.NotNull(someInstance);
        Assert.Equal(1, someInstance.ConstructorIndex);
    }
    
    [Fact]
    public void CreateGeneric_WithoutExistArgument_ShouldThrowException()
    {
        Assert.ThrowsAny<Exception>(() => _serviceProvider.Object.Create<SomeClass2>());
    }
    
    [Fact]
    public void CreateGeneric_WithoutExistOptionalArgument_ShouldCreate()
    {
        var someInstance = _serviceProvider.Object.Create<SomeClass3>();
        
        Assert.IsType<SomeClass3>(someInstance);
        Assert.NotNull(someInstance);
        Assert.Null(someInstance.Instance);
    }
    
    [Fact]
    public void CreateGeneric_WithOptionalArgument_ShouldCreate()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface))).Returns(Mock.Of<ISomeInterface>());

        var someInstance = _serviceProvider.Object.Create<SomeClass3>();
        
        Assert.IsType<SomeClass3>(someInstance);
        Assert.NotNull(someInstance);
        Assert.NotNull(someInstance.Instance);
    }
    
    [Fact]
    public void CreateGeneric_WithExistArguments_ShouldCreateAndChooseBiggerConstructor()
    {
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface))).Returns(Mock.Of<ISomeInterface>());
        _serviceProvider.Setup(i => i.GetService(typeof(ISomeInterface2))).Returns(Mock.Of<ISomeInterface2>());

        var someInstance = _serviceProvider.Object.Create<SomeClass2>();
        
        Assert.IsType<SomeClass2>(someInstance);
        Assert.NotNull(someInstance);
        Assert.Equal(2, someInstance.ConstructorIndex);
    }

    public interface ISomeInterface{}
    public interface ISomeInterface2{}
    
    private class SomeClass
    {
        
    }
    
    private class SomeClass3
    {
        public ISomeInterface? Instance { get; }
        
        public SomeClass3(ISomeInterface? service)
        {
            Instance = service;
        }
    }
    
    private class SomeClass2
    {
        public int ConstructorIndex { get; }
        public SomeClass2(ISomeInterface service)
        {
            ConstructorIndex = 1;
        }
        
        public SomeClass2(ISomeInterface service, ISomeInterface2 service2)
        {
            ConstructorIndex = 2;
        }
    }
}